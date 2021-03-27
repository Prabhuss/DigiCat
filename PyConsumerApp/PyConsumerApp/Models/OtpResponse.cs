using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Xamarin.Forms.Internals;

namespace PyConsumerApp.Models
{
    [Preserve(AllMembers = true)]
    [DataContract]
    public partial class OtpResponse
    {
        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "access_key")]
        public string AccessKey { get; set; }

        [DataMember(Name = "data")]
        public OtpApiData Data { get; set; }
    }

    public partial class OtpApiData
    {
        [DataMember(Name = "ID")]
        public string Id { get; set; }

        [DataMember(Name = "MerchantBranchId")]
        public string MerchantBranchId { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        [DataMember(Name = "Address")]
        public string Address { get; set; }

        [DataMember(Name = "CreatePermission")]
        public string CreatePermission { get; set; }

        [DataMember(Name = "DeletePermission")]
        public string DeletePermission { get; set; }

        [DataMember(Name = "CreatedDate")]
        public string CreatedDate { get; set; }

        [DataMember(Name = "ModifiedDate")]
        public string ModifiedDate { get; set; }

        [DataMember(Name = "IsActive")]
        public string IsActive { get; set; }

        [DataMember(Name = "Role")]
        public string Role { get; set; }
    }

}
