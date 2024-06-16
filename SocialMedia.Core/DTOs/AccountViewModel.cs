using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.DTOs
{
    public class UserRegisterViewModel
    {
        [DisplayName("نام کاربری")]
        [MaxLength(255, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string UserName { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد.")]
        public string Password { get; set; }

        [DisplayName("بیوگرافی")]
        [MaxLength(255, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string Bio { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد.")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی‌باشد")]
        public string Email { get; set; }

        public bool IsAvtive { get; set; } = false;
    }

    public class UserViewModel
    {
        [DisplayName("نام کاربری")]
        [MaxLength(255, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string UserName { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد.")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی‌باشد")]
        public string Email { get; set; }

        [DisplayName("بیوگرافی")]
        [MaxLength(255, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string Bio { get; set; }
    }

    public class UserLoginViewModel
    {
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد.")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی‌باشد")]
        public string Email { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد.")]
        public string Password { get; set; }

    }

    public class ForgotPasswordViewModel
    {
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد")]
        public string Email { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [DisplayName("نام کاربری")]
        [MaxLength(255, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string UserName { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد.")]
        public string Password { get; set; }

        [Display(Name = "ایمیل")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد")]
        public string Email { get; set; } = "example@example.com";
    }

    public class PartialUpdateUserViewModel
    {
        [DisplayName("نام کاربری")]
        [MaxLength(255, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string UserName { get; set; }

        [DisplayName("بیوگرافی")]
        [MaxLength(255, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string Bio { get; set; }

        public IFormFile ImageName { get; set; }
    }

    public class FullUpdateUserViewModel
    {
        [DisplayName("نام کاربری")]
        [MaxLength(255, ErrorMessage = "{۰} نمیتواند بیشتر از {۱} کاراکتر باشد")]
        [Required(ErrorMessage = "لطفا را {0} وارد نمایید")]
        public string OldUserName { get; set; }

        [DisplayName("نام کاربری")]
        [MaxLength(255, ErrorMessage = "{۰} نمیتواند بیشتر از {۱} کاراکتر باشد")]
        [Required(ErrorMessage = "لطفا را {0} وارد نمایید")]
        public string NewUserName { get; set; }

        [DisplayName("ایمیل")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نیست")]
        [MaxLength(255, ErrorMessage = "{۰} نمیتواند بیشتر از {۱} کاراکتر باشد")]
        [Required(ErrorMessage = "لطفا را {0} وارد نمایید")]
        public string NewEmail { get; set; }

        [DisplayName("بیوگرافی")]
        [MaxLength(300, ErrorMessage = "{۰} نمیتواند بیشتر از {۱} کاراکتر باشد")]
        public string NewBio { get; set; }
    }

    public class UserForGetInformation
    {
        [DisplayName("نام کاربری")]
        [MaxLength(255, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string UserName { get; set; }

    }
}
