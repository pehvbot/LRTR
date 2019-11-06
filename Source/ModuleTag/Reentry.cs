using System;
using System.Collections.Generic;
using System.Text;

namespace LRTR.ModuleTags
{
    public class ModuleTagReentry : ModuleTag
    {
        public override string GetInfo()
        {
            string str = string.Empty;
            str = "Contains reentry equipment that needs proper integration and testing for safety. It will increase the overall rollout costs.\n\n" +
                "<b><color=orange>Rollout Cost: Cost of This Part * 1.5</color></b>";
            return str;
        }
    }
}
