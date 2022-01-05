﻿// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.NativeHosts;

public class HookEventArgs : EventArgs
{
	public int HookCode;    // Hook code
	public IntPtr wParam;   // WPARAM argument
	public IntPtr lParam;   // LPARAM argument
}