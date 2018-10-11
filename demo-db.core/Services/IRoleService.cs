using demo_db.Data.DataModels;

namespace demo_db.Services
{
    public interface IRoleService
    {
        Role RetrieveRole(string roleName);
    }
}