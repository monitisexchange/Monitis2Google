using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Monitis2GoogleDocs.Monitis.MonitisData.ExternalSnapshot
{
    [DataContract]
    class ExternalSnapshots
    {
        [DataMember]
        internal List<ExternalSnapshot> resultList = new List<ExternalSnapshot>();


    }
}
