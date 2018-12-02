﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomSchemeHandlerFactory.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefSharp.Winapi.Tests.Models
{
    using global::CefSharp;

    /// <summary>
    /// The CefSharp http scheme handler factory.
    /// </summary>
    public class CustomSchemeHandlerFactory : ISchemeHandlerFactory
    {
        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="frame">
        /// The frame.
        /// </param>
        /// <param name="schemeName">
        /// The scheme name.
        /// </param>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="IResourceHandler"/>.
        /// </returns>
        public IResourceHandler Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
        {
            return null;
        }
    }
}
