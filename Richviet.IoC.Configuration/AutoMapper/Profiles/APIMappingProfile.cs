using AutoMapper;
using C = Richviet.API.DataContracts.Dto;
using M = Richviet.Services.Models;

namespace Richviet.IoC.Configuration.AutoMapper.Profiles
{
    public class APIMappingProfile : Profile
    {
        public APIMappingProfile()
        {
            CreateMap<M.UserInfoView, C.UserInfoDTO>().ReverseMap();
        }
    }
}
