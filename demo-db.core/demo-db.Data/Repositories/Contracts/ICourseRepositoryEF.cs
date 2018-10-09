using demo_db.Data.Context;
using demo_db.Data.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace demo_db.Data.Repositories.Contracts
{
    public interface ICourseRepositoryEF
    {
        IQueryable<Course> All();

        void Add(Course entity);

        void Update(Course entity);

        void Delete(Course entity);
    }
}
