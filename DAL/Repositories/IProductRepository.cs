using refactor_me.DomainObjects.Entities;
using System;

namespace refactor_me.Dal.Repositories
{
    public interface IProductRepository
    {
        void Create(Product entity);
        void Delete(Guid id);
        Product Get(Guid id);
        Product[] GetAll();
        Product[] SearchByName(string pattern);
        void Update(Product entity);
    }
}