using System;
using System.Collections.Generic;
using System.Text;

namespace MyHealthPassAuth.System
{
    public struct RequestData
    {
        private string ipAddress;
        public string IpAddress
        {
            get { return ipAddress; }
            set { ipAddress = value;  }
        }

        private string userAgent;
        public string UserAgent
        {
            get { return userAgent;  }
            set { userAgent = value;  }
        }

        private string request;
        public string Request
        {
            get { return request; }
            set { request = value;  }
        }
    }
}
