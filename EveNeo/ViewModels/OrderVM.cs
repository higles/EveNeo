using EveNeo.Classes;
using EveNeo.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace EveNeo.ViewModels
{
    public class OrderVM
    {
        /// <summary>
        /// Gets or sets the order ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the order duration
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is a buy order
        /// </summary>
        public bool IsBuyOrder { get; set; }

        /// <summary>
        /// Gets or sets the date this order was issued
        /// </summary>
        public DateTime DateIssued { get; set; }

        /// <summary>
        /// Gets or sets the station in which this order was placed
        /// </summary>
        public Station Station { get; set; }

        /// <summary>
        /// Gets or sets the minimum volume of the order
        /// </summary>
        public int MinVolume { get; set; }

        /// <summary>
        /// Gets or sets the pice of the order
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Gets or sets the range of the order
        /// </summary>
        public string Range { get; set; }

        /// <summary>
        /// Gets or sets the solar system this order was placed in
        /// </summary>
        public SolarSystem System { get; set; }

        /// <summary>
        /// Gets or sets the type
        /// </summary>
        public EntityType Type { get; set; }

        /// <summary>
        /// Gets or sets the quantity remaining
        /// </summary>
        public int VolumeRemain { get; set; }

        /// <summary>
        /// Gets or sets the total quantity
        /// </summary>
        public int VolumeTotal { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderVM"/> class.
        /// </summary>
        /// <param name="order">Order to convert</param>
        public OrderVM(Order order)
        {

            this.ID = order.ID;
            this.Duration = order.Duration;
            this.IsBuyOrder = order.IsBuyOrder;
            this.DateIssued = DateTime.Parse(order.Issued);
            this.Station = APIHandler.GetStation(order.LocationID).Result;
            this.MinVolume = order.MinVolume;
            this.Price = order.Price;
            this.Range = this.Range;
            this.System = APIHandler.GetSystem(order.SystemID).Result;
            this.Type = APIHandler.GetEntityType(order.TypeID).Result;
            this.VolumeRemain = order.VolumeRemain;
            this.VolumeTotal = order.VolumeTotal;
        }
    }
}
