namespace SIMAPI.Business.Helper
{
    public static class FileUtility
    {
        public static string uploadImage(IFormFile file, string folderName)
        {
            if (file != null)
            {
                var folderPath = Path.Combine("Resources", "Images", folderName);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderPath);
                if (file.Length > 0)
                {
                    //var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(file.FileName);
                    var fullPath = Path.Combine(pathToSave, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return fileName;
                }
            }
            return null;
        }

        public static string GetImagePath(string folderName, string imageName)
        {
            var folderPath = Path.Combine("Resources", "Images", folderName);
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderPath);
            return folderPath.Replace("\\", "/") + "/" + imageName;
        }


        public static string uploadFile(IFormFile file, string folderName)
        {
            if (file != null)
            {
                var folderPath = Path.Combine("Resources", "BulkUploadFiles", folderName);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderPath);
                if (file.Length > 0)
                {
                    //var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
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
