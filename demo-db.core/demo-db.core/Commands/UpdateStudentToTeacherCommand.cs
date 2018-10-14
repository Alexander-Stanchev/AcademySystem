using demo_db.Common.Wrappers;
using demo_db.core.Contracts;
using demo_db.Services.Abstract;


namespace demo_db.core.Commands
{
    class UpdateStudentToTeacherCommand : CommandAbstract
    {
        private IUserService service;
        private const int teacherRoleId = 2;

        public UpdateStudentToTeacherCommand(ISessionState state, IStringBuilderWrapper builder, IUserService service) : base(state, builder)
        {
            this.service = service;
        }

        public override string Execute(string[] parameters)
        {
            if (!this.State.IsLogged)
            {
                return "You have to log in first.";
            }

            if (this.State.RoleId != 1)
            {
                return "You dont have access.";
            }

            string userName = parameters[0];
            this.service.UpdateRole(userName, teacherRoleId);

            return $"User {userName} role is updated.";
        }
    }
}