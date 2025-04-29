using Tagerly.Models;

namespace Tagerly.Repositories.Interfaces
{
    public interface IProductRepo
    {
        public void Add(Product obj);

        public void Update(Product obj);

        public void Delete(Product obj);

        public List<Product> GetAll();

        public Product GetById(int id);


        //best practice
        public void Save();
    }
}
