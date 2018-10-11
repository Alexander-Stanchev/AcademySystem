using System.Linq;
using demo_db.Data.DataModels;

namespace demo_db.Data.Repositories
{
    public interface IRoleRepositoryEF
    {
        void Add(Role entity);
        IQueryable<Role> All();
        void Delete(Role entity);
        void Update(Role entity);
    }
}