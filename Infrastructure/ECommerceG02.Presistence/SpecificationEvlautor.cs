using ECommerceG02.Domian.Contacts;
using ECommerceG02.Domian.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Presistence
{
    public static class SpecificationEvlautor
    {
        public static IQueryable<Tentity> CreateQuery<Tentity, Tkey>(IQueryable<Tentity> BaseQuery, ISpecifications<Tentity, Tkey> Specificstion) where Tentity : BaseEntity<Tkey>
        {
            var Query = BaseQuery;

            if (Specificstion.Criteria != null)

            {
                Query = Query.Where(Specificstion.Criteria);
            }


            if (Specificstion.OrderBy != null)
            {
                Query = Query.OrderBy(Specificstion.OrderBy);
            }

            if (Specificstion.OrderByDesc != null)
            {
                Query = Query.OrderBy(Specificstion.OrderByDesc);
            }
            if( Specificstion.IsPaginated)
            {
                Query = Query.Skip(Specificstion.Skip).Take(Specificstion.Take);
            }
            if (Specificstion.Includes is not null && Specificstion.Includes.Any())
            {
                Query = Specificstion.Includes.Aggregate(Query, (CurrentQuery, expression) => CurrentQuery.Include(expression));
            }
            return Query;
        }
    }
}
