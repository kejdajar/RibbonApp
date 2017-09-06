using RibbonApp.Model;
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

namespace RibbonApp.Windows
{
    /// <summary>
    /// Interaction logic for ExportWindow.xaml
    /// </summary>
    public partial class NewCustomerWindow
    {
        public NewCustomerWindow()
        {           
            InitializeComponent();
        }

        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnNewCustomer_Click(object sender, RoutedEventArgs e)
        {
            Customer newCustomer = new Customer() { Name = tbName.Text, Surname = tbSurname.Text , Orders = new List<Order>() { } };
            RibbonApp.Database.Configuration.DatabaseHelper.AddNewCustomer(newCustomer);
            this.Close();
        }
    }

   
}
