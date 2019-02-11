using EveNeo.Classes;
using EveNeo.Models;
using EveNeo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EveNeo.Controllers
{
    public class BaseController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public BaseController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        /// <summary>
        /// Get market data for the given items in the given region
        /// </summary>
        /// <param name="items">Items to get market data for</param>
        /// <param name="tradeHub">Trade hub to get market data</param>
        /// <returns>Collection of Item view models</returns>
        public async Task<List<ItemVM>> GetItemMarketData(List<Item> items, TradeHub tradeHub)
        {
            List<ItemVM> viewModels = new List<ItemVM>();
            var request = new HttpRequestMessage();
            var response = new HttpResponseMessage();

            foreach (var item in items)
            {
                int i = 1;
                List<Order> orders = new List<Order>();
                List<Order> responseOrders = new List<Order>();
                // get all orders
                do
                {
                    request = new HttpRequestMessage(HttpMethod.Get, "https://esi.evetech.net/latest/markets/" + tradeHub.RegionID + "/orders/?datasource=tranquility&order_type=sell&page=" + i + "&type_id=" + item.ID);
                    response = await MakeEsiRequest(request);

                    if (response.IsSuccessStatusCode)
                    {
                        responseOrders = await response.Content.ReadAsAsync<List<Order>>();
                        orders.AddRange(responseOrders);
                    }
                    else
                    {
                        throw new Exception($"Failed to get '{item.Name}' Orders for page '{i}'");
                    }

                    i++;
                } while (responseOrders.Count >= 1000);

                // add the viewmodel
                ItemVM itemVM = new ItemVM(item);
                itemVM.Price = Convert.ToDecimal(orders.Where(o => o.SystemID == tradeHub.SystemID).Min(o => o.Price));
                viewModels.Add(itemVM);
            }

            return viewModels;
        }

        /// <summary>
        /// Gets a response for the given request
        /// </summary>
        /// <param name="request">Request to make</param>
        /// <returns>An http response</returns>
        public async Task<HttpResponseMessage> MakeEsiRequest(HttpRequestMessage request)
        {
            var client = _clientFactory.CreateClient();

            return await client.SendAsync(request);
        }
    }
}
