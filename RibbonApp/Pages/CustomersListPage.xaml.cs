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
using RibbonApp.Database;
using RibbonApp.Model;

namespace RibbonApp.Pages
{
    /// <summary>
    /// Interaction logic for CustomersListPage.xaml
    /// </summary>
    public partial class CustomersListPage : Page
    {
        public CustomersListPage()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
            ReloadDatagrid();
        }

        public void ReloadDatagrid()
        {
            customersGrid.ItemsSource = Configuration.DatabaseHelper.GetAllCustomers();
            
        }

        private void customersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            customerDetailsUserControl.CustomerId = (customersGrid.SelectedItem as Customer).Id;
            customerDetailsUserControl.Reload();
        }
    }
}
