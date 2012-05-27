using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Monitis2GoogleDocs.Monitis.MonitisData
{
    [DataContract]
    class Location
    {
        [DataMember]
        internal int id;

        [DataMember]
        internal string name;

        [DataMember]
        internal int checkInterval;

        [DataMember]
        internal string fullName;

    }
}
