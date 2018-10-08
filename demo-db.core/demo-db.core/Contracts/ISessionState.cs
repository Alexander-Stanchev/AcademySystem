using demo_db.core.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.core.Contracts
{
    public interface ISessionState
    {
        bool IsLogged { get; }
        string UserName { get; }
        int RoleId { get; }
        void Login(int roleId, string userName);

    }
}
