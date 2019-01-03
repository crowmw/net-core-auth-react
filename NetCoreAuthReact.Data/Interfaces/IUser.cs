using NetCoreAuthReact.Dtos;
using System.Threading.Tasks;

namespace NetCoreAuthReact.Data.Interfaces
{
    public interface IUser
    {
        Task<UserDto> Login(string userNameOrEmail, string password);
        Task<UserDto> Register(string userName, string email, string password, string passwordConfirm);
        Task Logout();
    }
}
