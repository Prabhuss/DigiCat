using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Xamarin.Forms.Internals;

namespace PyConsumerApp.Models
{
    [Preserve(AllMembers = true)]
    [DataContract]
    public class MerchantContactDetailsModel
    {
        [DataMember(Name = "ContactNumber")]
        public string ContactNumber { get; set; }

        [DataMember(Name = "Email")]
        public string Email { get; set; }

        [DataMember(Name = "Website")]
        public string Website { get; set; }

        [DataMember(Name = "ShopName")]
        public string ShopName { get; set; }

        [DataMember(Name = "NatureofBusiness")]
        public string NatureofBusiness { get; set; }

        [DataMember(Name = "Location")]
        public string Location { get; set; }

        [DataMember(Name = "Longitude")]
        public string Longitude { get; set; }

        [DataMember(Name = "Latitude")]
        public string Latitude { get; set; }

        [DataMember(Name = "OpenHours")]
        public string OpenHours { get; set; }

        [DataMember(Name = "SocialMediaDetails")]
        public List<MerchantSocialMediaDetail> SocialMediaDetails { get; set; }
    }

    public class MerchantSocialMediaDetail
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "MerchantBranchId")]
        public int MerchantBranchId { get; set; }

        [DataMember(Name = "PlatformName")]
        public string PlatformName { get; set; }

        [DataMember(Name = "PlatformURL")]
        public string PlatformURL { get; set; }
    }
}
