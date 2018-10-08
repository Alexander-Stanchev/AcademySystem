using demo_db.Data.Context;
using demo_db.Data.DataModels;
using demo_db.Services.Abstract;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace demo_db.Services
{
    public class UserService : IUserService
    {
        private IAcademyContext context;

        public UserService(IAcademyContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
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
                    RoleId = 2,
                    Deleted = false,
                    RegisteredOn = DateTime.Now
                };
            }
            context.Users.Add(user);
            context.SaveChanges();
        }

        public User RetrieveUser(string username)
        {
            var user = this.context.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.UserName == username);

            return user;
        }
    }
}
