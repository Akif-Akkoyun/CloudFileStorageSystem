using AutoMapper;

namespace App.WebUI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Models.LoginViewModel, Dto.AuthDtos.LoginDto>()
                .ReverseMap();
            CreateMap<Models.RegisterViewModel, Dto.AuthDtos.RegisterDto>()
                .ReverseMap();
            CreateMap<Models.ForgotPasswordViewModel, Dto.AuthDtos.ForgotPasswordDto>()
                .ReverseMap();
            CreateMap<Models.RenewPasswordViewModel, Dto.AuthDtos.ResetPasswordDto>()
                .ReverseMap();
        }
    }
}
