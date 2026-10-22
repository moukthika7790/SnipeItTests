using Microsoft.Playwright.NUnit;
using SnipeItTests.Config;
using SnipeItTests.Models;
using SnipeItTests.Pages;
using static Microsoft.Playwright.Assertions;

namespace SnipeItTests.Tests;

[TestFixture]
public class AssetLifecycleTest : PageTest
{
    [Test]
    public async Task CreateCheckoutAndVerifyAsset()
    {
        var asset = NewAsset.Create();

        // 1. log in
        await new LoginPage(Page).LoginAsync(Settings.Username, Settings.Password);

        // 2. create the asset, checked out to a random user
        var createPage = new CreateAssetPage(Page);
        await createPage.OpenAsync();
        var userText = await createPage.CreateAsync(asset);
        var userName = NameOnly(userText);

        // 3. find it in the list
        var assets = new AssetsPage(Page);
        await assets.OpenAsync();
        await assets.SearchAsync(asset.Tag);
        await Expect(assets.RowFor(asset.Tag)).ToBeVisibleAsync();

        // 4. open it and check the details
        await assets.OpenAssetAsync(asset.Tag);
        var details = new AssetDetailsPage(Page);
        await details.VerifyDetailsAsync(asset.Tag, asset.Serial, asset.Model, userName);

        // 5. check the history tab
        await details.VerifyHistoryAsync(userName);
    }

    // The dropdown shows something like "Smith, Jane (jsmith)".
    // The asset page only shows the name, so drop the username part.
    private static string NameOnly(string dropdownText)
    {
        var bracket = dropdownText.IndexOf('(');
        var name = bracket > 0 ? dropdownText[..bracket] : dropdownText;
        return name.Trim().TrimEnd(',');
    }
}
