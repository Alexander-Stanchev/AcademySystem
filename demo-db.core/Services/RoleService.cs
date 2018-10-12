using demo_db.Data.DataModels;
using demo_db.Data.Repositories.Contracts;
using demo_db.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace demo_db.Services
{
    public class RoleService : IRoleService
    {
        private IDataHandler data;

        public RoleService(IDataHandler context)
        {
            this.data = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Role RetrieveRole(string roleName)
        {

            Role role = this.data.Roles.All()
                .FirstOrDefault(u => u.Name == roleName);

            return role;
        }
    }
}
