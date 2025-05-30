﻿using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

public class ImageOptimizerService
{
    private readonly string _wwwroot;

    public ImageOptimizerService(IWebHostEnvironment env)
    {
        _wwwroot = env.WebRootPath;
    }

    public void OptimizeSpecificImageFolders()
    {
        var logPath = Path.Combine(_wwwroot, "optimize-log.txt");
        File.AppendAllText(logPath, $"Optimizer ran at {DateTime.UtcNow}\n");

        var targetFolders = new[]
        {
        Path.Combine(_wwwroot, "uploads"),
        Path.Combine(_wwwroot, "Images"),
        Path.Combine(_wwwroot, "Content", "Images")
    };

        var imageExtensions = new[] { ".webp", ".webp", ".webp" };

        foreach (var folder in targetFolders)
        {
            if (!Directory.Exists(folder))
            {
                File.AppendAllText(logPath, $"Skipped missing folder: {folder}\n");
                continue;
            }

            var images = Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories)
                                  .Where(f => imageExtensions.Contains(Path.GetExtension(f).ToLower()))
                                  .ToList();

            foreach (var imgPath in images)
            {
                File.AppendAllText(logPath, $"Processing: {imgPath}\n");

                var webpPath = Path.ChangeExtension(imgPath, ".webp");
                if (File.Exists(webpPath))
                {
                    File.AppendAllText(logPath, $"Already exists: {webpPath}\n");
                    continue;
                }

                using var image = Image.Load(imgPath);
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(1024, 768),
                    Mode = ResizeMode.Max
                }));

                image.Save(webpPath, new WebpEncoder { Quality = 75 });

                File.Delete(imgPath);
                File.AppendAllText(logPath, $"Optimized and deleted: {imgPath}\n");
            }
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
