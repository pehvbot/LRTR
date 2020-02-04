using System;
using System.Collections.Generic;
using System.Text;

namespace LRTR.ModuleTags
{
    public class ModuleTagNotRescaled : ModuleTag
    {
        public override string GetInfo()
        {
            string str = string.Empty;
            str = "This part has not been rescaled for Real Solar System.";
            return str;
        }
    }
}