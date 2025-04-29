using Tagerly.DataAccess;
using Tagerly.Models;

namespace Tagerly.Repositories.Implementations
{
    public class ProductRepo
    {
        TagerlyDbContext _context;
        public ProductRepo(TagerlyDbContext context)
        {
            _context = context;
        }
        public void Add(Product obj)
        {
            _context.Products.Add(obj);
        }

        public void Delete(Product obj)
        {
            _context.Products.Remove(obj);
        }

        public List<Product> GetAll()
        {
            return _context.Products.ToList();
        }

        public Product GetById(int id)
        {
            return _context.Products.FirstOrDefault(O => O.Id == id);
        }

        public void Update(Product obj)
        {
            _context.Products.Update(obj);
        }

        //best practice
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
