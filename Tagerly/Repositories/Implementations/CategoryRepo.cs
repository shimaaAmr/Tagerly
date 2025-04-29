using Tagerly.DataAccess;
using Tagerly.Models;
using Tagerly.Repositories.Interfaces;

namespace Tagerly.Repositories.Implementations
{
    public class CategoryRepo : ICategoryRepo
    {
        TagerlyDbContext _context;
        public CategoryRepo(TagerlyDbContext context)
        {
             _context = context;
        }
        public void Add(Category obj)
        {
            _context.Categories.Add(obj);
        }

        public void Delete(Category obj)
        {
           _context.Categories.Remove(obj);
        }

        public List<Category> GetAll()
        {
            return _context.Categories.ToList();
        }

        public Category GetById(int id)
        {
            return _context.Categories.FirstOrDefault(c => c.Id == id);
        }

        public void Update(Category obj)
        {
            _context.Categories.Update(obj);
        }

        //best practice
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
