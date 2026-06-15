namespace SIMAPI.Business.Helper
{
    public  class FileUtility: IFileUtility
    {
        IConfiguration _configuration;
        
        public FileUtility(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public  async Task<string> UploadImageAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
                return null;

            // Use app base directory (more stable under IIS/containers than CurrentDirectory)
            var basePath = _configuration["AppSettings:UploadPath"];
            var folderPath = Path.Combine(basePath, "Resources", "Images", folderName);
            var pathToSave = folderPath;

            // Ensure folder exists
            if (!Directory.Exists(pathToSave))
                Directory.CreateDirectory(pathToSave);

            // Unique filename
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var fullPath = Path.Combine(pathToSave, fileName);

            try
            {
                // Write file asynchronously with exclusive write access
                using (var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, 81920, useAsync: true))
                {
                    await file.CopyToAsync(stream);
                }

                return fileName;
            }
            catch (IOException ioEx)
            {
                // Add context and rethrow so caller can log or fallback
                throw new IOException($"Failed to save uploaded image to '{fullPath}'. Check disk/network availability and permissions.", ioEx);
            }
        }

        public  string GetImagePath(string folderName, string imageName)
        {
            // Build a relative path used by the front-end (keeps previous behavior)
            var relativeFolder = Path.Combine("Resources", "Images", folderName).Replace("\\", "/");
            return relativeFolder + "/" + imageName;
        }

        public  string UploadFile(IFormFile file, string folderName)
        {
            if (file != null)
            {
                var folderPath = Path.Combine("Resources", "BulkUploadFiles", folderName);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderPath);
                if (file.Length > 0)
                {
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(file.FileName);
                    var fullPath = Path.Combine(pathToSave, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return fullPath;
                }
            }
            return null;
        }
    }
}
