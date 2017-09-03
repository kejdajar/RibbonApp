using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using RibbonApp.Model;



namespace RibbonApp.Database
{
    ///  Následující princip je tzv. CODE FIRST DATABASE APPROACH - SQL je vygenerováno automaticky
    ///  Jedná se o třídu, která byla ručně napsána a dědí od DbContext, což je
    ///  třída Entity Frameworku. Projekt používá Entity Framework 6.1.3. Entity framework
    ///  na základě této třídy sám vygeneruje databázi = database.sdf. Momentálně je databáze
    ///  uložena v %APPDATA%, protože po instalaci programu do Program Files nemá uživatel nikdy
    ///  defaultně práva pro zápis do Program Files, takže se data ukládají jinam.
    public class DatabaseContext : DbContext
    {
        // Databáze bude vytvořena v C:\Users\user_name\Data\Roaming\name_of_application\database.sdf
        public DatabaseContext() : base(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), RibbonApp.Database.Configuration.NameOfApplication,"database.sdf"))
        {

        }

        // Toto představuje jednu tabulku v databázi
        // Musí to být vždy public a virtual
        public virtual DbSet<Entity> Entities {get;set;}
       
    }
}
