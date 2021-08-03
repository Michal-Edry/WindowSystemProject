using CustomersManagementDAL;
using CustomersManagementDP;
using CustomersManagementProjectML.Model;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Font = iTextSharp.text.Font;

namespace CustomersManagementBL
{
    public class BL_imp : IBL
    {
        public IDAL idal { get; set; }
        //private GoogleDriveAPIManager googleDriveAPI_manager;

        public BL_imp()
        {
            idal = new DAL_imp();
        }

        public void Init()
        {
            //googleDriveAPI_manager = new GoogleDriveAPIManager(this);
            //googleDriveAPI_manager.QuickStart();
        }

       
          
        public void AddItem(Item item)
        {
            idal.AddItem(item);
        }

        public List<string> getAllStoreNames()
        {
            List<Item> items = idal.getAllItems();
            return (from item in items
                              select item.Store_name).Distinct().ToList();
        }

        public List<Item> getRecommendationsForToday()
        {
            DayOfWeek today = DateTime.Now.DayOfWeek;
            List<Item> itemsRecommended = new List<Item>();
            List<Tuple<string, string>> tuples = getAllProductsTupleNameKey();
            List<DayOfWeek> days = new List<DayOfWeek>();
            days.Add(DayOfWeek.Sunday);
            days.Add(DayOfWeek.Monday);
            days.Add(DayOfWeek.Tuesday);
            days.Add(DayOfWeek.Wednesday);
            days.Add(DayOfWeek.Thursday);
            days.Add(DayOfWeek.Friday);
            days.Add(DayOfWeek.Saturday);
            foreach (var tuple in tuples)
            {
                int quantity = 0;
                int maxQuantity = 0;
                DayOfWeek dayOfWeek = new DayOfWeek();
                foreach (DayOfWeek day in days)
                {
                    List<Item> items = idal.getAllItems(y => y.Date_of_purchase.DayOfWeek == day && y.SerialKey == tuple.Item1);
                    foreach (var item in items)
                    {
                        quantity += item.Quantity;
                    }
                    if (quantity > maxQuantity)
                    {
                        maxQuantity = quantity;
                        dayOfWeek = day;
                    }
                 if (dayOfWeek == today && itemsRecommended.Find(x=>x.ItemName == tuple.Item2) == null)
                    {
                        itemsRecommended.Add(idal.getAllItems(x => x.SerialKey == tuple.Item1).FirstOrDefault());
                    }
                }
             
            }
             return itemsRecommended;
         
          }

        public void CreatePdfForDayRecomendations(string path)
        {
            List<Tuple<string, string>> tuples = getAllProductsTupleNameKey();
            List<DayOfWeek> days = new List<DayOfWeek>();
            days.Add(DayOfWeek.Sunday);
            days.Add(DayOfWeek.Monday);
            days.Add(DayOfWeek.Tuesday);
            days.Add(DayOfWeek.Wednesday);
            days.Add(DayOfWeek.Thursday);
            days.Add(DayOfWeek.Friday);
            days.Add(DayOfWeek.Saturday);

            PdfPTable table = new PdfPTable(3);

            Font x = FontFactory.GetFont(BaseFont.TIMES_ROMAN, 22, Font.BOLD);
            Font x3 = FontFactory.GetFont(BaseFont.TIMES_ROMAN, 24, Font.BOLD, BaseColor.ORANGE);
            Font x1 = FontFactory.GetFont(BaseFont.TIMES_ROMAN, 18, Font.BOLD);
            Font x2 = FontFactory.GetFont(BaseFont.TIMES_ROMAN, 14, Font.NORMAL);

            PdfPCell cell = new PdfPCell(new Phrase(@"", x1));
            cell.UseVariableBorders = true;
            cell.BorderColor = BaseColor.WHITE;
            cell.Colspan = 1;
            cell.HorizontalAlignment = 1; //0=Left, 1=Center, 2=Right
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase(@"Item Name", x1));
            cell.UseVariableBorders = true;
            cell.BorderColor = BaseColor.WHITE;
            cell.Colspan = 1;
            cell.HorizontalAlignment = 1; //0=Left, 1=Center, 2=Right
            table.AddCell(cell);


