using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Special;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using Weasel.Properties;
using Weasel.Components.Base;

namespace Weasel
{
    public abstract class WeaselComponent : GH_Component
    {

        // Override the GH_Component's Icon property properly
        protected override System.Drawing.Bitmap Icon => Resources.Weasel;

        // Abstract properties for authorship
        protected abstract string Author { get; }
        protected abstract string CoAuthor { get; }

        // Configuration properties - can be overridden if needed
        protected virtual bool AutoConfigureEnums => true;
        protected virtual bool ShowAuthorInDescription => true;

        protected WeaselComponent(string name, string nickName, string description, string category, string subCategory)
            : base(name, nickName, description, category, subCategory)
        {
            // Only attach event handler if auto-configuration is enabled
            if (AutoConfigureEnums)
            {
                this.Params.ParameterSourcesChanged += this.OnEnumParamSourcesChanged;
            }

            // Add author info to description if enabled
            if (ShowAuthorInDescription)
            {
                UpdateDescriptionWithAuthors();
            }
        }

        // Proper cleanup to prevent memory leaks
        public override void RemovedFromDocument(GH_Document document)
        {
            if (AutoConfigureEnums)
            {
                this.Params.ParameterSourcesChanged -= this.OnEnumParamSourcesChanged;
            }
            base.RemovedFromDocument(document);
        }

        // Event handler for parameter source changes with error handling
        private void OnEnumParamSourcesChanged(object sender, GH_ParamServerEventArgs e)
        {
            try
            {
                if (e?.Parameter is Param_Enum param && e.ParameterSide == GH_ParameterSide.Input)
                {
                    // Update Value Lists
                    foreach (IGH_Param source in e.Parameter.Sources.OfType<GH_ValueList>())
                    {
                        var valueList = source as GH_ValueList;
                        ConfigureEnumValueList(valueList, param);
                    }
                }
            }
            catch (Exception ex)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning,
                    $"Failed to configure enum parameter: {ex.Message}");
            }
        }

        // Separated enum configuration logic for cleaner code
        private void ConfigureEnumValueList(GH_ValueList valueList, Param_Enum param)
        {
            if (valueList == null || param?.EnumType == null) return;

            // Clear and configure the value list
            valueList.ListItems.Clear();
            valueList.Name = param.Name;
            valueList.NickName = param.Name;

            // Add enum values
            foreach (var enumValue in Enum.GetValues(param.EnumType))
            {
                string displayName = enumValue.ToString(); // Simple version without TEXT utility
                string value = ((int)enumValue).ToString();
                valueList.ListItems.Add(new GH_ValueListItem(displayName, value));
            }

            // Configure appearance and refresh
            valueList.SelectItem(0);
            valueList.ListMode = GH_ValueListMode.DropDown;
            valueList.ExpireSolution(false);
        }

        // Virtual method for extensibility - components can override this
        protected virtual void OnComponentAdded(GH_Document document) { }

        public override void AddedToDocument(GH_Document document)
        {
            OnComponentAdded(document);
            base.AddedToDocument(document);
        }

        // Helper method to update description with author information
        private void UpdateDescriptionWithAuthors()
        {
            var authorInfo = new List<string>();

            if (!string.IsNullOrEmpty(Author))
                authorInfo.Add($"Author: {Author}");

            if (!string.IsNullOrEmpty(CoAuthor))
                authorInfo.Add($"Co-Author: {CoAuthor}");

            if (authorInfo.Any())
            {
                Description += $"\n\n{string.Join("\n", authorInfo)}";
            }
        }
    }
}