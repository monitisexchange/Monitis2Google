using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Monitis2GoogleDocs.Monitis.MonitisData
{
    [DataContract]
    class ExternalMonitorInfo
    {

        [DataMember]
        internal string startDate;

        [DataMember]
        internal string postData;

        [DataMember]
        internal int interval;

        [DataMember]
        internal int testId;

        [DataMember]
        internal string detailedType;

        [DataMember]
        internal string authPassword;

        [DataMember]
        internal string tag;

        [DataMember]
        internal string authUsername;

        [DataMember]
        internal Param_s param_s;

        [DataMember]
        internal string type;

        [DataMember]
        internal string url;

        [DataMember]
        internal List<Location> locations = new List<Location>();

        [DataMember]
        internal string name;

        [DataMember]
        internal string sla;

        [DataMember]
        internal string match;

        [DataMember]
        internal string matchText;
             
        [DataMember]
        internal int timeout;


    }

}
