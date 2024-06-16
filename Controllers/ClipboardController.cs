using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ClipboardSyncServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClipboardController : ControllerBase
    {
        private static Dictionary<string, string> clipboardData = new Dictionary<string, string>
        {
            { "phone", "" },
            { "pc", "" }
        };

        [HttpPost]
        public IActionResult UpdateClipboard([FromBody] ClipboardData data)
        {
            if (data == null || string.IsNullOrEmpty(data.Device) || string.IsNullOrEmpty(data.Data))
            {
                return BadRequest("Invalid data.");
            }

            clipboardData[data.Device] = data.Data;
            return Ok(new { success = true });
        }

        [HttpGet]
        public IActionResult GetClipboard([FromQuery] string device)
        {
            if (string.IsNullOrEmpty(device) || !clipboardData.ContainsKey(device))
            {
                return BadRequest("Invalid device.");
            }

            var otherDevice = device == "pc" ? "phone" : "pc";
            return Ok(new { data = clipboardData[otherDevice] });
        }
    }
}