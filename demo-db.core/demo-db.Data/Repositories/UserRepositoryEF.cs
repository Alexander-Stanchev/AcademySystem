using demo_db.Data.Context;
using demo_db.Data.DataModels;
using demo_db.Data.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace demo_db.Data.Repositories
{
    public class UserRepositoryEF : IUserRepositoryEF
    {
        private readonly IAcademyContext context;

        public UserRepositoryEF(IAcademyContext context)
        {
            this.context = context;
        }

        public IQueryable<User> All()
        {
            return this.context.Users;
        }

        public void Add(User entity)
        {
            this.context.Users.Add(entity);
        }

        public void Delete(User entity)
        {
            throw new NotImplementedException();
        }

        public void Update(User entity)
        {
            context.Users.Update(entity);
        }
    }
}
