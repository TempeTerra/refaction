using refactor_me.Entities;
using System;
using Tests.TestData.StaticEntities.Products;

namespace Tests.TestData.StaticEntities.ProductOptions
{
    public class GalaxyS7White : ProductOption
    {
        public const string ID = "0643ccf0-ab00-4862-b3c5-40e2731abcc9";

        public static GalaxyS7White Instance { get; } = new GalaxyS7White();

        private GalaxyS7White()
            :base()
        {
            Id = Guid.Parse(GalaxyS7White.ID);
            ProductId = Guid.Parse(GalaxyS7.ID);
            Name = "White";
            Description = "White Samsung Galaxy S7";
        }
    }
}
