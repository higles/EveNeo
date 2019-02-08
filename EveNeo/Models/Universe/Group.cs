using Newtonsoft.Json;
using System.Collections.Generic;

namespace EveNeo.Models
{
    /// <summary>
    /// Group class
    /// </summary>
    public class Group
    {
        /// <summary>
        /// Gets or sets the group ID
        /// </summary>
        [JsonProperty("group_id")]
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the category ID
        /// </summary>
        [JsonProperty("category_id")]
        public int CategoryID { get; set; }

        /// <summary>
        /// Gets or sets the group name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the group is published
        /// </summary>
        [JsonProperty("published")]
        public bool Published { get; set; }

        /// <summary>
        /// Gets or sets the types within this group
        /// </summary>
        [JsonProperty("types")]
        public List<int> Types { get; set; } = new List<int>();
    }
}
