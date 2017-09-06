﻿using RibbonApp.Model;
using RibbonApp.Printing;
using RibbonApp.ViewModel;
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
using System.Windows.Shapes;
using System.Xml.Linq;
using RibbonApp.Database;

namespace RibbonApp.Windows
{
    /// <summary>
    /// Interaction logic for ExportWindow.xaml
    /// </summary>
    public partial class EditCustomerWindow
    {
        public EditCustomerWindow()
        {           
            InitializeComponent();
        }

       public int? CustomerId { get; set; } = null;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(CustomerId != null)
            {
                Customer c = Configuration.DatabaseHelper.GetCustomer(CustomerId ?? default(int));

                tblockNameOfCustomer.Text = c.Name + " " + c.Surname;
                tbName.Text = c.Name;
                tbSurname.Text = c.Surname;
            }
        }

        private void btnEditCustomer_Click(object sender, RoutedEventArgs e)
        {
            Customer editedCustomerData = new Customer() { Name = tbName.Text, Surname = tbSurname.Text };
            Configuration.DatabaseHelper.EditCustomer(CustomerId ?? default(int), editedCustomerData);
            this.Close();
        }
    }

   
}
