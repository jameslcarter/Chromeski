﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WinApiCefGlueBrowserWindow.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using Chromely.CefGlue.BrowserWindow;
using Chromely.Core;
// ReSharper disable UnusedMember.Global

namespace Chromely.CefGlue.Winapi.BrowserWindow
{
    /// <summary>
    /// The cef glue chromium window.
    /// </summary>
    public class CefGlueWindow : HostBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CefGlueWindow"/> class.
        /// </summary>
        /// <param name="hostConfig">
        /// The host config.
        /// </param>
        public CefGlueWindow(ChromelyConfiguration hostConfig) 
            : base(hostConfig)
        {
        }

        /// <summary>
        /// The platform initialize.
        /// </summary>
        protected override void Initialize()
        {
        }

        /// <summary>
        /// The platform shutdown.
        /// </summary>
        protected override void Shutdown()
        {
        }

        /// <summary>
        /// The platform run message loop.
        /// </summary>
        protected override void RunMessageLoop()
        {
            NativeWindow.RunMessageLoop();
        }

        /// <summary>
        /// The platform quit message loop.
        /// </summary>
        protected override void QuitMessageLoop()
        {
            NativeWindow.Exit();
        }

        /// <summary>
        /// The create main view.
        /// </summary>
        /// <returns>
        /// The <see cref="Window"/>.
        /// </returns>
        protected override IWindow CreateMainView()
        {
            return new Window(this, HostConfig);
        }
    }
}
