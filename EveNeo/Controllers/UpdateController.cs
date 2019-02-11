using EveNeo.Data;
using EveNeo.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EveNeo.Controllers
{
    public class UpdateController : BaseController
    {
        private readonly UpdateContext _context;

        public UpdateController(UpdateContext context, IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                //await UpdateCategories(_context);
            }
            catch(Exception ex)
            {
                ViewBag.Alert = "Error while updating categories";
                ViewBag.Message = ex.Message + "<br/>" + ex.InnerException;
                return View(false);
            }

            try
            {
                //await UpdateGroups(_context);
            }
            catch (Exception ex)
            {
                ViewBag.Alert = "Error while updating groups";
                ViewBag.Message = ex.Message;
                return View(false);
            }

            try
            {
                //await UpdateItems(_context);
            }
            catch (Exception ex)
            {
                ViewBag.Alert = "Error while updating items";
                ViewBag.Message = ex.Message;
                return View(false);
            }

            return View(true);
        }

        /// <summary>
        /// Updates categories in the database
        /// </summary>
        private async Task<int> UpdateCategories(UpdateContext context)
        {
            List<int> ids = new List<int>();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://esi.evetech.net/latest/universe/categories/?datasource=tranquility");
            var response = await MakeEsiRequest(request);

            // get all category ids
            if (response.IsSuccessStatusCode)
            {
                ids = await response.Content.ReadAsAsync<List<int>>();
            }
            else
            {
                throw new Exception("Failed to get Category IDs");
            }

            // remove categories not in ESI from Db
            var categories = context.Categories.Select(c => c.ID).ToList();
            var deleteList = categories.Where(c => !ids.Any(id => id == c)).ToList();

            foreach(var id in deleteList)
            {
                var category = await context.Categories.FindAsync(id);
                context.Categories.Remove(category);
            }
            await context.SaveChangesAsync();

            // update categories from ESI
            ids = ids.Where(id => !categories.Any(c => c == id)).ToList();
            int i = 0;
            foreach (var id in ids)
            {
                request = new HttpRequestMessage(HttpMethod.Get, "https://esi.evetech.net/latest/universe/categories/" + id + "/?datasource=tranquility&language=en-us");
                response = await MakeEsiRequest(request);

                if (response.IsSuccessStatusCode)
                {
                    Category esiCategory = await response.Content.ReadAsAsync<Category>();
                    Category dbCategory = await context.Categories.FindAsync(id);

                    if (dbCategory != null)
                    {
                        dbCategory.Name = esiCategory.Name;
                        dbCategory.Published = esiCategory.Published;
                    }
                    else
                    {
                        await context.Categories.AddAsync(esiCategory);
                    }
                }
                else
                {
                    throw new Exception("Failed to get Category '" + id + "'");
                }

                i++;

                // save every 50 so we don't overload the context
                if (i % 50 == 0)
                {
                    await context.SaveChangesAsync();
                }
            }

            return await context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates groups in the database
        /// </summary>
        private async Task<int> UpdateGroups(UpdateContext context)
        {
            List<int> responseIds = new List<int>();
            List<int> ids = new List<int>();
            int i = 1;
            var request = new HttpRequestMessage();
            var response = new HttpResponseMessage();

            // get all group ids
            do
            {
                request = new HttpRequestMessage(HttpMethod.Get, "https://esi.evetech.net/latest/universe/groups/?datasource=tranquility&page=" + i);
                response = await MakeEsiRequest(request);

                if (response.IsSuccessStatusCode)
                {
                    responseIds = await response.Content.ReadAsAsync<List<int>>();
                    ids.AddRange(responseIds);
                }
                else
                {
                    throw new Exception("Failed to get Group IDs for page '" + i + "'");
                }

                i++;
            } while (responseIds.Count != 0);

            // remove groups not in ESI from Db
            var groups = context.Groups.Select(g => g.ID).ToList();
            var deleteList = groups.Where(g => !ids.Any(id => id == g)).ToList();

            foreach (var id in deleteList)
            {
                var group = await context.Groups.FindAsync(id);
                context.Groups.Remove(group);
            }
            await context.SaveChangesAsync();

            // update groups from ESI
            ids = ids.Where(id => !groups.Any(g => g == id)).ToList();
            i = 0;
            foreach (var id in ids)
            {
                request = new HttpRequestMessage(HttpMethod.Get, "https://esi.evetech.net/latest/universe/groups/" + id + "/?datasource=tranquility&language=en-us");
                response = await MakeEsiRequest(request);

                if (response.IsSuccessStatusCode)
                {
                    Group esiGroup = await response.Content.ReadAsAsync<Group>();
                    Group dbGroup = await context.Groups.FindAsync(id);

                    if (dbGroup != null)
                    {
                        dbGroup.Name = esiGroup.Name;
                        dbGroup.Published = esiGroup.Published;
                        dbGroup.CategoryID = esiGroup.CategoryID;
                    }
                    else
                    {
                        await context.Groups.AddAsync(esiGroup);
                    }
                }
                else
                {
                    throw new Exception("Failed to get Group '" + id + "'");
                }

                i++;

                // save every 50 so we don't overload the context
                if (i % 50 == 0)
                {
                    await context.SaveChangesAsync();
                }
            }

            return await context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates items in the database
        /// </summary>
        private async Task<int> UpdateItems(UpdateContext context)
        {
            List<int> responseIds = new List<int>();
            List<int> ids = new List<int>();
            int i = 1;
            var request = new HttpRequestMessage();
            var response = new HttpResponseMessage();

            // get all item ids
            do
            {
                request = new HttpRequestMessage(HttpMethod.Get, "https://esi.evetech.net/latest/universe/types/?datasource=tranquility&page=" + i);
                response = await MakeEsiRequest(request);

                if (response.IsSuccessStatusCode)
                {
                    responseIds = await response.Content.ReadAsAsync<List<int>>();
                    ids.AddRange(responseIds);
                }
                else
                {
                    throw new Exception("Failed to get Item IDs for page '" + i + "'");
                }

                i++;
            } while (responseIds.Count != 0);

            // remove items not in ESI from Db
            var items = context.Items.Select(it => it.ID).ToList();
            var deleteList = items.Where(it => !ids.Any(id => id == it)).ToList();

            foreach (var id in deleteList)
            {
                var item = await context.Items.FindAsync(id);
                context.Items.Remove(item);
            }
            await context.SaveChangesAsync();

            // update items from ESI
            ids = ids.Where(id => !items.Any(it => it == id)).ToList();
            i = 0;
            foreach (var id in ids)
            {
                request = new HttpRequestMessage(HttpMethod.Get, "https://esi.evetech.net/latest/universe/types/" + id + "/?datasource=tranquility&language=en-us");
                response = await MakeEsiRequest(request);

                if (response.IsSuccessStatusCode)
                {
                    Item esiItem = await response.Content.ReadAsAsync<Item>();
                    Item dbItem = await context.Items.FindAsync(id);

                    if (dbItem != null)
                    {
                        dbItem.Name = esiItem.Name;
                        dbItem.Published = esiItem.Published;
                        dbItem.Capacity = esiItem.Capacity;
                        dbItem.Description = esiItem.Description;
                        dbItem.GraphicID = esiItem.GraphicID;
                        dbItem.GraphicID = esiItem.GroupID;
                        dbItem.IconID = esiItem.IconID;
                        dbItem.MarketGroupID = esiItem.MarketGroupID;
                        dbItem.Mass = esiItem.Mass;
                        dbItem.PackagedVolume = esiItem.PackagedVolume;
                        dbItem.PortionSize = esiItem.PortionSize;
                        dbItem.Radius = esiItem.Radius;
                        dbItem.Volume = esiItem.Volume;
                    }
                    else
                    {
                        await context.Items.AddAsync(esiItem);
                    }
                }
                else
                {
                    throw new Exception("Failed to get Item '" + id + "'");
                }

                i++;

                // save every 50 so we don't overload the context
                if (i % 50 == 0)
                {
                    await context.SaveChangesAsync();
                }
            }

            return await context.SaveChangesAsync();
        }
    }
}