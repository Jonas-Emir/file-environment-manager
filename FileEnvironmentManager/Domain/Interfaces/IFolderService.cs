namespace FileEnvironmentManager.Domain.Interfaces
{
    public interface IFolderService
    {
        void CreateSimpleFolder(string baseDirectory, string folderName);
        void CreateFoldersWithCategory(string baseDirectory, int start, int end, string category);
        void RemoveAllFilesFromFolders(string baseDirectory);
    }
}
