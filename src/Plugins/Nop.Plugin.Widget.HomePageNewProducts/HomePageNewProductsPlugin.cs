using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;

namespace Nop.Plugin.Widgets.HomePageNewProducts
{
    public class HomePageNewProductsPlugin : BasePlugin, IWidgetPlugin
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;

        #endregion

        #region Ctor

        public HomePageNewProductsPlugin(ILocalizationService localizationService, ISettingService settingService, IWebHelper webHelper)
        {
            _localizationService = localizationService;
            _settingService = settingService;
            _webHelper = webHelper;
        }

        #endregion

        #region Prop
        /// <summary>
        /// Gets a value indicating whether to hide this plugin on the widget list page in the admin area
        /// </summary>
        public bool HideInWidgetList => false;

        #endregion

        #region Methods

        public IList<string> GetWidgetZones()
        {
            return new List<string> { _settingService.GetSettingByKey<string>(typeof(HomePageNewProductsSettings).Name + "." + "WidgetZone") ?? "home_page_top" };

        }

        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/HomePageNewProducts/Configure";
        }

        /// <summary>
        /// Gets a name of a view component for displaying widget
        /// </summary>
        /// <param name="widgetZone">Name of the widget zone</param>
        /// <returns>View component name</returns>
        public string GetWidgetViewComponentName(string widgetZone)
        {
            return "HomePageNewProducts";
        }

        /// <summary>
        /// Install plugin
        /// </summary>
        public override void Install()
        {

            //settings
            var settings = new HomePageNewProductsSettings
            {
                WidgetZone = _settingService.GetSettingByKey<string>(typeof(HomePageNewProductsSettings).Name + "." + "WidgetZone") ?? "home_page_top",
                NumberOfProducts = _settingService.GetSettingByKey<int>(typeof(HomePageNewProductsSettings).Name + "." + "NumberOfProducts")

            };

            _settingService.SaveSetting(settings);


            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Widgets.HomePageNewProducts.WidgetZones", "Widget Zones");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Widgets.HomePageNewProducts.NumberOfProducts", "Number of Products");

            base.Install();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<HomePageNewProductsSettings>();

            //locales
            _localizationService.DeletePluginLocaleResource("Plugins.Widgets.HomePageNewProducts.WidgetZones");
            _localizationService.DeletePluginLocaleResource("Plugins.Widgets.HomePageNewProducts.NumberOfProducts");

            base.Uninstall();
        }

        #endregion

    }
}
