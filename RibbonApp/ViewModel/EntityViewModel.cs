using RibbonApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.ObjectModel;
using System.ComponentModel;
using RibbonApp.Database;

namespace RibbonApp.ViewModel
{
   
    /// Tato třída se stará o konverzi dat z datagridu do formátu, který přijme databáze.
    /// Datagridy používají speciální třídu pro data, která implementuje rozhraní INotifyPropertyChanged.
    /// Díky tomu se do třídy ihned promítnou změny, když někdo něco upraví v datagridu. Data v datgridu jsou v kolekci typu
    /// ObservableCollection, takže když někdo změní data v kódu, tak se to hned promítne do grafického rozhraní.
    class EntityViewModel
    {
       /// <summary>
       ///  Druhý parametr je metoda, která se volá pokaždé, když někdo upraví data v datagridu. Tato
       ///  metoda ví, jaký prvek a jaká jeho vlastnost byla změněna.
       /// </summary>
       /// <param name="_data1"></param>
       /// <param name="propertyChanged"></param>
        public EntityViewModel(List<Entity> _data1, PropertyChangedEventHandler propertyChanged)
        {          

            // přeměníme databázová data na jejich obdobu, které rozumí datagrid
            var transform = _data1.Select(d => new EntityNotify {
                Id = d.Id,
                Check = d.Check,
                Date = d.Date,
                Name = d.Name,         
                
            }).ToList(); // ! .ToList() okamžitě vykoná tento příkaz, nedochází k tzv. deffered execution

            // událost změny vlastnosti přiřadíme až zde,
            // jinak by se změny hlásily již při plnění dat výše
            foreach(var item in transform)
            {
                item.PropertyChanged += propertyChanged;
            }            

            // Touto kolekcí se plní datagrid
            Data = new ObservableCollection<EntityNotify>(transform);           

        }

        // Touto kolekcí se plní datagrid
        // ObservableCollection = datagrid reaguje automaticky na změnu dat ve třídě
        // EntityNotify = entita oznamuje automaticky, že v ní uživatel něco skrze datagrid změnil
        public ObservableCollection<EntityNotify> Data { get; set; }        

    }

    /// <summary>
    /// Pouze modifikovaná třída Entity, která umí hlásit změnu svých datových složek.
    /// </summary>
    public class EntityNotify: INotifyPropertyChanged
    {     
        private int _id { get; set; }
        private string _name { get; set; }
        private bool _check { get; set; }
        private DateTime _date { get; set; }       
        
        public int Id
        {
            get
            {
                return _id;
            }

            set
            {
                if (_id != value)
                {
                    _id = value;
                    RaisePropertyChanged("Id"); // pokud se hodnota změnila, vyvolá to upozornění na změnu hodnoty

                }
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                if (_name != value)
                {
                    _name = value;
                   
                    RaisePropertyChanged("Name");

                }
            }
        }

        public bool Check
        {
            get
            {
                return _check;
            }

            set
            {
                if (_check != value)
                {
                    _check = value;
                    RaisePropertyChanged("Check");

                }
            }
        }

        public DateTime Date
        {
            get
            {
                return _date;
            }

            set
            {
                if (_date != value)
                {
                    _date = value;
                    RaisePropertyChanged("Date");

                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        // Tato metoda spustí metodu, která hlásí, co se změnilo
        private void RaisePropertyChanged(string property)
        {   
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }

   
}