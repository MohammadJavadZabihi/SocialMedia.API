using AutoMapper;
using SocialMedia.Core.DTOs;
using SocialMedia.DataLayer.Entites;

namespace SocialMedia.API.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserViewModel>();
            CreateMap<UserViewModel, User>();
            CreateMap<PartialUpdateUserViewModel, UserViewModel>();
            CreateMap<UserViewModel, PartialUpdateUserViewModel>();
            CreateMap<PartialUpdateUserViewModel, User>();
            CreateMap<User, PartialUpdateUserViewModel>();
            CreateMap<User, ResetPasswordViewModel>();
            CreateMap<ResetPasswordViewModel, User>();
            CreateMap<FullUpdateUserViewModel, User>();
            CreateMap<User, FullUpdateUserViewModel>();
        }
    }
}
