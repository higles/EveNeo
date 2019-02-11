using EveNeo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EveNeo.Classes
{
    public static class APIHandler
    {
        private static HttpClient client = new HttpClient();

        public static async Task<Station> GetStation(int stationID)
        {
            Station station = null;
            HttpResponseMessage response = await client.GetAsync($"https://esi.evetech.net/latest/universe/stations/{stationID}/?datasource=tranquility");
            if (response.IsSuccessStatusCode)
            {
                station = await response.Content.ReadAsAsync<Station>();
            }

            return station;
        }

        public static async Task<SolarSystem> GetSystem(int systemID)
        {
            SolarSystem system = null;
            HttpResponseMessage response = await client.GetAsync($"https://esi.evetech.net/latest/universe/systems/{systemID}/?datasource=tranquility&language=en-us");
            if (response.IsSuccessStatusCode)
            {
                system = await response.Content.ReadAsAsync<SolarSystem>();
            }

            return system;
        }

        public static async Task<Item> GetEntityType(int typeID)
        {
            Item type = null;
            HttpResponseMessage response = await client.GetAsync($"https://esi.evetech.net/latest/universe/types/{typeID}/?datasource=tranquility&language=en-us");
            if (response.IsSuccessStatusCode)
            {
                type = await response.Content.ReadAsAsync<Item>();
            }

            return type;
        }
    }
}
