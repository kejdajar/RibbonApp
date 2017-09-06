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
            if (string.IsNullOrWhiteSpace(tbSearch.Text))
            {
              customersGrid.ItemsSource = Configuration.DatabaseHelper.GetAllCustomers();    
            }
            else
            {
                Search();
            }
                    
        }

        private void customersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            customerDetailsUserControl.Customer = (customersGrid.SelectedItem as Customer);
            customerDetailsUserControl.Reload();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

            Search();
           
        }

        private void Search()
        {
            string search = tbSearch.Text;
            if (!string.IsNullOrWhiteSpace(search))
            {
                List<Customer> dataGridDataSource = Configuration.DatabaseHelper.GetAllCustomers();
                List<Customer> searchResult = dataGridDataSource.Where(c => c.Name.ToLower().Contains(search.ToLower()) || c.Surname.ToLower().Contains(search.ToLower())).ToList();
                customersGrid.ItemsSource = searchResult;
            }
            else
            {
                customersGrid.ItemsSource = Configuration.DatabaseHelper.GetAllCustomers();
            }
        }

        private void tbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key== Key.Enter)
            {
                Search();
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            tbSearch.Text = string.Empty;
            Search();
        }
    }
}
