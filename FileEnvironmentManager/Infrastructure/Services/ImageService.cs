using FileEnvironmentManager.Domain.Interfaces;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using FileEnvironmentManager.Domain.Constants;

namespace FileEnvironmentManager.Infrastructure.Services
{
    public class ImageService : IImageService
    {
        public async Task AssignThumbnailsToProjectFolders(string projectsPath, string sourcePath, bool copy)
        {
            var sourceByCategory = LoadSourceFilesByCategory(sourcePath);
            var currentIndex = InitializeIndices(sourceByCategory);

            var projectFolders = Directory.GetDirectories(projectsPath);

            foreach (var folderPath in projectFolders)
            {
                var category = ExtractCategoryFromFolderName(folderPath);
                if (category == null)
                    continue;

                if (!sourceByCategory.TryGetValue(category, out var files) || files.Count == 0)
                    continue;

                int index = currentIndex[category];
                var sourceFile = files[index];

                currentIndex[category] = (index + 1) % files.Count;

                var destFile = Path.Combine(folderPath, Path.GetFileName(sourceFile));
                await ResizeAndSaveImageAsync(sourceFile, destFile);

                if (!copy)
                    File.Delete(sourceFile);
            }
        }

        private Dictionary<string, List<string>> LoadSourceFilesByCategory(string sourcePath)
        {
            var dict = new Dictionary<string, List<string>>();
            foreach (var category in CategoryAliases.Known.Values.Distinct())
            {
                var categoryFolder = Path.Combine(sourcePath, category);
                var files = Directory.Exists(categoryFolder)
                    ? Directory.GetFiles(categoryFolder).ToList()
                    : new List<string>();
                dict[category] = files;
            }
            return dict;
        }

        private Dictionary<string, int> InitializeIndices(Dictionary<string, List<string>> sourceByCategory)
        {
            return sourceByCategory.ToDictionary(kvp => kvp.Key, kvp => 0);
        }

        private string? ExtractCategoryFromFolderName(string folderPath)
        {
            var folderName = Path.GetFileName(folderPath);
            var parts = folderName.Split('_');
            if (parts.Length < 2) return null;

            var categoryPart = parts.Last();

            return CategoryAliases.Known.Values
                .FirstOrDefault(c => c.Equals(categoryPart, StringComparison.OrdinalIgnoreCase));
        }

        private async Task ResizeAndSaveImageAsync(string sourceFile, string destFile)
        {
            using var image = await Image.LoadAsync(sourceFile);
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(600, 600),
                Mode = ResizeMode.Max
            }));

            await image.SaveAsync(destFile, new JpegEncoder());
        }
    }
}
