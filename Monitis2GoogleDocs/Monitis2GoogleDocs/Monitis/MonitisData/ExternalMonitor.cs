using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;


namespace Monitis2GoogleDocs.Monitis.MonitisData
{
    [DataContract]
    class ExternalMonitor
    {
        [DataMember]
        internal int id;

        [DataMember]
        internal int isSuspended;

        [DataMember]
        internal string name;

        [DataMember]
        internal string type;

    }
}
