﻿using System;
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

        public void EditOnlySinglePropertyOfEntity(EntityNotify entityThatWasEdited, string changedPropertyName)
        {
            int propertyBeingEditedId = entityThatWasEdited.Id;
            Entity entityToEditFromDb = _database.Entities.Where(e => e.Id == propertyBeingEditedId).Single();

            object editedPropertyValue = GetPropValue(entityThatWasEdited, changedPropertyName);
            SetProperty(entityToEditFromDb, changedPropertyName, editedPropertyValue);
            _database.SaveChanges();
            System.Diagnostics.Debug.WriteLine(changedPropertyName + "saved!!!");
        }

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        public void SetProperty(object source,String propertyName, object value)
        {
            source.GetType().GetProperty(propertyName).SetValue(source, value);
        }

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




    }
}
