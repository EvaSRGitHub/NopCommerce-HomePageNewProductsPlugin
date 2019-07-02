using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Widgets.HomePageNewProducts.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Widgets.HomePageNewProducts.Controllers
{
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    public class HomePageNewProductsController : BasePluginController
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;

        #endregion

        #region Ctor

        public HomePageNewProductsController(ILocalizationService localizationService, INotificationService notificationService, IPermissionService permissionService, ISettingService settingService, IStoreContext storeContext)
        {
            _localizationService = localizationService;
            _notificationService = notificationService;
            _permissionService = permissionService;
            _settingService = settingService;
            _storeContext = storeContext;
        }

        #endregion

        #region Methods

        public IActionResult Configure()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var homePageNewProductsSettings = _settingService.LoadSetting<HomePageNewProductsSettings>(storeScope);

            var zones = GetHomePageWidgetZones();

            var model = new ConfigurationModel
            {
                WidgetZones = zones,
                WidgetZone = homePageNewProductsSettings.WidgetZone ?? "",
                NumberOfProducts = homePageNewProductsSettings.NumberOfProducts,
                ActiveStoreScopeConfiguration = storeScope
            };

            if (storeScope > 0)
            {
                model.WidgetZone_OverrideForStore = _settingService.SettingExists(homePageNewProductsSettings, x => x.WidgetZone, storeScope);
                model.NumberOfProducts_OverrideForStore = _settingService.SettingExists(homePageNewProductsSettings, x => x.NumberOfProducts, storeScope);
            }

            return View("~/Plugins/Widgets.HomePageNewProducts/Views/Configure.cshtml", model);
        }

        private static List<string> GetHomePageWidgetZones()
        {
            var type = Assembly.Load("Nop.Web.Framework").GetTypes().First(t => t.Name == "PublicWidgetZones");

            var props = type.GetProperties(BindingFlags.Static | BindingFlags.Public).Where(x => x.Name.StartsWith("Homepage")).ToList();

            var zones = new List<string>();

            foreach (var prop in props)
            {
                zones.Add(prop.GetValue(null).ToString());
            }

            return zones;
        }

        [HttpPost]
        public IActionResult Configure(ConfigurationModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var homePagbgeNewProductsSettings = _settingService.LoadSetting<HomePageNewProductsSettings>(storeScope);

            homePagbgeNewProductsSettings.WidgetZone = model.WidgetZone;
            homePagbgeNewProductsSettings.NumberOfProducts = model.NumberOfProducts;

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            _settingService.SaveSettingOverridablePerStore(homePagbgeNewProductsSettings, x => x.WidgetZone, model.WidgetZone_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(homePagbgeNewProductsSettings, x => x.NumberOfProducts, model.NumberOfProducts_OverrideForStore, storeScope, false);

            //now clear settings cache
            _settingService.ClearCache();

            _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));
            return Configure();
        }

        #endregion
    }
}
