using demo_db.Data.Context;
using demo_db.Data.DataModels;
using demo_db.Data.Repositories.Contracts;
using System.Linq;


namespace demo_db.Data.Repositories
{
    public class RoleRepositoryEF : IRoleRepositoryEF
    {
        private readonly IAcademyContext context;

        public RoleRepositoryEF(IAcademyContext context)
        {
            this.context = context;
        }

        public IQueryable<Role> All()
        {
            return this.context.Roles;
        }

        public void Add(Role entity)
        {
            this.context.Roles.Add(entity);
        }

        public void Update(Role entity)
        {
            context.Roles.Update(entity);
        }
    }
}
