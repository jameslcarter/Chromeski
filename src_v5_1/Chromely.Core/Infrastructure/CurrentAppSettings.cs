﻿// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core.Defaults;

namespace Chromely.Core.Infrastructure
{
    public class CurrentAppSettings : ChromelyAppUser
    {
        private IChromelyAppSettings appSettings;

        public override IChromelyAppSettings Properties
        {
            get
            {
                if (appSettings == null)
                {
                    appSettings = new DefaultAppSettings();
                }

                return appSettings;
            }
            set
            {
                appSettings = value;
            }
        }

    }
}
