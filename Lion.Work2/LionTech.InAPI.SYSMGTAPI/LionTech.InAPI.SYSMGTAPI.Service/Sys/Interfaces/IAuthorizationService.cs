using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces
{
    public interface IAuthorizationService
    {
        Task<object> GetAllUserSystemRolesObj();

        Task<object> GetAllUserAssignSystemFunsObj();
    }
}