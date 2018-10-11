using demo_db.Data.Context;
using demo_db.Data.DataModels;
using demo_db.Data.Repositories.Contracts;
using System.Linq;


namespace demo_db.Data.Repositories
{
    public class CourseRepositoryEF : ICourseRepositoryEF
    {
        private readonly IAcademyContext context;

        public CourseRepositoryEF(IAcademyContext context)
        {
            this.context = context;
        }

        public IQueryable<Course> All()
        {
            return this.context.Courses;
        }

        public void Add(Course entity)
        {
            this.context.Courses.Add(entity);
        }

        public void Update(Course entity)
        {
            context.Courses.Update(entity);
        }
    }
}
