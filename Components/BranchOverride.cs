using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Weasel.Components
{
    public class BranchOverride : WeaselComponent
    {
        protected override string Author => "Antoine Maes";
        protected override string CoAuthor => "";
        public override Guid ComponentGuid => new Guid("5588A689-44DF-470E-9F9E-202C58755F03");
        // protected override System.Drawing.Bitmap Icon => Resources.<specific icon>;
        public BranchOverride()
            : base("BranchOverride", "BrOver",
                "Override branches in tree", "Weasel", "Trees")
        {
        }
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            Param_GenericObject param_MainTree = new Param_GenericObject();
            param_MainTree.Name = "Original Tree";
            param_MainTree.NickName = "OGTree";
            param_MainTree.Description = "Original tree";
            param_MainTree.Access = GH_ParamAccess.tree;
            param_MainTree.Optional = false;
            pManager.AddParameter(param_MainTree);

            Param_GenericObject param_SecondTree = new Param_GenericObject();
            param_SecondTree.Name = "Second tree";
            param_SecondTree.NickName = "SeTree";
            param_SecondTree.Description = "Tree that will override branches in 'Original Tree'";
            param_SecondTree.Access = GH_ParamAccess.tree;
            param_SecondTree.Optional = true;
            pManager.AddParameter(param_SecondTree);

            Param_Boolean param_Boolean = new Param_Boolean();
            param_Boolean.Name = "Add unmatched branches";
            param_Boolean.NickName = "Add";
            param_Boolean.Description = "Add unmatched branches into 'Original Tree' on True, ignores them on False";
            param_Boolean.Access = GH_ParamAccess.item;
            param_Boolean.Optional = true;
            pManager.AddParameter(param_Boolean);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            Param_GenericObject param_ResultTree = new Param_GenericObject();
            param_ResultTree.Name = "Result Tree";
            param_ResultTree.NickName = "Result";
            param_ResultTree.Description = "Resulting tree from the override";
            param_ResultTree.Access = GH_ParamAccess.tree;
            pManager.AddParameter(param_ResultTree);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_Structure<IGH_Goo> originalTree = null;
            GH_Structure<IGH_Goo> secondTree = null;
            bool addUnmatched = false;

            if (!DA.GetDataTree(0, out originalTree)) return;
            if (!DA.GetDataTree(1, out secondTree))
            {
                // If no secondary tree provided, just output the original tree
                DA.SetDataTree(0, originalTree);
                return;
            }

            DA.GetData(2, ref addUnmatched);

            GH_Structure<IGH_Goo> resultTree = OverrideBranches(originalTree, secondTree, addUnmatched);
            DA.SetDataTree(0, resultTree);
        }

        private GH_Structure<IGH_Goo> OverrideBranches(GH_Structure<IGH_Goo> originalTree, GH_Structure<IGH_Goo> secondTree, bool addUnmatched)
        {
            // Create a new structure as a copy of the original
            GH_Structure<IGH_Goo> resultTree = new GH_Structure<IGH_Goo>();

            // First, copy all branches from original tree
            foreach (GH_Path path in originalTree.Paths)
            {
                var originalBranch = originalTree.get_Branch(path);
                foreach (IGH_Goo item in originalBranch)
                {
                    resultTree.Append(item, path);
                }
            }

            // Get paths from both trees
            List<GH_Path> originalPaths = ExtractPathsFromTree(originalTree);
            List<GH_Path> secondaryPaths = ExtractPathsFromTree(secondTree);

            // Override matching branches and handle unmatched ones
            foreach (GH_Path secondaryPath in secondaryPaths)
            {
                bool pathExists = false;

                // Check if this path exists in the original tree
                foreach (GH_Path originalPath in originalPaths)
                {
                    if (originalPath.IsCoincident(secondaryPath))
                    {
                        pathExists = true;
                        // Override the branch in the result tree
                        var secondaryBranch = secondTree.get_Branch(secondaryPath);
                        resultTree.RemovePath(secondaryPath); // Remove existing branch
                        foreach (IGH_Goo item in secondaryBranch)
                        {
                            resultTree.Append(item, secondaryPath);
                        }
                        break;
                    }
                }

                // If path doesn't exist in original and addUnmatched is true, add it
                if (!pathExists && addUnmatched)
                {
                    var secondaryBranch = secondTree.get_Branch(secondaryPath);
                    foreach (IGH_Goo item in secondaryBranch)
                    {
                        resultTree.Append(item, secondaryPath);
                    }
                }
            }

            return resultTree;
        }

        private List<GH_Path> ExtractPathsFromTree(GH_Structure<IGH_Goo> tree)
        {
            List<GH_Path> paths = new List<GH_Path>();

            if (tree != null)
            {
                foreach (GH_Path path in tree.Paths)
                {
                    paths.Add(path);
                }
            }

            return paths;
        }
    }
}