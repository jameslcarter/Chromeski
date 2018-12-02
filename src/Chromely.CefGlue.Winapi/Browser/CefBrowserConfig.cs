﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefBrowserConfig.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefGlue.Winapi.Browser
{
    using System;
    using Xilium.CefGlue;

    /// <summary>
    /// The cef browser config.
    /// </summary>
    public class CefBrowserConfig
    {
        /// <summary>
        /// Gets or sets the parent handle.
        /// </summary>
        public IntPtr ParentHandle { get; set; }

        /// <summary>
        /// Gets or sets the cef rectangle.
        /// </summary>
        public CefRectangle CefRectangle { get; set; }

        /// <summary>
        /// Gets or sets the app args.
        /// </summary>
        public string[] AppArgs { get; set; }

        /// <summary>
        /// Gets or sets the start url.
        /// </summary>
        public string StartUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether start web socket.
        /// </summary>
        public bool StartWebSocket { get; set; }

        /// <summary>
        /// Gets or sets the websocket address.
        /// </summary>
        public string WebsocketAddress { get; set; }

        /// <summary>
        /// Gets or sets the websocket port.
        /// </summary>
        public int WebsocketPort { get; set; }
    }
}
