using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RibbonApp.Model
{
    [Serializable]
    public class Entity
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
    }

    [Serializable]
    public class ConcreteEntity : Entity
    {
        public string ConcreteInfo { get; set; }
    }
}
