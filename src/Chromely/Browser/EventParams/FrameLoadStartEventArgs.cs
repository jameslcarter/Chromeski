﻿// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;
using Xilium.CefGlue;

namespace Chromely.Browser
{
    /// <summary>
    /// The load start event args.
    /// </summary>
    public class FrameLoadStartEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameLoadStartEventArgs"/> class.
        /// </summary>
        /// <param name="frame">
        /// The frame.
        /// </param>
        public FrameLoadStartEventArgs(CefFrame frame)
        {
            Frame = frame;
        }

        /// <summary>
        /// Gets the frame.
        /// </summary>
        public CefFrame Frame { get; }
    }
}