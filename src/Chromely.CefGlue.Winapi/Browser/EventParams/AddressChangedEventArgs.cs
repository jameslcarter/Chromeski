﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddressChangedEventArgs.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefGlue.Winapi.Browser.EventParams
{
    using System;
    using Xilium.CefGlue;

    /// <summary>
    /// The address changed event args.
    /// </summary>
    public class AddressChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddressChangedEventArgs"/> class.
        /// </summary>
        /// <param name="frame">
        /// The frame.
        /// </param>
        /// <param name="address">
        /// The address.
        /// </param>
        public AddressChangedEventArgs(CefFrame frame, string address)
        {
            this.Address = address;
            this.Frame = frame;
        }

        /// <summary>
        /// Gets the address.
        /// </summary>
        public string Address { get; }

        /// <summary>
        /// Gets the frame.
        /// </summary>
        public CefFrame Frame { get; }
        }
}
