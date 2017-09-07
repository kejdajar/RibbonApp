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
using System.Collections.ObjectModel;

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
            var data = new ObservableCollection<Customer>( Configuration.DatabaseHelper.GetAllCustomers());
            customersGrid.ItemsSource = data;           

            // Inicializace vyhledávače
            ControlPanel.DataToTransform = data;
            ControlPanel.GetAllDataMethod += () => { return new ObservableCollection<Customer>(Configuration.DatabaseHelper.GetAllCustomers()); };
            ControlPanel.SearchMethod += (dataGridDataSource, search) => { return new ObservableCollection<Customer>(dataGridDataSource.Where(c => c.Name.ToLower().Contains(search.ToLower()) || c.Surname.ToLower().Contains(search.ToLower())).ToList()); };
            ControlPanel.Transform();
                    
        }

        private void customersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            customerDetailsUserControl.Customer = (customersGrid.SelectedItem as Customer);
            customerDetailsUserControl.Reload();
        }

       
    }
}
