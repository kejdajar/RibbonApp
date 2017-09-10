using RibbonApp.Database;
using RibbonApp.Model;
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
            Refresh();
        }

        private void grid1_InitializingNewItem(object sender, InitializingNewItemEventArgs e)
        {
            MessageBox.Show("initializing new item");
            EntityNotify itemCreatedByGrid = e.NewItem as EntityNotify;

            // Defaultní hodnoty pro nový řádek
            itemCreatedByGrid.Date = DateTime.Now;
            itemCreatedByGrid.Name = "Jméno";
            itemCreatedByGrid.Check = false;
            itemCreatedByGrid.PropertyChanged += (obj,args) => { Configuration.DatabaseHelper.EditOnlySinglePropertyOfEntity(obj as EntityNotify, args.PropertyName); };

            EntityNotify entityAlreadySavedInDb= Configuration.DatabaseHelper.AddNewEntity(itemCreatedByGrid);
            itemCreatedByGrid.Id = entityAlreadySavedInDb.Id; // Aktualizujeme Id dle databáze
        }


        public void Refresh()
        {
            // Přístup do Db opět jen skrze DatabaseHelper třídu
            List<Entity> allEntitiesInDb = Configuration.DatabaseHelper.GetAllEntities();

            // ViewModel je spojovací článek mezi grafickým rozhraním (.xaml soubory) a datovými modely (složka Models)
            // View model může provádět dodatečné formátování dat (např čas, měna apod.)
            // Druhý parametr je funkce, která se provede pokaždé, když dojde ke změně alespoň jedné property itemu, který je v kolekci v datagridu.
            // obj .... objekt, který byl upraven, typ EntityNotify; arg ... string se jménem property, např "Name" nebo "Date"
            EntityViewModel viewModel = new EntityViewModel(allEntitiesInDb,
            (obj, arg) =>
            {
                // --- zastaralé ---
                // po změně jedné vlastnosti přepíše celý objekt znovu
                //Configuration.DatabaseHelper.EditEntity(obj as EntityNotify); 
                // ---zastaralé ---

                // lépe: po změně jedné vlastnosti změní pouze danou vlastnost a zbytek nepřepisuje zbytečně
                Configuration.DatabaseHelper.EditOnlySinglePropertyOfEntity(obj as EntityNotify, arg.PropertyName);
            });


            // Grafickému rozhraní pošleme ViewModel, z žádného jiného místa již do GUI data neposíláme
            // Data, která zadá uživatel, se budou opět promítat jen do ViewModelu.  
            //defaultPage = new DefaultPage(); // vytvoříme stránku, která se dosazuje do Frame v souboru MainWindow.xaml
           this.DataContext = viewModel; // pošleme do stránky data skrze ViewModel
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
