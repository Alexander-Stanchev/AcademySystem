using demo_db.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.Data.Context
{
    public interface IAcademyContext
    {
        DbSet<User> Users { get; set; }

        DbSet<Role> Roles { get; set; }

        DbSet<Course> Courses { get; set; }

        DbSet<Assaignment> Assaignments { get; set; }

        DbSet<EnrolledStudent> EnrolledStudents { get; set; }

        DbSet<Grade> Grades { get; set; }

        int SaveChanges();
    }
}
