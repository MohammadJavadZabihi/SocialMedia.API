using Azure.Identity;
using SocialMedia.Core.Servies.Interface;
using SocialMedia.DataLayer.Context;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Security;
using SocialMedia.Core.Convertor;
using SocialMedia.Core.Generator;
//using Microsoft.CodeAnalysis.CSharp.Syntax;
using SocialMedia.DataLayer.Entites;

namespace SocialMedia.Core.Servies
{
    public class UserServies : IUserServies
    {
        private SocialMediaContext _context;

        public UserServies(SocialMediaContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }
        public int AddUser(User user)
        {
            _context.Add(user);
            _context.SaveChanges();

            return user.UserId;
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public void DeleteUserById(int id)
        {
            var user =_context.Users.FirstOrDefault(u => u.UserId == id);
            if(user != null)
            {
                DeletUser(user);
                _context.SaveChanges();
            }
        }

        public void DeletUser(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public async Task<bool> GetExixstUserAsync(int userid)
        {
            return await _context.Users.AnyAsync(u => u.UserId == userid);
        }

        public async Task<User?> GetSingleUserAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.SingleOrDefault(u => u.Email == email);
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public bool ActiveAccount(string activeCode)
        {
            var user = _context.Users.FirstOrDefault(u => u.ActiveCode == activeCode);
            if (user == null || user.IsActive)
            {
                return false;
            }
            user.IsActive = true;
            user.ActiveCode = NameGenerator.GenerateUniqCode();
            _context.SaveChanges();

            return true;
        }

        public bool IsExistEmail(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public bool IsExistUser(string username)
        {
            return _context.Users.Any(u => u.UserName == username);
        }

        public User LoginUser(UserLoginViewModel user)
        {
            string hashPassword = PasswordHelper.EncodePasswordMd5(user.Password);
            string email = FixedText.FixEmail(user.Email);

            return _context.Users.FirstOrDefault(u => u.Email == email && u.Userpassword == hashPassword);
        }

        public async Task<User> RegisterUserAsync(UserRegisterViewModel userViewModel)
        {
            try
            {


                var user = new User
                {
                    ActiveCode = NameGenerator.GenerateUniqCode(),
                    Bio = userViewModel.Bio,
                    Email = userViewModel.Email,
                    ProfileURL = "AvatarPic/AVA.png",
                    UserName = userViewModel.UserName,
                    Userpassword = PasswordHelper.EncodePasswordMd5(userViewModel.Password)
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return user;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }

        public void UpdateUser(User user)
        {
            _context.Update(user);
            _context.SaveChanges();
        }

        public User GetUserByActiveCode(string activeCode)
        {
            return _context.Users.FirstOrDefault(u => u.ActiveCode == activeCode);
        }

        public async Task<User?> GetUserByActiveCodeAsync(string activeCode)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.ActiveCode == activeCode);
        }

        public async Task<bool> ActiveUserAccountAsync(string activeCode)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.ActiveCode == activeCode);
            if(user == null || user.IsActive)
            {
                return false;
            }

            user.IsActive = true;
            user.ActiveCode = NameGenerator.GenerateUniqCode();
            _context.SaveChanges();

            return true;
        }

        public async Task<User?> LoginuserAsync(UserLoginViewModel user)
        {
            string hashPassword = PasswordHelper.EncodePasswordMd5(user.Password);
            string fixedEmail = FixedText.FixEmail(user.Email);

            return await _context.Users.FirstOrDefaultAsync(u => u.Email == fixedEmail &&
                u.Userpassword == hashPassword);
        }

        public async Task<User?> GetUserByNameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task DeletUserWithTokenAsync(User user)
        {
            if( user != null )
            {
                _context.Users.Remove(user);
                await SaveChangesAsync();
            }
        }

        public async Task<User?> GetUserWithUserName(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> ResetUserPassword(string password, string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                return false;

            user.Userpassword = PasswordHelper.EncodePasswordMd5(password);
            _context.SaveChangesAsync();

            return true;
        }
    }
}
