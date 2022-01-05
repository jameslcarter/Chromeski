﻿// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Browser;

public enum ProcessType
{
    None,
    Browser,
    Renderer,
    Zygot,
    Other,
}

public static class ClientAppUtils
{
    const string ArgumentType = "--type";
    const string RendererType = "renderer";
    const string ZygoteType = "zygote";

    public static bool ExecuteProcess(ChromelyPlatform platform, IEnumerable<string> args)
    {
        if (platform != ChromelyPlatform.MacOSX)
        {
            return true;
        }

        return HasArgument(args, ArgumentType);
    }

    public static ProcessType GetProcessType(IEnumerable<string> args)
    {
        if (args is null || !args.Any())
        {
            return ProcessType.Browser;
        }

        if (HasArgument(args, ArgumentType))
        {
            string? type = GetArgumentValue(args, ArgumentType);
            if (type is not null && !string.IsNullOrWhiteSpace(type) && type.Equals(RendererType, StringComparison.InvariantCultureIgnoreCase))
            {
                return ProcessType.Renderer;
            }

            type = GetArgumentValue(args, ArgumentType);
            if (type is not null && !string.IsNullOrWhiteSpace(type) && type.Equals(ZygoteType, StringComparison.InvariantCultureIgnoreCase))
            {
                return ProcessType.Renderer;
            }

            return ProcessType.Other;
        }

        return ProcessType.Browser;
    }

    private static bool HasArgument(IEnumerable<string> args, string arg)
    {
        return args.Any(a => a.StartsWith(arg));
    }

    private static string? GetArgumentValue(this IEnumerable<string> args, string argumentName)
    {
        if (args is not null)
        {
            var arg = args.FirstOrDefault(a => a.StartsWith(argumentName));
            if (arg is not null)
            {
                return arg.Split('=').Last();
            }
        }

        return default;
    }
}