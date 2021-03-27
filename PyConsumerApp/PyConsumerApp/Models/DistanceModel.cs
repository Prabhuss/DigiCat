using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Xamarin.Forms.Internals;

namespace PyConsumerApp.Models
{
    [Preserve(AllMembers = true)]
    [DataContract]
    public partial class DistanceData
    {
        [DataMember(Name = "distance")]
        public string distance { get; set; }


        [DataMember(Name = "active")]
        public string Active { get; set; }


        [DataMember(Name = "deliverable")]
        public string Deliverable { get; set; }


        [DataMember(Name = "Message")]
        public string Message { get; set; }

    }

    public class DistanceModel
    {
        [DataMember(Name = "status")]
        public string status { get; set; }


        [DataMember(Name = "data")]
        public DistanceData data { get; set; }

    }


}
