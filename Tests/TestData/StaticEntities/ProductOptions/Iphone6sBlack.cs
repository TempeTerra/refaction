using refactor_me.DomainObjects.Entities;
using System;
using Tests.TestData.StaticEntities.Products;

namespace Tests.TestData.StaticEntities.ProductOptions
{
    public class Iphone6sBlack : ProductOption
    {
        public const string ID = "4e2bc5f2-699a-4c42-802e-ce4b4d2ac0ef";

        public static Iphone6sBlack Instance { get; } = new Iphone6sBlack();

        private Iphone6sBlack()
            :base()
        {
            Id = Guid.Parse(Iphone6sBlack.ID);
            ProductId = Guid.Parse(Iphone6s.ID);
            Name = "Black";
            Description = "Black Apple iPhone 6S";
        }
    }
}
