﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefSharpBrowserHostTest.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefSharp.Winapi.Tests.ChromeHost
{
    using System.Linq;

    using Chromely.CefSharp.Winapi.Browser;
    using Chromely.CefSharp.Winapi.Tests.Models;
    using Chromely.Core;
    using Chromely.Core.Helpers;
    using Chromely.Core.Infrastructure;

    using Xunit;
    using Xunit.Abstractions;

    using CefSharpGlobal = global::CefSharp;

    /// <summary>
    /// The chromely configuration test.
    /// </summary>
    public class CefSharpBrowserHostTest
    {
        /// <summary>
        /// The output.
        /// </summary>
        // ReSharper disable once NotAccessedField.Local
        private readonly ITestOutputHelper mTestOutput;

        /// <summary>
        /// Initializes a new instance of the <see cref="CefSharpBrowserHostTest"/> class.
        /// </summary>
        /// <param name="testOutput">
        /// The test output.
        /// </param>
        public CefSharpBrowserHostTest(ITestOutputHelper testOutput)
        {
            mTestOutput = testOutput;
        }

        /// <summary>
        /// The custom schemer tests.
        /// </summary>
        [Fact]
        public void CustomSchemerTests()
        {
            var config = GetBaseConfig()
                .RegisterSchemeHandler("http", "cefsharp1.com", new CustomSchemeHandlerFactory());

            var schemeHandlerObjs = IoC.GetAllInstances(typeof(ChromelySchemeHandler));

            Assert.NotNull(schemeHandlerObjs);

            var schemeHandlers = schemeHandlerObjs.ToList();
            int count = schemeHandlers.Count;
            Assert.Equal(1, count);

            Assert.True(schemeHandlers[0] is ChromelySchemeHandler);

            ChromelySchemeHandler customSchemeHandler1 = (ChromelySchemeHandler)schemeHandlers[0];

            Assert.Equal("http", customSchemeHandler1.SchemeName);
            Assert.Equal("cefsharp1.com", customSchemeHandler1.DomainName);
            Assert.True(customSchemeHandler1.HandlerFactory is CustomSchemeHandlerFactory);
        }

        /// <summary>
        /// The settings update test.
        /// </summary>
        [Fact]
        public void SettingsUpdateTest()
        {
            var hostConfig = GetConfigWithDefaultValues();
            var settings = new CefSettings
            {
                Locale = hostConfig.Locale,
                MultiThreadedMessageLoop = false,
                LogFile = hostConfig.LogFile
            };

            // Update configuration settings
            settings.Update(hostConfig.CustomSettings);

            Assert.True(settings.MultiThreadedMessageLoop);
            Assert.True(settings.ExternalMessagePump);
            Assert.True(settings.WindowlessRenderingEnabled);
            Assert.True(settings.CommandLineArgsDisabled);
            Assert.True(settings.PackLoadingDisabled);
            Assert.True(settings.IgnoreCertificateErrors);

            Assert.Equal(nameof(CefSettingKeys.BrowserSubprocessPath), settings.BrowserSubprocessPath);
            Assert.Equal(nameof(CefSettingKeys.CachePath), settings.CachePath);
            Assert.Equal(nameof(CefSettingKeys.UserDataPath), settings.UserDataPath);
            Assert.Equal(nameof(CefSettingKeys.UserAgent), settings.UserAgent);
            Assert.Equal(nameof(CefSettingKeys.ProductVersion), settings.ProductVersion);
            Assert.Equal(nameof(CefSettingKeys.Locale), settings.Locale);
            Assert.Equal(nameof(CefSettingKeys.LogFile), settings.LogFile);
            Assert.Equal(nameof(CefSettingKeys.JavaScriptFlags), settings.JavascriptFlags);
            Assert.Equal(nameof(CefSettingKeys.ResourcesDirPath), settings.ResourcesDirPath);
            Assert.Equal(nameof(CefSettingKeys.LocalesDirPath), settings.LocalesDirPath);
            Assert.Equal(nameof(CefSettingKeys.AcceptLanguageList), settings.AcceptLanguageList);

            Assert.Equal(1025, settings.RemoteDebuggingPort);
            Assert.Equal(1000, settings.UncaughtExceptionStackSize);
        }

        /// <summary>
        /// The get config with default values.
        /// </summary>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/>.
        /// </returns>
        private ChromelyConfiguration GetConfigWithDefaultValues()
        {
            var config = GetBaseConfig()
                .UseDefaultResourceSchemeHandler("local", string.Empty)
                .UseDefaultHttpSchemeHandler("http", "chromely.com")
                .UseDefautJsHandler("boundedObject", true)
                .WithCustomSetting(CefSettingKeys.NoSandbox, true)
                .WithCustomSetting(CefSettingKeys.SingleProcess, true)
                .WithCustomSetting(CefSettingKeys.BrowserSubprocessPath, nameof(CefSettingKeys.BrowserSubprocessPath))
                .WithCustomSetting(CefSettingKeys.MultiThreadedMessageLoop, true)
                .WithCustomSetting(CefSettingKeys.ExternalMessagePump, true)
                .WithCustomSetting(CefSettingKeys.WindowlessRenderingEnabled, true)
                .WithCustomSetting(CefSettingKeys.CommandLineArgsDisabled, true)
                .WithCustomSetting(CefSettingKeys.CachePath, nameof(CefSettingKeys.CachePath))
                .WithCustomSetting(CefSettingKeys.UserDataPath, nameof(CefSettingKeys.UserDataPath))
                .WithCustomSetting(CefSettingKeys.UserAgent, nameof(CefSettingKeys.UserAgent))
                .WithCustomSetting(CefSettingKeys.ProductVersion, nameof(CefSettingKeys.ProductVersion))
                .WithCustomSetting(CefSettingKeys.Locale, nameof(CefSettingKeys.Locale))
                .WithCustomSetting(CefSettingKeys.LogFile, nameof(CefSettingKeys.LogFile))
                .WithCustomSetting(CefSettingKeys.LogSeverity, (int)CefSharpGlobal.LogSeverity.Error)
                .WithCustomSetting(CefSettingKeys.JavaScriptFlags, nameof(CefSettingKeys.JavaScriptFlags))
                .WithCustomSetting(CefSettingKeys.ResourcesDirPath, nameof(CefSettingKeys.ResourcesDirPath))
                .WithCustomSetting(CefSettingKeys.LocalesDirPath, nameof(CefSettingKeys.LocalesDirPath))
                .WithCustomSetting(CefSettingKeys.PackLoadingDisabled, true)
                .WithCustomSetting(CefSettingKeys.RemoteDebuggingPort, 1025)
                .WithCustomSetting(CefSettingKeys.UncaughtExceptionStackSize, 1000)
                .WithCustomSetting(CefSettingKeys.IgnoreCertificateErrors, true)
                .WithCustomSetting(CefSettingKeys.AcceptLanguageList, nameof(CefSettingKeys.AcceptLanguageList))
                .WithCustomSetting(CefSettingKeys.FocusedNodeChangedEnabled, true);

            return config;
        }

        /// <summary>
        /// The get base config.
        /// </summary>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/>.
        /// </returns>
        private ChromelyConfiguration GetBaseConfig()
        {
            var title = "chromely";
            var iconFile = "chromely.ico";

            var hostWidth = 1200;
            var hostHeight = 900;

            var cefLogFile = "logs\\chromely.cef_new.log";
            var defaultLogFile = "logs\\chromely_new.log";
            string startUrl = "www.google.com";

            var logSeverity = LogSeverity.Error;
            var config = ChromelyConfiguration
                .Create()
                .WithHostTitle(title)
                .WithHostIconFile(iconFile)
                .WithAppArgs(null)
                .WithHostSize(hostWidth, hostHeight)
                .WithLogFile(cefLogFile)
                .WithStartUrl(startUrl)
                .WithLogSeverity(logSeverity)
                .UseDefaultLogger(defaultLogFile);

            return config;
        }
    }
}
