using demo_db.Data.Context;
using demo_db.Data.DataModels;
using demo_db.Data.Repositories.Contracts;
using System;
using System.Linq;


namespace demo_db.Data.Repositories
{
    public class AssaignmentRepositoryEF : IAssaignmentRepositoryEF
    {
        private readonly IAcademyContext context;

        public AssaignmentRepositoryEF(IAcademyContext context)
        {
            this.context = context;
        }

        public IQueryable<Assaignment> All()
        {
            return this.context.Assaignments;
        }

        public void Add(Assaignment entity)
        {
            this.context.Assaignments.Add(entity);
        }

        public void Delete(Assaignment entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Assaignment entity)
        {
            context.Assaignments.Update(entity);
        }
    }
}
