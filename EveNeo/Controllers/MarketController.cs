using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EveNeo.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EveNeo.Controllers
{
    public class MarketController : BaseController
    {
        public MarketController(IHttpClientFactory clientFactory) : base(clientFactory)
        {
        }

        public async Task<IActionResult> Index()
        {
            var marketGroupIds = await ESI.Market.GetItemGroupsAsync();
            var marketGroups = await ESI.Market.GetItemGroupInformationAsync(marketGroupIds.ToList());
            var itemIds = new List<int>();
            marketGroups.ToList().ForEach(mg => itemIds.AddRange(mg.Types));
            var items = await ESI.Universe.GetTypeInformationAsync(itemIds);
            var marketGroupVMs = marketGroups.Select(mg => new MarketGroupVM()
            {
                ID = mg.ID,
                Description = mg.Description,
                Name = mg.Name,
                Items = items.Where(i => mg.Types.Any(t => t == i.ID)).ToList()
            });
            return View();
        }
    }
}