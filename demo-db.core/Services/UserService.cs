using demo_db.Data.DataModels;
using demo_db.Services.Abstract;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using demo_db.Data.Repositories.Contracts;
using demo_db.Common.Exceptions;
using demo_db.Services.ViewModels;
using System.Collections.Generic;

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
            Validations.ValidateLength(Validations.MIN_USERNAME, Validations.MAX_USERNAME, username, $"The username can't be less than {Validations.MIN_USERNAME} and greater than {Validations.MAX_USERNAME}");
            Validations.ValidateLength(Validations.MIN_FULLNAME, Validations.MAX_FULLNAME, fullname, $"The full name provided can't be less than {Validations.MIN_USERNAME} and greater than {Validations.MAX_USERNAME}");
            Validations.ValidateLength(Validations.MIN_PASSWORD, Validations.MAX_PASSWORD, password, $"The password can`t be less than {Validations.MIN_PASSWORD} and greater than {Validations.MAX_PASSWORD}");
            Validations.VerifyUserName(username);

            var user = RetrieveUser(username);

            if (user != null)
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

        public User RetrieveUser(string username)
        {
            var user = this.data.Users.All()
                .Include(u => u.Role)
                .FirstOrDefault(u => u.UserName == username);

            return user;
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
            else if (user.Password != password)
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
            data.SaveChanges();
        }

        public IList<UserViewModel> RetrieveUsers(int roleId)
        {
            var users = this.data.Users.All().Where(us => us.RoleId == roleId).ToList();

            IList<UserViewModel> returnValues = new List<UserViewModel>();

            foreach (var user in users)
            {
                returnValues.Add(new UserViewModel { FullName = user.FullName, Username = user.UserName });
            }
            return returnValues;
        }

        public void EvaluateStudent(string username, int assignmentId, int grade, string teacherUsername)
        {
            Validations.ValidateLength(Validations.MIN_USERNAME, Validations.MAX_USERNAME, username, $"The username can't be less than {Validations.MIN_USERNAME} and greater than {Validations.MAX_USERNAME}");
            Validations.VerifyUserName(username);

            var teacher = this.data.Users.All().Include(us => us.TaughtCourses).FirstOrDefault(us => us.UserName == teacherUsername);
            var student = this.data.Users.All().Include(us => us.EnrolledStudents).Include(us => us.Grades).FirstOrDefault(us => us.UserName == username);
            var assaignment = this.data.Assaignments.All().Include(c => c.Course).FirstOrDefault(a => a.Id == assignmentId);

            if (assaignment == null)
            {
                throw new ArgumentNullException("Unfortunately there is no such an assignment");
            }
            
            if (teacher != null && assaignment.Course.TeacherId != teacher.Id)
            {
                throw new ArgumentException($"Teacher {teacher.UserName} is not assigned to {assaignment.Name}.");
            }

            if (student != null && student.EnrolledStudents.All(c => c.CourseId != assaignment.CourseId))
            {
                throw new ArgumentException($"Student {student.UserName} is not assigned to {assaignment.Name}.");
            }

            if (student.Grades.Any(g => g.AssaignmentId == assaignment.Id))
            {
                throw new ArgumentException("Student already received grade for this assignment.");
            }

            var newGrade = new Grade
            {
                    AssaignmentId = assaignment.Id,
                    StudentId = student.Id,
                    ReceivedGrade = grade
            };

            student.Grades.Add(newGrade);
                this.data.SaveChanges();
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
    }
}
