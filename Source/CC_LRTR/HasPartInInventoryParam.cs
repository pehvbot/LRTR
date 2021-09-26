using ContractConfigurator.Parameters;
using System.Collections.Generic;
using UnityEngine;

namespace ContractConfigurator.LRTR
{
    public class HasPartInInventory : VesselParameter
    {
        protected string partName;
        protected double partCount;

        public HasPartInInventory() : base(null) { }

        public HasPartInInventory(string title, string partName, double partCount) : base(title)
        {
            this.partName = partName;
            this.partCount = partCount;
            this.title = GetParameterTitle();
        }

        protected override void OnParameterSave(ConfigNode node)
        {
            base.OnParameterSave(node);

            node.AddValue("partName", partName);
            node.AddValue("partCount", partCount);
        }

        protected override void OnParameterLoad(ConfigNode node)
        {
            base.OnParameterLoad(node);

            node.TryGetValue("partName", ref partName);
            node.TryGetValue("partCount", ref partCount);
        }

        protected override string GetParameterTitle()
        {
            AvailablePart p = PartLoader.getPartInfoByName(partName);
            if (partCount == 1)
                return $"Have at least 1 {p.title} in inventory";
            else
                return $"Have at least {partCount} {p.title}s in inventory";
        }

        protected override bool VesselMeetsCondition(Vessel vessel)
        {
            List<Part> parts = vessel.Parts;
            double count = 0;
     
            foreach(var part in parts)
            {
                PartModuleList modules = part.Modules;
                foreach (var module in modules)
                {
                    if (module.moduleName == "ModuleInventoryPart")
                    {
                        ModuleInventoryPart inv = (ModuleInventoryPart)module;
                        DictionaryValueList<int,StoredPart>  storedParts = inv.storedParts;
                        for(int i = 0; i < storedParts.Count; i++)
                        {
                            if (storedParts[i].partName == partName)
                                count += storedParts[i].quantity;
                        }
                    }
                }
            }
            return count >= partCount;
        }
    }
}
