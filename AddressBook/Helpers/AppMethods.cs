using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace AddressBook.Helpers
{
    /// <summary>
    /// Class taht will contain static methods that can be used anywhere in app.
    /// </summary>
    public static class AppMethods
    {
        private static string[] _allowedImageExtensions = new string[] { ".jpg", ".jpeg", ".png", ".bmp" };

        /// <summary>
        /// Methods that takes photo from given path, reads the image and converts it to byte array.
        /// </summary>
        /// <param name="profilePhoto">Profile photo path.</param>
        /// <returns>Image in byte array or null if image doesn't exist.</returns>
        public static byte[] GetImageAsByteArray(string path)
        {
            FileInfo imagePath = new FileInfo(path);

            if (!imagePath.Exists)
            {
                return null;
            }
                        
            using (MemoryStream ms = new MemoryStream())
            {
                Image img = Image.FromFile(imagePath.FullName);
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Methods that takes photo from given path, reads the image and converts it to base 64 string.
        /// </summary>
        /// <param name="profilePhoto">Profile photo path.</param>
        /// <returns>Image in Base 64 format or null if image doesn't exist.</returns>
        public static string GetImageAsBase64(string path)
        {
            FileInfo imagePath = new FileInfo(path);

            if (!imagePath.Exists)
            {
                return null;
            }

            var bytes = GetImageAsByteArray(imagePath.FullName);

            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Method that checks if image has allowed extension.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static bool IsValidImageType(FileInfo imageInfo)
        {
            return _allowedImageExtensions.Contains(imageInfo.Extension);
        }
    }
}