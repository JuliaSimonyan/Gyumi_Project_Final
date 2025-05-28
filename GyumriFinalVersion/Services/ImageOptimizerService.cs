using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

public class ImageOptimizerService
{
    private readonly string _wwwroot;

    public ImageOptimizerService(IWebHostEnvironment env)
    {
        _wwwroot = env.WebRootPath;
    }

    public void OptimizeAllImagesInWwwroot()
    {
        var imageExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var allImages = Directory.GetFiles(_wwwroot, "*.*", SearchOption.AllDirectories)
                                 .Where(f => imageExtensions.Contains(Path.GetExtension(f).ToLower()))
                                 .ToList();

        foreach (var imgPath in allImages)
        {
            var webpPath = Path.ChangeExtension(imgPath, ".webp");

            if (File.Exists(webpPath))
                continue;

            using var image = Image.Load(imgPath);
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(1024, 768),
                Mode = ResizeMode.Max
            }));

            image.Save(webpPath, new WebpEncoder { Quality = 75 });

            File.Delete(imgPath);
        }
    }
    public string OptimizeAndSaveUploadedImage(IFormFile file, string subFolder = "uploads")
    {
        var folderPath = Path.Combine(_wwwroot, subFolder);
        Directory.CreateDirectory(folderPath);

        var fileName = Path.GetFileNameWithoutExtension(file.FileName);
        var optimizedName = $"{fileName}_{DateTime.UtcNow.Ticks}.webp";
        var savePath = Path.Combine(folderPath, optimizedName);

        using var stream = file.OpenReadStream();
        using var image = Image.Load(stream);

        image.Mutate(x => x.Resize(new ResizeOptions
        {
            Size = new Size(1024, 768),
            Mode = ResizeMode.Max
        }));

        image.Save(savePath, new WebpEncoder { Quality = 75 });

        return $"/{subFolder}/{optimizedName}";
    }

}
