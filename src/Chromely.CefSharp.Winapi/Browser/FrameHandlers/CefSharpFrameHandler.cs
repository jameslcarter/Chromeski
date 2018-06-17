﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefSharpFrameHandler.cs" company="Chromely">
//   Copyright (c) 2017-2018 Kola Oyewumi
// </copyright>
// <license>
// MIT License
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </license>
// <note>
// Chromely project is licensed under MIT License. CefGlue, CefSharp, Winapi may have additional licensing.
// </note>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefSharp.Winapi.Browser.FrameHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::CefSharp;

    /// <summary>
    /// The CefSharp frame handler.
    /// </summary>
    internal class CefSharpFrameHandler
    {
        /// <summary>
        /// The browser.
        /// </summary>
        private readonly IBrowser browser;

        /// <summary>
        /// Initializes a new instance of the <see cref="CefSharpFrameHandler"/> class.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        public CefSharpFrameHandler(IBrowser browser)
        {
            this.browser = browser;
        }

        /// <summary>
        /// Gets the browser.
        /// </summary>
        public IBrowser Browser
        {
            get
            {
                if (this.browser == null)
                {
                    throw new Exception("Browser object cannot be null.");
                }

                return this.browser;
            }
        }

        /// <summary>
        /// Gets the get frame identifiers.
        /// </summary>
        public List<long> GetFrameIdentifiers => this.Browser.GetFrameIdentifiers()?.ToList();

        /// <summary>
        /// Gets the get frame names.
        /// </summary>
        public List<string> GetFrameNames => this.Browser.GetFrameNames();

        /// <summary>
        /// The get main frame.
        /// </summary>
        /// <returns>
        /// The <see>
        ///         <cref>IFrame</cref>
        ///     </see>
        ///     .
        /// </returns>
        public IFrame GetMainFrame()
        {
            return this.Browser.MainFrame;
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
        public IFrame GetFrame(string frameName)
        {
            return this.Browser.GetFrame(frameName);
        }
    }
}
