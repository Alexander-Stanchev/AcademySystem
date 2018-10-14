using demo_db.Data.Context;
using demo_db.Data.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace demo_db.Data.Repositories.Contracts
{
    public interface IAssaignmentRepositoryEF
    {
        IQueryable<Assaignment> All();

        void Add(Assaignment entity);

        void Update(Assaignment entity);
    }
}
