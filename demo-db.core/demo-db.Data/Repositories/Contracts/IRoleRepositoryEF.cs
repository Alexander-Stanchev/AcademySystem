using demo_db.Data.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace demo_db.Data.Repositories.Contracts
{
    public interface IRoleRepositoryEF
    {
        void Add(Role entity);
        IQueryable<Role> All();
        void Update(Role entity);
    }
}
