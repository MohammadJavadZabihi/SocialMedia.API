using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.DataLayer.Entites
{
    public class UserAuthenticationForApiKey
    {
        [Key]
        public int UserId { get; set; }

        [DisplayName("نام کاربری")]
        [MaxLength(255, ErrorMessage = "{0} نمیتواند بیشتر از {۱} کاراکتر باشد")]
        [Required(ErrorMessage = "لطفا را {0} وارد نمایید")]
        public string UserName { get; set; }

        [DisplayName("کلمه عبور")]
        [MaxLength(255, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        [Required(ErrorMessage = "لطفا را {0} وارد نمایید")]
        public string Password { get; set; }
    }
}
