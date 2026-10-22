namespace SnipeItTests.Models;

// The demo is shared with other people, so tag and serial must be unique per run.
public class NewAsset
{
    public string Tag { get; init; } = "";
    public string Serial { get; init; } = "";
    public string Model { get; init; } = "Macbook Pro 13";
    public string Status { get; init; } = "Ready to Deploy";

    public static NewAsset Create()
    {
        var stamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var suffix = Random.Shared.Next(100, 999);

        return new NewAsset
        {
            Tag = $"AUTO-{stamp}{suffix}",
            Serial = $"SN{stamp}{suffix}"
        };
    }
}
