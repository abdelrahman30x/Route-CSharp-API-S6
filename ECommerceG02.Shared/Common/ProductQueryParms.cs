using System;

namespace ECommerceG02.Shared.Common
{
    public class ProductQueryParms
    {
        public const int DefaultPageSize = 5;
        private const int DefaultPageIndex = 1;
        private const int MinPageSize = 1;
        private const int MaxPageSize = 10;

        private int _pageIndex = DefaultPageIndex;
        private int _pageSize = DefaultPageSize;

        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public ProductSortingOptions SortingOption { get; set; }
        public string? SearchValue { get; set; }

        public int PageIndex
        {
            get => _pageIndex;
            set => _pageIndex = value >= 1 ? value : DefaultPageIndex;
        }

        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (value > MaxPageSize)
                    _pageSize = MaxPageSize;
                else if (value < MinPageSize)
                    _pageSize = MinPageSize;
                else
                    _pageSize = value;
            }
        }
    }
}
