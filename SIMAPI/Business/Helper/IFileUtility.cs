namespace SIMAPI.Business.Helper
{
    public interface IFileUtility
    {
        Task<string> UploadImageAsync(IFormFile file, string folderName);
        string GetImagePath(string folderName, string imageName);
        string UploadFile(IFormFile file, string folderName);

    }
}
