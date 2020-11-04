using AutoMapper;
using Richviet.Services.Models;
using C = Richviet.API.DataContracts.Dto;
using M = Richviet.Services.Models;
using R = Richviet.API.DataContracts.Requests;

namespace Richviet.IoC.Configuration.AutoMapper.Profiles
{
    public class APIMappingProfile : Profile
    {
        public APIMappingProfile()
        {
            CreateMap<M.UserInfoView, C.UserInfoDTO>().ReverseMap();
            CreateMap<M.ReceiveBank, C.BankDTO>().ReverseMap();
            CreateMap<M.PayeeRelationType, C.RelationDTO>().ReverseMap();
            CreateMap<R.OftenBeneficiarRequest, M.OftenBeneficiar>().ForMember(x => x.PayeeType, opt => opt.Ignore()).ReverseMap();
            CreateMap<M.OftenBeneficiar, C.UserBeneficiarDTO>().ForMember(
              dest => dest.PayeeType
              , opt => opt.MapFrom(src => src.PayeeType.Type)
            ).ForMember(
              dest => dest.PayeeRelationType
              , opt => opt.MapFrom(src => src.PayeeRelation.Type)
            ).ReverseMap();
            CreateMap<M.OftenBeneficiar, M.OftenBeneficiar>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
