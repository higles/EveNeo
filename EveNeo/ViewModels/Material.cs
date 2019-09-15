using EveNeo.Models;

namespace EveNeo.ViewModels
{
    /// <summary>
    /// Industry Material class
    /// </summary>
    public class Material : ItemVM
    {
        /// <summary>
        /// Gets or sets the quantity
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Material"/> class.
        /// </summary>
        public Material()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Material"/> class.
        /// </summary>
        /// <param name="item">Item to clone</param>
        /// <param name="quantity">quatity of entity type</param>
        public Material(ItemVM item, int quantity = 0)
        {
            var props = typeof(ItemVM).GetProperties();
            foreach (var prop in props)
            {
                var value = prop.GetValue(item);
                typeof(Material).GetProperty(prop.Name).SetValue(this, value);
            }

            Quantity = quantity;
        }
    }
}
