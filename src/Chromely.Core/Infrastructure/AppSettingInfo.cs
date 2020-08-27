﻿// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Chromely.Core.Infrastructure
{
    public static class AppSettingInfo
    {
        public static string GetSettingsFilePath(ChromelyPlatform platform, string appName = "chromely", bool onSave = false)
        {
            try
            {
                var appSettingsDir = string.Empty;
                var fileName = $"{appName}_appsettings.config";

                switch (platform)
                {
                    case ChromelyPlatform.Windows:
                        appSettingsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.DoNotVerify), "chromely");
                        break;

                    case ChromelyPlatform.Linux:
                        appSettingsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.DoNotVerify), "chromely");
                        break;

                    case ChromelyPlatform.MacOSX:
                        appSettingsDir = Environment.GetFolderPath(Environment.SpecialFolder.Personal, Environment.SpecialFolderOption.DoNotVerify);
                        appSettingsDir = appSettingsDir.Replace("/Documents", "/Library/Application Support/chromely/");
                        break;
                }

                if (onSave)
                {
                    Directory.CreateDirectory(appSettingsDir);
                    if (Directory.Exists(appSettingsDir))
                    {
                        return Path.Combine(appSettingsDir, fileName);
                    }
                }
                else
                {
                    return Path.Combine(appSettingsDir, fileName);
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.LogError(exception, exception.Message);
            }

            return null;
        }
    }
}
