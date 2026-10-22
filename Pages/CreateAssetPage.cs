using Microsoft.Playwright;
using SnipeItTests.Config;
using SnipeItTests.Helpers;
using SnipeItTests.Models;

namespace SnipeItTests.Pages;

public class CreateAssetPage
{
    private readonly IPage _page;

    public CreateAssetPage(IPage page) => _page = page;

    private ILocator AssetTag => _page.Locator("#asset_tag");
    private ILocator Serial => _page.Locator("#serial");
    private ILocator SaveButton => _page.GetByRole(AriaRole.Button, new() { Name = "Save" });

    public Task OpenAsync() => _page.GotoAsync($"{Settings.BaseUrl}/hardware/create");

    // Fills in the form and saves it. Returns the text of the user the asset was checked out to.
    public async Task<string> CreateAsync(NewAsset asset)
    {
        await Select2.PickAsync(_page, "#select2-model_select_id-container", asset.Model);
        await Select2.PickAsync(_page, "#select2-status_select_id-container", asset.Status);

        await AssetTag.FillAsync(asset.Tag);
        await Serial.FillAsync(asset.Serial);

        var user = await Select2.PickRandomAsync(_page, "#select2-assigned_user_select-container");

        await SaveButton.ClickAsync();
        return user;
    }
}
