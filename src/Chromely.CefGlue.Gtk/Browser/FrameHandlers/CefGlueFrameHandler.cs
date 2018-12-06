﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueFrameHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefGlue.Gtk.Browser.FrameHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Xilium.CefGlue;

    /// <summary>
    /// The CefGlue frame handler.
    /// </summary>
    internal class CefGlueFrameHandler
    {
        /// <summary>
        /// The browser.
        /// </summary>
        private readonly CefBrowser browser;

        /// <summary>
        /// Initializes a new instance of the <see cref="CefGlueFrameHandler"/> class.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        public CefGlueFrameHandler(CefBrowser browser)
        {
            browser = browser;
        }

        /// <summary>
        /// Gets the browser.
        /// </summary>
        public CefBrowser Browser
        {
            get
            {
                if (browser == null)
                {
                    throw new Exception("Browser object cannot be null.");
                }

                return browser;
            }
        }


        /// <summary>
        /// Gets the get frame identifiers.
        /// </summary>
        public List<long> GetFrameIdentifiers => Browser.GetFrameIdentifiers()?.ToList();

        /// <summary>
        /// Gets the get frame names.
        /// </summary>
        public List<string> GetFrameNames => Browser.GetFrameNames()?.ToList();

        /// <summary>
        /// The get main frame.
        /// </summary>
        /// <returns>
        /// The <see>
        ///         <cref>CefFrame</cref>
        ///     </see>
        ///     .
        /// </returns>
        public CefFrame GetMainFrame()
        {
            return Browser.GetMainFrame();
        }

        /// <summary>
        /// The get frame.
        /// </summary>
        /// <param name="frameName">
        /// The frame name.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>IFrame</cref>
        ///     </see>
        ///     .
        /// </returns>
        public CefFrame GetFrame(string frameName)
        {
            return Browser.GetFrame(frameName);
        }
    }
}
