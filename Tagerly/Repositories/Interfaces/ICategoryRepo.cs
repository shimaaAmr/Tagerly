using Tagerly.Models;

namespace Tagerly.Repositories.Interfaces
{
    public interface ICategoryRepo
    {
        public void Add(Category obj);

        public void Update(Category obj);

        public void Delete(Category obj);

        public List<Category> GetAll();

        public Category GetById(int id);


        //best practice
        public void Save();
    }
}
