using demo_db.Data.DataModels;
using demo_db.Services.Abstract;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using demo_db.Data.Repositories.Contracts;
using demo_db.Common.Exceptions;

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
            if(user != null)
            {
                throw new UserAlreadyExistsException("User already exists");
            }
            user = new User
            {
                UserName = username,
                Password = password,
                FullName = fullname,
                RoleId = 3,
                Deleted = false,
                RegisteredOn = DateTime.Now
            };
            data.Users.Add(user);
            data.SaveChanges();
        }

        private User RetrieveUser(string username)
        {
            
            var user = this.data.Users.All()
                .Include(u => u.Role)
                .FirstOrDefault(u => u.UserName == username);

            return user;
        }

        private User RetrieveFullUser(string username)
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

        public int LoginUser(string username, string password)
        {
            var user = this.RetrieveUser(username);
            if (user == null)
            {
                throw new UserDoesntExistsException("User doesn't exists.");
            }
            else if(user.Password != password)
            {
                throw new InvalidPasswordException("Invalid password");
            }
            return user.RoleId;
        }
    }
}
