﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EveNeo.Models
{
    /// <summary>
    /// System Jumps class
    /// </summary>
    public class SystemJumps
    {
        /// <summary>
        /// Gets or sets the number of ship jumps for the system
        /// </summary>
        [JsonProperty("ship_jumps")]
        public int ShipJumps { get; set; }

        /// <summary>
        /// Gets or sets the system id
        /// </summary>
        [JsonProperty("system_id")]
        public int SystemID { get; set; }
    }
}
