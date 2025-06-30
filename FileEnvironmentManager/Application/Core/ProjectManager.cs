using FileEnvironmentManager.Domain.Interfaces;

namespace FileEnvironmentManager.Application
{
    public class ProjectManager
    {
        private readonly IFolderService _folderService;
        private readonly IImageService _imageService;

        public ProjectManager(IFolderService folderService, IImageService imageService)
        {
            _folderService = folderService;
            _imageService = imageService;
        }

        public void CreateSimpleFolder(string basePath, string name)
        {
            _folderService.CreateSimpleFolder(basePath, name);
        }

        public void CreateFoldersWithCategory(string basePath, int start, int end, string category)
        {
            _folderService.CreateFoldersWithCategory(basePath, start, end, category);
        }

        public async Task AssignThumbnailsToProjectFolders(string projectsPath, string sourcePath, bool copy)
        {
            await _imageService.AssignThumbnailsToProjectFolders(projectsPath, sourcePath, copy);
        }

        public void RemoveAllFilesFromFolders(string directory)
        {
            _folderService.RemoveAllFilesFromFolders(directory);
        }
    }
}
