using ECommerceG02.Domian.Contacts.Repos;
using ECommerceG02.Domian.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Domian.Contacts.UOW
{
    public interface IUnitOfWork
    {
        public IGenericRepository<Tentity, Tkey> GetReposatory<Tentity, Tkey>() where Tentity : BaseEntity<Tkey>;
        Task<int> SaveChangesAsync();
    }
}
