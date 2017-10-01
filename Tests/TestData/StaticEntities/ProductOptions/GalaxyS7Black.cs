using refactor_me.DomainObjects.Entities;
using System;
using Tests.TestData.StaticEntities.Products;

namespace Tests.TestData.StaticEntities.ProductOptions
{
    public class GalaxyS7Black : ProductOption
    {
        public const string ID = "a21d5777-a655-4020-b431-624bb331e9a2";

        public static GalaxyS7Black Instance { get; } = new GalaxyS7Black();

        private GalaxyS7Black()
            :base()
        {
            Id = Guid.Parse(GalaxyS7Black.ID);
            ProductId = Guid.Parse(GalaxyS7.ID);
            Name = "Black";
            Description = "Black Samsung Galaxy S7";
        }
    }
}
