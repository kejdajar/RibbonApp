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
using RibbonApp.Windows;

namespace RibbonApp
{
    /// <summary>
    /// Tato třída nedědí od Window, protože používá custom okno.
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow() 
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
            // "this" představuje instanci této třídy - v jiných částích programu je potřeba reference na toto hlavní okno
            Configuration.Initialize(this);

            defaultPage = new DefaultPage();
        

            frDefult.Navigate(defaultPage); // musíme říci rámu, že se má přepnout na danou stránku

        }

        public DefaultPage defaultPage;  // hlavní stránka po zapnutní programu
        public CustomersListPage customersListPage; // stránka se seznamem zákazníků
        public OrderListPage orderListPage;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
            frDefult.Navigate(defaultPage); // návrat na hlavní stránku           
        }       

        private void BtnShutDown_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // zavře celý program
        }

        private void XMLButton_Click(object sender, RoutedEventArgs e)
        {            
            ExportWindow ew = new ExportWindow();
            ew.Owner = this;
            ew.WindowStartupLocation = WindowStartupLocation.CenterOwner;       
            ew.ExportType = ExportType.XML;
            ew.Show();
        }

        private void HTMLButton_Click(object sender, RoutedEventArgs e)
        {           
            ExportWindow ew = new ExportWindow();
            ew.Owner = this;
            ew.WindowStartupLocation = WindowStartupLocation.CenterOwner;           
            ew.ExportType = ExportType.HTML;
            ew.Show();
        }

        private void PDFButton_Click(object sender, RoutedEventArgs e)
        {           
            ExportWindow ew = new ExportWindow();
            ew.Owner = this;
            ew.WindowStartupLocation = WindowStartupLocation.CenterOwner;         
            ew.ExportType = ExportType.PDF;
            ew.Show();
        }

        private void OpenDatabaseFolder_Click(object sender, RoutedEventArgs e)
        {
            string databaseFileFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), RibbonApp.Database.Configuration.NameOfApplication);
            Process.Start(databaseFileFolder);
        }      

        private void CustomersListBtn_Click(object sender, RoutedEventArgs e)
        {
            customersListPage = new CustomersListPage();
            frDefult.Navigate(customersListPage);
        }

        // Přidání nového zákazníka
        private void CustomerAddBtn_Click(object sender, RoutedEventArgs e)
        {
            NewCustomerWindow newCustomerWindow = new NewCustomerWindow();
            newCustomerWindow.Owner = Configuration.MainWindow;
            newCustomerWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            newCustomerWindow.ShowDialog();

            if (customersListPage != null) // okno nemuselo být ještě otevřeno
            customersListPage.ReloadDatagrid();
        }

        private void OrderListBtn_Click(object sender, RoutedEventArgs e)
        {
            orderListPage = new OrderListPage();
            frDefult.Navigate(orderListPage);
        }

       
    }
}
