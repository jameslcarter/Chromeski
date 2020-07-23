﻿// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Network
{
    public interface IChromelyCommandTaskRunner
    {
        void Run(string url);
        void RunAsync(string url);
    }
}
