using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widgets.HomePageNewProducts.Models
{
    public class ConfigurationModel : BaseNopModel
    {
        public ConfigurationModel()
        {
            this.WidgetZones = new List<string>();
        }
        public int ActiveStoreScopeConfiguration { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.HomePageNewProducts.WidgetZones")]
        [Display(Name = "Widget Zones")]
        public IEnumerable<string> WidgetZones { get; set; }

        public string WidgetZone { get; set; }

        public bool WidgetZone_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.HomePageNewProducts.NumberOfProducts")]
        [Display(Name = "Numper of Products")]
        public int NumberOfProducts { get; set; }

        public bool NumberOfProducts_OverrideForStore { get; set; }
    }
}
