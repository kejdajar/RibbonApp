using RibbonApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RibbonApp.ViewModel
{
    class EntityViewModel
    {
        public EntityViewModel()
        {
        }

        public List<Entity> data1 { get; set; }
        public List<ConcreteEntity> data2 { get; set; }

    }
}