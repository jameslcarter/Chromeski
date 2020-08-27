﻿// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core.Infrastructure;
using Chromely.Core.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace Chromely
{
    public static class BrowserLauncher
    {
        public static void Open(ChromelyPlatform platform, string url)
        {
            try
            {
                try
                {
                    Process.Start(url);
                }
                catch
                {
                    try
                    {
                        // hack because of this: https://github.com/dotnet/corefx/issues/10361
                        switch (platform)
                        {
                            case ChromelyPlatform.Windows:
                                url = url.Replace("&", "^&");
                                Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                                break;

                            case ChromelyPlatform.Linux:
                                Process.Start("xdg-open", url);
                                break;

                            case ChromelyPlatform.MacOSX:
                                Process.Start("open", url);
                                break;

                            default:
                                Process.Start(url);
                                break;
                        }
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.Log.LogError(exception, exception.Message);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.LogError(exception, exception.Message);
            }
        }
    }
}
