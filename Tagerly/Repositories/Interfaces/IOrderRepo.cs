using Tagerly.Models;

namespace Tagerly.Repositories.Interfaces
{
    public interface IOrderRepo
    {
        public void Add(Order obj);

        public void Update(Order obj);

        public void Delete(Order obj);

        public List<Order> GetAll();

        public Order GetById(int id);


        //best practice
        public void Save();
    }
}
