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
using RibbonApp.ViewModel;
using Fluent;
using RibbonApp.Pages;
using RibbonApp.Printing;
using System.IO;
using System.Xml.Linq;
using System.Diagnostics;

namespace RibbonApp
{
    /// <summary>
    /// Tato třída nedědí od Window, protože používá custom okno.
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow() 
        {            
             
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {          
            Configuration.Initialize();

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
            defaultPage = new DefaultPage(); // vytvoříme stránku, která se dosazuje do Frame v souboru MainWindow.xaml
            defaultPage.DataContext = viewModel; // pošleme do stránky data skrze ViewModel
            frDefult.Navigate(defaultPage); // musíme říci rámu, že se má přepnout na danou stránku

        }

        DefaultPage defaultPage; // hlavní stránka po zapnutní programu
        

        // Prozatím testovací metody - office-like grafické rozhraní podporuje klasické události:
        private void Button_Click(object sender, RoutedEventArgs e)
        {            
            frDefult.Navigate(defaultPage); // návrat na hlavní stránku

           
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Přepnutí na jinou stránku
            SecondPage secondPage = new SecondPage();
            frDefult.Navigate(secondPage);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close(); // zavře celý program
        }

        private void XMLButton_Click(object sender, RoutedEventArgs e)
        {
            EntityViewModel vm = defaultPage.DataContext as EntityViewModel;          
            ExportWindow ew = new ExportWindow();
            ew.Data = vm.Data.ToList();
            ew.ExportType = ExportType.XML;
            ew.ShowDialog();
        }

        private void HTMLButton_Click(object sender, RoutedEventArgs e)
        {
            EntityViewModel vm = defaultPage.DataContext as EntityViewModel;
            ExportWindow ew = new ExportWindow();
            ew.Data = vm.Data.ToList();
            ew.ExportType = ExportType.HTML;
            ew.ShowDialog();
        }

        private void openDatabaseFolder_Click(object sender, RoutedEventArgs e)
        {
            string databaseFileFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), RibbonApp.Database.Configuration.NameOfApplication);
            Process.Start(databaseFileFolder);
        }

        private void customersListBtn_Click(object sender, RoutedEventArgs e)
        {
            CustomersListPage customersListPage = new CustomersListPage();
            frDefult.Navigate(customersListPage);
        }

        private void customerAddBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
