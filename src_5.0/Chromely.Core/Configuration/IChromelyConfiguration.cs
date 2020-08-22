﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IChromelyConfiguration.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using Chromely.Core.Infrastructure;
using Chromely.Core.Network;

namespace Chromely.Core.Configuration
{
    public interface IChromelyConfiguration
    {
        string AppName { get; set; }
        string StartUrl { get; set; }
        string AppExeLocation { get; set; }
        string ChromelyVersion { get; set; }
        ChromelyPlatform Platform { get; set; }
        bool DebuggingMode { get; set; }
        string DevToolsUrl { get; set; }
        IDictionary<string, string> CommandLineArgs { get; set; }
        List<string> CommandLineOptions { get; set; }
        List<ControllerAssemblyInfo> ControllerAssemblies { get; set; }
        IDictionary<string, string> CustomSettings { get; set; }
        List<ChromelyEventHandler<object>> EventHandlers { get; set; }
        IDictionary<string, object> ExtensionData { get; set; }
        IChromelyJavaScriptExecutor JavaScriptExecutor { get; set; }
        List<UrlScheme> UrlSchemes { get; set; }
        CefDownloadOptions CefDownloadOptions { get; set; }
        IWindowOptions WindowOptions { get; set; }
    }
}