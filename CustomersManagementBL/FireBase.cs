using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Storage;
using System.IO;

namespace CustomersManagementBL
{
    public class FireBase
    {
        public FireBase()
        {
            stam();

            Console.ReadLine();
        }
        public async static Task stam()
        {
            var stream = File.Open(@"C:\Users\micha\Desktop\School\year C sem B\Windows project\project\QRcodes\10-02\QR12.png", FileMode.Open);

            // Construct FirebaseStorage with path to where you want to upload the file and put it there
            var task = new FirebaseStorage("windowproject-c1d25.appspot.com")
             .Child("10-02/QR12")
             .PutAsync(stream);

            // Track progress of the upload
            task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");

            // Await the task to wait until upload is completed and get the download url
            var downloadUrl = await task;
            Console.WriteLine(downloadUrl);

            //showDetails(downloadUrl);
        }
    }
}
