using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;
using Weasel.Components.Base;

namespace Weasel.Components
{
    public class EnumTestComponent : WeaselComponent
    {
        /// <summary>
        /// Initializes a new instance of the EnumTestComponent class.
        /// </summary>
        public EnumTestComponent()
          : base("EnumTestComponent", "enumTest",
              "description", "Weasle", "Time")
        {
        }

        protected override string Author => "Antoine Maes";
        protected override string CoAuthor => "";

        private enum TestEnum
        {
            Option1 = 0,
            Option2 = 1,
            Option3 = 2
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
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

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            // Add integer output for the enum value
            pManager.AddIntegerParameter("Value", "V", "The integer value of the selected enum", GH_ParamAccess.item);

            // Add string output for the enum name
            pManager.AddTextParameter("Name", "N", "The name of the selected enum", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
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

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("E8343DBA-A36D-4B87-9280-3CAC657DCA28"); }
        }
    }
}