            cell = new PdfPCell(new Phrase(@"Best Day", x1));
            cell.UseVariableBorders = true;
            cell.BorderColor = BaseColor.WHITE;
            cell.Colspan = 1;
            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table.AddCell(cell);

            foreach (var tuple in tuples)
            {
                int quantity = 0;
                int maxQuantity = 0;
                DayOfWeek dayOfWeek = new DayOfWeek();
                foreach(DayOfWeek day in days)
                {
                    List<Item> items = idal.getAllItems(y => y.Date_of_purchase.DayOfWeek == day && y.SerialKey == tuple.Item1);
                    foreach(var item in items)
                    {
                        quantity += item.Quantity;
                    }
                    if (quantity > maxQuantity)
                    {
                        maxQuantity = quantity;
                        dayOfWeek = day;
                    }
                }
                
                string ProductImagePath = path + tuple.Item1 + ".jpg";
                if (!File.Exists(ProductImagePath))
                {
                    ProductImagePath = path + "nothing.jpg";
                }

                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(ProductImagePath);
                jpg.ScaleToFit(25f, 25f);
                PdfPCell imageCell = new PdfPCell(jpg);
                imageCell.Colspan = 1; // either 1 if you need to insert one cell
                imageCell.UseVariableBorders = true;
                imageCell.BorderColor = BaseColor.WHITE;
                imageCell.HorizontalAlignment = 1;
                table.AddCell(imageCell);

                
                 cell = new PdfPCell(new Phrase(@tuple.Item2, x2));
                cell.UseVariableBorders = true;
                cell.BorderColor = BaseColor.WHITE;
                cell.Colspan = 1;
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(@dayOfWeek.ToString(), x2));
                cell.UseVariableBorders = true;
                cell.BorderColor = BaseColor.WHITE;
                cell.Colspan = 1;
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell); 
                                 
            }
            Document doc = new Document(PageSize.A4, 7f, 5f, 5f, 0f);
            doc.AddTitle("Machine Learning results");
            PdfWriter.GetInstance(doc, new FileStream(AppDomain.CurrentDomain.BaseDirectory + "Recommended Days.pdf", FileMode.Create));
            doc.Open();
            //     Paragraph p1 = new Paragraph(text);
            //   doc.Add(p1);

            Paragraph c2 = new Paragraph("\nRecommendation on which day to buy each product\n\n", x);
            c2.Alignment = 1;
            doc.Add(c2);

            doc.Add(table);

            c2 = new Paragraph("\n\n\nThank you for using Shop Top!", x3);
            c2.Alignment = 1;
            doc.Add(c2);

            doc.Close();
        }

        public void CreatePdfForStoreRecomendations(string path)
        {
            string ProductImagePath;

            List<string> storeNames = getAllStoreNames();
            List<Tuple<string, string>> tuples = getAllProductsTupleNameKey();
            //string text = "";

            PdfPTable table = new PdfPTable(3);

            Font x = FontFactory.GetFont(BaseFont.TIMES_ROMAN, 22, Font.BOLD);
            Font x3 = FontFactory.GetFont(BaseFont.TIMES_ROMAN, 24, Font.BOLD, BaseColor.ORANGE);
            Font x1 = FontFactory.GetFont(BaseFont.TIMES_ROMAN, 18, Font.BOLD);
            Font x2 = FontFactory.GetFont(BaseFont.TIMES_ROMAN, 14, Font.NORMAL);


            PdfPCell cell = new PdfPCell(new Phrase(@"", x1));
            cell.UseVariableBorders = true;
            cell.BorderColor = BaseColor.WHITE;
            cell.Colspan = 1;
            cell.HorizontalAlignment = 1; //0=Left, 1=Center, 2=Right
            table.AddCell(cell);

            
            cell = new PdfPCell(new Phrase(@"Item Name", x1));
            cell.UseVariableBorders = true;
            cell.BorderColor = BaseColor.WHITE;
            cell.Colspan = 1;
            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table.AddCell(cell);
            

            cell = new PdfPCell(new Phrase(@"Store Name", x1));
            cell.UseVariableBorders = true;
            cell.BorderColor = BaseColor.WHITE;
            cell.Colspan = 1;
            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table.AddCell(cell);


            foreach (var tuple in tuples)
            {
                float bestScore = 0;
                string storeName = "";
                foreach (var store in storeNames)
                {
                    ModelInput sampleData = new ModelInput()
                    {
                        Store_name = store,
                        SerialKey = tuple.Item1,
                    };
                    var predictionResult = ConsumeModel.Predict(sampleData);
                    if (predictionResult.Score>=bestScore || bestScore==0)
                    {
                        bestScore = predictionResult.Score;
                        storeName = store;
                    }
                }

                ProductImagePath = path + tuple.Item1 + ".jpg";
                if (!File.Exists(ProductImagePath))
                {
                    ProductImagePath = path + "nothing.jpg";
                }
                
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(ProductImagePath);
                jpg.ScaleToFit(25f, 25f);
                PdfPCell imageCell = new PdfPCell(jpg);
                imageCell.Colspan = 1; // either 1 if you need to insert one cell
                imageCell.UseVariableBorders = true;
                imageCell.BorderColor = BaseColor.WHITE;
                imageCell.HorizontalAlignment = 1;
                table.AddCell(imageCell);

                cell = new PdfPCell(new Phrase(@tuple.Item2, x2));
                cell.UseVariableBorders = true;
                cell.BorderColor = BaseColor.WHITE;
                cell.Colspan = 1;
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(@storeName, x2));
                cell.UseVariableBorders = true;
                cell.BorderColor = BaseColor.WHITE;
                cell.Colspan = 1;
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);

            }
            

            Document doc = new Document(PageSize.A4, 7f, 5f, 5f, 0f);
            doc.AddTitle("Machine Learning results");
            PdfWriter.GetInstance(doc, new FileStream(AppDomain.CurrentDomain.BaseDirectory + "Recommended Stores.pdf", FileMode.Create));
            doc.Open();
            //     Paragraph p1 = new Paragraph(text);
            //   doc.Add(p1);


            Paragraph c2 = new Paragraph("\nRecommendation in which store to buy each product\n\n", x);
            c2.Alignment = 1;
            doc.Add(c2);

            doc.Add(table);

            c2 = new Paragraph("\n\n\nThank you for using Shop Top!", x3);
            c2.Alignment = 1;
            doc.Add(c2);

            doc.Close();

        }
       

        public List<Item> getAllItems(Func<Item, bool> pred = null)
        {
           return idal.getAllItems(pred);
        }

        public IEnumerable<IGrouping<DateTime,Item>> GetDateGroups()
        {
            return idal.getGroupByDate();
        }

        public IEnumerable<IGrouping<string, IGrouping<DateTime, Item>>> groupByDate()
        {
            return idal.groupByDate();
        }


        public List<Tuple<string, string>> getAllProductsTupleNameKey()
        {
            List<Tuple<string, string>> list = new List<Tuple<string, string>>();
            foreach (var grpItm in idal.getGroupBySerialKey())
            {
                list.Add(new Tuple<string, string>(grpItm.Key, grpItm.First().ItemName));
            }
            return list;
        }


        public void RemoveItem(int itemId)
        {
            idal.RemoveItem(itemId);
        }    

        public void UpdateItem(Item item)
        {
            idal.UpdateItem(item);
        }

    }
}
