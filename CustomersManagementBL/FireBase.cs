using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Storage;
using System.IO;
using ZXing;
using System.Net;
using System.Drawing;
using CustomersManagementDP;
using Newtonsoft.Json;

namespace CustomersManagementBL
{
    public class FireBase
    {
        IBL ibl;
        public FireBase(IBL bl)
        {
            this.ibl = bl;

            //stam();

            Console.ReadLine();
        }

        public async Task addItem(string path)
        {
            var stream = File.Open(path, FileMode.Open);
            var task = new FirebaseStorage("windowproject-c1d25.appspot.com")
                .Child("test/QR100")
            .PutAsync(stream);

            // Track progress of the upload
            task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");

            // Await the task to wait until upload is completed and get the download url
            var downloadUrl = await task;
            Console.WriteLine(downloadUrl);

            showDetails(downloadUrl);
        }
        public async Task stam()
        {
            string first = @"C:\Users\micha\Desktop\QR\QR";
            string second = "1.png";
            string both = first+second;
            //var stream = File.Open(both, FileMode.Open);
            for (int i=1; i<=45; i++)
            {
                second = i.ToString() + ".png";
                both = first + second;
                var stream = File.Open(both, FileMode.Open);

                // Construct FirebaseStorage with path to where you want to upload the file and put it there
                var task = new FirebaseStorage("windowproject-c1d25.appspot.com")
            .Child("test/QR"+i.ToString())
            .PutAsync(stream);

                // Track progress of the upload
                task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");

                // Await the task to wait until upload is completed and get the download url
                var downloadUrl = await task;
                Console.WriteLine(downloadUrl);

                showDetails(downloadUrl);
            }
        }
       
        
        private void showDetails(string downloadUrl)
        {
            string imageUrl = downloadUrl;
            // Install-Package ZXing.Net -Version 0.16.5
            var client = new WebClient();
            var stream = client.OpenRead(imageUrl);
            if (stream == null) return;
            var bitmap = new Bitmap(stream);
            IBarcodeReader reader = new BarcodeReader();
            var result = reader.Decode(bitmap);
            Console.WriteLine(result.Text);
            //Console.ReadLine();
            
            try
            {
                if (result != null)
                {
                    ibl.AddItem(CreateItem(result.ToString()));
                }
                else
                    Console.WriteLine("failed to scan");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public Item CreateItem(string json)
        {
            Item item = JsonConvert.DeserializeObject<Item>(json);
            /* Probably should be later..*/
            //if (createdTime != null)
            //    item.Date_of_purchase = (DateTime)createdTime;
            return item;
        }
    }
}
