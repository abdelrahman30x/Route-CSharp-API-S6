using ECommerceG02.Domian.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Domian.Contacts.Repos
{
    public interface IGenericRepository<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(Tkey id);

        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);

        Task<IEnumerable<TEntity>> GetAllWithSpecificationAsync(ISpecifications<TEntity, Tkey> _specification);
        Task<TEntity> GetByIdWithSpecifiactionAsync(ISpecifications<TEntity, Tkey> _specification);

        Task<int> GetCountWithSpecificationAsync(ISpecifications<TEntity, Tkey> _specification);

    }
}
