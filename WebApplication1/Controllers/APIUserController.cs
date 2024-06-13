using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Core.Convertor;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Generator;
using SocialMedia.Core.Security;
using SocialMedia.Core.Senders;
using SocialMedia.Core.Servies;
using SocialMedia.Core.Servies.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace SocialMedia.API.Controllers
{
    [ApiController]
    [Route("api/Users")]
    [Authorize]
    public class APIUserController : ControllerBase
    {
        #region Injection

        private readonly IUserServies _userServies;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IAuthentication _authenticationservies;
        private readonly SendEmailServies _sendEmailServies;
        public APIUserController(IUserServies userServies, IMapper mapper,
            IConfiguration configuration, IAuthentication authentication, SendEmailServies sendEmailServies)
        {
            _userServies = userServies ?? throw new ArgumentException(nameof(userServies));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _configuration = configuration ?? throw new ArgumentException(nameof(configuration));
            _authenticationservies = authentication ?? throw new ArgumentException(nameof(authentication));
            _sendEmailServies = sendEmailServies;

        }

        #endregion

        #region Get User FullInformation from DataBase

        [HttpGet]
        [Route("FullInforMation")]
        public async Task<ActionResult<IEnumerable<UserViewModel>>> GetFullInfoUsers()
        {
            try
            {
                var users = await _userServies.GetUsersAsync();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internet Server Error {ex.Message}");
            }
        }

        [HttpGet]
        [Route("FullInforMationWithUserName")]
        public async Task<ActionResult<IEnumerable<UserViewModel>>> FullInforMationWithUserName([FromBody] UserForGetInformation user)
        {
            try
            {
                var users = await _userServies.GetUserWithUserNameAndEmail(user.UserName, user.Email);

                if (users == null)
                    return NotFound(new
                    {
                        StatuceFindUser = $"Cannot Find user with {user.UserName} and {user.Email}"
                    });

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internet Server Error {ex.Message}");
            }
        }


        [Route("GetUserWithoutPassword")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserViewModel>>> GetUserWithoutPassword()
        {
            var users = await _userServies.GetUsersAsync();

            return Ok(_mapper.Map<IEnumerable<UserViewModel>>(users));
        }

        #endregion

        #region Register User

        [HttpPost]
        [Route("RegisterUser")]
        public async Task<ActionResult> RegisterUser([FromBody] UserRegisterViewModel userRegisterViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (_userServies.IsExistUser(userRegisterViewModel.UserName))
                {
                    return BadRequest("User Name is Invalid");
                }

                if (_userServies.IsExistEmail(userRegisterViewModel.Email))
                {
                    return BadRequest("Email is Invalid");
                }

                var userId = await _userServies.RegisterUserAsync(userRegisterViewModel);

                var user = await _userServies.GetUserByNameAsync(userRegisterViewModel.UserName);

                var token = await _authenticationservies.GetJWTTokenForCookiesAnaAuthenticate(user.UserName,
    user.Email, user.ActiveCode, _configuration["Authentication:SecretForKey"]);

                string linkActivateUserAccount;
                if (user != null)
                {
                    linkActivateUserAccount = $"https://localhost:8080/api/Users/ActiveUserAccount/{user.ActiveCode}";
                }
                else
                {
                    linkActivateUserAccount = "Cannot Find User Activate Code";
                }

                return Ok(new
                {
                    UserId = userId,
                    Email = userRegisterViewModel.Email,
                    UserName = userRegisterViewModel.UserName,
                    StatuceActivatUserAccount = userRegisterViewModel.IsActive,
                    StatuceUserRegiser = "SuccessFully",
                    ActivateLink = linkActivateUserAccount,
                    Token = token
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internet Server Error : {ex.Message}");
            }
        }

        #endregion

        #region Partial Update for User Table

        [HttpPatch]
        [Route("PartialUpdate/{id}")]
        public async Task<ActionResult> UpdateUserPatch(int id, JsonPatchDocument<PartialUpdateUserViewModel> patchDoc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (patchDoc == null)
            {
                return BadRequest(ModelState);
            }

            var existUser = await _userServies.GetUserByIdAsync(id);
            if (existUser == null)
            {
                return NotFound();
            }

            try
            {
                var userVieModel = _mapper.Map<PartialUpdateUserViewModel>(existUser);

                patchDoc.ApplyTo(userVieModel, ModelState);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updateUser = _mapper.Map(userVieModel, existUser);
                _userServies.UpdateUser(updateUser);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Interner Server Error : {ex.Message}");
            }
        }

        #endregion

        #region Update(Put)

        [HttpPut]
        [Route("FullUpdate")]
        public async Task<ActionResult> UpdateUser([FromBody] FullUpdateUserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var username = user.UserName;
            var existUser = await _userServies.GetUserByNameAsync(username);

            if (existUser == null)
            {
                return NotFound(new
                {
                    StatuceFindUser = $"NotFound User with {user.UserName}"
                });
            }

            try
            {
                var userVieModel = _mapper.Map<FullUpdateUserViewModel>(existUser);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                existUser.Email = user.Email;
                existUser.Bio = user.Bio;

                return Ok(new
                {
                    StatuceUpdateUser = "SuccessFully",
                    UserName = user.UserName,
                    ReplaceEmail = user.Email,
                    ReplaceBio = user.Bio,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Interner Server Error : {ex.Message}");
            }
        }

        #endregion

        #region Delet User with User ID

        [HttpDelete]
        [Route("DelelUser")]
        public async Task<ActionResult> DeletUser([FromBody] int userId)
        {
            var user = await _userServies.GetSingleUserAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _userServies.DeletUser(user);
                return Ok(new
                {
                    DeletUserStatuce = "SuccessFully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internet Server Error : {ex.Message}");
            }
        }

        #endregion

        #region DeletUserWithUserName

        [HttpDelete]
        [Route("DeletUserWithUserName")]
        public async Task<ActionResult> DeletUserWithToken([FromBody] UserForGetInformation user)
        {
            try
            {
                var exixstUser = await _userServies.GetUserWithUserNameAndEmail(user.UserName, user.Email);

                if (user == null)
                {
                    return NotFound("Not Found User For Deleting");
                }

                _userServies.DeletUserWithTokenAsync(exixstUser);

                return Ok("User Deleted SuccessFully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Interner Server Error : {ex.Message}");
            }
        }

        #endregion

        #region Edite user Password

        [HttpPatch]
        [Route("ResetUserPassword/{activeCode}")]
        public async Task<ActionResult> ResetUserPassword(string activeCode,
            [FromBody] JsonPatchDocument<ResetPasswordViewModel> resetPatchDoc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existUser = await _userServies.GetUserByActiveCodeAsync(activeCode);

            if (existUser == null)
            {
                return NotFound();
            }

            try
            {
                var userViewModel = _mapper.Map<ResetPasswordViewModel>(existUser);
                resetPatchDoc.ApplyTo(userViewModel, ModelState);

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var updatePasswordUser = _mapper.Map(userViewModel, existUser);
                updatePasswordUser.Userpassword = PasswordHelper.EncodePasswordMd5(userViewModel.Password);

                updatePasswordUser.ActiveCode = NameGenerator.GenerateUniqCode();

                _userServies.UpdateUser(updatePasswordUser);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internet Servies Error {ex.Message}");
            }
        }

        #endregion

        #region Active user

        [Route("ActiveUserAccount/{Id}")]
        [HttpPost]
        public async Task<ActionResult> ActiveUserAccount(string Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool IsSuccess = await _userServies.ActiveUserAccountAsync(Id);

            if (!IsSuccess)
            {
                return BadRequest();
            }

            return Ok("User Account Active Successesfully");

        }

        #endregion

        #region Login User

        [Route("LoginUser")]
        [HttpPost]
        public async Task<ActionResult> LoginUser([FromBody] UserLoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userServies.LoginuserAsync(login);

            if (user != null)
            {
                var token = await _authenticationservies.GetJWTTokenForCookiesAnaAuthenticate(user.UserName,
                    user.Email, user.ActiveCode, _configuration["Authentication:SecretForKey"]);

                if (user.IsActive)
                {
                    return Ok(new
                    {
                        UserLoginStatuce = "SuccessFully",
                        UserId = user.UserId,
                        UserName = user.UserName,
                        UserEmail = user.Email,
                        UserActiveAccount = true,
                        token = token
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        UserLoginStatuce = "Unsuccessful",
                        UserActiveAccount = false,
                        UserName = user.UserName,
                        UserEmail = user.Email,
                    });
                }
            }
            else
            {
                return NotFound($"NotFound User Account with {login.Email} email");
            }
        }

        #endregion
    }
}