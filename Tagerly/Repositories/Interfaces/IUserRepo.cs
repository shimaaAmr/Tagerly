using Tagerly.Models;

namespace Tagerly.Repositories.Interfaces
{
    public interface IUserRepo
    {
        public void Add(ApplicationUser obj);

        public void Update(ApplicationUser obj);

        public void Delete(ApplicationUser obj);

        public List<ApplicationUser> GetAll();

        public ApplicationUser GetById(string id);


        //best practice
        public void Save();
    }
}
