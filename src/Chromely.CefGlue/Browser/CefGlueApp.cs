﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueApp.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using Chromely.CefGlue.Browser.Handlers;
using Chromely.Core;
using Chromely.Core.Configuration;
using Chromely.Core.Infrastructure;
using Xilium.CefGlue;

namespace Chromely.CefGlue.Browser
{
    public class CefGlueApp : CefApp
    {
        private readonly IChromelyContainer _container;
        private readonly IChromelyConfiguration _config;
        private readonly CefRenderProcessHandler _renderProcessHandler;
        private readonly CefBrowserProcessHandler _browserProcessHandler;

        public CefGlueApp(IChromelyContainer container, IChromelyConfiguration config)
        {
            _container = container;
            _config = config;
             _renderProcessHandler = RenderProcessHandler;
            _browserProcessHandler = BrowserProcessHandler;
        }

        /// <summary>
        /// The on register custom schemes.
        /// </summary>
        /// <param name="registrar">
        /// The registrar.
        /// </param>
        protected override void OnRegisterCustomSchemes(CefSchemeRegistrar registrar)
        {
            var resourceSchemes = _config?.UrlSchemes?.GetAllResouceSchemes();
            if (resourceSchemes != null)
            {
                foreach (var item in resourceSchemes)
                {
                    bool isStandardScheme = UrlScheme.IsStandardScheme(item.Scheme);
                    if (!isStandardScheme)
                    {
                        var option = CefSchemeOptions.Local | CefSchemeOptions.CorsEnabled;
                        registrar.AddCustomScheme(item.Scheme, option);
                    }
                }
            }

            var customSchemes = _config?.UrlSchemes?.GetAllCustomSchemes();
            if (customSchemes != null)
            {
                foreach (var item in customSchemes)
                {
                    bool isStandardScheme = UrlScheme.IsStandardScheme(item.Scheme);
                    if (!isStandardScheme)
                    {
                        var option = CefSchemeOptions.Local | CefSchemeOptions.CorsEnabled;
                        registrar.AddCustomScheme(item.Scheme, option);
                    }
                }
            }
        }

        /// <summary>
        /// The on before command line processing.
        /// </summary>
        /// <param name="processType">
        /// The process type.
        /// </param>
        /// <param name="commandLine">
        /// The command line.
        /// </param>
        protected override void OnBeforeCommandLineProcessing(string processType, CefCommandLine commandLine)
        {
            // Get all custom command line argument switches
            if (_config?.CommandLineArgs != null)
            {
                foreach (var commandArg in _config.CommandLineArgs)
                {
                    commandLine.AppendSwitch(commandArg.Key ?? string.Empty, commandArg.Value);
                }
            }

            if (_config?.CommandLineOptions != null)
            {
                foreach (var commmandOption in _config?.CommandLineOptions)
                {
                    commandLine.AppendSwitch(commmandOption ?? string.Empty);
                }
            }
        }

        /// <summary>
        /// The get render process handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefRenderProcessHandler"/>.
        /// </returns>
        protected override CefRenderProcessHandler GetRenderProcessHandler()
        {
            return _renderProcessHandler;
        }

        /// <summary>
        /// The get browser process handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefBrowserProcessHandler"/>.
        /// </returns>
        protected override CefBrowserProcessHandler GetBrowserProcessHandler()
        {
            return _browserProcessHandler;
        }

        private CefGlueRenderProcessHandler RenderProcessHandler
        {
            get
            {
                var handler = GetCustomHandler(typeof(CefGlueRenderProcessHandler)) as CefGlueRenderProcessHandler;
                if (handler != null)
                {
                    return handler;
                }

                return new CefGlueRenderProcessHandler(_config);
            }
        }

        private CefGlueBrowserProcessHandler BrowserProcessHandler
        {
            get
            {
                var handler = GetCustomHandler(typeof(CefGlueBrowserProcessHandler)) as CefGlueBrowserProcessHandler;
                if (handler != null)
                {
                    return handler;
                }

                return new CefGlueBrowserProcessHandler(_config);
            }
        }
        private object GetCustomHandler(Type handlerType)
        {
            var customHandlers = _container.GetAllInstances(typeof(IChromelyCustomHandler));
            if (customHandlers != null && customHandlers.Any())
            {
                return customHandlers.FirstOrDefault(x => x.GetType() == handlerType);
            }

            return null;
        }
    }
}
