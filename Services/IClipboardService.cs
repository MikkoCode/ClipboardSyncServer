using ClipboardSyncServer.Models;

namespace ClipboardSyncServer.Services;

public interface IClipboardService
{
    void UpdateClipboardData(ClipboardData data);
    ClipboardData GetClipboardData(string device);
}