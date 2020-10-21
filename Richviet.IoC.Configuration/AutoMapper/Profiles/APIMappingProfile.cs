using AutoMapper;
using DC = Richviet.API.DataContracts;
using S = Richviet.Services.Models;

namespace Richviet.IoC.Configuration.AutoMapper.Profiles
{
    public class APIMappingProfile : Profile
    {
        public APIMappingProfile()
        {
            //CreateMap<DC.User, S.User>().ReverseMap();
            //CreateMap<DC.Address, S.Address>().ReverseMap();
        }
    }
}
