using demo_db.Data.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.Services.Abstract
{
    public interface IUserService
    {
        void AddUser(string username, string password, string fullname);
        void EnrollCourse(User user, Course course);
        User RetrieveUser(string username);
        User RetrieveFullUser(string username);

    }
}
