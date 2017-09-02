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
    class EntityViewModel
    {
       
        public EntityViewModel(List<Entity> _data1, List<ConcreteEntity> _data2)
        {


            var transform = _data1.Select(d => new EntityNotify {
                Id = d.Id,
                Check = d.Check,
                Date = d.Date,
                Name = d.Name,
                EnableNotification = false
            }).ToList();
            foreach(var item in transform)
            {
                item.EnableNotification = true;
            }

            data1 = new ObservableCollection<EntityNotify>(transform);
          
        }

        public ObservableCollection<EntityNotify> data1 { get; set; }
        public ObservableCollection<ConcreteEntity> data2 { get; set; }

      

    }

    public class EntityNotify: INotifyPropertyChanged
    {
        public bool EnableNotification = false;

        private int _id { get; set; }
        private string _name { get; set; }
        private bool _check { get; set; }
        private DateTime _date { get; set; }

        public EntityNotify()
        {
            this.PropertyChanged = (obj, arg) =>
            {
                Configuration.DatabaseHelper.EditEntity(obj as EntityNotify);
            };
        }


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
                    RaisePropertyChanged("Id");

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
        private void RaisePropertyChanged(string property)
        {
            

            if (PropertyChanged != null && EnableNotification)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }

    public class ConcreteEntityNotify: INotifyPropertyChanged
    {
        private int _id { get; set; }
        private string _name { get; set; }
        private bool _check { get; set; }
        private DateTime _date { get; set; }
        private string _concreteInfo { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}