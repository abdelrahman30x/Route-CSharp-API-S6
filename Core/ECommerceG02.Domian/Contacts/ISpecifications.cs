using ECommerceG02.Domian.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Domian.Contacts
{
    public interface ISpecifications<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        Expression<Func<TEntity, bool>> Criteria { get; }
        List<Expression<Func<TEntity, object>>> Includes { get; }

        Expression<Func<TEntity, object>> OrderBy { get; }
        Expression<Func<TEntity, object>> OrderByDesc { get; }

        int Take { get; }
        int Skip { get; }

        bool IsPaginated { get; }

    }
}
