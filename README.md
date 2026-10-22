# Snipe-IT Asset Lifecycle – Playwright (.NET) Automation

End-to-end test for the public Snipe-IT demo (https://demo.snipeitapp.com).

The test:
1. Logs in
2. Creates a **Macbook Pro 13"** asset with status **Ready to Deploy**, checked out to a **random user**
3. Finds the new asset in the assets list
4. Opens the asset page and validates asset tag, serial, model, status and assigned user
5. Validates the **History** tab (creation + checkout entries for the chosen user)

## Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download) 10 (latest)
- PowerShell (`pwsh`), needed once to install Playwright browsers

## Setup
```bash
dotnet build
pwsh bin/Debug/net10.0/playwright.ps1 install chromium
```

## Run
```bash
dotnet test
```
Watch the browser while it runs:
```bash
HEADED=1 dotnet test            # macOS / Linux
$env:HEADED=1; dotnet test      # Windows PowerShell
```

## Configuration (optional environment variables)
| Variable | Default |
|---|---|
| `SNIPEIT_BASE_URL` | `https://demo.snipeitapp.com` |
| `SNIPEIT_USERNAME` | `admin` |
| `SNIPEIT_PASSWORD` | `password` |

## Design
- **Page Object Model** (`Pages/`): each page owns its locators; tests read as business steps.
- **`Select2` helper**: Snipe-IT dropdowns are Select2 widgets, handled in one place.
- **Unique test data** (`Models/NewAsset.cs`): asset tag and serial are generated per run because the demo is shared.
- **Web-first assertions**: Playwright `Expect` auto-waits, so there are no fixed sleeps.
- **Configuration** (`Config/Settings.cs`): URLs and credentials come from environment variables.

## Notes / assumptions
- The demo instance resets periodically and is shared, so the random user and existing data can vary between runs.
- After checkout, Snipe-IT may show the status as "Deployed"; the test accepts either "Ready to Deploy" or "Deployed".
