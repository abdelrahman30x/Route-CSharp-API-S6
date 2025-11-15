using ECommerceG02.Domian.Contacts;
using ECommerceG02.Domian.Contacts.Repos;
using ECommerceG02.Domian.Models;
using ECommerceG02.Presistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Presistence.Repos
{
    public class GenericRepository<Tentity, Tkey>(StoreDbContext _context) : IGenericRepository<Tentity, Tkey> where Tentity : BaseEntity<Tkey>
    {
        public async Task<IEnumerable<Tentity>> GetAllAsync()

           => await _context.Set<Tentity>().ToListAsync();

        public async Task<Tentity> GetByIdAsync(Tkey id)

            => await _context.Set<Tentity>().FindAsync(id);

        public void Add(Tentity entity)

             => _context.Set<Tentity>().Add(entity);

        public void Update(Tentity entity)
              => _context.Set<Tentity>().Update(entity);

        public void Delete(Tentity entity)
              => _context.Set<Tentity>().Remove(entity);

        public async Task<IEnumerable<Tentity>> GetAllWithSpecificationAsync(ISpecifications<Tentity, Tkey> _specification)

        {
            var Query = await SpecificationEvlautor.CreateQuery(_context.Set<Tentity>(), _specification).ToListAsync();
            return Query;
        }
        public async Task<Tentity> GetByIdWithSpecifiactionAsync(ISpecifications<Tentity, Tkey> _specification)
        {
            var Query = await SpecificationEvlautor.CreateQuery(_context.Set<Tentity>(), _specification).FirstOrDefaultAsync();
            return Query;
        }

        public async Task<int> GetCountWithSpecificationAsync(ISpecifications<Tentity, Tkey> _specification)
        {
            var Query = await SpecificationEvlautor.CreateQuery(_context.Set<Tentity>(), _specification).CountAsync();
            return Query;
        }
    }
}
