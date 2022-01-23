﻿// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Defaults;

/// <summary>
/// The default implementation of <see cref="IChromelyAppSettings"/>.
/// </summary>
public class DefaultAppSettings : IChromelyAppSettings
{
    private ChromelyDynamic? _chromelyDynamic;

    /// <inheritdoc/>
    public string AppName { get; set; }

    /// <inheritdoc/>
    public string? DataPath { get; set; }

    /// <summary>
    /// Initializes a new instance of <see cref="DefaultAppSettings"/>.
    /// </summary>
    /// <param name="appName">The application name.</param>
    public DefaultAppSettings(string appName = "chromely")
    {
        AppName = appName;
    }

    /// <inheritdoc/>
    public dynamic Settings
    {
        get
        {
            if (_chromelyDynamic is null)
            {
                _chromelyDynamic = new ChromelyDynamic();
            }

            return _chromelyDynamic;
        }
    }

    /// <inheritdoc/>
    public virtual void Read(IChromelyConfiguration config)
    {
        try
        {
            var appSettingsFile = AppSettingInfo.GetSettingsFilePath(config.Platform, AppName);

            if (appSettingsFile is null)
            {
                return;
            }

            var info = new FileInfo(appSettingsFile);
            if ((info.Exists) && info.Length > 0)
            {
                using StreamReader jsonReader = new(appSettingsFile);
                string json = jsonReader.ReadToEnd();
                var options = new JsonSerializerOptions
                {
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    AllowTrailingCommas = true
                };

                var settingsJsonElement = JsonSerializer.Deserialize<JsonElement>(json, options);
                var settingsDic = JsonToDictionary(settingsJsonElement);
                _chromelyDynamic = new ChromelyDynamic(settingsDic);
            }

            if (File.Exists(appSettingsFile))
            {
                DataPath = appSettingsFile;
            }
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError(exception);
        }
    }

    /// <inheritdoc/>
    public virtual void Save(IChromelyConfiguration config)
    {
        try
        {
            if (_chromelyDynamic is null ||
                _chromelyDynamic.Empty ||
                _chromelyDynamic.Dictionary is null ||
                _chromelyDynamic.Dictionary.Count == 0)
            {
                return;
            }

            var appSettingsFile = DataPath;

            if (string.IsNullOrWhiteSpace(appSettingsFile))
            {
                appSettingsFile = AppSettingInfo.GetSettingsFilePath(config.Platform, AppName, true);
            }

            if (appSettingsFile is null)
            {
                return;
            }

            using StreamWriter streamWriter = File.CreateText(appSettingsFile);
            try
            {
                var options = new JsonSerializerOptions
                {
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    AllowTrailingCommas = true
                };

                var jsonDic = JsonSerializer.Serialize(_chromelyDynamic.Dictionary, options);
                streamWriter.Write(jsonDic);

                Logger.Instance.Log.LogInformation("AppSettings FileName:{appSettingsFile}", appSettingsFile);
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.LogWarning("{ exception.Message}", exception.Message);
                Logger.Instance.Log.LogWarning("If this is about cycle was detecttion please see - https://github.com/dotnet/corefx/issues/41288");
            }
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError(exception);
        }
    }

#nullable disable
    private IDictionary<string, object> JsonToDictionary(JsonElement jsonElement)
    {
        var dic = new Dictionary<string, object>();

        foreach (var jsonProperty in jsonElement.EnumerateObject())
        {
            switch (jsonProperty.Value.ValueKind)
            {
                case JsonValueKind.Null:
                    dic.Add(jsonProperty.Name, null);
                    break;
                case JsonValueKind.Number:
                    dic.Add(jsonProperty.Name, jsonProperty.Value.GetDouble());
                    break;
                case JsonValueKind.False:
                    dic.Add(jsonProperty.Name, false);
                    break;
                case JsonValueKind.True:
                    dic.Add(jsonProperty.Name, true);
                    break;
                case JsonValueKind.Undefined:
                    dic.Add(jsonProperty.Name, null);
                    break;
                case JsonValueKind.String:
                    var strValue = jsonProperty.Value.GetString();
                    if (DateTime.TryParse(strValue, out DateTime date))
                    {
                        dic.Add(jsonProperty.Name, date);
                    }
                    else
                    {
                        dic.Add(jsonProperty.Name, strValue);
                    }

                    break;
                case JsonValueKind.Object:
                    dic.Add(jsonProperty.Name, JsonToDictionary(jsonProperty.Value));
                    break;
                case JsonValueKind.Array:
                    ArrayList objectList = new();
                    foreach (JsonElement item in jsonProperty.Value.EnumerateArray())
                    {
                        switch (item.ValueKind)
                        {
                            case JsonValueKind.Null:
                                objectList.Add(null);
                                break;
                            case JsonValueKind.Number:
                                objectList.Add(item.GetDouble());
                                break;
                            case JsonValueKind.False:
                                objectList.Add(false);
                                break;
                            case JsonValueKind.True:
                                objectList.Add(true);
                                break;
                            case JsonValueKind.Undefined:
                                objectList.Add(null);
                                break;
                            case JsonValueKind.String:
                                var itemValue = item.GetString();
                                if (DateTime.TryParse(itemValue, out DateTime itemDate))
                                {
                                    objectList.Add(itemDate);
                                }
                                else
                                {
                                    objectList.Add(itemValue);
                                }

                                break;
                            default:
                                objectList.Add(JsonToDictionary(item));
                                break;
                        }
                    }
                    dic.Add(jsonProperty.Name, objectList);
                    break;
            }
        }

        return dic;
    }
#nullable restore
}