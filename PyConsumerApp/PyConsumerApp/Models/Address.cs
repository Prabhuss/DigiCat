using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;
using Xamarin.Forms.Internals;

namespace PyConsumerApp.Models
{
    /// <summary>
    /// Model for address.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DataContract]
    public class Address
    {
        [DataMember(Name = "Id")]
        public int Id { get; set; }
        [DataMember(Name = "StoreCustomerId")]
        public int StoreCustomerId { get; set; }
        [DataMember(Name = "MerchantBranchId")]
        public int MerchantBranchId { get; set; }
        [DataMember(Name = "Address1")]

        public string Address1 { get; set; }

        [DataMember(Name = "Address2")]
        public string Address2 { get; set; }

        [DataMember(Name = "Longitude")]
        public string Longitude { get; set; }

        [DataMember(Name = "Latitude")]
        public string Latitude { get; set; }

        [DataMember(Name = "TagName")]
        public string TagName { get; set; }


        [DataMember(Name = "FirstName")]
        public string FirstName { get; set; }

        [DataMember(Name = "PrimaryPhone")]
        public string PrimaryPhone { get; set; }
        
        [DataMember(Name = "SecondaryPhone")]
        public string AlternatePhone { get; set; }

        [DataMember(Name = "Society/BuildingNo")]
        public string SocietyBuildingNo { get; set; }

        [DataMember(Name = "FlatNo/DoorNo")]
        public string FlatNoDoorNo { get; set; }

        [DataMember(Name = "City")]
        public string City { get; set; }

        [DataMember(Name = "State")]
        public string State { get; set; }

        [DataMember(Name = "Country")]
        public string Country { get; set; }

        [DataMember(Name = "Area")]
        public string Area { get; set; }

        [DataMember(Name = "PostalCode/ZipCode")]
        public string PostalCodeZipCode { get; set; }
        /*
        public int ID { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string DoorNo { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string AddressType { get; set; }
        public int UserId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }*/
    }
}
