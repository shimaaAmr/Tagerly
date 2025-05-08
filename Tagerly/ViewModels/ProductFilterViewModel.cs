using System;
using System.Linq;
using System.Linq.Expressions;
using Tagerly.Models;
using Tagerly.Models.Enums;

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

        // Properties for role-based filtering
        public string SellerId { get; set; }
        public bool? IsApproved { get; set; }

        public Expression<Func<Product, bool>> BuildFilter()
        {
            // Start with basic filter (not deleted)
            Expression<Func<Product, bool>> filter = p => p.Status != ProductStatus.Deleted; // Filter out deleted products by default

            // Apply approval status filter based on the IsApproved flag
            if (IsApproved.HasValue)
            {
                if (IsApproved.Value)
                {
                    // If IsApproved is true (set by BuyerController),
                    // filter for products with Status == Approved
                    filter = Combine(filter, p => p.Status == ProductStatus.Approved);
                }
                else
                {
                    // If IsApproved is false, you might want to filter for
                    // products that are NOT Approved (Pending, Rejected).
                    filter = Combine(filter, p => p.Status == ProductStatus.Pending || p.Status == ProductStatus.Rejected);
                }
            }
            // If IsApproved is null, no specific approval status filter is applied
            // (This would be useful for an admin list showing ALL statuses except deleted)


            // Apply seller filter if specified
            if (!string.IsNullOrEmpty(SellerId))
            {
                filter = Combine(filter, p => p.SellerId == SellerId);
            }

            // Apply search string filter
            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                filter = Combine(filter, p =>
                    (p.Name != null && p.Name.Contains(SearchString)) || // Null check for safety
                    (p.Description != null && p.Description.Contains(SearchString))); // Null check for safety
            }

            // Apply category filter
            if (CategoryId.HasValue && CategoryId.Value > 0)
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

        // Helper method to combine expressions using AndAlso
        private Expression<Func<T, bool>> Combine<T>(
            Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(first.Parameters[0], parameter);
            var left = leftVisitor.Visit(first.Body);

            var rightVisitor = new ReplaceExpressionVisitor(second.Parameters[0], parameter);
            var right = rightVisitor.Visit(second.Body);

            var body = Expression.AndAlso(left, right);

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }
    }

    // Helper visitor to replace parameters in expressions
    // Required for correctly combining expressions
    public class ReplaceExpressionVisitor : ExpressionVisitor
    {
        private readonly Expression _oldValue;
        private readonly Expression _newValue;

        public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }

        public override Expression Visit(Expression node)
        {
            if (node == _oldValue)
            {
                return _newValue;
            }
            return base.Visit(node);
        }
    }
}