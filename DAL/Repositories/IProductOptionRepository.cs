using refactor_me.DomainObjects.Entities;
using System;

namespace refactor_me.Dal.Repositories
{
    public interface IProductOptionRepository
    {
        void Create(ProductOption entity);
        void Delete(Guid id);
        ProductOption Get(Guid id);
        ProductOption[] GetAll();
        ProductOption[] GetForProduct(Guid productId);
        void Update(ProductOption entity);
    }
}