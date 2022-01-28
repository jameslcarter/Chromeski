﻿// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Infrastructure;

public static class AppBuilderExtensions
{
    /// <summary>
    /// Ensure that the reference type is derived from base type.
    /// </summary>
    /// <remarks>
    /// If it fails an exception is thrown.
    /// </remarks>
    /// <typeparam name="T">Type parameter of <see cref="Type" />.</typeparam>
    /// <param name="derivedType">The derived type to check.</param>
    /// <exception cref="Exception"></exception>
    public static void EnsureIsDerivedFromType<T>(this Type derivedType)
    {
        var baseType = typeof(T);

        if (baseType == derivedType)
        {
            throw new Exception($"Cannot specify the base type {baseType.Name} itself as generic type parameter.");
        }

        if (!baseType.IsAssignableFrom(derivedType))
        {
            throw new Exception($"Type {derivedType.Name} must implement {baseType.Name}.");
        }

        if (derivedType.IsAbstract || derivedType.IsInterface)
        {
            throw new Exception($"Type {derivedType.Name} cannot be an interface or abstract class.");
        }
    }
}