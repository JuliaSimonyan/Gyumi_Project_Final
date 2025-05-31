using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace GyumriFinalVersion.Services
{
    public class PdfGenerator
    {
        public async Task<Stream> GeneratePdfAsync(string htmlContent)
        {
            // 1. Download the Chromium browser if needed
            await new BrowserFetcher().DownloadAsync();

            // 2. Launch the browser
            var launchOptions = new LaunchOptions
            {
                Headless = true,
                Args = new[] { "--no-sandbox" } // Important for some environments
            };

            await using var browser = await Puppeteer.LaunchAsync(launchOptions);
            await using var page = await browser.NewPageAsync();

            // Set content and generate PDF
            await page.SetContentAsync(htmlContent);
            return await page.PdfStreamAsync(new PdfOptions
            {
                Format = PaperFormat.A4,
                MarginOptions = new MarginOptions
                {
                    Right = "2cm",
                    Bottom = "2cm",
                    Left = "1cm"
                },
                //PrintBackground = true
            });
        }
    }
}