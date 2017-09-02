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
    public class DatabaseHelper
    {
        public DatabaseHelper(DatabaseContext database)
        {
            this._database = database;
        }

        private DatabaseContext _database = null;

        public void EditEntity(EntityNotify entityToEdit)
        {
            // Převedení EntityNotify na Db Entitu
            Entity converted = new Entity() { Id = entityToEdit.Id, Check = entityToEdit.Check, Date = entityToEdit.Date, Name = entityToEdit.Name };

            Entity entityToEditFromDb = _database.Entities.Where(e => e.Id == converted.Id).Single();
            _database.Entry(entityToEditFromDb).CurrentValues.SetValues(converted);
            _database.SaveChanges();
        }
    }
}
