using FileEnvironmentManager.Domain.Interfaces;
using FileEnvironmentManager.Domain.Models;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace FileEnvironmentManager.Infrastructure.Services
{
    public class ImageService : IImageService
    {
        public async Task AssignThumbnailsToProjectFolders(string projectsPath, string sourcePath, bool copy)
        {
            var sourceByCategory = new Dictionary<string, List<string>>();
            var currentIndex = new Dictionary<string, int>();

            foreach (var category in Category.Known)
            {
                string categoryFolder = Path.Combine(sourcePath, category.Value);
                var files = Directory.Exists(categoryFolder)
                    ? Directory.GetFiles(categoryFolder).ToList()
                    : new List<string>();

                sourceByCategory[category.Value] = files;
                currentIndex[category.Value] = 0;
            }

            var folders = Directory.GetDirectories(projectsPath);
            foreach (var folder in folders)
            {
                var name = Path.GetFileName(folder);
                var parts = name.Split('_');
                if (parts.Length < 2) continue;

                var category = parts.Last();
                var match = Category.Known.Values.FirstOrDefault(x => x.Equals(category, StringComparison.OrdinalIgnoreCase));
                if (string.IsNullOrEmpty(match)) continue;

                var list = sourceByCategory[match];
                if (list.Count == 0) continue;

                int idx = currentIndex[match];
                var file = list[idx];
                currentIndex[match] = (idx + 1) % list.Count;

                var fileName = Path.GetFileName(file);
                var dest = Path.Combine(folder, fileName);

                using var image = await Image.LoadAsync(file);
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(600, 600),
                    Mode = ResizeMode.Max
                }));

                await image.SaveAsync(dest, new JpegEncoder());
                if (!copy) File.Delete(file);
            }
        }
    }
}
