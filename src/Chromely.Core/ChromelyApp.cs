﻿// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Chromely.Core.Configuration;
using Chromely.Core.Defaults;
using Chromely.Core.Infrastructure;
using Chromely.Core.Logging;
using Chromely.Core.Network;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Chromely.Core
{
    public abstract class ChromelyApp
    {
        protected bool _servicesConfigured;
        protected bool _coreServicesConfigured;
        protected bool _servicesInitialized;
        protected bool _resolversConfigured;
        protected bool _defaultHandlersConfigured;

        public virtual void ConfigureServices(ServiceCollection services)
        {
            _servicesConfigured = true;
        }

        public virtual void ConfigureCoreServices(ServiceCollection services)
        {
            if (!_servicesConfigured)
            {
                throw new Exception("Custom services must be configured before core default services are set.");
            }

            // Add core services if not already added.
            // Expected core services are -
            // IChromelyAppSettings, IChromelyConfiguration, IChromelyLogger, IChromelyRouteProvider
            // DefaultAppSettings  DefaultConfiguration SimpleLogger, DefaultRouteProvider
            // Logger is added in Initialize method

            services.TryAddSingleton<IChromelyConfiguration>(DefaultConfiguration.CreateForRuntimePlatform());
            services.TryAddSingleton<IChromelySerializerUtil, DefaultSerializerUtil>();
            services.TryAddSingleton<IChromelyAppSettings, DefaultAppSettings>();

            _coreServicesConfigured = true;
        }

        public virtual void ConfigureServiceResolvers(ServiceCollection services)
        {
            /*  Collection service resolvers for types: 
                IChromelyJsBindingHandler
                IChromelyCustomHandler
                IChromelyResourceHandlerFactory
                IChromelySchemeHandlerFactory
             */
            services.AddTransient<ChromelyHandlersResolver>(serviceProvider => (serviceType) =>
            {
                return serviceProvider.GetServices(serviceType);
            });

            _resolversConfigured = true;
        }

        public virtual void ConfigureDefaultHandlers(ServiceCollection services)
        {
            _defaultHandlersConfigured = true;
        }

        public virtual void Initialize(ServiceProvider serviceProvider)
        {
            if (!_servicesConfigured || !_coreServicesConfigured || !_resolversConfigured || !_defaultHandlersConfigured)
            {
                throw new Exception("Services must be configured before application is initialized.");
            }

            #region Configuration

            var config = serviceProvider.GetService<IChromelyConfiguration>();
            if (config == null)
            {
                config = DefaultConfiguration.CreateForRuntimePlatform();
            }

            InitConfiguration(config);

            #endregion Configuration

            #region Application/User Settings

            var appSettings = serviceProvider.GetService<IChromelyAppSettings>();
            if (appSettings == null)
            {
                appSettings = new DefaultAppSettings();
            }

            var currentAppSettings = new CurrentAppSettings
            {
                Properties = appSettings
            };

            ChromelyAppUser.App = currentAppSettings;
            ChromelyAppUser.App.Properties.Read(config);

            #endregion

            #region Logger

            var logger = GetCurrentLogger(serviceProvider);
            if (logger == null)
            {
                logger = new SimpleLogger();
            }

            var defaultLogger = new DefaultLogger();
            defaultLogger.Log = logger;
            Logger.Instance = defaultLogger;

            #endregion

            EnsureExpectedWorkingDirectory();

            _servicesInitialized = true;
        }

        public virtual void RegisterControllerRoutes(ServiceProvider serviceProvider)
        {
            if (!_servicesInitialized)
            {
                throw new Exception("Services must be initialized before controller assemblies are scanned.");
            }

            var routeProvider = serviceProvider.GetService<IChromelyRouteProvider>();
            if (routeProvider != null)
            {
                var controllers = serviceProvider.GetServices<ChromelyController>();
                routeProvider.RegisterAllRoutes(controllers?.ToList());
            }
        }

        public virtual void RegisterControllerAssembly(ServiceCollection services, string assemblyFullPath)
        {
            if (string.IsNullOrWhiteSpace(assemblyFullPath))
            {
                return;
            }

            try
            {
                if (File.Exists(assemblyFullPath))
                {
                    var assembly = Assembly.LoadFrom(assemblyFullPath);
                    RegisterControllerAssembly(services, assembly);
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.LogError(exception, "ChromelyApp:RegisterControllerAssembly");
            }

        }

        public virtual void RegisterControllerAssembly(ServiceCollection services, Assembly assembly)
        {
            if (assembly == null)
            {
                return;
            }

            try
            {
                services.RegisterAssembly(assembly, ServiceLifetime.Singleton);
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.LogError(exception, "ChromelyApp:RegisterControllerAssembly");
            }

        }

        protected virtual ILogger GetCurrentLogger(ServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetService<ILogger>();
            if (logger != null)
            {
                return logger;
            }

            var appName = Assembly.GetEntryAssembly()?.GetName().Name;
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            if (loggerFactory != null)
            {
                return loggerFactory.CreateLogger(appName);
            }

            var loggerProvider = serviceProvider.GetService<ILoggerProvider>();
            if (loggerProvider != null)
            {
                return loggerProvider.CreateLogger(appName);
            }

            return null;
        }

        /// <summary>
        /// Using local resource handling requires files to be relative to the 
        /// Expected working directory
        /// For example, if the app is launched via the taskbar the working directory gets changed to
        /// C:\Windows\system32
        /// This needs to be changed to the right one.
        /// </summary>
        protected static void EnsureExpectedWorkingDirectory()
        {
            try
            {
                var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                Directory.SetCurrentDirectory(appDirectory);
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.LogError(exception, "ChromelyApp:EnsureExpectedWorkingDirectory");
            }
        }

        protected void InitConfiguration(IChromelyConfiguration config)
        {
            if (config == null)
            {
                throw new Exception("Configuration cannot be null.");
            }

            if (config.UrlSchemes == null) config.UrlSchemes = new List<UrlScheme>();
            if (config.CommandLineArgs == null) config.CommandLineArgs = new Dictionary<string, string>();
            if (config.CommandLineOptions == null) config.CommandLineOptions = new List<string>();
            if (config.CustomSettings == null) config.CustomSettings = new Dictionary<string, string>();
            if (config.WindowOptions == null) config.WindowOptions = new Configuration.WindowOptions();
        }
    }
}
