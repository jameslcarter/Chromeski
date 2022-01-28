﻿// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core.Network;

namespace Chromely.Core
{
    public interface IChromelySchemeHandler
    {
        string Name { get; set; }
        UrlScheme Scheme { get; set; }
        object Handler { get; set; }
        object HandlerFactory { get; set; }
        bool IsCorsEnabled { get; set; }
        bool IsSecure { get; set; }
    }
}
