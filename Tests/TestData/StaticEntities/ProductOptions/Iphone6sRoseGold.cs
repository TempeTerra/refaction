using refactor_me.Entities;
using System;
using Tests.TestData.StaticEntities.Products;

namespace Tests.TestData.StaticEntities.ProductOptions
{
    public class Iphone6sRoseGold : ProductOption
    {
        public const string ID = "5c2996ab-54ad-4999-92d2-89245682d534";

        public static Iphone6sRoseGold Instance { get; } = new Iphone6sRoseGold();

        private Iphone6sRoseGold()
            :base()
        {
            Id = Guid.Parse(Iphone6sRoseGold.ID);
            ProductId = Guid.Parse(Iphone6s.ID);
            Name = "Rose Gold";
            Description = "Gold Apple iPhone 6S";
        }
    }
}
