﻿using CustomersManagementDP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomersManagementBL
{
    public interface IBL
    {
        void AddItemFB(string path);

        void AddItem(Item item);

        void RemoveItem(int itemId);

        void UpdateItem(Item item);

        List<Item> getAllItems(Func<Item, bool> pred = null);

        IEnumerable<IGrouping<DateTime, Item>> GetDateGroups();
        IEnumerable<IGrouping<string, IGrouping<DateTime, Item>>> groupByDate();

        List<Tuple<string, string>> getAllProductsTupleNameKey(); // Returns tuples <SerialKey, ItemName>

        List<string> getAllStoreNames();

        void CreatePdfForStoreRecomendations(string path);

        void CreatePdfForDayRecomendations(string path);

        void CreatePdfForAssociationRules(string path);

        List<Item> getRecommendationsForToday();


    }
}
