﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TooltipEventArgs.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefGlue.Winapi.Browser.EventParams
{
    using System;

    /// <summary>
    /// The tooltip event args.
    /// </summary>
    public class TooltipEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TooltipEventArgs"/> class.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        public TooltipEventArgs(string text)
        {
            this.Text = text;
        }

        /// <summary>
        /// Gets the text.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether handled.
        /// </summary>
        public bool Handled { get; set; }
    }
}
