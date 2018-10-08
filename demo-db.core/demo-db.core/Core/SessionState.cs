using demo_db.core.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.core.Core
{
    public class SessionState : ISessionState
    {
        public SessionState()
        {
            this.IsLogged = false;
        }
        public bool IsLogged { get; private set; }
        public int RoleId { get; private set; }
        public string UserName { get; private set; }

        public void Login(int roleId, string userName)
        {
            this.IsLogged = true;

            SetRole(roleId, userName);
        }

        private void SetRole(int roleId, string userName)
        {
            this.UserName = userName;
            this.RoleId = roleId;
        }
    }
}
