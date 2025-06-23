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

        // Constructor without default value (original behavior)
        public Param_Enum(Type enumType) : base()
        {
            EnumType = enumType;
            // No default value set - parameter will be empty until user provides input
        }

        // Constructor with automatic default (first enum value)
        public Param_Enum(Type enumType, bool setDefaultValue) : base()
        {
            EnumType = enumType;

            if (setDefaultValue && enumType.IsEnum)
            {
                var firstValue = Enum.GetValues(enumType).Cast<int>().FirstOrDefault();
                SetPersistentData(firstValue);
            }
        }

        public Param_Enum(Type enumType, object defaultValue) : base()
        {
            EnumType = enumType;

            // Set custom default value
            if (enumType.IsEnum && Enum.IsDefined(enumType, defaultValue))
            {
                SetPersistentData((int)defaultValue);
            }
            else if (enumType.IsEnum)
            {
                // Fallback to first enum value if invalid default provided
                var firstValue = Enum.GetValues(enumType).Cast<int>().FirstOrDefault();
                SetPersistentData(firstValue);
            }
        }
    }
}