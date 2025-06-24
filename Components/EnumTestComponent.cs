using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;
using Weasel.Components.Base;
using Weasel.Properties;

namespace Weasel.Components
{
    public class EnumTestComponent : WeaselComponent
    {
        protected override string Author => "Antoine Maes";
        protected override string CoAuthor => "";

        public override Guid ComponentGuid => new Guid("E8343DBA-A36D-4B87-9280-3CAC657DCA28");

        // protected override System.Drawing.Bitmap Icon => Resources.<specific icon>;


        public EnumTestComponent()
          : base("EnumTestComponent", "enumTest",
              "description", "Weasel", "Time")
        {
        }

        private enum TestEnum
        {
            Option1 = 0,
            Option2 = 1,
            Option3 = 2
        }


        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            Param_Enum test = new Param_Enum(typeof(TestEnum));
            test.Name = "Test";
            test.NickName = "Test";
            test.Description = "Select a test enum value";
            test.Access = GH_ParamAccess.item;
            test.Optional = true;
            pManager.AddParameter(test);

            Param_Number numberTest = new Param_Number();
            numberTest.Name = "Number Test";
            numberTest.NickName = "NumTest";
            numberTest.Description = "Select a number test value";
            numberTest.Access = GH_ParamAccess.item;
            numberTest.Optional = true;
            pManager.AddParameter(numberTest);
        }


        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            // Add integer output for the enum value
            pManager.AddIntegerParameter("Value", "V", "The integer value of the selected enum", GH_ParamAccess.item);

            // Add string output for the enum name
            pManager.AddTextParameter("Name", "N", "The name of the selected enum", GH_ParamAccess.item);
        }



        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int enumValue = 0;
            bool hasInput = DA.GetData(0, ref enumValue);

            if (!hasInput)
            {
                // No input provided - don't produce any output
                AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, "No enum value selected");
                return;  // ← Key difference: EXIT early, no output
            }

            // Only process if we have actual input
            if (Enum.IsDefined(typeof(TestEnum), enumValue))
            {
                TestEnum selectedEnum = (TestEnum)enumValue;
                DA.SetData(0, enumValue);
                DA.SetData(1, selectedEnum.ToString());
            }
            else
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Invalid enum value selected");
            }
        }
    }
}