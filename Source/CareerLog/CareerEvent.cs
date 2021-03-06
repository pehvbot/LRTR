﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace LRTR
{
    public abstract class CareerEvent : IConfigNode
    {
        [Persistent]
        public double UT;

        public CareerEvent(double ut)
        {
            UT = ut;
        }

        public CareerEvent(ConfigNode n)
        {
            Load(n);
        }

        public void Load(ConfigNode node)
        {
            ConfigNode.LoadObjectFromConfig(this, node);
        }

        public void Save(ConfigNode node)
        {
            ConfigNode.CreateConfigFromObject(this, node);
        }

        public bool IsInPeriod(LogPeriod p)
        {
            return UT >= p.StartUT && UT < p.EndUT;
        }
    }
}
