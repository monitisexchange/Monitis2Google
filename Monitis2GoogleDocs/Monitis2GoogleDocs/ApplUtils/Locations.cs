using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monitis2GoogleDocs.ApplUtils
{
    class Locations
    {
        public IDictionary<int, Monitis.MonitisData.Location> locationsUsed = new Dictionary<int, Monitis.MonitisData.Location>();
        public string listOfUsedLocationIds;

        public Locations(Monitis.MonitisData.ExternalMonitorsInfo _externalMonitorInfoList)
        {

            locationsUsed = new Dictionary<int, Monitis.MonitisData.Location>();
            foreach (Monitis.MonitisData.ExternalMonitorInfo externalMonitorInfo in _externalMonitorInfoList.iExternalMonitorInfoList.Values)
            {
                foreach (Monitis.MonitisData.Location location in externalMonitorInfo.locations)
                {
                    if (!locationsUsed.ContainsKey(location.id))
                    {
                        if (listOfUsedLocationIds != null)
                        {
                            listOfUsedLocationIds = listOfUsedLocationIds + ",";
                        }

                        locationsUsed.Add(location.id, location);
                        listOfUsedLocationIds = listOfUsedLocationIds + location.id;
                    }
                }

            }

        }
    }
}
