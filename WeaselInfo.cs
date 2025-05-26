using System;
using System.Drawing;
using Grasshopper;
using Grasshopper.Kernel;

namespace Weasel
{
    public class WeaselInfo : GH_AssemblyInfo
    {
        public override string Name => "Weasel";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => null;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "";

        public override Guid Id => new Guid("a4d226a1-d019-4869-9376-0c50c6aa3a34");

        //Return a string identifying you or your company.
        public override string AuthorName => "";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "";

        //Return a string representing the version.  This returns the same version as the assembly.
        public override string AssemblyVersion => GetType().Assembly.GetName().Version.ToString();
    }
}