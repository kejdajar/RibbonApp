﻿using System;
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
        public DatabaseContext() : base(System.IO.Path.Combine(Environment.CurrentDirectory,"database.sdf"))
        {

        }

        public virtual DbSet<Entity> Entities {get;set;}
        public virtual DbSet<ConcreteEntity> ConcreteEntities { get; set; }
    }
}