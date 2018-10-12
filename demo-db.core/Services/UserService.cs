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
        private IRoleService roleService;

        public UserService(IDataHandler context, IRoleService roleService)
        {
            this.data = context ?? throw new ArgumentNullException(nameof(context));
            this.roleService = roleService;
        }

        public void AddUser(string username, string password, string fullname)
        {
            Validations.ValidateLength(Validations.MIN_USERNAME, Validations.MAX_USERNAME, username, $"The username can't be less than {Validations.MIN_USERNAME} and greater than {Validations.MAX_USERNAME}");
            Validations.ValidateLength(Validations.MIN_FULLNAME, Validations.MAX_FULLNAME, fullname, $"The full name provided can't be less than {Validations.MIN_USERNAME} and greater than {Validations.MAX_USERNAME}");
            Validations.VerifyUserName(username);

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
            Validations.ValidateLength(Validations.MIN_USERNAME, Validations.MAX_USERNAME, username, $"The username can't be less than {Validations.MIN_USERNAME} and greater than {Validations.MAX_USERNAME}");
            Validations.VerifyUserName(username);

            var user = this.data.Users.All()
               .Include(u => u.Role)
               .Include(u => u.Grades)
               .Include(u => u.EnrolledStudents)
               .FirstOrDefault(u => u.UserName == username);

            return user;
        }

        public void EnrollCourse(User user, Course course)
        {
            if (user == null)
            {
                throw new UserDoesntExistsException("Can't enroll course. User is null.");
            }

            if (course == null)
            {
                throw new ArgumentNullException("Can't enroll course. Course is null.");
            }

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
            Validations.ValidateLength(Validations.MIN_USERNAME, Validations.MAX_USERNAME, username, $"The username can't be less than {Validations.MIN_USERNAME} and greater than {Validations.MAX_USERNAME}");
            Validations.VerifyUserName(username);

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

        public void UpdateRole(string username, int newRoleString)
        {
            Validations.ValidateLength(Validations.MIN_USERNAME, Validations.MAX_USERNAME, username, $"The username can't be less than {Validations.MIN_USERNAME} and greater than {Validations.MAX_USERNAME}");
            Validations.VerifyUserName(username);

            if (newRoleString == 1)
            {
                throw new InvalidOperationException("You are not allowed to set someone's role to Adminitrator");
            }

            var user = RetrieveFullUser(username);

            if (user == null)
            {
                throw new ArgumentNullException("User doesn't exist");
            }
            else
            {
                user.RoleId = newRoleString;
            }

            //data.Users.Update(user);
            data.SaveChanges();
        }
    }
}
