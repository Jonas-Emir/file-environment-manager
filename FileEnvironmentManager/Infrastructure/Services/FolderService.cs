using FileEnvironmentManager.Domain.Interfaces;

namespace FileEnvironmentManager.Infrastructure.Services
{
    public class FolderService : IFolderService
    {
        public void CreateSimpleFolder(string baseDirectory, string folderName)
        {
            var fullPath = Path.Combine(baseDirectory, folderName);
            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);
        }

        public void CreateFoldersWithCategory(string baseDirectory, int start, int end, string category)
        {
            for (int i = start; i <= end; i++)
            {
                var folderName = $"{i}_{category}";
                var path = Path.Combine(baseDirectory, folderName);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
        }

        public void RemoveAllFilesFromFolders(string baseDirectory)
        {
            var subdirs = Directory.GetDirectories(baseDirectory);
            foreach (var dir in subdirs)
            {
                foreach (var file in Directory.GetFiles(dir))
                {
                    File.Delete(file);
                }
            }
        }
    }
}
