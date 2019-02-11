using EveNeo.Models;

namespace EveNeo.ViewModels
{
    public class ItemVM : Item
    {
        /// <summary>
        /// Gets or sets the group name
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Gets or sets the category id
        /// </summary>
        public int CategoryID { get; set; }

        /// <summary>
        /// Gets or sets the category name
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// Gets or sets the price of the item
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemVM"/> class
        /// </summary>
        public ItemVM ()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemVM"/> class
        /// </summary>
        /// <param name="item">Item to clone</param>
        public ItemVM (Item item)
        {
            var props = typeof(Item).GetProperties();
            foreach (var prop in props)
            {
                var value = prop.GetValue(item);
                typeof(ItemVM).GetProperty(prop.Name).SetValue(this, value);
            }
        }
    }
}
