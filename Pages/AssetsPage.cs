using Microsoft.Playwright;
using SnipeItTests.Config;

namespace SnipeItTests.Pages;

public class AssetsPage
{
    private readonly IPage _page;

    public AssetsPage(IPage page) => _page = page;

    private ILocator SearchBox => _page.Locator(".search-input").First;

    public Task OpenAsync() => _page.GotoAsync($"{Settings.BaseUrl}/hardware");

    public Task SearchAsync(string text) => SearchBox.FillAsync(text);

    public ILocator RowFor(string assetTag) =>
        _page.Locator("table tbody tr").Filter(new() { HasTextString = assetTag });

    public Task OpenAssetAsync(string assetTag) =>
        _page.GetByRole(AriaRole.Link, new() { Name = assetTag, Exact = true }).First.ClickAsync();
}
