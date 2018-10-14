using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.Data.Repositories.Contracts
{
    public interface IDataHandler
    {
        IAssaignmentRepositoryEF Assaignments { get; }

        ICourseRepositoryEF Courses { get; }

        IUserRepositoryEF Users { get; }

        IRoleRepositoryEF Roles { get; }

        int SaveChanges();
    }
}
