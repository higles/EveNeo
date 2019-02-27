using ESI;
using ESI.Models;
using EveNeo.Data;
using Microsoft.AspNetCore.Mvc;
using System;
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
            var esiCategories = await Universe.GetItemCategoryInformationAsync();

            // remove categories not in ESI from Db
            var deleteList = context.Categories.Where(c => !esiCategories.Any(esi => esi.ID == c.ID));
            context.Categories.RemoveRange(deleteList);
            await context.SaveChangesAsync();

            // update categories from ESI
            int i = 0;
            foreach (var category in esiCategories)
            {
                Category dbCategory = await context.Categories.FindAsync(category.ID);

                // if we find the given id in the database then update else insert
                if (dbCategory != null)
                {
                    dbCategory.Name = category.Name;
                    dbCategory.Published = category.Published;
                }
                else
                {
                    await context.Categories.AddAsync(category);
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
            // get all groups
            var esiGroups = await Universe.GetItemGroupInformationAsync();

            // remove groups not in ESI from Db
            var deleteList = context.Groups.Where(g => !esiGroups.Any(esi => esi.ID == g.ID));
            context.Groups.RemoveRange(deleteList);
            await context.SaveChangesAsync();

            // update groups from ESI
            int i = 0;
            foreach (var group in esiGroups)
            {
                Group dbGroup = await context.Groups.FindAsync(group.ID);

                // if we find the given id in the database then update else insert
                if (dbGroup != null)
                {
                    dbGroup.Name = group.Name;
                    dbGroup.Published = group.Published;
                    dbGroup.CategoryID = group.CategoryID;
                }
                else
                {
                    await context.Groups.AddAsync(group);
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
            // get all items
            var esiItems = await Universe.GetTypeInformationAsync();

            // remove items not in ESI from Db
            var deleteList = context.Items.Where(it => !esiItems.Any(esi => esi.ID == it.ID)).ToList();
            context.Items.RemoveRange(deleteList);
            await context.SaveChangesAsync();

            // update items from ESI
            int i = 0;
            foreach (var item in esiItems)
            {
                Item dbItem = await context.Items.FindAsync(item.ID);

                if (dbItem != null)
                {
                    dbItem.Name = item.Name;
                    dbItem.Published = item.Published;
                    dbItem.Capacity = item.Capacity;
                    dbItem.Description = item.Description;
                    dbItem.GraphicID = item.GraphicID;
                    dbItem.GraphicID = item.GroupID;
                    dbItem.IconID = item.IconID;
                    dbItem.MarketGroupID = item.MarketGroupID;
                    dbItem.Mass = item.Mass;
                    dbItem.PackagedVolume = item.PackagedVolume;
                    dbItem.PortionSize = item.PortionSize;
                    dbItem.Radius = item.Radius;
                    dbItem.Volume = item.Volume;
                }
                else
                {
                    await context.Items.AddAsync(item);
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