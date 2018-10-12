using demo_db.Data.DataModels;
using demo_db.Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.Services.Abstract
{
    public interface IUserService
    {
        void AddUser(string username, string password, string fullname);
        void EnrollCourse(User user, Course course);
        int LoginUser(string username, string password);
        void UpdateRole(string userName, int newRoleString);
        User RetrieveUser(string username);
        IList<UserViewModel> RetrieveUsers(int roleId);
        void EvaluateStudent(string username, int assignmentId, int grade, string teacherUsername);
    }
}
