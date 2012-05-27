using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Monitis2GoogleDocs.Monitis.MonitisData
{
    [DataContract]
    class ExternalMonitors
    {
        [DataMember]
        internal List<ExternalMonitor> testList = new List<ExternalMonitor>();

        private IDictionary<String, ExternalMonitor> iTestList = new Dictionary<String, ExternalMonitor>();

        public void setITestList()
        {
            iTestList = new Dictionary<String, ExternalMonitor>();
            foreach (ExternalMonitor ext in testList)
            {
                iTestList.Add(ext.name, ext);
            }

        }

        public IDictionary<String, ExternalMonitor> getItestList()
        {
            return iTestList;
        }


        public int getExternalMonitorId(string _monitorName)
        {
            return iTestList[_monitorName].id;
        }

    }
}
