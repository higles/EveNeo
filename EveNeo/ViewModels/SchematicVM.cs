using EveNeo.Models;
using System.Collections.Generic;

namespace EveNeo.ViewModels
{
    /// <summary>
    /// Schematic View Model
    /// </summary>
    public class SchematicVM : Schematic
    {
        /// <summary>
        /// Gets or sets the input materials
        /// </summary>
        public List<Material> Inputs { get; set; }

        /// <summary>
        /// Gets or sets the output type
        /// </summary>
        public Material Output { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchematicVM"/> class.
        /// </summary>
        public SchematicVM ()
        {
            Inputs = new List<Material>();
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SchematicVM"/> class.
        /// </summary>
        public SchematicVM(Schematic schematic)
        {
            ID = schematic.ID;
            Name = schematic.Name;
            CycleTime = schematic.CycleTime;
            Inputs = new List<Material>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchematicVM"/> class.
        /// </summary>
        public SchematicVM(Schematic schematic, Material output = null, List<Material> inputs = null)
        {
            ID = schematic.ID;
            Name = schematic.Name;
            CycleTime = schematic.CycleTime;
            Output = output;
            Inputs = inputs == null ? new List<Material>() : inputs ;
        }
    }
}
