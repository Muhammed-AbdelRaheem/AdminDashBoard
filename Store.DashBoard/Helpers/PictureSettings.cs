﻿using System.IO;

namespace Store.DashBoard.Helpers
{
	public static class PictureSettings
	{
        public static string UploadFile(IFormFile file, string folderName)
        {
            // 1. Get Folder Path
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", folderName);
            // 2. Set FileName UINQUE
            var fileName = Guid.NewGuid() + file.FileName;
            // 3. Get File Path
            var filePath = Path.Combine(folderPath, fileName);
            // 4. Save File as Streams
          using  var fs = new FileStream(filePath, FileMode.Create);
            // 5. Copy File Into Streams
            file.CopyTo(fs);
            // 6. Retun FileName

            return Path.Combine("images\\products", fileName);
        }

        public static void DeleteFile(string folderName)
        {

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);

            if (File.Exists(filePath))
            {

                File.Delete(filePath);
            }
        }
    }
}
