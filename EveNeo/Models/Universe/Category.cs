﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace EveNeo.Models
{
    /// <summary>
    /// Category class
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Gets or sets the category ID
        /// </summary>
        [JsonProperty("category_id")]
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the groups within this category
        /// </summary>
        [JsonProperty("groups")]
        public List<int> Groups { get; set; } = new List<int>();

        /// <summary>
        /// Gets or sets the category name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this category is published
        /// </summary>
        [JsonProperty("published")]
        public bool Published { get; set; }
    }
}