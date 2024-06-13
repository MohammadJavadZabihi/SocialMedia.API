using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.DataLayer.Entites
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [DisplayName("نام کاربری")]
        [MaxLength(255, ErrorMessage = "{۰} نمیتواند بیشتر از {۱} کاراکتر باشد")]
        [Required(ErrorMessage = "لطفا را {0} وارد نمایید")]
        public string UserName { get; set; }

        [DisplayName("کلمه عبور")]
        [MaxLength(255, ErrorMessage = "{۰} نمیتواند بیشتر از {۱} کاراکتر باشد")]
        [Required(ErrorMessage = "لطفا را {0} وارد نمایید")]
        public string Userpassword { get; set; }

        [DisplayName("ایمیل")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نیست")]
        [MaxLength(255, ErrorMessage = "{۰} نمیتواند بیشتر از {۱} کاراکتر باشد")]
        [Required(ErrorMessage = "لطفا را {0} وارد نمایید")]
        public string Email { get; set; }

        [DisplayName("بیوگرافی")]
        [MaxLength(300, ErrorMessage = "{۰} نمیتواند بیشتر از {۱} کاراکتر باشد")]
        public string Bio { get; set; }

        [DisplayName("پروفایل")]
        [MaxLength(300, ErrorMessage = "{۰} نمیتواند بیشتر از {۱} کاراکتر باشد")]
        public string ProfileURL { get; set; }
        [DisplayName("کد فعال سازی")]
        [MaxLength(50)]
        public string ActiveCode { get; set; }
        public bool IsActive { get; set; }
    }
}
