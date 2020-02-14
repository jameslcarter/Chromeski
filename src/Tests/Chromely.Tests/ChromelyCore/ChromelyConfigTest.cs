using Chromely.Core;
using Chromely.Core.Configuration;
using Chromely.Core.Host;
using Chromely.Core.Infrastructure;
using Chromely.Core.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Chromely.Tests.ChromelyCore
{
    public class ChromelyConfigTests
    {
        [Fact]
        public void ConfigFromFileTest()
        {
            var configFromFile = DefaultConfigFromFile;
            var configFromFileExpectedValues = DefaultConfigFromFileExpectedValues;

            Assert.NotNull(configFromFile);
            Assert.NotNull(configFromFileExpectedValues);

            Assert.Equal(configFromFile.AppName, configFromFileExpectedValues.AppName);
            Assert.Equal(configFromFile.StartUrl, configFromFileExpectedValues.StartUrl);
            Assert.Equal(configFromFile.CefDownloadOptions.AutoDownloadWhenMissing, configFromFileExpectedValues.CefDownloadOptions.AutoDownloadWhenMissing);
            Assert.Equal(configFromFile.CefDownloadOptions.DownloadSilently, configFromFileExpectedValues.CefDownloadOptions.DownloadSilently);
            Assert.Equal(configFromFile.DebuggingMode, configFromFileExpectedValues.DebuggingMode);

            Assert.Equal(configFromFile.WindowOptions.Position.X, configFromFileExpectedValues.WindowOptions.Position.X);
            Assert.Equal(configFromFile.WindowOptions.Position.Y, configFromFileExpectedValues.WindowOptions.Position.Y);
            Assert.Equal(configFromFile.WindowOptions.Size.Width, configFromFileExpectedValues.WindowOptions.Size.Width);
            Assert.Equal(configFromFile.WindowOptions.Size.Height, configFromFileExpectedValues.WindowOptions.Size.Height);
            Assert.Equal(configFromFile.WindowOptions.DisableResizing, configFromFileExpectedValues.WindowOptions.DisableResizing);
            Assert.Equal(configFromFile.WindowOptions.DisableMinMaximizeControls, configFromFileExpectedValues.WindowOptions.DisableMinMaximizeControls);
            Assert.Equal(configFromFile.WindowOptions.WindowFrameless, configFromFileExpectedValues.WindowOptions.WindowFrameless);
            Assert.Equal(configFromFile.WindowOptions.StartCentered, configFromFileExpectedValues.WindowOptions.StartCentered);
            Assert.Equal(configFromFile.WindowOptions.KioskMode, configFromFileExpectedValues.WindowOptions.KioskMode);
            Assert.Equal(configFromFile.WindowOptions.WindowState, configFromFileExpectedValues.WindowOptions.WindowState);
            Assert.Equal(configFromFile.WindowOptions.Title, configFromFileExpectedValues.WindowOptions.Title);
            Assert.Equal(configFromFile.WindowOptions.RelativePathToIconFile, configFromFileExpectedValues.WindowOptions.RelativePathToIconFile);

            Assert.NotNull(configFromFile.WindowOptions.CustomStyle);
            Assert.NotNull(configFromFileExpectedValues.WindowOptions.CustomStyle);

            Assert.Equal(configFromFile.WindowOptions.UseCustomStyle, configFromFileExpectedValues.WindowOptions.UseCustomStyle);
            Assert.Equal(configFromFile.WindowOptions.CustomStyle.WindowStyles, configFromFileExpectedValues.WindowOptions.CustomStyle.WindowStyles);
            Assert.Equal(configFromFile.WindowOptions.CustomStyle.WindowExStyles, configFromFileExpectedValues.WindowOptions.CustomStyle.WindowExStyles);

            Assert.NotNull(configFromFile.UrlSchemes);
            Assert.NotNull(configFromFileExpectedValues.UrlSchemes);

            Assert.Equal(configFromFile.UrlSchemes.Count, configFromFileExpectedValues.UrlSchemes.Count);

            var schemeDefaultResource = configFromFile.UrlSchemes.FirstOrDefault(x => x.Name == "default-resource");
            var schemeCustomHttp = configFromFile.UrlSchemes.FirstOrDefault(x => x.Name == "default-custom-http");
            var schemeCommandHttp = configFromFile.UrlSchemes.FirstOrDefault(x => x.Name == "default-command-http");
            var schemeExternal1 = configFromFile.UrlSchemes.FirstOrDefault(x => x.Name == "chromely-site");

            Assert.NotNull(schemeDefaultResource);
            Assert.NotNull(schemeCustomHttp);
            Assert.NotNull(schemeCommandHttp);
            Assert.NotNull(schemeExternal1);

            var expectedSchemeDefaultResource = configFromFileExpectedValues.UrlSchemes.FirstOrDefault(x => x.Name == "default-resource");
            var expectedSchemeCustomHttp = configFromFileExpectedValues.UrlSchemes.FirstOrDefault(x => x.Name == "default-custom-http");
            var expectedSchemeCommandHttp = configFromFileExpectedValues.UrlSchemes.FirstOrDefault(x => x.Name == "default-command-http");
            var expectedSchemeExternal1 = configFromFileExpectedValues.UrlSchemes.FirstOrDefault(x => x.Name == "chromely-site");

            Assert.NotNull(expectedSchemeDefaultResource);
            Assert.NotNull(expectedSchemeCustomHttp);
            Assert.NotNull(expectedSchemeCommandHttp);
            Assert.NotNull(expectedSchemeExternal1);

            Assert.NotNull(configFromFile.ControllerAssemblies);
            Assert.NotNull(configFromFileExpectedValues.ControllerAssemblies);

            Assert.True(configFromFile.ControllerAssemblies.Count == 1);
            Assert.True(configFromFileExpectedValues.ControllerAssemblies.Count == 1);

            Assert.Equal(configFromFile.ControllerAssemblies[0].Assembly, configFromFileExpectedValues.ControllerAssemblies[0].Assembly);

            // Custom settings
            Assert.NotNull(configFromFile.CustomSettings);
            Assert.NotNull(configFromFileExpectedValues.CustomSettings);

             Assert.Contains("cefLogFile", configFromFile.CustomSettings.Keys);
            Assert.Contains("cefLogFile", configFromFileExpectedValues.CustomSettings.Keys);
            Assert.Contains("logSeverity", configFromFile.CustomSettings.Keys);
            Assert.Contains("logSeverity", configFromFileExpectedValues.CustomSettings.Keys);
            Assert.Contains("locale", configFromFile.CustomSettings.Keys);
            Assert.Contains("locale", configFromFileExpectedValues.CustomSettings.Keys);

            Assert.Contains("logs\\chromely.cef.log", configFromFile.CustomSettings.Values);
            Assert.Contains("logs\\chromely.cef.log", configFromFileExpectedValues.CustomSettings.Values);
            Assert.Contains("info", configFromFile.CustomSettings.Values);
            Assert.Contains("info", configFromFileExpectedValues.CustomSettings.Values);
            Assert.Contains("en-US", configFromFile.CustomSettings.Values);
            Assert.Contains("en-US", configFromFileExpectedValues.CustomSettings.Values);

            // Command line args
            Assert.NotNull(configFromFile.CommandLineArgs);
            Assert.NotNull(configFromFileExpectedValues.CommandLineArgs);

            Assert.Contains("disable-gpu", configFromFile.CommandLineArgs.Keys);
            Assert.Contains("disable-gpu", configFromFileExpectedValues.CommandLineArgs.Keys);

            Assert.Contains("1", configFromFile.CommandLineArgs.Values);
            Assert.Contains("1", configFromFileExpectedValues.CommandLineArgs.Values);

            // Command line options
            Assert.NotNull(configFromFile.CommandLineOptions);
            Assert.NotNull(configFromFileExpectedValues.CommandLineOptions);

            Assert.Contains("no-zygote", configFromFile.CommandLineOptions);
            Assert.Contains("no-zygote", configFromFileExpectedValues.CommandLineOptions);
            Assert.Contains("disable-gpu", configFromFile.CommandLineOptions);
            Assert.Contains("disable-gpu", configFromFileExpectedValues.CommandLineOptions);
            Assert.Contains("disable-software-rasterizer", configFromFile.CommandLineOptions);
            Assert.Contains("disable-software-rasterizer", configFromFileExpectedValues.CommandLineOptions);
        }

        [Fact]
        public void ConfigTest()
        {
            // Arrange
            var appName = System.Reflection.Assembly.GetEntryAssembly()?.GetName().Name;
            var windowTitle = appName;
            var platform = ChromelyRuntime.Platform;
            var appExeLocation = AppDomain.CurrentDomain.BaseDirectory;

            // Act
            var config = DefaultConfig;

            // Assert
            Assert.NotNull(config);
            Assert.Equal(appName, config.AppName);
            Assert.Equal(windowTitle, config.WindowOptions.Title);
            Assert.Equal(platform, config.Platform);
            Assert.Equal(appExeLocation, config.AppExeLocation);
        }

        private IChromelyConfiguration DefaultConfig
        {
            get
            {
                var config = new DefaultConfiguration();
                return config;
            }
        }

        private IChromelyConfiguration DefaultConfigFromFile
        {
            get
            {
                var handler = new ConfigurationHandler();
                return handler.Parse<DefaultConfiguration>();
            }
        }

        private IChromelyConfiguration DefaultConfigFromFileExpectedValues
        {
            get
            {
                var config = new DefaultConfiguration
                {
                    AppName = "chromely_test",
                    StartUrl = "local://app/chromely.html",
                    DebuggingMode = true,
                    CefDownloadOptions = new CefDownloadOptions()
                    {
                        AutoDownloadWhenMissing = true,
                        DownloadSilently = false
                    },

                    WindowOptions = new WindowOptions
                    {
                        Size = new WindowSize(1200, 900),
                        Position = new WindowPosition(1, 2),
                        DisableResizing = false,
                        DisableMinMaximizeControls = false,
                        WindowFrameless = false,
                        StartCentered = true,
                        KioskMode = false,
                        WindowState = WindowState.Normal,
                        Title = "chromely",
                        RelativePathToIconFile = "chromely.ico",
                        CustomStyle = new WindowCustomStyle(0, 0),
                        UseCustomStyle = false
                    },

                    UrlSchemes = new List<UrlScheme>()
                };

                var schemeDefaultResource = new UrlScheme("default-resource", "local", string.Empty, string.Empty, UrlSchemeType.Resource, false);
                var schemeCustomHttp = new UrlScheme("default-custom-http", "http", "chromely.com", string.Empty, UrlSchemeType.Custom, false);
                var schemeCommandHttp = new UrlScheme("default-command-http", "http", "command.com", string.Empty, UrlSchemeType.Command, false);
                var schemeExternal1 = new UrlScheme("chromely-site", string.Empty, string.Empty, "https://github.com/chromelyapps/Chromely", UrlSchemeType.External, true);

                config.UrlSchemes.Add(schemeDefaultResource);
                config.UrlSchemes.Add(schemeCustomHttp);
                config.UrlSchemes.Add(schemeCommandHttp);
                config.UrlSchemes.Add(schemeExternal1);

                config.ControllerAssemblies = new List<ControllerAssemblyInfo>();
                config.ControllerAssemblies.RegisterServiceAssembly("Chromely.External.Controllers.dll");

                config.CustomSettings = new Dictionary<string, string>();
                config.CustomSettings["cefLogFile"] = "logs\\chromely.cef.log";
                config.CustomSettings["logSeverity"] = "info";
                config.CustomSettings["locale"] = "en-US";

                config.CommandLineArgs = new Dictionary<string, string>();
                config.CommandLineArgs["disable-gpu"] = "1";

                config.CommandLineOptions = new List<string>();
                config.CommandLineOptions.Add("no-zygote");
                config.CommandLineOptions.Add("disable-gpu");
                config.CommandLineOptions.Add("disable-software-rasterizer");

                return config;
            }
        }
    }
}
