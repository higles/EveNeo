using ESI.Models;
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
        public async Task<List<ItemVM>> GetItemMarketData(IEnumerable<Item> items, TradeHub tradeHub)
        {
            List<ItemVM> viewModels = new List<ItemVM>();
            //var request = new HttpRequestMessage();
            //var response = new HttpResponseMessage();

            int i = 1;
            List<Order> orders = new List<Order>();
            List<Order> responseOrders = new List<Order>();
            // get all orders
            do
            {
                List<string> urls = new List<string>();
                foreach (var item in items)
                {
                    urls.Add("https://esi.evetech.net/v1/markets/" + tradeHub.RegionID + "/orders/?datasource=tranquility&order_type=sell&page=" + i + "&type_id=" + item.ID);
                }

                var responses = await MakeEsiRequest(urls);
                //response = await MakeEsiRequest(request);

                foreach (var response in responses)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        responseOrders = await response.Content.ReadAsAsync<List<Order>>();
                        orders.AddRange(responseOrders);
                    }
                    else
                    {
                        throw new Exception($"Failed to get all Orders for page '{i}'");
                    }
                }                

                i++;
            } while (responseOrders.Count >= 1000);

            // add the viewmodel
            foreach (var item in items)
            {
                ItemVM itemVM = new ItemVM(item);
                var itemOrders = orders.Where(o => o.SystemID == tradeHub.SystemID && o.TypeID == item.ID).ToList();
                itemVM.Price = itemOrders.Count > 0 ? Convert.ToDecimal(itemOrders.Min(o => o.Price)) : 0;
                viewModels.Add(itemVM);
            }

            return viewModels;
        }

        /// <summary>
        /// Gets a response for the given request
        /// </summary>
        /// <param name="request">Request to make</param>
        /// <returns>An http response</returns>
        public async Task<IEnumerable<HttpResponseMessage>> MakeEsiRequest(IEnumerable<string> urls)
        {
            var client = new HttpClient();

            // Start requests for the given urls
            var requests = urls.Select(url => client.GetAsync(url)).ToList();

            // Wait for all the requests to finish
            await Task.WhenAll(requests);

            // Get the responses
            var responses = requests.Select(task => task.Result).ToList();

            return responses;
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
