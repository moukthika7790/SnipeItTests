using Microsoft.Playwright;

namespace SnipeItTests.Helpers;

// Snipe-IT uses Select2 for its dropdowns, so a normal SelectOption won't work.
// All the fiddly bits are kept here.
public static class Select2
{
    private const string Option = ".select2-results__option[role='option'], .select2-results__option[role='treeitem']";

    // Type some text into the dropdown and click the first matching result.
    public static async Task PickAsync(IPage page, string container, string text)
    {
        await page.Locator(container).ClickAsync();
        await page.Locator(".select2-search__field").FillAsync(text);

        var match = page.Locator(Option).Filter(new() { HasTextString = text }).First;
        await match.ClickAsync();
    }

    // Open the dropdown, pick any real entry at random, and return its text.
    public static async Task<string> PickRandomAsync(IPage page, string container)
    {
        await page.Locator(container).ClickAsync();

        // Results are loaded over ajax, wait for at least one real entry.
        var options = page.Locator(Option).Filter(new() { HasNotTextString = "Searching" });
        await options.First.WaitForAsync();

        var all = await options.AllAsync();
        var chosen = all[Random.Shared.Next(all.Count)];

        var text = (await chosen.InnerTextAsync()).Trim();
        await chosen.ClickAsync();
        return text;
    }
}
