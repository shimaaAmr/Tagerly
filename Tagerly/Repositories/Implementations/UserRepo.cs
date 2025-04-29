using Tagerly.DataAccess;
using Tagerly.Models;
using Tagerly.Repositories.Interfaces;

namespace Tagerly.Repositories.Implementations
{
    public class UserRepo:IUserRepo
    {
        TagerlyDbContext _context;
        public UserRepo(TagerlyDbContext context)
        {
            _context = context;
        }
        public void Add(ApplicationUser obj)
        {
            _context.Users.Add(obj);
        }

        public void Delete(ApplicationUser obj)
        {
            _context.Users.Remove(obj);
        }

        public List<ApplicationUser> GetAll()
        {
            return _context.Users.ToList();
        }

        public ApplicationUser GetById(string id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public void Update(ApplicationUser obj)
        {
            _context.Users.Update(obj);
        }

        //best practice
        public void Save()
        {
            _context.SaveChanges();
        }

    }
}
