﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Microsoft.WindowsAzure.ServiceRuntime;
using Ninject;
using Ninject.Infrastructure;
using Ninject.Planning.Bindings;
using NuGetGallery.Areas.Admin.ViewModels;
using NuGetGallery.Configuration;

namespace NuGetGallery.Areas.Admin.Controllers
{
    public partial class ConfigController : AdminControllerBase
    {
        private readonly ConfigurationService _config;

        public ConfigController(ConfigurationService config)
        {
            _config = config;
        }

        public virtual ActionResult Index()
        {
            var settings = (from p in typeof(IAppConfiguration).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        where p.CanRead
                        select p)
                       .ToDictionary(p => p.Name, p => Tuple.Create(p.PropertyType, p.GetValue(_config.Current)));
            var features = (from p in typeof(FeatureConfiguration).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                            where p.CanRead
                            select new FeatureConfigViewModel(p, _config.Features))
                            .ToList();


            var configModel = new ConfigViewModel(settings, features);

            return View(configModel);
        }
    }
}