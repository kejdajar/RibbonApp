﻿using System;
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
            // "this" představuje instanci této třídy - v jiných částech programu je potřeba reference na toto hlavní okno
            Configuration.Initialize(this);

            // zatím prototyp pro zadávání informací do tabulky
            defaultPage = new DefaultPage(); 
            frDefult.Navigate(defaultPage);
        }

        public DefaultPage defaultPage;  // hlavní stránka po zapnutní programu
        public CustomersListPage customersListPage; // stránka se seznamem zákazníků
        public OrderListPage orderListPage; // stránka se seznamem objednávek              

        private void btnMainPage_Click(object sender, RoutedEventArgs e)
        {
            frDefult.Navigate(defaultPage); // návrat na hlavní stránku           
        }

        private void btnShutDown_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // zavře celý program
        }        

        // XML export
        private void btnXML_Click(object sender, RoutedEventArgs e)
        {
            ExportWindow ew = new ExportWindow();
            ew.Owner = this;
            ew.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            ew.ExportType = ExportType.XML;
            ew.Show();
        }        

        // HTML export
        private void btnHtml_Click(object sender, RoutedEventArgs e)
        {
            ExportWindow ew = new ExportWindow();
            ew.Owner = this;
            ew.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            ew.ExportType = ExportType.HTML;
            ew.Show();
        }      

        // PDF export
        private void btnPDF_Click(object sender, RoutedEventArgs e)
        {
            ExportWindow ew = new ExportWindow();
            ew.Owner = this;
            ew.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            ew.ExportType = ExportType.PDF;
            ew.Show();
        }

        // Otevření složky s databází
        private void btnOpenDatabaseFolder_Click(object sender, RoutedEventArgs e)
        {
            string databaseFileFolder = Configuration.AppDataPath;
            Process.Start(databaseFileFolder);
        }      

        // Otevřít seznam zákazníků
        private void customersListBtn_Click(object sender, RoutedEventArgs e)
        {
            customersListPage = new CustomersListPage();
            frDefult.Navigate(customersListPage);
        }

        // Přidání nového zákazníka
        private void customerAddBtn_Click(object sender, RoutedEventArgs e)
        {
            NewCustomerWindow newCustomerWindow = new NewCustomerWindow();
            newCustomerWindow.Owner = Configuration.MainWindow;
            newCustomerWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            newCustomerWindow.ShowDialog();

            if (customersListPage != null) // okno nemuselo být ještě otevřeno
            customersListPage.ReloadDatagrid();
        }

        // Otevřít seznam objednávek
        private void orderListBtn_Click(object sender, RoutedEventArgs e)
        {
            orderListPage = new OrderListPage();
            frDefult.Navigate(orderListPage);
        }

        // Reset databáze
        private void btnResetDatabase_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Opravdu chcete smazat obsah databáze? Tento krok je nevratný. \n Aplikace bude automaticky restartována.", "Varování", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    string path = System.IO.Path.Combine(Configuration.AppDataPath, "database.sdf");
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                        Configuration.Initialize(this);

                        System.Windows.Forms.Application.Restart();
                        System.Windows.Application.Current.Shutdown();
                       
                    }
                }
                catch 
                {
                    MessageBox.Show("Databáze nemohla být resetována, protože je momentálně využívána jiným procesem.","Upozornění",MessageBoxButton.OK,MessageBoxImage.Information);
                }
            }
            else
            {
                return;
            }

        }
    }
}
