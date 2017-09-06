using RibbonApp.Model;
using RibbonApp.Printing;
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
using System.Windows.Shapes;
using System.Xml.Linq;
using RibbonApp.Database;

namespace RibbonApp.Windows
{
    /// <summary>
    /// Interaction logic for ExportWindow.xaml
    /// </summary>
    public partial class EditCustomerWindow
    {
        public EditCustomerWindow()
        {           
            InitializeComponent();
        }

       public Customer Customer { get; set; } = null;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(Customer != null)
            {
                Customer c = Customer;

                tblockNameOfCustomer.Text = c.Name + " " + c.Surname;
                tbName.Text = c.Name;
                tbSurname.Text = c.Surname;
            }
        }

        private void btnEditCustomer_Click(object sender, RoutedEventArgs e)
        {
            Customer editedCustomerData = new Customer() {Id = Customer.Id, Name = tbName.Text, Surname = tbSurname.Text };
            Configuration.DatabaseHelper.EditCustomer(editedCustomerData);
            this.Close();

            // Možná lze aktualizaci GUI vyřešit lépe
            Configuration.MainWindow.customersListPage.ReloadDatagrid();
            Configuration.MainWindow.customersListPage.customerDetailsUserControl.Reload();
        }
    }

   
}
