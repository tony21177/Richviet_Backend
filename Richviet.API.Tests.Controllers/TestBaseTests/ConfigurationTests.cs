﻿using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Richviet.API.Common.Settings;

namespace Richviet.API.Tests.Controllers.TestBaseTests
{
    [TestClass]
    public class ConfigurationTests : TestBase
    {
        [TestMethod]
        public async System.Threading.Tasks.Task ConfigurationRoot_OK()
        {
            Assert.IsNotNull(_configurationRoot);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task AppSettingsIConfiguration_OK()
        {
            Assert.IsNotNull(_configurationRoot);

            var appSettings = _configurationRoot.GetSection(nameof(AppSettings));
            Assert.IsNotNull(appSettings);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task AppSettings_OK()
        {
            Assert.IsNotNull(_configurationRoot);

            var iConfiguration = _configurationRoot.GetSection(nameof(AppSettings));
            Assert.IsNotNull(iConfiguration);

            var appSettings = iConfiguration.Get<AppSettings>();
            Assert.IsNotNull(appSettings);
        }
    }
}
