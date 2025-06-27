namespace FileEnvironmentManager.Domain.Interfaces
{
    public interface IImageService
    {
        Task AssignThumbnailsToProjectFolders(string projectsPath, string sourcePath, bool copy);
    }
}
