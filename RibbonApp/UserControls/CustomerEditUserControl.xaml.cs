﻿using System;
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
using RibbonApp.Database;
using RibbonApp.Model;
using RibbonApp.Windows;

namespace RibbonApp.UserControls
{
    /// <summary>
    /// Interaction logic for CustomerEditUserControl.xaml
    /// </summary>
    public partial class CustomerEditUserControl : UserControl
    {
        public CustomerEditUserControl()
        {
            InitializeComponent();
        }

        public Customer Customer { get; set; } = null;

        private void Control_Loaded(object sender, RoutedEventArgs e)
        {

            if(Customer==null)
            {
                mainContainer.Visibility = Visibility.Hidden;
                noneUserContainer.Visibility = Visibility.Visible;
            }
            else
            {
              
                Reload();
            }
           
        }

       public void Reload()
        {
            if (Customer == null) return;


            mainContainer.Visibility = Visibility.Visible;
            noneUserContainer.Visibility = Visibility.Hidden;
            noneOrdersMessage.Visibility = Visibility.Hidden;

            Customer customer = Customer;

            tbNameOfCustomer.Text = customer.Name + " " + customer.Surname;

            tbId.Text = customer.Id.ToString();
            tbName.Text = customer.Name;
            tbSurname.Text = customer.Surname;

            if(customer.Orders != null && customer.Orders.Count>0)
            {
                dgCustomerOrders.Visibility = Visibility.Visible;
                dgCustomerOrders.ItemsSource = customer.Orders;
            }
            else
            {
                dgCustomerOrders.Visibility = Visibility.Hidden;
                noneOrdersMessage.Visibility = Visibility.Visible;

            }
            
        }

        private void btnCustomerEdit_Click(object sender, RoutedEventArgs e)
        {
            EditCustomerWindow editCustomerWindow = new EditCustomerWindow();
            editCustomerWindow.Customer = Customer;
            editCustomerWindow.Owner = RibbonApp.Database.Configuration.MainWindow;
            editCustomerWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            editCustomerWindow.ShowDialog();
        }
    }
}
