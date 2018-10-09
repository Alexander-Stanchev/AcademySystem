using demo_db.Data.Context;
using demo_db.Data.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace demo_db.Data.Repositories.Contracts
{
    public interface IUserRepositoryEF
    {
        IQueryable<User> All();

        void Add(User entity);

        void Update(User entity);

        void Delete(User entity);
    }
}
