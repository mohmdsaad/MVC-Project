using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace C42G02Demo.PL.Helpers
{
    public static class DocumentSettings
    {
        public static string UploadFile(IFormFile file, string folderName)
        {
            // 1. Get Located Folder Path
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName);

            // 2. Get File Name and Make it Unique
            string FileName = $"{Guid.NewGuid()}{file.FileName}";
            
            // 3. Get File Path [Folder Path + FileName]
            string FilePath = Path.Combine(FolderPath, FileName);

            // 4. Save File As Streams
            using var Fs = new FileStream(FilePath, FileMode.Create);
            file.CopyTo(Fs);
            
            // 5. Return File Name
            return FileName;
        }

        public static void DeleteFile(string FileName , string FolderName)
        {
            // Get File Path
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\Files",FolderName,FileName);

            //Check if file existes, if exits delete it
            if (File.Exists(FilePath))
                File.Delete(FilePath);
        }
    }
}