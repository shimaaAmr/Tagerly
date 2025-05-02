using System;
using System.Linq;
using System.Linq.Expressions;
using Tagerly.Models;

namespace Tagerly.ViewModels
{
    public class ProductFilterViewModel
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortOrder { get; set; }
        public string SearchString { get; set; }
        public int? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? InStockOnly { get; set; }

        public Expression<Func<Product, bool>> BuildFilter()
        {
            Expression<Func<Product, bool>> filter = p => true;

            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                filter = Combine(filter, p =>
                    p.Name.Contains(SearchString) ||
                    p.Description.Contains(SearchString));
            }

            if (CategoryId.HasValue)
            {
                filter = Combine(filter, p => p.CategoryId == CategoryId.Value);
            }

            if (MinPrice.HasValue)
            {
                filter = Combine(filter, p => p.Price >= MinPrice.Value);
            }

            if (MaxPrice.HasValue)
            {
                filter = Combine(filter, p => p.Price <= MaxPrice.Value);
            }

            if (InStockOnly.HasValue && InStockOnly.Value)
            {
                filter = Combine(filter, p => p.Quantity > 0);
            }

            return filter;
        }

        private Expression<Func<Product, bool>> Combine(
            Expression<Func<Product, bool>> first,
            Expression<Func<Product, bool>> second)
        {
            var parameter = Expression.Parameter(typeof(Product));
            var body = Expression.AndAlso(
                Expression.Invoke(first, parameter),
                Expression.Invoke(second, parameter));
            return Expression.Lambda<Func<Product, bool>>(body, parameter);
        }

        public Func<IQueryable<Product>, IOrderedQueryable<Product>> BuildSort()
        {
            return SortOrder?.ToLower() switch
            {
                "name_desc" => q => q.OrderByDescending(p => p.Name),
                "price" => q => q.OrderBy(p => p.Price),
                "price_desc" => q => q.OrderByDescending(p => p.Price),
                "stock" => q => q.OrderBy(p => p.Quantity),
                "stock_desc" => q => q.OrderByDescending(p => p.Quantity),
                _ => q => q.OrderBy(p => p.Name), // default
            };
        }
    }
}
