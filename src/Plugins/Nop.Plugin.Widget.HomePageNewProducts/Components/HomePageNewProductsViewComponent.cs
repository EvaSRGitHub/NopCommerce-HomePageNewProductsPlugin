using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.HomePageNewProducts.Components
{
    [ViewComponent(Name = "HomePageNewProducts")]
    public class HomePageNewProductsViewComponent : NopViewComponent
    {
        #region Fields

        private readonly IStoreContext _storeContext;
        private readonly IStaticCacheManager _cacheManager;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly IProductModelFactory _productModelFactory;
        private readonly IProductService _productService;

        #endregion

        #region Ctor

        public HomePageNewProductsViewComponent(IStoreContext storeContext,
            IStaticCacheManager cacheManager,
            ISettingService settingService,
            IWebHelper webHelper,
            IProductModelFactory productModelFactory,
            IProductService productService)
        {
            _storeContext = storeContext;
            _cacheManager = cacheManager;
            _settingService = settingService;
            _webHelper = webHelper;
            _productModelFactory = productModelFactory;
            _productService = productService;
        }

        #endregion

        #region Methods

        public IViewComponentResult Invoke(string widgetZone, object additionalData)
        {
            var homePageNewProductsSettings = _settingService.LoadSetting<HomePageNewProductsSettings>(_storeContext.CurrentStore.Id);

            var numberOfNewProductsToShow = _settingService.GetSettingByKey<int>(typeof(HomePageNewProductsSettings).Name + "." + "NumberOfProducts");

            var newProducts = _productService.SearchProducts(storeId: _storeContext.CurrentStore.Id,
                visibleIndividuallyOnly: true,
                markedAsNewOnly: true).OrderByDescending(p => p.CreatedOnUtc).Take(numberOfNewProductsToShow).ToList();

            //if (!newProducts.Any())
            //{
            //    return Content("");
            //}

            var model = _productModelFactory.PrepareProductOverviewModels(newProducts, true, true).ToList();

            return View("~/Plugins/Widgets.HomePageNewProducts/Views/Default.cshtml", model);
        }

        #endregion

    }
}
