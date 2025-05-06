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
        
        // New properties for role-based filtering
        public string SellerId { get; set; }
        public bool? IsApproved { get; set; } // null=pending, true=approved, false=rejected

        public Expression<Func<Product, bool>> BuildFilter()
        {
            // Start with basic filter (not deleted)
            Expression<Func<Product, bool>> filter = p => !p.IsDeleted;

            // Apply approval filter if specified
            if (IsApproved.HasValue)
            {
                filter = Combine(filter, p => p.IsApproved == IsApproved.Value);
            }

            // Apply seller filter if specified
            if (!string.IsNullOrEmpty(SellerId))
            {
                filter = Combine(filter, p => p.SellerId == SellerId);
            }

            // Apply search string filter
            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                filter = Combine(filter, p => 
                    p.Name.Contains(SearchString) || 
                    p.Description.Contains(SearchString));
            }

            // Apply category filter
            if (CategoryId.HasValue)
            {
                filter = Combine(filter, p => p.CategoryId == CategoryId.Value);
            }

            // Apply price range filters
            if (MinPrice.HasValue)
            {
                filter = Combine(filter, p => p.Price >= MinPrice.Value);
            }

            if (MaxPrice.HasValue)
            {
                filter = Combine(filter, p => p.Price <= MaxPrice.Value);
            }

            // Apply stock filter
            if (InStockOnly.HasValue && InStockOnly.Value)
            {
                filter = Combine(filter, p => p.Quantity > 0);
            }

            return filter;
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
                _ => q => q.OrderBy(p => p.Name), // Default sorting
            };
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
    }
}