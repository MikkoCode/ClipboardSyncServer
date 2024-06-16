using ClipboardSyncServer.Models;

namespace ClipboardSyncServer.Services;

public class ClipboardService : IClipboardService
{
    private static readonly Dictionary<string, ClipboardData> clipboardData = new()
    {
        { "phone", new ClipboardData() },
        { "pc", new ClipboardData() }
    };

    public void UpdateClipboardData(ClipboardData data)
    {
        clipboardData[data.Device] = data;
    }

    public ClipboardData GetClipboardData(string device)
    {
        var otherDevice = device == "pc" ? "phone" : "pc";
        return clipboardData.ContainsKey(otherDevice) ? clipboardData[otherDevice] : null;
    }
}