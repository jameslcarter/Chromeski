﻿// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core.Configuration;
using Chromely.Core.Host;
using Chromely.Core.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace Chromely.Core
{
    public sealed class AppBuilder
    {
        private ServiceProvider _serviceProvider;
        private ChromelyApp _chromelyApp;
        private IChromelyConfiguration _config;
        private IChromelyWindow _chromelyWindow;
        private Type _chromelyUseConfigType;
        private Type _chromelyUseWindowType;
        private int _stepCompleted;

        private AppBuilder()
        {
            _config = null;
            _chromelyUseConfigType = null;
            _chromelyUseWindowType = null;
            _stepCompleted = 1;
        }

        public static AppBuilder Create()
        {
            var appBuilder = new AppBuilder();
            return appBuilder;
        }

        public AppBuilder UseConfig<TService>(IChromelyConfiguration config = null) where TService : IChromelyConfiguration
        {
            if (config != null)
            {
                _config = config;
            }
            else
            {
                _chromelyUseConfigType = null;
                EnsureIsDerivedType(typeof(IChromelyConfiguration), typeof(TService));
                _chromelyUseConfigType = typeof(TService);
            }

            return this;
        }

        public AppBuilder UseWindow<TService>(IChromelyWindow chromelyWindow = null) where TService : IChromelyWindow
        {
            if (chromelyWindow != null)
            {
                _chromelyWindow = chromelyWindow;
            }
            else
            {
                _chromelyUseWindowType = null;
                EnsureIsDerivedType(typeof(IChromelyWindow), typeof(TService));
                _chromelyUseWindowType = typeof(TService);
            }

            return this;
        }

        public AppBuilder UseApp<T>(ChromelyApp ChromelyApp = null) where T : ChromelyApp
        {
            _chromelyApp = ChromelyApp;
            if (_chromelyApp == null)
            {
                EnsureIsDerivedType(typeof(ChromelyApp), typeof(T));
                _chromelyApp = (T)Activator.CreateInstance(typeof(T));
            }

            _stepCompleted = 1;
            return this;
        }

        public AppBuilder Build()
        {
            if (_stepCompleted != 1)
            {
                throw new Exception("Invalid order: Step 1: UseApp must be completed before Step 2: Build.");
            }

            if (_chromelyApp == null)
            {
                throw new Exception($"ChromelyApp {nameof(_chromelyApp)} cannot be null.");
            }

            var serviceCollection = new ServiceCollection();
            _chromelyApp.ConfigureServices(serviceCollection);

            // This must be done before registring core services
            RegisterUseComponents(serviceCollection);

            _chromelyApp.ConfigureCoreServices(serviceCollection);
            _chromelyApp.ConfigureServiceResolvers(serviceCollection);
            _chromelyApp.ConfigureDefaultHandlers(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _chromelyApp.Initialize(_serviceProvider);
            _chromelyApp.RegisterControllerRoutes(_serviceProvider);

            _stepCompleted = 2;
            return this;
        }

        public void Run(string[] args)
        {
            if (_stepCompleted != 2)
            {
                throw new Exception("Invalid order: Step 2: Build must be completed before Step 3: Run.");
            }

            if (_serviceProvider == null)
            {
                throw new Exception("ServiceProvider is not initialized.");
            }

            try
            {
                var appName = Assembly.GetEntryAssembly()?.GetName().Name;
                var windowController = _serviceProvider.GetService<ChromelyWindowController>();
                try
                {
                    Logger.Instance.Log.LogInformation($"Running application:{appName}.");
                    windowController.Run(args);
                }
                catch (Exception exception)
                {
                    Logger.Instance.Log.LogError(exception, $"Error running application:{appName}.");
                }
                finally
                {
                    windowController.Dispose();
                    _serviceProvider.Dispose();
                }

            }
            catch (Exception exception)
            {
                var appName = Assembly.GetEntryAssembly()?.GetName().Name;
                Logger.Instance.Log.LogError(exception, $"Error running application:{appName}.");
            }
        }

        private void EnsureIsDerivedType(Type baseType, Type derivedType)
        {
            if (baseType == derivedType)
            {
                throw new Exception($"Cannot specify the base type {baseType.Name} itself as generic type parameter.");
            }

            if (!baseType.IsAssignableFrom(derivedType))
            {
                throw new Exception($"Type {derivedType.Name} must implement {baseType.Name}.");
            }

            if (derivedType.IsAbstract || derivedType.IsInterface)
            {
                throw new Exception($"Type {derivedType.Name} cannot be an interface or abstract class.");
            }
        }

        private void RegisterUseComponents(ServiceCollection services)
        {
            if (_config != null)
            {
                services.TryAddSingleton<IChromelyConfiguration>(_config);
            }
            else if (_chromelyUseConfigType != null)
            {
                services.TryAddSingleton(typeof(IChromelyConfiguration), _chromelyUseConfigType);
            }

            if (_chromelyWindow != null)
            {
                services.TryAddSingleton<IChromelyWindow>(_chromelyWindow);
            }
            else if (_chromelyUseWindowType != null)
            {
                services.TryAddSingleton(typeof(IChromelyWindow), _chromelyUseWindowType);
            }
        }
    }
}