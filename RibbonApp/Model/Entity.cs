using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RibbonApp.Model
{
    /// <summary>
    /// Jedná se o jeden záznam v databázi.
    /// </summary>
    [Serializable] // Serializable pouze kvůli zpětné kompatibilitě se složkou LegacyCode
    public class Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Databáze sama přiřadí Id
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Check { get; set; }
        public DateTime Date { get; set; }
    }
       
}
