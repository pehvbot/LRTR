using System;
using System.Collections.Generic;
using System.Linq;

namespace LRTR
{
    public class LaunchEvent : CareerEvent
    {
        [Persistent]
        public string VesselName;

        public LaunchEvent(double UT) : base(UT)
        {
        }

        public LaunchEvent(ConfigNode n) : base(n)
        {
        }
    }
}
