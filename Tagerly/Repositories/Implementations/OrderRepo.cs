using Tagerly.DataAccess;
using Tagerly.Models;
using Tagerly.Repositories.Interfaces;

namespace Tagerly.Repositories.Implementations
{
    public class OrderRepo : IOrderRepo
    {
        TagerlyDbContext _context;
        public OrderRepo(TagerlyDbContext context)
        {
            _context = context;
        }
        public void Add(Order obj)
        {
            _context.Orders.Add(obj);
        }

        public void Delete(Order obj)
        {
            _context.Orders.Remove(obj);
        }

        public List<Order> GetAll()
        {
            return _context.Orders.ToList();
        }

        public Order GetById(int id)
        {
            return _context.Orders.FirstOrDefault(O => O.Id == id);
        }

        public void Update(Order obj)
        {
            _context.Orders.Update(obj);
        }

        //best practice
        public void Save()
        {
            _context.SaveChanges();
        }

    }
}
