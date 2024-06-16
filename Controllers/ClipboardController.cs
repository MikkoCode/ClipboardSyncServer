using ClipboardSyncServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace ClipboardSyncServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClipboardController : ControllerBase
    {
        private readonly ILogger<ClipboardController> _logger;

        private static Dictionary<string, ClipboardData> clipboardData = new Dictionary<string, ClipboardData>
        {
            { "phone", new ClipboardData() },
            { "pc", new ClipboardData() }
        };

        public ClipboardController(ILogger<ClipboardController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult UpdateClipboard([FromBody] ClipboardData data)
        {
            if (data == null || string.IsNullOrEmpty(data.Device) || string.IsNullOrEmpty(data.Data) || string.IsNullOrEmpty(data.DeviceName))
            {
                _logger.LogWarning("Invalid data received for clipboard update.");
                return BadRequest("Invalid data.");
            }

            clipboardData[data.Device] = data;
            _logger.LogInformation($"Clipboard data updated for device: {data.Device}");
            return Ok(new { success = true });
        }

        [HttpGet]
        public IActionResult GetClipboard([FromQuery] string device)
        {
            if (string.IsNullOrEmpty(device) || !clipboardData.ContainsKey(device))
            {
                _logger.LogWarning("Invalid device requested for clipboard data.");
                return BadRequest("Invalid device.");
            }

            var otherDevice = device == "pc" ? "phone" : "pc";
            var data = clipboardData[otherDevice];
            _logger.LogInformation($"Clipboard data retrieved for device: {device}");
            return Ok(data);
        }
    }
}