using CustomersManagementBL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomersManagementDP;
using System.Data.SqlClient;

namespace CustomersManagementProject
{
    class Program
    {
        static void Main(string[] args)
        {
            IBL bl = new BL_imp();

            //bl.UpdateItem(new Item(2, "Pasta", DateTime.Now, "Tel-Aviv", "Rami Levi", 8, Enums.TYPE.Food, "Sweet Pasta!!!", "iuor643"));
            //bl.AddItem(new Item("Pasta", DateTime.Now, "Tel-Aviv", "Rami Levi", 8, Enums.TYPE.Food, "Sweet Pasta!!!", "iuor643", 25.00));
            //bl.RemoveItem(2);

            ////create and add all items to fireBase and DBset
            //FireBase fireBase = new FireBase(bl);

            List<Item> items = bl.getAllItems();
            int i = 0; //count items

            ////remove all items from DBSet
            //foreach (var item in items)
            //{
            //    bl.RemoveItem(item.ItemId);
            //}
            //items = bl.getAllItems();

            //print all Items in DBSet
            foreach (var item in items)
            {
                i++;
                Console.WriteLine("Number: " + i);
                Console.WriteLine(item.ItemId + " " + item.ItemName + " " + item.Description + " " + item.SerialKey + " " + item.Store_location + " " + item.Store_name + " " + item.Price);
            }
            Console.WriteLine("Done!!");

            //create and add all items to fireBase and DBset
            //FireBase fireBase = new FireBase(bl);

            Console.ReadLine();

        }
    }
}
