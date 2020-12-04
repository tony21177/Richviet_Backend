using AutoMapper;
using Richviet.API.DataContracts.Dto;
using Richviet.API.DataContracts.Requests;
using Richviet.IoC.Configuration.AutoMapper.Converter;
using Frontend.DB.EF.Models;
using System;
using C = Richviet.API.DataContracts.Dto;
using M = Frontend.DB.EF.Models;
using R = Richviet.API.DataContracts.Requests;
using A = Richviet.Admin.API.DataContracts.Dto;
using Richviet.Services.Constants;

namespace Richviet.IoC.Configuration.AutoMapper.Profiles
{
    public class APIMappingProfile : Profile
    {
        public APIMappingProfile()
        {
            CreateMap<M.UserInfoView, C.UserInfoDTO>().ReverseMap();
            CreateMap<M.ReceiveBank, C.BankDTO>().ReverseMap();
            CreateMap<M.PayeeRelationType, C.RelationDTO>().ReverseMap();
            CreateMap<R.OftenBeneficiaryRequest, M.OftenBeneficiary>().ForMember(x => x.PayeeType, opt => opt.Ignore()).ReverseMap();

            CreateMap<M.OftenBeneficiary, C.UserBeneficiaryDTO>().ForMember(
              dest => dest.PayeeType
              , opt => opt.MapFrom(src => src.PayeeType.Type)
            ).ForMember(
              dest => dest.PayeeRelationType
              , opt => opt.MapFrom(src => src.PayeeRelation.Type)
            ).ReverseMap();

            CreateMap<M.OftenBeneficiary, M.OftenBeneficiary>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<CurrencyCode, CurrencyInfoDTO>().ReverseMap();

            CreateMap<ExchangeRate,ExchangeRateDTO>().ReverseMap();
            CreateMap<DateTimeOffset, long>().ConvertUsing<UnixTimestampDateTimeOffsetConverter>();
            CreateMap<DateTime, long>().ConvertUsing<UnixTimestampDateTimeConverter>();

            CreateMap<BussinessUnitRemitSetting,RemitSettingDTO>().ReverseMap();

            CreateMap<RemitRecord,RemitRecordDTO>().ReverseMap();

            _ = CreateMap<RemitRecord, RemitRecordDTO>().ForMember(
              dest => dest.PayeeName
              , opt => opt.MapFrom(src => src.Beneficiary.Name)
            ).ForMember(
              dest => dest.PayeeAddress
              , opt => opt.MapFrom(src => src.Beneficiary.PayeeAddress)
            ).ForMember(
              dest => dest.PayeeRelationType
              , opt => opt.MapFrom(src => src.Beneficiary.PayeeRelation.Type)
            ).ForMember(
              dest => dest.PayeeRelationTypeDescription
              , opt => opt.MapFrom(src => src.Beneficiary.PayeeRelation.Description)
            ).ForMember(
              dest => dest.ToCurrency
              , opt => opt.MapFrom(src => src.ToCurrency.CurrencyName)
            ).ForMember(
              dest => dest.PaymentCode
              , opt => opt.MapFrom(src => src.PaymentCode.Split(',', StringSplitOptions.None))
            ).ReverseMap();


            CreateMap<M.PushNotificationSetting, C.NotificationDTO>().ReverseMap();

            //Admin
            CreateMap<M.ReceiveBank, A.EditBankDTO>().ReverseMap();
            CreateMap<M.PayeeRelationType, A.EditRelationDTO>().ReverseMap();
            CreateMap<M.BussinessUnitRemitSetting, A.EditRemitSettingDTO>().ReverseMap();
            CreateMap<M.CurrencyCode, A.EditCurrencyInfoDTO>().ReverseMap();
            CreateMap<M.RemitRecord, A.RemitRecordAdminDTO>().ReverseMap();
            _ = CreateMap < M.RemitRecord, A.RemitRecordAdminDTO>().ForMember(
              dest => dest.PayeeName
              , opt => opt.MapFrom(src => src.Beneficiary.Name)
            ).ForMember(
              dest => dest.PayeeAddress
              , opt => opt.MapFrom(src => src.Beneficiary.PayeeAddress)
            ).ForMember(
              dest => dest.PayeeRelationType
              , opt => opt.MapFrom(src => src.Beneficiary.PayeeRelation.Type)
            ).ForMember(
              dest => dest.PayeeRelationTypeDescription
              , opt => opt.MapFrom(src => src.Beneficiary.PayeeRelation.Description)
            ).ForMember(
              dest => dest.ToCurrency
              , opt => opt.MapFrom(src => src.ToCurrency.CurrencyName)
            ).ForMember(
              dest => dest.PaymentCode
              , opt => opt.MapFrom(src => src.PaymentCode.Split(',', StringSplitOptions.None))
            ).ReverseMap();
        }
    }
}
