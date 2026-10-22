using System.Text.RegularExpressions;
using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace SnipeItTests.Pages;

public class AssetDetailsPage
{
    private readonly IPage _page;

    public AssetDetailsPage(IPage page) => _page = page;

    private ILocator Body => _page.Locator("body");

    public async Task VerifyDetailsAsync(string tag, string serial, string model, string assignedName)
    {
        await Expect(Body).ToContainTextAsync(tag);
        await Expect(Body).ToContainTextAsync(serial);
        await Expect(Body).ToContainTextAsync(model);

        // Checking out usually flips the status to Deployed, so accept both.
        await Expect(Body).ToContainTextAsync(new Regex("Ready to Deploy|Deployed"));

        await Expect(Body).ToContainTextAsync(assignedName);
    }

    public async Task VerifyHistoryAsync(string assignedName)
    {
        await _page.GetByRole(AriaRole.Link, new() { Name = "History" }).ClickAsync();

        var rows = _page.Locator("table tbody tr");

        // One entry for the creation...
        await Expect(rows.Filter(new() { HasTextString = "create new" }).First).ToBeVisibleAsync();

        // ...and one for the checkout, which should name our user.
        var checkout = rows.Filter(new() { HasTextString = "checkout" }).First;
        await Expect(checkout).ToBeVisibleAsync();
        await Expect(checkout).ToContainTextAsync(assignedName);
    }
}
