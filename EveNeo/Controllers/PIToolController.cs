using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ESI.Models;
using EveNeo.Classes;
using EveNeo.Data;
using EveNeo.Models;
using EveNeo.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EveNeo.Controllers
{
    public class PIToolController : BaseController
    {
        private readonly PIContext _context;

        public PIToolController(PIContext context, IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _context = context;
        }

        [Route("[controller]/{systemName}")]
        public async Task<IActionResult> Index(string systemName)
        {
            // Get items with prices
            Constants.Systems systemId;
            Enum.TryParse(systemName, out systemId);
            TradeHub tradeHub = Constants.TradeHubs.First(th => th.SystemID == (int)systemId);
            List<int> categoryIds = new List<int>() { (int)Constants.Categories.PlanetaryCommodities, (int)Constants.Categories.PlanetaryResources };
            List<Category> categories = _context.Categories.Where(c => categoryIds.Contains(c.ID)).ToList();
            List<Group> groups = _context.Groups.Where(g => categoryIds.Contains(g.CategoryID)).ToList();
            List<Item> items = _context.Items.Where(i => groups.Any(g => g.ID == i.GroupID)).ToList();

            List<ItemVM> itemVMs = await GetItemMarketData(items, tradeHub);
            foreach (var item in itemVMs)
            {
                var group = groups.First(g => g.ID == item.GroupID);
                var category = categories.First(c => c.ID == group.CategoryID);

                item.GroupName = group.Name;
                item.CategoryID = category.ID;
                item.CategoryName = category.Name;
            }

            List<SchematicVM> schematics = GetSchematicVMs(itemVMs).ToList();

            ViewBag.TradeHub = tradeHub;
            ViewBag.PlanetTypes = GetPlanetTypes(items).OrderBy(pt => pt.Name).ToList();
            ViewBag.RawMats = itemVMs.Where(i => groups.Any(g => g.CategoryID == (int)Constants.Categories.PlanetaryResources && g.ID == i.GroupID)).OrderBy(m => m.Name).ToList();
            return View(schematics.OrderBy(s => s.Output.Volume).ThenBy(s => s.Name).ToList());
        }

        /// <summary>
        /// Gets view models of all the PI schematics
        /// </summary>        
        public IEnumerable<SchematicVM> GetSchematicVMs(IEnumerable<ItemVM> items)
        {
            List<SchematicTypeMap> maps = _context.SchematicTypeMaps.ToList();
            List<Schematic> schematics = _context.Schematics.ToList();
            List<SchematicVM> schematicVMs = new List<SchematicVM>();
            
            foreach (var schematic in schematics)
            {
                SchematicVM schematicVM = new SchematicVM(schematic);
                var inputItems = items.Where(i => maps.Any(m => m.SchematicID == schematic.ID && m.IsInput && i.ID == m.TypeID)).ToList();
                Material outputItem = new Material(items.FirstOrDefault(i => maps.Any(m => m.SchematicID == schematic.ID && !m.IsInput && i.ID == m.TypeID)));
                outputItem.Quantity = maps.First(m => m.SchematicID == schematic.ID && !m.IsInput && m.TypeID == outputItem.ID).Quantity;

                schematicVM.Output = outputItem;
                foreach (var input in inputItems)
                {
                    var inputMat = new Material(input);
                    inputMat.Quantity = maps.First(m => m.SchematicID == schematic.ID && m.IsInput && m.TypeID == inputMat.ID).Quantity;
                    schematicVM.Inputs.Add(inputMat);
                }

                schematicVMs.Add(schematicVM);
            }

            return schematicVMs;
        }

        /// <summary>
        /// Gets view models for all planet types
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PlanetType> GetPlanetTypes(IEnumerable<Item> items)
        {
            var planetTypes = new List<PlanetType>();
            var planetItems = _context.Items.Where(i => i.GroupID == (int)Constants.Groups.Planets).ToList();
            var maps = _context.PlanetResourceMaps.ToList();

            foreach (var planet in planetItems)
            {
                PlanetType planetType = new PlanetType(planet);
                planetType.Resources = items.Where(i => maps.Any(m => m.PlanetTypeID == planet.ID && m.ResourceTypeID == i.ID)).OrderBy(r => r.Name).ToList();

                planetTypes.Add(planetType);
            }

            return planetTypes;
        }
    }
}