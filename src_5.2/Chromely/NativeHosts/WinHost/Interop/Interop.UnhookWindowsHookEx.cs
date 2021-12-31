﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.InteropServices;

namespace Chromely
{
    public static partial class Interop
    {
        public static partial class User32
        {
            [DllImport(Libraries.User32, ExactSpelling = true, SetLastError = true)]
            public static extern BOOL UnhookWindowsHookEx(IntPtr hhk);

            public static BOOL UnhookWindowsHookEx(HandleRef hhk)
            {
                BOOL result = UnhookWindowsHookEx(hhk.Handle);
                GC.KeepAlive(hhk.Wrapper);
                return result;
            }
        }
    }
}
