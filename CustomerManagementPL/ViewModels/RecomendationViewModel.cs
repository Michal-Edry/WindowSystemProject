﻿using CustomerManagementPL.Commands;
using CustomerManagementPL.Models;
using CustomersManagementDP;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

using System.IO;
using Accord.MachineLearning.Rules;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using System.Linq;
using System.Collections.Generic;

namespace CustomerManagementPL.ViewModels
{
    public class RecomendationViewModel
    {
        public CreatePDFCommand CreatePDFAR { get; set; }
        public CreatePDFCommand CreatePDFStores { get; set; }
        public CreatePDFCommand CreatePDFDays { get; set; }
        public string PopUpbarMessage { get; set; }
        public bool PopUpEnabled { get; set; }

        public SnackbarMessageQueue PDFMessageQueue { get; set; }

        public string today { get; set; }
        ItemsModel itemsModel = new ItemsModel();
        public ObservableCollection<RecommendedItemViewModel> ItemsVM { get; set; }

        public RecomendationViewModel()
        {
            today =  "Recommendations for today (" + DateTime.Now.DayOfWeek + "):";
            CreatePDFAR = new CreatePDFCommand();
            CreatePDFAR.GeneratePdfEvent += CreatePDFAssociationRules_function;

            CreatePDFStores = new CreatePDFCommand();
            CreatePDFDays = new CreatePDFCommand();
            CreatePDFStores.GeneratePdfEvent += CreatePDFStores_function;
            CreatePDFDays.GeneratePdfEvent += CreatePDFDays_function;
            generateCollectionFromModel();
            PopUpEnabled = false;
            PDFMessageQueue = new SnackbarMessageQueue();
        }

        public void CreatePDFStores_function()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            itemsModel.CreatePdfForStoreRecomendations(path);
            PDFMessageQueue.Enqueue("The recommendations saved as 'Recommended Stores.pdf'");
            PDFMessageQueue.Enqueue("path: " + AppDomain.CurrentDomain.BaseDirectory + "Recommended Stores.pdf");
        }

        public void CreatePDFDays_function()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            itemsModel.CreatePdfForDayRecomendations(path);
            PDFMessageQueue.Enqueue("The recommendations saved as 'Recommended Days.pdf'");
            PDFMessageQueue.Enqueue("path: " + AppDomain.CurrentDomain.BaseDirectory + "Recommended Days.pdf");
        }

        public void CreatePDFAssociationRules_function()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            itemsModel.CreatePdfForAssociationRules(path);
            PDFMessageQueue.Enqueue("The recommendations saved as 'Association Rules.pdf'");
            PDFMessageQueue.Enqueue("path: " + AppDomain.CurrentDomain.BaseDirectory + "Association Rules.pdf");
        }
        
        private void generateCollectionFromModel()
        {
            ItemsVM = new ObservableCollection<RecommendedItemViewModel>();
            foreach (var item in itemsModel.GetRecommendationsForToday()) 
            {
                ItemsVM.Add(new RecommendedItemViewModel(new Item(item), itemsModel));
            }
        }
    }
}
