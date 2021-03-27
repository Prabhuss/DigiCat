using Newtonsoft.Json;
using PyConsumerApp.Controls;
using PyConsumerApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace PyConsumerApp.DataService
{
    public interface IOTPDataService
    {
        Task<OtpResponse> GenerateOtp(string phoneNumber, string NotificationToken);
        Task<bool> validateOtp(string merchantId, string phoneNumber, string otp);
    }


    [Preserve(AllMembers = true)]
    public class OTPDataService : BaseService, IOTPDataService
    {
        #region fields
        private static OTPDataService instance;
        #endregion

        #region Properties
        /// <summary>
        /// Gets an instance of the <see cref="PhotosDataService"/>.
        /// </summary>
        public static OTPDataService Instance => instance ?? (instance = new OTPDataService());
        #endregion

        public OTPDataService() : base()
        {

        }

        public async Task<OtpResponse> GenerateOtp( string phoneNumber,string NotificationToken)
        {
            string hashKey = System.Web.HttpUtility.UrlEncode(DependencyService.Get<IHashService>().GenerateHashkey());
            Dictionary<string, object> payload = new Dictionary<string, object>();
            payload.Add("phone_number", phoneNumber);
            payload.Add("reg_token", NotificationToken);
            payload.Add("hash_key", hashKey);

            OtpResponse robject = await this.Post<OtpResponse>(this.getAuthUrl("generateOtp_digicat"), payload, null);
            if (robject != null)
            {
                return robject;
            }
            return null;
        }

        public async Task<bool> validateOtp(string merchantId, string phoneNumber, string otp)
        {
            Dictionary<string, object> payload = new Dictionary<string, object>();
            payload.Add("otp", otp);
            payload.Add("phone_number", phoneNumber);
            payload.Add("merchant_id", merchantId);
            Dictionary<string, string> robject = await this.Post<Dictionary<string, string>>(this.getAuthUrl("otpValidateForDeliveryboy"), payload, null);
            if (robject != null)
            {
                if(robject["status"].ToLower()=="success")
                    return true;
            }
            return false;
        }
    }
}
