﻿using CustomerManagementPL.ViewModels;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CustomerManagementPL.UserControls
{
    /// <summary>
    /// Interaction logic for RecomendationsUserControl.xaml
    /// </summary>
    public partial class RecomendationsUserControl : UserControl
    {
        public RecomendationViewModel currentVM = new RecomendationViewModel();
        public RecomendationsUserControl()
        {
           
            InitializeComponent();
            DataContext = currentVM;
         //   this.TodayLabel.Content = currentVM.today;
        }

        
    }
}

