using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tagerly.DataAccess.DbContexts;
using Tagerly.Models;
using Tagerly.Repositories.Interfaces;

namespace Tagerly.Repositories.Implementations
{
    public class CategoryRepo : GenericRepo<Category>, ICategoryRepo
    {
        #region Fields & Constructor
        private readonly TagerlyDbContext _context;

        public CategoryRepo(TagerlyDbContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        #endregion

        #region Extended Read Operations
        public async Task<IEnumerable<Category>> GetAllWithProductsAsync()
        {
            return await _context.Categories
                .Include(c => c.Products)
                .AsNoTracking()
                .ToListAsync();
        }
        #endregion
    }
}