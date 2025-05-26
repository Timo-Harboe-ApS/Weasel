using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Weasel.Components
{
    public class CompareTicksComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CompareTicks class.
        /// </summary>
        public CompareTicksComponent()
          : base("Compare Time", "CmpT", "Is A more recent than B?", "Weasle", "Time")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTimeParameter("Time A", "A", "First timestamp", GH_ParamAccess.item);
            pManager.AddTimeParameter("Time B", "B", "Second timestamp", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("Is Newer", "N", "True if Time A is more recent than Time B", GH_ParamAccess.item);
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
            get { return new Guid("78364590-2713-4D7D-A91C-C222DD9830EC"); }
        }
    }
}