using RibbonApp.Database;
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
            dgOrders.ItemsSource = Configuration.DatabaseHelper.GetAllOrders();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ReloadGrid();
        }
    }
}
