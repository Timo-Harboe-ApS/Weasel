using Grasshopper.Kernel.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weasel.Components.Base
{
    public class Param_Enum : Param_Integer
    {
        public Type EnumType;

        public Param_Enum(Type enumType) : base()
        {
            EnumType = enumType;
        }

    }
}
