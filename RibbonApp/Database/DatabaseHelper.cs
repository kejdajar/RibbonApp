using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RibbonApp.Database;
using RibbonApp.Model;
using RibbonApp.ViewModel;

namespace RibbonApp.Database
{
    /// <summary>
    /// Přes tuto třídu se provádí 100% všech operací s databází. (Díky tomu lze později pouze prohodit
    /// databázový zdroj za něco jiného)
    /// </summary>
    public class DatabaseHelper
    {
        public DatabaseHelper(DatabaseContext database)
        {
            this._database = database;
        }

        private DatabaseContext _database = null;

        /// <summary>
        /// Vrátí list všech Entit v DB, jinak vrací Null.
        /// </summary>       
        public List<Entity> GetAllEntities()
        {
            if (_database.Entities.Any())
            {
                return _database.Entities.ToList();
            }
            else return null;
        }


        public void EditEntity(EntityNotify entityToEdit)
        {
            // Převedení EntityNotify na Db Entitu
            Entity converted = new Entity() { Id = entityToEdit.Id, Check = entityToEdit.Check, Date = entityToEdit.Date, Name = entityToEdit.Name };

            Entity entityToEditFromDb = _database.Entities.Where(e => e.Id == converted.Id).Single();
            _database.Entry(entityToEditFromDb).CurrentValues.SetValues(converted);
            _database.SaveChanges();
        }

        /// <summary>
        /// Podívá se do prvního parametru (entityThatWasEdited) a poté veme hodnotu vlastnosti (changedPropertyName)
        /// a zrcadlí jí do databáze. Ostatní datové složky jsou zachovány.
        /// </summary>
        /// <param name="entityThatWasEdited"></param>
        /// <param name="changedPropertyName"></param>
        public void EditOnlySinglePropertyOfEntity(EntityNotify entityThatWasEdited, string changedPropertyName)
        {
            int propertyBeingEditedId = entityThatWasEdited.Id;
            Entity entityToEditFromDb = _database.Entities.Where(e => e.Id == propertyBeingEditedId).Single();

            object editedPropertyValue = GetPropValue(entityThatWasEdited, changedPropertyName);
            SetProperty(entityToEditFromDb, changedPropertyName, editedPropertyValue);
            _database.SaveChanges();
            System.Diagnostics.Debug.WriteLine(changedPropertyName + "saved!!!");
        }

        // Pomocná metoda
        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        // Pomocná metoda
        public void SetProperty(object source,String propertyName, object value)
        {
            source.GetType().GetProperty(propertyName).SetValue(source, value);
        }

        /// <summary>
        /// Přidá do databáze novou Entitu a nechá databázi, ať jí vygeneruje její unikátní Id.
        /// Metoda vrací poté tuto entitu zpět, i s daným ID.
        /// </summary>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        public EntityNotify AddNewEntity(EntityNotify newEntity)
        {
            Entity converted = new Entity()
            {               
                Check = newEntity.Check,
                Date = newEntity.Date,
                Name = newEntity.Name
            };

            _database.Entities.Add(converted);
            _database.SaveChanges();

            Entity entityWithDbGeneratedId =  _database.Entities.Where(e => e.Id == converted.Id).Single();
            return new EntityNotify
            {
                Id = entityWithDbGeneratedId.Id,
                Check = entityWithDbGeneratedId.Check,
                Date = entityWithDbGeneratedId.Date,
                Name = entityWithDbGeneratedId.Name,

            };
        }

        // Customers
        public List<Customer> GetAllCustomers()
        {
            if (_database.Customers.Any())
            {
                return _database.Customers.ToList();
            }
            else return null;
        }

        public Customer GetCustomer(int Id)
        {
            if (_database.Customers.Any())
            {
                return _database.Customers.Where(c => c.Id == Id).Single();
            }
            else return null;
        }

        public void AddNewCustomer(Customer newCustomer)
        {
            _database.Customers.Add(newCustomer);
            _database.SaveChanges();
        }

        public void DeleteCustomer(int customerId)
        {
            Customer customerToBeRemoved = _database.Customers.Where(c => c.Id == customerId).Single();
            _database.Customers.Remove(customerToBeRemoved);
            _database.SaveChanges();
        }

        //public void EditCustomer(int customerId,Customer detachedCustomer)
        //{
        //    Customer customerToBeEditedFromDb = _database.Customers.Where(c=>c.Id == customerId).Single();
        //    customerToBeEditedFromDb.Name = detachedCustomer.Name;
        //    customerToBeEditedFromDb.Surname = detachedCustomer.Surname;
        //    _database.SaveChanges();
        //}

        public void EditCustomer(Customer customerWithDbId)
        {
            Customer customerToBeEditedFromDb = _database.Customers.Where(c => c.Id == customerWithDbId.Id).Single();
            customerToBeEditedFromDb.Name = customerWithDbId.Name;
            customerToBeEditedFromDb.Surname = customerWithDbId.Surname;
            _database.SaveChanges();
        }

        // Orders
        public List<Order> GetAllOrders()
        {
            if (_database.Orders.Any())
            {
                return _database.Orders.ToList();
            }
            else return null;
        }
    }
}
