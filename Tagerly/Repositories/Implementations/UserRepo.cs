using Tagerly.DataAccess;
using Tagerly.Models;
using Tagerly.Repositories.Interfaces;

namespace Tagerly.Repositories.Implementations
{
    public class UserRepo : GenericRepo<ApplicationUser>,IUserRepo
    {
        TagerlyDbContext _context;
        public UserRepo(TagerlyDbContext context) : base(context)
        {
        }

        public Task AddAsync(ApplicationUser entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(ApplicationUser entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> GetUserWithOrdersAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public void Update(ApplicationUser entity)
        {
            throw new NotImplementedException();
        }
    }
}
