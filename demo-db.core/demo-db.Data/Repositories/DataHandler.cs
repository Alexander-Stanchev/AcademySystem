using demo_db.Data.Context;
using demo_db.Data.Repositories.Contracts;


namespace demo_db.Data.Repositories
{
    public class DataHandler : IDataHandler
    {
        private readonly IAcademyContext context;
        private IAssaignmentRepositoryEF assaignments;
        private ICourseRepositoryEF courses;
        private IUserRepositoryEF users;

        public DataHandler(IAcademyContext context)
        {
            this.context = context;
        }
        public IAssaignmentRepositoryEF Assaignments
        {
            get
            {
                if(this.assaignments == null)
                {
                    this.assaignments = new AssaignmentRepositoryEF(context);
                    return this.assaignments; 
                }
                else
                {
                    return this.assaignments;
                }
            }
            private set
            {
                this.assaignments = value;
            }

        }

        public ICourseRepositoryEF Courses
        {
            get
            {
                if(this.courses == null)
                {
                    this.courses = new CourseRepositoryEF(context);
                    return this.courses;
                    
                }
                else
                {
                    return this.courses;
                }
            }
            private set
            {
                this.courses = value;
            }
        }

        public IUserRepositoryEF Users
        {
            get
            {
                if(this.users == null)
                {
                    this.users = new UserRepositoryEF(context);
                    return this.users;
                }
                else
                {
                    return this.users;
                }
            }
            set
            {
                this.users = value;
            }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }
    }
}
