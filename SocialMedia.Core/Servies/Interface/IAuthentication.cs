using SocialMedia.DataLayer.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Servies.Interface
{
    public interface IAuthentication
    {
        Task AddUserAsync(UserAuthenticationForApiKey user);
        Task<bool> RemoveUserAsync(int Id);
        Task<UserAuthenticationForApiKey?> FindeUser(string username, string password);
        Task<string> GetJWTTokenForCookiesAnaAuthenticate(string username, string email, string activeCode,string key);
    }
}
