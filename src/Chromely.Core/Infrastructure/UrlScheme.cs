﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UrlScheme.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.Core.Infrastructure
{
    using System;

    /// <summary>
    /// The url scheme.
    /// </summary>
    public class UrlScheme
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UrlScheme"/> class.
        /// </summary>
        /// <param name="scheme">
        /// The scheme.
        /// </param>
        /// <param name="host">
        /// The host.
        /// </param>
        /// <param name="isExternal">
        /// The is external.
        /// </param>
        public UrlScheme(string scheme, string host, bool isExternal)
        {
            this.Scheme = scheme;
            this.Host = host;
            this.IsExternal = isExternal;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlScheme"/> class.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <param name="isExternal">
        /// The is external.
        /// </param>
        public UrlScheme(string url, bool isExternal)
        {
            if (!string.IsNullOrEmpty(url))
            {
                var uri = new Uri(url);
                this.Scheme = uri.Scheme;
                this.Host = uri.Host;
                this.IsExternal = isExternal;
            }
        }

        /// <summary>
        /// Gets or sets the scheme.
        /// </summary>
        public string Scheme { get; set; }

        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is external.
        /// </summary>
        public bool IsExternal { get; set; }

        /// <summary>
        /// Check if scheme is a standard type.
        /// </summary>
        /// <param name="scheme">
        /// The scheme.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsStandardScheme(string scheme)
        {
            if (string.IsNullOrEmpty(scheme))
            {
                return false;
            }

            switch (scheme.ToLower())
            {
                case "http":
                case "https":
                case "file":
                case "ftp":
                case "about":
                case "data":
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if url is of same scheme as the object url.
        /// </summary>
        /// <param name="url">
        /// The url to check.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool IsUrlOfSameScheme(string url)
        {
            if (string.IsNullOrEmpty(this.Scheme) ||
                string.IsNullOrEmpty(this.Host) ||
                string.IsNullOrEmpty(url))
            {
                return false;
            }

            var uri = new Uri(url);

            if (string.IsNullOrEmpty(uri.Scheme) ||
                string.IsNullOrEmpty(uri.Host))
            {
                return false;
            }

            if (this.Scheme.ToLower().Equals(uri.Scheme) &&
                this.Host.ToLower().Equals(uri.Host))
            {
                return true;
            }

            return false;
        }
    }
}
