﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceRouteProvider.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Chromely.Core.Network;

namespace Chromely.Core.Infrastructure
{
    public static class ServiceRouteProvider
    {
        public static void RegisterActionRoute(IChromelyContainer container, string key, ActionRoute route)
        {
            container.RegisterInstance(typeof(ActionRoute), key, route);
        }

        public static void RegisterCommandRoute(IChromelyContainer container, string key, CommandRoute command)
        {
            container.RegisterInstance(typeof(CommandRoute), key, command);
        }

        public static void RegisterActionRoutes(IChromelyContainer container, Dictionary<string, ActionRoute> newRouteDictionary)
        {
            if (newRouteDictionary != null && newRouteDictionary.Any())
            {
                foreach (var item in newRouteDictionary)
                {
                    container.RegisterInstance(typeof(ActionRoute), item.Key, item.Value);
                }
            }
        }

        public static void RegisterCommnandRoutes(IChromelyContainer container, Dictionary<string, CommandRoute> newCommandDictionary)
        {
            if (newCommandDictionary != null && newCommandDictionary.Any())
            {
                foreach (var item in newCommandDictionary)
                {
                    container.RegisterInstance(typeof(CommandRoute), item.Key, item.Value);
                }
            }
        }

        public static ActionRoute GetActionRoute(IChromelyContainer container, RoutePath routePath)
        {
            var route = container.GetInstance(typeof(ActionRoute), routePath.Key) as ActionRoute;
            if (route == null)
            {
                throw new Exception($"No route found for method:{routePath.Method} route path:{routePath.Path}.");
            }

            return route;
        }

        public static CommandRoute GetCommandRoute(IChromelyContainer container, string commandPath)
        {
            var key = CommandRoute.GetKeyFromPath(commandPath);
            var command = container.GetInstance(typeof(CommandRoute), key) as CommandRoute;
            if (command == null)
            {
                throw new Exception($"No route found for command with key:{key}.");
            }

            return command;
        }

        public static bool IsActionRouteAsync(IChromelyContainer container, RoutePath routePath)
        {
            try
            {
                var route = container.GetInstance(typeof(ActionRoute), routePath.Key) as ActionRoute;
                return route == null ? false : route.IsAsync;
            }
            catch {}

            return false;
        }
    }
}