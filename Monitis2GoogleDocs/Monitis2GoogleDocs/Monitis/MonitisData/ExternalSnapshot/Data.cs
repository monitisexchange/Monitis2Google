using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Monitis2GoogleDocs.Monitis.MonitisData.ExternalSnapshot
{
    [DataContract]
    class Data
    {
        [DataMember]
        internal int id;

        [DataMember]
        internal string testType;

        [DataMember]
        internal string time;

        [DataMember]
        internal float perf;

        [DataMember]
        internal string status;

        [DataMember]
        internal string tag;

        [DataMember]
        internal string Default;

        [DataMember]
        internal string name;

        [DataMember]
        internal string frequency;

        [DataMember]
        internal int timeout;

    
    
    
    }
}
