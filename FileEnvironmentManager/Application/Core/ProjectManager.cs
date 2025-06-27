using FileEnvironmentManager.Domain.Interfaces;

namespace FileEnvironmentManager.Application
{
    public class ProjectManager
    {
        private readonly IImageService _imageService;

        public ProjectManager(IImageService imageService)
        {
            _imageService = imageService;
        }

        public async Task AssignThumbnailsToProjectFolders(string projectsPath, string sourcePath, bool copy)
        {
            await _imageService.AssignThumbnailsToProjectFolders(projectsPath, sourcePath, copy);
        }
    }
}
