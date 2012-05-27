using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monitis2GoogleDocs.ApplUtils
{
    public class ExternalMonitor
    {
        private string externalMonitorName;
        private bool isSelected;


        public ExternalMonitor(string _externalMonitor, bool _isSelected)
        {
            this.externalMonitorName = _externalMonitor;
            this.isSelected = _isSelected;
        }


        public string ExternalMonitorName
        {
            get { return externalMonitorName; }
            set { externalMonitorName = value; }
        }

        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }

    }
}
