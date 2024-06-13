using SocialMedia.Core.DTOs;
using SocialMedia.DataLayer.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Servies.Interface
{
    public interface IUserServies
    {
        #region User Account Models
        int AddUser(User user);
        bool IsExistUser(string username);
        bool IsExistEmail(string email);
        void DeletUser(User user);
        void DeleteUserById(int id);
        void UpdateUser(User user);
        User LoginUser(UserLoginViewModel user);
        User GetUserByEmail(string email);
        bool ActiveAccount(string activeCode);
        User GetUserByActiveCode(string activeCode);
        #endregion

        //Create a Task Interface for Implement Async Method in UserServies

        #region User ApI Models
        Task<IEnumerable<User>> GetUsersAsync();
        Task<bool> GetExixstUserAsync(int userid);
        Task AddUserAsync(User user);
        Task<int> RegisterUserAsync(UserRegisterViewModel userViewModel);
        Task<bool> SaveChangesAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetSingleUserAsync(int id);
        Task<User?> GetUserByActiveCodeAsync(string activeCode);
        Task<bool> ActiveUserAccountAsync(string activeCode);
        Task<User?> LoginuserAsync(UserLoginViewModel user);
        Task<User?> GetUserByNameAsync(string username);
        Task DeletUserWithTokenAsync(User user);
        Task<User?> GetUserWithUserNameAndEmail(string username, string email);
        #endregion
    }
}
