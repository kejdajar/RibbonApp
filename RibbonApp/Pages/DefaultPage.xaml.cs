using RibbonApp.Database;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RibbonApp.Pages
{
    /// <summary>
    /// Interaction logic for DefaultPage.xaml
    /// </summary>
    public partial class DefaultPage : Page
    {
        public DefaultPage()
        {
            InitializeComponent();
        }

        private void grid1_InitializingNewItem(object sender, InitializingNewItemEventArgs e)
        {
            MessageBox.Show("initializing new item");
            EntityNotify itemCreatedByGrid = e.NewItem as EntityNotify;

            // Defaultní hodnoty pro nový řádek
            itemCreatedByGrid.Date = DateTime.Now;
            itemCreatedByGrid.Name = "Jméno";
            itemCreatedByGrid.Check = false;

            EntityNotify entityAlreadySavedInDb= Configuration.DatabaseHelper.AddNewEntity(itemCreatedByGrid);
            itemCreatedByGrid.Id = entityAlreadySavedInDb.Id; // Aktualizujeme Id dle databáze
        }




        //private void grid1_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    var dg = sender as DataGrid;
        //    if (dg == null) return;
        //    var index = dg.SelectedIndex;
        //    //here we get the actual row at selected index
        //    DataGridRow row = dg.ItemContainerGenerator.ContainerFromIndex(index) as DataGridRow;

        //    //here we get the actual data item behind the selected row
        //    var item = dg.ItemContainerGenerator.ItemFromContainer(row);

        //    EntityNotify entityEdited = item as EntityNotify;

        //    // Configuration.DatabaseHelper.EditEntity(entityEdited);
        //    MessageBox.Show(entityEdited.Name);

        //    MessageBox.Show("database write finished");
        //}
    }
}
