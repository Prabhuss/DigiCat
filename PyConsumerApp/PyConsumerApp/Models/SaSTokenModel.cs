using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;
using Xamarin.Forms.Internals;

namespace PyConsumerApp.Models
{
    [Preserve(AllMembers = true)]
    [DataContract]
    public class SaSTokenModel
    {
        [DataMember(Name = "status")]
        public string status { get; set; }

        [DataMember(Name = "SasToken")]
        public string SasToken { get; set; }

        [DataMember(Name = "blobURL")]
        public string blobURL { get; set; }

        [DataMember(Name = "StorageName")]
        public string StorageName { get; set; }
        
        [DataMember(Name = "ContainerName")]
        public string ContainerName { get; set; }
    }
}
