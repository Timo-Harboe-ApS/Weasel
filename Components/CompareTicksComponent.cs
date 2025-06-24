using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

namespace Weasel.Components
{
    public class CompareTicksComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CompareTicks class.
        /// </summary>
        public CompareTicksComponent()
          : base("Compare Time", "CmpT", "Is A more recent than B?", "Weasel", "Time")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            Param_Time timeParamA = new Param_Time();
            timeParamA.Name = "Time A";
            timeParamA.NickName = "A";
            timeParamA.Description = "Time A for comparison";
            timeParamA.Access = GH_ParamAccess.item;
            timeParamA.Optional = false;
            pManager.AddParameter(timeParamA);

            Param_Time timeParamB = new Param_Time();
            timeParamB.Name = "Time B";
            timeParamB.NickName = "B";
            timeParamB.Description = "Time B for comparison";
            timeParamB.Access = GH_ParamAccess.item;
            timeParamB.Optional = false;
            pManager.AddParameter(timeParamB);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            Param_Boolean resultParam = new Param_Boolean();
            resultParam.Name = "Result";
            resultParam.NickName = "R";
            resultParam.Description = "True if Time A is more recent than Time B";
            resultParam.Access = GH_ParamAccess.item;
            pManager.AddParameter(resultParam);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DateTime timeA = DateTime.MinValue;
            DateTime timeB = DateTime.MinValue;

            if (!DA.GetData(0, ref timeA)) return;
            if (!DA.GetData(1, ref timeB)) return;

            bool isNewer = timeA.Ticks > timeB.Ticks;
            DA.SetData(0, isNewer);
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("78364590-2713-4D7D-A91C-C222DD9830EC"); }
        }
    }
}