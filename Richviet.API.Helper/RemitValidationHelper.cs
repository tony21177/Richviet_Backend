using Frontend.DB.EF.Models;
using Richviet.API.DataContracts.Requests;
using Richviet.Services.Constants;
using Richviet.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Richviet.API.Helper
{
    public class RemitValidationHelper
    {
        private readonly IExchangeRateService exchangeRateService;

        private readonly ICurrencyService currencyService;

        private readonly IRemitSettingService remitSettingService;

        private readonly IDiscountService discountService;

        private readonly IBeneficiarService beneficiarService;

        private readonly IUploadPic uploadPicService;

        public RemitValidationHelper(IExchangeRateService exchangeRateService, ICurrencyService currencyService, IRemitSettingService remitSettingService,
            IDiscountService discountService, IBeneficiarService beneficiarService, IUploadPic uploadPicService)
        {
            this.exchangeRateService = exchangeRateService;
            this.currencyService = currencyService;
            this.remitSettingService = remitSettingService;
            this.discountService = discountService;
            this.beneficiarService = beneficiarService;
            this.uploadPicService = uploadPicService;
        }

        private string CheckIfAmountOutOfRange(int amount, string country)
        {
            BussinessUnitRemitSetting remitSetting = remitSettingService.GetRemitSettingByCountry(country);
            if (remitSetting == null) return "no remit setting for {country}";
            if (amount < remitSetting.RemitMin || amount > remitSetting.RemitMax) return "amount out of range";
            return null;
        }

        private string CheckBenificiarExistence(long id)
        {
            OftenBeneficiar beneficiar = beneficiarService.GetBeneficiarById(id);
            if (beneficiar == null)
            {
                return "beneficiar does not exist!";
            }
            return null;
        }

        private string CheckDiscountExistence(int id)
        {
            Discount discount = discountService.GetDoscountById(id);
            if (discount == null)
            {
                return "discount does not exist!";
            }
            return null;
        }

        private string CheckDiscountExistence(RemitRequest remitRequest)
        {
            Discount discount = discountService.GetDoscountById((long)remitRequest.DiscountId);
            if (discount == null)
            {
                return "discount does not exist!";
            }
            remitRequest.DiscountAmount = discount.Value;
            return null;
        }

        private string CheckCurrencyExistence(int id)
        {
            CurrencyCode currency = currencyService.GetCurrencyById(id);
            if (currency == null)
            {
                return "currency does not exist!";
            }
            return null;
        }

        private string CheckCurrencyExistence(RemitRequest remitRequest)
        {
            CurrencyCode currency = currencyService.GetCurrencyById(remitRequest.ToCurrencyId);
            if (currency == null)
            {
                return "currency does not exist!";
            }
            remitRequest.ToCurrency = currency.CurrencyName;
            remitRequest.FeeType = currency.FeeType;
            remitRequest.Fee = currency.Fee;
            return null;
        }

        private string CheckPhotoFileExistence(UserArc userArc, string filename)
        {
            if (!uploadPicService.CheckUploadFileExistence(userArc, PictureTypeEnum.Instant, filename))
                return "Photo file does not exist!";
            return null;
        }

        private string CheckSignatureFileExistence(UserArc userArc, string filename)
        {
            if (!uploadPicService.CheckUploadFileExistence(userArc, PictureTypeEnum.Signature, filename))
                return "Signature file does not exist!";
            return null;
        }

        public bool CheckIfKYCPassed(UserArc userArc)
        {

            if (userArc != null && userArc.KycStatus == (short)KycStatusEnum.PASSED_KYC_FORMAL_MEMBER)
            {
                return true;
            }
            return false;
        }

        public string CheckAndSetDraftRemitProperty(UserArc userArc, RemitRecord remitRecord, DraftRemitRequest draftRemitRequest, String country)
        {
            string error = null;
            if (draftRemitRequest.FromAmount != null)
            {
                error = CheckIfAmountOutOfRange((int)draftRemitRequest.FromAmount, country);
                if (error != null) return error;
                remitRecord.FromAmount = (double)draftRemitRequest.FromAmount;
            }

            if (draftRemitRequest.BeneficiarId != null)
            {
                error = CheckBenificiarExistence((int)draftRemitRequest.BeneficiarId);
                if (error != null) return error;
                remitRecord.BeneficiarId = draftRemitRequest.BeneficiarId;
            }


            if (draftRemitRequest.DiscountId != null)
            {
                error = CheckDiscountExistence((int)draftRemitRequest.DiscountId);
                if (error != null) return error;
                remitRecord.DiscountId = draftRemitRequest.DiscountId;
            }

            if (draftRemitRequest.ToCurrencyId != null)
            {
                error = CheckCurrencyExistence((int)draftRemitRequest.ToCurrencyId);
                if (error != null) return error;
                remitRecord.ToCurrencyId = draftRemitRequest.ToCurrencyId;
            }
            return null;
        }

        public string ValidateFormalRemitRequestAndUpdateRemitRecord(RemitRequest remitRequest, RemitRecord remitRecord, UserArc userArc, String country)
        {
            string error = null;

            // check ToCurrency
            error = CheckCurrencyExistence(remitRequest);
            if (error != null) return error;
            remitRecord.FeeType = remitRequest.FeeType;
            remitRecord.Fee = remitRequest.Fee;
            remitRecord.ToCurrencyId = remitRequest.ToCurrencyId;

            // check amount
            error = CheckIfAmountOutOfRange(remitRequest.FromAmount, "TW");
            if (error != null) return error;
            ExchangeRate applyExchangeRate = exchangeRateService.GetExchangeRateByCurrencyName(remitRequest.ToCurrency);
            remitRecord.FromAmount = remitRequest.FromAmount;
            remitRecord.ApplyExchangeRate = applyExchangeRate.Rate;
            remitRecord.FromCurrencyId = currencyService.GetCurrencyByCountry(country)[0].Id;

            // check beneficiar
            error = CheckBenificiarExistence(remitRequest.BeneficiarId);
            if (error != null) return error;
            remitRecord.BeneficiarId = remitRequest.BeneficiarId;

            // check uploaded picture
            error = CheckPhotoFileExistence(userArc, remitRecord.RealTimePic);
            if (error != null) return error;
            error = CheckSignatureFileExistence(userArc, remitRecord.ESignature);
            if (error != null) return error;

            // check discount
            if (remitRequest.DiscountId != null)
            {
                error = CheckDiscountExistence(remitRequest);
                if (error != null) return error;
                remitRecord.DiscountId = remitRequest.DiscountId;
                remitRecord.DiscountAmount = remitRequest.DiscountAmount;
            }

            remitRecord.TransactionStatus = (short)RemitTransactionStatusEnum.WaitingArcVerifying;

            return error;
        }
    }
}
