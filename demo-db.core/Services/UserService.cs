using demo_db.Data.Context;
using demo_db.Data.DataModels;
using demo_db.Services.Abstract;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using demo_db.Data.Repositories.Contracts;

namespace demo_db.Services
{
    public class UserService : IUserService
    {
        private IDataHandler data;

        public UserService(IDataHandler context)
        {
            this.data = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void AddUser(string username, string password, string fullname)
        {
            var user = RetrieveUser(username);
            if(user == null)
            {
                user = new User
                {
                    UserName = username,
                    Password = password,
                    FullName = fullname,
                    RoleId = 3,
                    Deleted = false,
                    RegisteredOn = DateTime.Now
                };
            }
            data.Users.Add(user);
            data.SaveChanges();
        }

        public void UpdateUserRole(string username, Role newRole)
        {
            var user = RetrieveFullUser(username);
            if (user == null)
            {
                throw new ArgumentNullException("User is null when trying to execute UpdateUserRole service");
            }
            else
            {
                user.RoleId = newRole.Id; 
            }
            data.Users.Update(user);
            data.SaveChanges();
        }

        public User RetrieveUser(string username)
        {
            
            var user = this.data.Users.All()
                .Include(u => u.Role)
                .FirstOrDefault(u => u.UserName == username);

            return user;
        }

        public User RetrieveFullUser(string username)
        {
            var user = this.data.Users.All()
               .Include(u => u.Role)
               .Include(u => u.Grades)
               .Include(u => u.EnrolledStudents)
               .FirstOrDefault(u => u.UserName == username);

            return user;
        }

        public void EnrollCourse(User user, Course course)
        {
            if (user.EnrolledStudents.FirstOrDefault(e => e.CourseId == course.CourseId) == null)
            {
                var enroll = new EnrolledStudent
                {
                    StudentId = user.Id,
                    CourseId = course.CourseId
                };
                
                user.EnrolledStudents.Add(enroll);
                this.data.SaveChanges();

            }
            else
            {
                {
                    throw new Exception();
                }
            }
        }
    }
}
