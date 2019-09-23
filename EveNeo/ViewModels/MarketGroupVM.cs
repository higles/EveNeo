using ESI.Models;
using System.Collections.Generic;

namespace EveNeo.ViewModels
{
    /// <summary>
    /// Market Group view model
    /// </summary>
    public class MarketGroupVM
    {
        /// <summary>
        /// Gets or sets the market group id
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the market group description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the market group name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the parent market group
        /// </summary>
        public MarketGroupVM ParentGroup { get; set; }

        /// <summary>
        /// Gets or sets the items
        /// </summary>
        public List<Item> Items { get; set; }
    }
}
