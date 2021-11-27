using ContractConfigurator.Parameters;
using System.Collections.Generic;
using UnityEngine;
using static Part;
using RealAntennas;

namespace ContractConfigurator.LRTR
{
    public class HasRealAntenna : VesselParameter
    {
        protected double minGain;
        protected double minAntennaDiameter;
        protected string RFBand;

        public HasRealAntenna() : base(null) { }

        public HasRealAntenna(string title, double minGain, double minAntennaDiameter, string RFBand) : base(title)
        {
            this.minGain = minGain;
            this.minAntennaDiameter = minAntennaDiameter;
            this.RFBand = RFBand;
            this.title = GetParameterTitle();
        }

        protected override void OnParameterSave(ConfigNode node)
        {
            base.OnParameterSave(node);

            node.AddValue("minGain", minGain);
            node.AddValue("minAntennaDiameter", minAntennaDiameter);
            node.AddValue("RFBand", RFBand);

        }

        protected override void OnParameterLoad(ConfigNode node)
        {
            base.OnParameterLoad(node);

            node.TryGetValue("minAntennaDiameter", ref minAntennaDiameter);
            node.TryGetValue("minGain", ref minGain);
            node.TryGetValue("RFBand", ref RFBand);
        }

        protected override string GetParameterTitle()
        {
            string r = "Have a Real Antenna";
            if (minGain > 0)
                r += " with a Gain of at least " + minGain + " dBi";
            if (minGain > 0 && minAntennaDiameter > 0 && RFBand != "")
                r += ", ";
            else if(minGain > 0 && minAntennaDiameter > 0)
                r += " and";
            if (minAntennaDiameter > 0)
                r += " a dish of at least " + minAntennaDiameter + "m diameter";
            if (minGain > 0 && minAntennaDiameter > 0 && RFBand != "")
                r += ", and";
            else if ((minGain > 0 || minAntennaDiameter > 0) && RFBand != "")
                r += " and";
                if (RFBand != "")
                r += " with RF Band:"+RFBand;
            return r;
        }

        protected override bool VesselMeetsCondition(Vessel vessel)
        {
            List<Part> parts = vessel.Parts;
            foreach (var part in parts)
            {
                PartModuleList modules = part.Modules;
                foreach (var module in modules)
                {
                    if (module.moduleName == "ModuleRealAntenna")
                    {
                        ModuleRealAntenna a = (ModuleRealAntenna)module;
                        bool mG, mAD, RB;

                        mG = a.Gain >= minGain;
                        mAD = a.antennaDiameter >= minAntennaDiameter;

                        if (RFBand == "")
                            RB = true;
                        else
                            RB = a.RFBand == RFBand;

                        if (mG && mAD && RB)
                            return true;
                    }
                }
            }
            return false;
        }
    }
}
