using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RibbonApp.Model
{
    [Serializable]
    public class Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Check { get; set; }
        public DateTime Date { get; set; }
    }

    [Serializable]
    public class ConcreteEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Check { get; set; }
        public DateTime Date { get; set; }
        public string ConcreteInfo { get; set; }
    }
}
