using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monitis2GoogleDocs.Monitis.MonitisData
{
    class ExternalMonitorsInfo
    {

        public IDictionary<String, ExternalMonitorInfo> iExternalMonitorInfoList = new Dictionary<String, ExternalMonitorInfo>();


        public ExternalMonitorsInfo()
        {
            iExternalMonitorInfoList = new Dictionary<String, ExternalMonitorInfo>();
        }

        public void add(ExternalMonitorInfo _externalMonitorInfo)
        {
            iExternalMonitorInfoList.Add(_externalMonitorInfo.name, _externalMonitorInfo);
        }

    }
}
