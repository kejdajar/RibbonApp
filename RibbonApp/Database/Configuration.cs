using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using RibbonApp.Model;
using System.IO;

namespace RibbonApp.Database
{
    /// <summary>
    /// Tato konfigurační třída má statické proměnné nebo třídy, které
    /// poskytují objekty potřebné pro celý program. Nikde jinde by 
    /// v programu neměly být statické třídy a proměnné, pouze zde.
    /// </summary>
    public static class Configuration
    {
        // Do složky s tímto názvem se bude ukládat Databázový soubor
        public static readonly string NameOfApplication = "RibbonApp";

        // Samotná databáze (Code-first třída, ze které se automatiky po spuštění vytvoří databáze)
        public static DatabaseContext Database { get; set; }
        
        // Pomocná třída pro ukládání a čtení dat z databáze
        // Veškerý přístup do databáze jen skrze tuto třídu!
        public static DatabaseHelper DatabaseHelper { get; set; } 

        /// <summary>
        /// Vytvoří instanci databáze pro celý program. Pokud databáze neexistuje, je vytvořena 
        /// nová databáze s testovacími daty.
        /// </summary>
        public static void Initialize()
        {
            Database = new DatabaseContext();
           
            if(!Database.Database.Exists())
            {
                Database.Database.Create();
                FillWithTestData();
            }
            
            // Pomocná třída pro CRUD operace
            DatabaseHelper = new DatabaseHelper(Database);
        }

        private static void CreateFolderForDatabase()
        {
            string databasePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); // %APPDATA%
            if(!Directory.Exists(databasePath))
            {
                Directory.CreateDirectory(System.IO.Path.Combine(databasePath, NameOfApplication));
            }
        }

        /// <summary>
        /// Naplní databázi testovacími daty.
        /// </summary>
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
