using AutoMapper;
using Richviet.API.DataContracts.Dto;
using Richviet.IoC.Configuration.AutoMapper.Converter;
using Richviet.Services.Models;
using System;
using C = Richviet.API.DataContracts.Dto;
using M = Richviet.Services.Models;
using R = Richviet.API.DataContracts.Requests;
using A = Richviet.Admin.API.DataContracts.Dto;

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
            CreateMap<CurrencyCode, CurrencyInfoDTO>().ReverseMap();
            CreateMap<ExchangeRate,ExchangeRateDTO>().ReverseMap();
            CreateMap<DateTimeOffset, long>().ConvertUsing<UnixTimestampDateTimeOffsetConverter>();
            CreateMap<DateTime, long>().ConvertUsing<UnixTimestampDateTimeConverter>();

            CreateMap<BussinessUnitRemitSetting,RemitSettingDTO>().ReverseMap();
            //Admin
            CreateMap<M.ReceiveBank, A.EditBankDTO>().ReverseMap();
            CreateMap<M.PayeeRelationType, A.EditRelationDTO>().ReverseMap();
        }
    }
}
