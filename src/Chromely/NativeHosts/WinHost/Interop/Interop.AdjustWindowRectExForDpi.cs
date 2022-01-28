﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Chromely;

public static partial class Interop
{
    public static partial class User32
    {
        [DllImport(Libraries.User32, ExactSpelling = true)]
        internal static extern BOOL AdjustWindowRectExForDpi(ref RECT lpRect, int dwStyle, BOOL bMenu, int dwExStyle, uint dpi);
    }
}
