using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Nop.Core.Configuration;

namespace Nop.Plugin.Widgets.HomePageNewProducts
{
    public class HomePageNewProductsSettings : ISettings
    {
        public string WidgetZone { get; set; }

        public int NumberOfProducts { get; set; }
    }
}
