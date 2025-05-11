using App.Dto.FileDtos;
using App.WebUI.Models;
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
            CreateMap<FileUploadViewModel,Dto.FileDtos.FileMetaDataDto>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.FilePath, opt => opt.Ignore()) 
                .ForMember(dest => dest.OwnerId, opt => opt.Ignore())
                .ForMember(dest => dest.UploadDate, opt => opt.Ignore());
        }
    }
}
