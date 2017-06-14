using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;
using Android.Net;
using Android.Database;
using Android.Graphics;
using System.Net;
using Android.Provider;

namespace App8
{
    public class OnlinePicture
    {
        public OnlinePicture()
        {
        }
        public static void GetBitMap()
        {

        }
        public static async Task Upload(Activity act,Android.Net.Uri filePath, String containerName, String referenceName)
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=storagedatabase666;AccountKey=+Y3VXCpGDPbxAvhalqCLbUNXLXxXlFYKFy09OavTRWZ/xmWQ9ofptnlTPBqvSqcalA3tKuQ35/Pg8xPg9deeDQ==");

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);

            await container.CreateIfNotExistsAsync();

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(referenceName);
            using (var fileStream = System.IO.File.OpenRead(GetRealPathFromURI(act,filePath)))
             {
               await blockBlob.UploadFromStreamAsync(fileStream);
            }
        }
        public static Bitmap Stream(String url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;

        }
        public static string GetRealPathFromURI(Activity act,Android.Net.Uri contentURI)//SOURCE
        {

            ICursor cursor = act.ContentResolver.Query(contentURI, null, null, null, null);
            cursor.MoveToFirst();
            string documentId = cursor.GetString(0);
            cursor.Close();
            cursor = act.ContentResolver.Query(
            MediaStore.Images.Media.ExternalContentUri,
            null, MediaStore.Images.Media.InterfaceConsts.Id + " = ? ", new[] { documentId }, null);
            cursor.MoveToFirst();
            string path = cursor.GetString(cursor.GetColumnIndex(MediaStore.Images.Media.InterfaceConsts.Data));
            cursor.Close();

            return path;
        }

    }

}
