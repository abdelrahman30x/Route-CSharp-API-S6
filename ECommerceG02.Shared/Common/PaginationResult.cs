using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Shared.Common
{
    public class PaginationResult<TEntity>
    {
     public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public IEnumerable<TEntity> Data { get; set; } = Enumerable.Empty<TEntity>();

    }
}
