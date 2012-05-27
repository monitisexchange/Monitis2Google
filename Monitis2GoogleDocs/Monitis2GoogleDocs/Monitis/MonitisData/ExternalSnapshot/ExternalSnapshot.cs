using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Monitis2GoogleDocs.Monitis.MonitisData.ExternalSnapshot
{
    [DataContract]
    class ExternalSnapshot
    {

        [DataMember]
        internal int id;

        [DataMember]
        internal string name;

        [DataMember]
        internal List<Data> data = new List<Data>();

        [DataMember]
        internal string locationShortName;

    }
}
