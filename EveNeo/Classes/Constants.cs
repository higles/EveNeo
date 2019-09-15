using EveNeo.ViewModels;
using System.Collections.Generic;

namespace EveNeo.Classes
{
    /// <summary>
    /// Constant values
    /// </summary>
    public static class Constants
    {
        public enum Categories
        {
            PlanetaryResources = 42,
            PlanetaryCommodities = 43
        }

        public enum Groups
        {
            BasicCommodities = 1042,
            RefinedCommodities = 1034,
            SpecializedCommodities = 1040,
            AdvancedCommodities = 1041,
            Planets = 7
        }

        public enum Systems
        {
            Jita = 30000142,
            Dodixie = 30002659,
            Amarr = 30002187,
        }

        public static List<TradeHub> TradeHubs = new List<TradeHub>()
        {
            new TradeHub()
            {
                SystemID = (int)Systems.Jita,
                ConstellationID = 20000020,
                RegionID = 10000002
            },
            new TradeHub()
            {
                SystemID = (int)Systems.Dodixie,
                ConstellationID = 20000389,
                RegionID = 10000032
            },
            new TradeHub()
            {
                SystemID = (int)Systems.Amarr,
                ConstellationID = 20000322,
                RegionID = 10000043
            }
        };
    }
}
