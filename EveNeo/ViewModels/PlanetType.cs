using EveNeo.Models;
using System.Collections.Generic;

namespace EveNeo.ViewModels
{
    public class PlanetType : Item
    {
        public IEnumerable<Item> Resources { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlanetType"/> class
        /// </summary>
        public PlanetType()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlanetType"/> class
        /// </summary>
        /// <param name="item">Item to clone</param>
        public PlanetType(Item item)
        {
            var props = typeof(Item).GetProperties();
            foreach (var prop in props)
            {
                var value = prop.GetValue(item);
                typeof(PlanetType).GetProperty(prop.Name).SetValue(this, value);
            }
        }
    }
}
