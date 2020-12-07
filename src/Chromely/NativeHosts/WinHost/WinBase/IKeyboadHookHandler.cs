﻿// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core.Host;
using System;

namespace Chromely.NativeHost
{
    public interface IKeyboadHookHandler
    {
        void SetNativeHost(IChromelyNativeHost nativeHost);
        bool HandleKey(IntPtr handle, object param);
    }
}
