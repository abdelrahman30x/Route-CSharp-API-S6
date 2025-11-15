using ECommerceG02.Domian.Contacts.Repos;
using ECommerceG02.Domian.Contacts.UOW;
using ECommerceG02.Domian.Models;
using ECommerceG02.Presistence.Contexts;
using ECommerceG02.Presistence.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Presistence.UOW
{
    public class UnitOfWork(StoreDbContext _context) : IUnitOfWork
    {
        private readonly Dictionary<string, object> _Repos = [];
        public IGenericRepository<Tentity, Tkey> GetReposatory<Tentity, Tkey>() where Tentity : BaseEntity<Tkey>
        {
            var TypeNmae = typeof(Tentity).Name;
            if (_Repos.ContainsKey(TypeNmae))
            {
                return (IGenericRepository<Tentity, Tkey>)_Repos[TypeNmae];
            }
            else
            {
                var repo = new GenericRepository<Tentity, Tkey>(_context);
                _Repos.Add(TypeNmae, repo);
                return repo;
            }
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
