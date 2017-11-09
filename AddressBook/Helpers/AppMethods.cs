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
        /// <summary>
        ///         
        /// Property that reads profile photo path from property ProfilePicPath and returns byte array if picture exists.        
        /// </summary>
        /// <param name="profilePhoto">Profile photo path. If profilePhoto is null then image on default profile photo path will be used.</param>
        /// <returns></returns>
        public static byte[] GetProfilePhotoContent(string profilePhotoPath)
        {
            FileInfo profilePic = new FileInfo(profilePhotoPath ?? Params.DefaultProfilePicPath);

            if (!profilePic.Exists)
            {
                return null;
            }

            using (MemoryStream ms = new MemoryStream())
            {
                Image img = Image.FromFile(profilePic.FullName);
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }
    }
}