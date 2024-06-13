using Microsoft.EntityFrameworkCore;
using SocialMedia.DataLayer.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.DataLayer.Context
{
    public class SocialMediaContext : DbContext
    {
        public SocialMediaContext(DbContextOptions<SocialMediaContext> options) : base(options)
        {
            
        }

        #region UsersModel

        public DbSet<User> Users { get; set; }

        public DbSet<UserAuthenticationForApiKey> userAuthenticationForApiKeys { get; set; }

        #endregion
    }
}
