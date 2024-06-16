using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
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

        #region Register User

        [HttpPost]
        [Route("RegisterUser")]
        public async Task<ActionResult> RegisterUser([FromBody] UserRegisterViewModel Register)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (_userServies.IsExistUser(Register.UserName))
                {
                    return BadRequest("Cannot Chose this User name");
                }

                if (_userServies.IsExistEmail(Register.Email))
                {
                    return BadRequest("Cannot Chose this Email");
                }

                var user = await _userServies.RegisterUserAsync(Register);

                if (user != null)
                {
                    var token = await _authenticationservies.GetJWTTokenForCookiesAnaAuthenticate(user.UserName,
                                    user.Email, user.ActiveCode, _configuration["Authentication:SecretForKey"], user.IsActive);

                    return Ok(new
                    {
                        UserName = user.UserName,
                        Email = user.Email,
                        Token = token,
                        StatuceUserRegiser = "SuccessFully",

                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        Statuce = "Register Is UnSuccessFully"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internet Server Error : {ex.Message}");
            }
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
                    user.Email, user.ActiveCode, _configuration["Authentication:SecretForKey"], user.IsActive);

                if (user.IsActive)
                {
                    return Ok(new
                    {
                        UserLoginStatuce = "SuccessFully",
                        UserName = user.UserName,
                        UserEmail = user.Email,
                        token = token,
                        Bio = user.Bio,
                        UserActiveAccount = true,

                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        UserLoginStatuce = "Unsuccessful, The user has not activated his account",
                        UserName = user.UserName,
                        UserEmail = user.Email,
                        UserActiveAccount = false
                    });
                }
            }
            else
            {
                return NotFound($"NotFound User Account with {login.Email} email");
            }
        }

        #endregion

        #region Edite user Password When User Is Login

        [HttpPost]
        [Authorize]
        [Route("ResetUserPassword")]
        public async Task<ActionResult> ResetUserPassword([FromBody] ResetPasswordViewModel resetPatss)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existUser = await _userServies.GetUserByNameAsync(resetPatss.UserName);

            if (existUser == null)
            {
                return NotFound();
            }

            try
            {
                existUser.Userpassword = PasswordHelper.EncodePasswordMd5(resetPatss.Password);
                _userServies.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internet Servies Error {ex.Message}");
            }
        }

        #endregion

        #region Edite User Password When User Forgot

        [HttpPost]
        [Authorize]
        [Route("ResetUserPassword/Forgot")]
        public async Task<ActionResult> ResetPassForGot([FromBody] ResetPasswordViewModel reset)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (reset.Email == null)
                return BadRequest("لطفا ایمیل را وارد نمایید");

            var exixstUser = await _userServies.GetUserByEmailAsync(reset.Email);

            if (exixstUser == null)
                return NotFound($"موجود نیست {reset.Email} کاربر مورد نظر با ایمیل");

            if (!exixstUser.IsActive)
                return BadRequest("حساب کاربری شما فعال نیست");

            try
            {
                var resesetpassw = await _userServies.ResetUserPassword(reset.Password, reset.Email);

                if (!resesetpassw)
                {
                    return BadRequest(new
                    {
                        Statuce = "UnSuccessFully"
                    });
                }

                return Ok(new
                {
                    Statuce = "SuccessFully",
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Internet Server Error : {ex.Message}");
            }
        }

        #endregion

        #region Update(Put)

        [HttpPut]
        [Route("FullUpdate")]
        [Authorize]
        public async Task<ActionResult> UpdateUser([FromBody] FullUpdateUserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existUser = await _userServies.GetUserByNameAsync(user.OldUserName);

            if (existUser == null)
            {
                return NotFound(new
                {
                    StatuceFindUser = $"NotFound User with {user.OldUserName}"
                });
            }

            try
            {
                var userVieModel = _mapper.Map<FullUpdateUserViewModel>(existUser);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if(_userServies.IsExistUser(userVieModel.NewUserName))
                    return BadRequest("نام کاربری در دسترس نیست, لطفا نام کاربری دیگری انتخواب کنید");

                existUser.UserName = user.NewUserName;
                existUser.Email = user.NewEmail;
                existUser.Bio = user.NewBio;

                _userServies.SaveChangesAsync();

                return Ok(new
                {
                    StatuceUpdateUser = "SuccessFully",
                    NewUserName = user.NewUserName,
                    NewEmail = user.NewEmail,
                    NewReplaceBio = user.NewBio,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Interner Server Error : {ex.Message}");
            }
        }

        #endregion

        #region DeletUserWithUserName

        [HttpDelete]
        [Route("DeletUserWithUserName")]
        [Authorize]
        public async Task<ActionResult> DeletUserWithToken([FromBody] UserForGetInformation user)
        {
            try
            {
                var exixstUser = await _userServies.GetUserWithUserName(user.UserName);

                if (exixstUser == null)
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

        #region Active user

        [Route("ActiveUserAccount/{Id}")]
        [HttpPost]
        [Authorize]
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
    }
}