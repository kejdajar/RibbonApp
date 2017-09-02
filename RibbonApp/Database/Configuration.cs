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
            Database.Entities.Add(new Entity() { Name = "Motor 01", Date = DateTime.Now, Check = false });
            Database.Entities.Add(new Entity() { Name = "Palivový článek 02", Date = DateTime.Now + TimeSpan.FromHours(1), Check = true });
            Database.Entities.Add(new Entity() { Name = "Karburátor 03", Date = DateTime.Now +  TimeSpan.FromHours(2), Check = false });
            Database.Entities.Add(new Entity() { Name = "Parní turbína", Date = DateTime.Now + TimeSpan.FromHours(3), Check = true });
            Database.SaveChanges();
        }
    }
}
