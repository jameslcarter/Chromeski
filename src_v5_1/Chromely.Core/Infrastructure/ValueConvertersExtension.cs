﻿// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;
using Chromely.Core.Logging;
using Microsoft.Extensions.Logging;

namespace Chromely.Core.Infrastructure
{
    public static class ValueConvertersExtension
    {
        public static bool TryParseBoolean(this object value, out bool result)
        {
            result = false;

            try
            {
                switch (value)
                {
                    case null:
                        return false;
                    case bool boolValue:
                        result = boolValue;
                        return true;
                }

                return bool.TryParse(value.ToString(), out result);
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.LogError(exception, exception.Message);
            }

            return false;
        }

        public static bool TryParseInteger(this object value, out int result)
        {
            result = 0;

            try
            {
                switch (value)
                {
                    case null:
                        return false;
                    case int intValue:
                        result = intValue;
                        return true;
                }

                return int.TryParse(value.ToString(), out result);
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.LogError(exception, exception.Message);
            }

            return false;
        }

        public static bool TryParseString(this object value, out string result)
        {
            result = string.Empty;

            try
            {
                switch (value)
                {
                    case null:
                        return false;
                    case string strValue:
                        result = strValue;
                        return true;
                }

                result = value.ToString();
                return true; 
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.LogError(exception, exception.Message);
            }

            return false;
        }

        public static string EnumToString(this CefEventKey key)
        {
            return Enum.GetName(key.GetType(), key);
        }

        public static TEnumType ToEnum<TEnumType>(this string enumValue)
        {
            return (TEnumType)Enum.Parse(typeof(TEnumType), enumValue);
        }
    }
}
