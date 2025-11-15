using ECommerceG02.Domian.Contacts;
using ECommerceG02.Domian.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Services.Specifications
{
    public abstract class BaseSpecification<TEntity, Tkey> : ISpecifications<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {


        public Expression<Func<TEntity, bool>> Criteria { get; private set; }
        protected BaseSpecification(Expression<Func<TEntity, bool>> _Criteria)

        {
            Criteria = _Criteria;
        }

        public Expression<Func<TEntity, object>> OrderBy { get; private set; }

        public Expression<Func<TEntity, object>> OrderByDesc { get; private set; }

        protected void AddOrderBy(Expression<Func<TEntity, object>>? _OrderBy)

        {
            OrderBy = _OrderBy;
        }

        protected void AddOrderByDesc(Expression<Func<TEntity, object>>? _OrderByDesc)

        {
            OrderByDesc = _OrderByDesc;
        }

        public List<Expression<Func<TEntity, object>>> Includes { get; private set; } = [];

        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool IsPaginated { get; set; }

        public void ApplyPagination(int PageSize, int PageIndex)
        {
            Skip = PageSize * (PageIndex - 1);
            Take = PageSize;
            IsPaginated = true;
        }

        protected void AddInclude(Expression<Func<TEntity, object>> IncludeExpression)
        {
            Includes.Add(IncludeExpression);
        }
 

    }
}
