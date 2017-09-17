using RibbonApp.Database;
using RibbonApp.Model;
using RibbonApp.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace RibbonApp.Pages
{
    /// <summary>
    /// Interaction logic for OrderListPage.xaml
    /// </summary>
    public partial class OrderListPage : Page
    {
        public OrderListPage()
        {
            InitializeComponent();
            
        }

       public void ReloadGrid()
        {
            var data = new ObservableCollection<Order>(Configuration.DatabaseHelper.GetAllOrders());
            dgOrders.ItemsSource = data;         
           
            ControlPanelGeneric<Order> genericContainer = new ControlPanelGeneric<Order>(controlPanel);
            genericContainer.DataToTransform = data;
            genericContainer.GetAllDataMethod += () => { return new ObservableCollection<Order>(Configuration.DatabaseHelper.GetAllOrders()); };
            genericContainer.SearchMethod += (dataGridDataSource, search) => { return new ObservableCollection<Order>(dataGridDataSource.Where(c => c.Comment.ToLower().Contains(search.ToLower()))); };
            genericContainer.SearchResultIsEmpty += () => { dgOrders.Visibility = Visibility.Hidden;  };
            genericContainer.SearchResultIsNotEmpty += () => {dgOrders.Visibility = Visibility.Visible; };
            genericContainer.OrderByCriteria = new List<string> {"Komentář","Id"};
            genericContainer.OrderByAlphabetical += (dataToSort, criterion) =>
            {
                if (criterion == "Komentář")
                {
                    return new ObservableCollection<Order>(dataToSort.OrderBy(o => o.Comment));
                }
                else if (criterion == "Id")
                {
                    return new ObservableCollection<Order>(dataToSort.OrderBy(o => o.Id));
                }
                else return dataToSort;
            };

            genericContainer.OrderByReverseAlphabetical += (dataToSort, criterion) =>
            {
                if (criterion == "Komentář")
                {
                    return new ObservableCollection<Order>(dataToSort.OrderByDescending(o => o.Comment));
                }
                else if (criterion == "Id")
                {
                    return new ObservableCollection<Order>(dataToSort.OrderByDescending(o => o.Id));
                }
                else return dataToSort;
            };



            genericContainer.Transform();

            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ReloadGrid();
            
        }

        private void dgOrders_GotFocus(object sender, RoutedEventArgs e)
        {
            RibbonApp.Printing.PrintHelper.DataToExport = ((DataGrid)sender).ItemsSource;
            RibbonApp.Printing.PrintHelper.ExportDataName = Printing.ExportDataName.CustomerOrdersDataGrid;
        }
    }
}
