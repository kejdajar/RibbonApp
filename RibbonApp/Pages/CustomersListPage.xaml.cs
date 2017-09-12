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
using RibbonApp.UserControls;

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


        public ControlPanelGeneric<Customer> genericContainer;
        public void ReloadDatagrid()
        {
            // Inicializace vyhledávače
            //ControlPanel.DataToTransform = data;
            //ControlPanel.GetAllDataMethod += () => { return new ObservableCollection<Customer>(Configuration.DatabaseHelper.GetAllCustomers()); };
            //ControlPanel.SearchMethod += (dataGridDataSource, search) => { return new ObservableCollection<Customer>(dataGridDataSource.Where(c => c.Name.ToLower().Contains(search.ToLower()) || c.Surname.ToLower().Contains(search.ToLower())).ToList()); };
            //ControlPanel.SearchResultIsEmpty += () => { customersGrid.Visibility = Visibility.Hidden; tblockEmptySearchResult.Visibility = Visibility.Visible; };
            //ControlPanel.SearchResultIsNotEmpty += () => { customersGrid.Visibility = Visibility.Visible; tblockEmptySearchResult.Visibility = Visibility.Hidden; };
            //ControlPanel.Transform();

            var data = new ObservableCollection<Customer>( Configuration.DatabaseHelper.GetAllCustomers());
            customersGrid.ItemsSource = data;
            genericContainer = new ControlPanelGeneric<Customer>(ControlPanel);
            genericContainer.DataToTransform = data;
            genericContainer.GetAllDataMethod += () => { return new ObservableCollection<Customer>(Configuration.DatabaseHelper.GetAllCustomers()); };

            //  genericContainer.SearchMethod += (dataGridDataSource, search) => { return new ObservableCollection<Customer>(dataGridDataSource.Where(c =>  (c.Name.ToLower() + " " + c.Surname.ToLower()).Contains(search.ToLower())).ToList());  };
            
            genericContainer.SearchMethod += (dataGridDataSource, search) => { return new ObservableCollection<Customer>(dataGridDataSource.Where(c => {

                string customerData = c.Name.ToLower() + " " + c.Surname.ToLower();
               
                    List<string> keywords = search.Split(' ').ToList();
                    foreach (string keyword in keywords)
                    {
                        if (customerData.Contains(keyword.ToLower().Trim()))
                        {
                            return true;
                        }

                    }
                            

                return false;


            }));  };

            genericContainer.SearchResultIsEmpty += () => { customersGrid.Visibility = Visibility.Hidden; tblockEmptySearchResult.Visibility = Visibility.Visible; };
            genericContainer.SearchResultIsNotEmpty += () => { customersGrid.Visibility = Visibility.Visible; tblockEmptySearchResult.Visibility = Visibility.Hidden; };

            genericContainer.OrderByCriteria = new List<string>() { "Id","Jméno", "Příjmení" };

            genericContainer.OrderByAlphabetical += (dataToSort, criterion) => 
            {
               
                if (criterion == "Jméno")
                {
                    return new ObservableCollection<Customer>(dataToSort.OrderBy(c => c.Name));
                }
                else if (criterion == "Příjmení")
                {
                    return new ObservableCollection<Customer>(dataToSort.OrderBy(c => c.Surname));
                }
                else if (criterion == "Id")
                {
                    return new ObservableCollection<Customer>(dataToSort.OrderBy(c => c.Id));
                }
                else return dataToSort;
              };

            genericContainer.OrderByReverseAlphabetical += (dataToSort, criterion) =>
            {
               
                if (criterion == "Jméno")
                {
                    return new ObservableCollection<Customer>(dataToSort.OrderByDescending(c => c.Name));
                }
                else if (criterion == "Příjmení")
                {
                    return new ObservableCollection<Customer>(dataToSort.OrderByDescending(c => c.Surname));
                }
                else if (criterion == "Id")
                {
                    return new ObservableCollection<Customer>(dataToSort.OrderByDescending(c => c.Id));
                }
                else return dataToSort;
            };  

            genericContainer.Transform();

        }

        private void customersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(customersGrid.SelectedItem != null)
            {
                customerDetailsUserControl.Customer = (customersGrid.SelectedItem as Customer);
                customerDetailsUserControl.Reload();
            }
            
        }

        private void customersGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            RibbonApp.Printing.PrintHelper.DataToExport = ((DataGrid)sender).ItemsSource;
            RibbonApp.Printing.PrintHelper.ExportDataName = "CustomersDataGrid";
        }

       
    }
}
