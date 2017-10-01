using refactor_me.Entities;
using System;

namespace Tests.TestData.StaticEntities.Products
{
    public class GalaxyS7 : Product
    {
        public const string ID = "8f2e9176-35ee-4f0a-ae55-83023d2db1a3";

        public static GalaxyS7 Instance { get; } = new GalaxyS7();

        private GalaxyS7()
            :base()
        {
            this.Id = Guid.Parse(GalaxyS7.ID);
            this.Name = "Samsung Galaxy S7";
            this.Description = "Newest mobile product from Samsung.";
            this.Price = 1024.99M;
            this.DeliveryPrice = 16.99M;
        }
    }
}
