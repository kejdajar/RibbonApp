using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using RibbonApp.Model;

namespace RibbonApp.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "database.sdf"))
        {

        }

        public virtual DbSet<Entity> Entities {get;set;}
       
    }
}
