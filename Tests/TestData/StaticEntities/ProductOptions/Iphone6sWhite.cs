using refactor_me.DomainObjects.Entities;
using System;
using Tests.TestData.StaticEntities.Products;

namespace Tests.TestData.StaticEntities.ProductOptions
{
    public class Iphone6sWhite : ProductOption
    {
        public const string ID = "9ae6f477-a010-4ec9-b6a8-92a85d6c5f03";

        public static Iphone6sWhite Instance { get; } = new Iphone6sWhite();

        private Iphone6sWhite()
            :base()
        {
            Id = Guid.Parse(Iphone6sWhite.ID);
            ProductId = Guid.Parse(Iphone6s.ID);
            Name = "White";
            Description = "White Apple iPhone 6S";
        }
    }
}
