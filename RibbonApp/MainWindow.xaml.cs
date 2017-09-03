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

namespace RibbonApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow() 
        {            
           // InitializeComponent(); 
          
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //// Vytvoření dat, která chceme uložit (mohou být libovolného typu)
            //Entity e1 = new Entity() { Id = 1, Name = "petrol engine" };
            //ConcreteEntity e2 = new ConcreteEntity() { Id = 2, Name = "jet engine", ConcreteInfo = "engine description" };

            //// Adresář, kam se ukládají soubory
            //string path = Environment.CurrentDirectory;

            //// Serializační třída starajícící se o serializaci dat do binární podoby
            //Serialization serializationClass = new Serialization(path);

            //// Uložení entity prvního typu
            //List<Entity> entitesToSave = new List<Entity>();
            //entitesToSave.Add(e1);
            //serializationClass.Save(entitesToSave);

            //// Uložení entity druhého typu
            //List<ConcreteEntity> concreteEntitesToSave = new List<ConcreteEntity>();
            //concreteEntitesToSave.Add(e2);
            //serializationClass.Save(concreteEntitesToSave);

            //// ViewModel je spojovací článek mezi grafickým rozhraním (.xaml soubory) a datovými modely (složka Models)
            //// View model může provádět dodatečné formátování dat (např čas, měna apod.)
            //EntityViewModel viewModel = new EntityViewModel();
            //viewModel.data1 = serializationClass.Load<Entity>(); // naplnění viewModelu daty
            //viewModel.data2 = serializationClass.Load<ConcreteEntity>(); // naplnění viewModelu daty

            //// Grafickému rozhraní pošleme ViewModel, z žádného jiného místa již do GUI data neposíláme
            //// Data, která zadá uživatel, se budou opět promítat jen do ViewModelu.  
            //defaultPage = new DefaultPage(); // vytvoříme stránku, která se dosazuje do Frame v souboru MainWindow.xaml
            //defaultPage.DataContext = viewModel; // pošleme do stránky data skrze ViewModel
            //frDefult.Navigate(defaultPage); // musíme říci rámu, co má zobrazit

            Configuration.Initialize();
            DatabaseContext database = Configuration.Database;


            // ViewModel je spojovací článek mezi grafickým rozhraním (.xaml soubory) a datovými modely (složka Models)
            // View model může provádět dodatečné formátování dat (např čas, měna apod.)
            EntityViewModel viewModel = new EntityViewModel(database.Entities.ToList(),
                (obj, arg) =>
            {
                //Configuration.DatabaseHelper.EditEntity(obj as EntityNotify);
                Configuration.DatabaseHelper.EditOnlySinglePropertyOfEntity(obj as EntityNotify, arg.PropertyName);
                MessageBox.Show("saved to Db");
            }
                );
                
                
                
                
           
            

            // Grafickému rozhraní pošleme ViewModel, z žádného jiného místa již do GUI data neposíláme
            // Data, která zadá uživatel, se budou opět promítat jen do ViewModelu.  
            defaultPage = new DefaultPage(); // vytvoříme stránku, která se dosazuje do Frame v souboru MainWindow.xaml
            defaultPage.DataContext = viewModel; // pošleme do stránky data skrze ViewModel
            frDefult.Navigate(defaultPage); // musíme říci rámu, co má zobrazit


        }

        DefaultPage defaultPage;
        

        private void Button_Click(object sender, RoutedEventArgs e)
        {            
             frDefult.Navigate(defaultPage);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           SecondPage secondPage = new SecondPage();
            frDefult.Navigate(secondPage);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
