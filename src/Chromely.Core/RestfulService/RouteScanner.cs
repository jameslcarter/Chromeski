﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RouteScanner.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.Core.RestfulService
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using Chromely.Core.Infrastructure;

    /// <summary>
    /// The route scanner.
    /// </summary>
    public class RouteScanner
    {
        /// <summary>
        /// Gets or sets the mAssembly.
        /// </summary>
        private readonly Assembly mAssembly;

        /// <summary>
        /// Initializes a new instance of the <see cref="RouteScanner"/> class.
        /// </summary>
        /// <param name="assembly">
        /// The assembly.
        /// </param>
        public RouteScanner(Assembly assembly)
        {
            this.mAssembly = assembly;
        }

        /// <summary>
        /// Scans all registered assemblies for controller classes of <see cref="ChromelyController"/> type.
        /// The classes must have the <see cref="ControllerPropertyAttribute"/> attribute.
        /// </summary>
        /// <returns>
        /// The <see cref="IDictionary"/>.
        /// </returns>
        public Dictionary<string, Route> Scan()
        {
            var routeDictionary = new Dictionary<string, Route>();

            try
            {
                var types = from type in this.mAssembly.GetLoadableTypes()
                            where Attribute.IsDefined(type, typeof(ControllerPropertyAttribute))
                            select type;

                foreach (var type in types)
                {
                    if (type.BaseType == typeof(ChromelyController))
                    {
                        var instance = (ChromelyController)Activator.CreateInstance(type);
                        var currentRouteDictionary = instance.RouteDictionary;

                        // Merge with return route dictionary
                        if ((currentRouteDictionary != null) && currentRouteDictionary.Any())
                        {
                            foreach (var item in currentRouteDictionary)
                            {
                                if (!routeDictionary.ContainsKey(item.Key))
                                {
                                    routeDictionary.Add(item.Key, item.Value);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }

            return routeDictionary;
        }
    }

    /// <summary>
    /// The route scanner extensions.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1400:AccessModifierMustBeDeclared", Justification = "Reviewed. Suppression is OK here.")]
    static class RouteScannerExtensions
    {
        /// <summary>
        /// Get loadable types.
        /// </summary>
        /// <param name="assembly">
        /// The assembly.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }
    }
}
