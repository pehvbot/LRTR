using System.Collections.Generic;
using UnityEngine;

namespace KerbalConstructionTime
{
    public class LRTRHomeWorldParameters : IConfigNode
    {
        [Persistent]
        public double karmanAltitude = FlightGlobals.GetHomeBody().atmosphereDepth;
        [Persistent]
        public float hoursPerDay = KSPUtil.dateTimeFormatter.Day / 3600;
        [Persistent]
        public float daysPerYear = KSPUtil.dateTimeFormatter.Year / KSPUtil.dateTimeFormatter.Day;
        [Persistent]
        public string kctPresetShortName = "LRTR_STOCK";

        public void Load(ConfigNode node)
        {
            ConfigNode.LoadObjectFromConfig(this, node);
        }

        public void Save(ConfigNode node)
        {
            ConfigNode.CreateConfigFromObject(this, node);
        }
    }
}