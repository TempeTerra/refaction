using refactor_me.Entities;
using System;

namespace Tests.TestData.StaticEntities.Products
{
    public class Iphone6s : Product
    {
        public const string ID = "de1287c0-4b15-4a7b-9d8a-dd21b3cafec3";

        public static Iphone6s Instance { get; } = new Iphone6s();

        private Iphone6s()
            :base()
        {
            this.Id = Guid.Parse(Iphone6s.ID);
            this.Name = "Apple iPhone 6S";
            this.Description = "Newest mobile product from Apple.";
            this.Price = 1299.99M;
            this.DeliveryPrice = 15.99M;
        }
    }
}
