using Microsoft.Playwright;
using SnipeItTests.Config;

namespace SnipeItTests.Pages;

public class LoginPage
{
    private readonly IPage _page;

    public LoginPage(IPage page) => _page = page;

    private ILocator Username => _page.Locator("#username");
    private ILocator Password => _page.Locator("#password");
    private ILocator LoginButton => _page.GetByRole(AriaRole.Button, new() { Name = "Login" });

    public async Task LoginAsync(string username, string password)
    {
        await _page.GotoAsync($"{Settings.BaseUrl}/login");
        await Username.FillAsync(username);
        await Password.FillAsync(password);
        await LoginButton.ClickAsync();

        // Once we're in, the login form is gone.
        await Assertions.Expect(Username).ToHaveCountAsync(0);
    }
}
