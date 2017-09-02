using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using RibbonApp.Model;

namespace RibbonApp.Database
{
    public static class Configuration
    {

        public static DatabaseContext Database { get; set; }
        public static DatabaseHelper DatabaseHelper { get; set; } 

        public static void Initialize()
        {
            Database = new DatabaseContext();
            //Database.Database.CreateIfNotExists();
            if(!Database.Database.Exists())
            {
                Database.Database.Create();
                FillWithTestData();
            }

            DatabaseHelper = new DatabaseHelper(Database);
        }

        private static void FillWithTestData()
        {
            Database.Entities.Add(new Entity() { Name = "test", Date = DateTime.Now, Check = false });
            Database.ConcreteEntities.Add(new ConcreteEntity() { Name = "test", Date = DateTime.Now, Check = false, ConcreteInfo = "some info" });
            Database.SaveChanges();
        }
    }
}
