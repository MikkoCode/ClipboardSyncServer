using ClipboardSyncServer.Models;
using ClipboardSyncServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClipboardSyncServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClipboardController : ControllerBase
{
    private readonly IClipboardService _clipboardService;
    private readonly ILogger<ClipboardController> _logger;

    public ClipboardController(IClipboardService clipboardService, ILogger<ClipboardController> logger)
    {
        _clipboardService = clipboardService;
        _logger = logger;
    }

    [HttpPost]
    public IActionResult UpdateClipboard([FromBody] ClipboardData data)
    {
        if (data == null || string.IsNullOrEmpty(data.Device) || string.IsNullOrEmpty(data.Data) ||
            string.IsNullOrEmpty(data.DeviceName))
        {
            _logger.LogWarning("Invalid data received for clipboard update.");
            return BadRequest(new ApiResponse<object> { Success = false, Message = "Invalid data." });
        }

        _clipboardService.UpdateClipboardData(data);
        _logger.LogInformation($"Clipboard data updated for device: {data.Device}");
        return Ok(new ApiResponse<object> { Success = true, Message = "Clipboard data updated successfully." });
    }

    [HttpGet]
    public IActionResult GetClipboard([FromQuery] string device)
    {
        if (string.IsNullOrEmpty(device))
        {
            _logger.LogWarning("Invalid device requested for clipboard data.");
            return BadRequest(new ApiResponse<object> { Success = false, Message = "Invalid device." });
        }

        var data = _clipboardService.GetClipboardData(device);
        if (data == null)
        {
            _logger.LogWarning($"No clipboard data found for device: {device}");
            return NotFound(new ApiResponse<object> { Success = false, Message = "Clipboard data not found." });
        }

        _logger.LogInformation($"Clipboard data retrieved for device: {device}");
        return Ok(new ApiResponse<ClipboardData>
            { Success = true, Message = "Clipboard data retrieved successfully.", Data = data });
    }
}