using demo_db.Data.Context;
using demo_db.Data.Repositories.Contracts;


namespace demo_db.Data.Repositories
{
    public class DataHandler : IDataHandler
    {
        private readonly IAcademyContext context;

        public DataHandler(IAcademyContext context, IAssaignmentRepositoryEF assaignments, ICourseRepositoryEF courses, IUserRepositoryEF users )
        {
            this.Assaignments = assaignments;
            this.Courses = courses;
            this.Users = users;
            this.context = context;
        }
        public IAssaignmentRepositoryEF Assaignments { get; private set; }

        public ICourseRepositoryEF Courses { get; private set; }

        public IUserRepositoryEF Users { get; private set; }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }
    }
}
