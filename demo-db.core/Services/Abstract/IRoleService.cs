using demo_db.Data.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.Services.Abstract
{
    public interface IRoleService
    {
        Role RetrieveRole(string roleName);
    }
}
