// -----------------------------------------------------------------------
// <copyright file="AttributeExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace MyNet.Observable.Attributes;

public static class AttributeExtensions
{
    public static bool CanBeValidated(this PropertyInfo property, object? obj = null)
    {
        if (property is { CanWrite: false, CanRead: false }) return false;

        var propertyType = obj is not null && property.GetValue(obj) is { } value ? value.GetType() : property.PropertyType;
        return property.GetCustomAttributes<ValidationAttribute>().Any()
               || property.GetCustomAttributes<CanBeValidatedAttribute>().Any(x => x.Value)
               || (property.GetCustomAttributes<CanBeValidatedAttribute>().All(x => x.Value) && propertyType.CanBeValidated()
                                                                                             && property.ReflectedType.CanBeValidated()
                                                                                             && property.DeclaringType.CanBeValidated());
    }

    public static bool CanBeValidated(this Type? type) => type == null || (type.GetCustomAttributes<CanBeValidatedAttribute>().All(x => x.Value) && type.GetCustomAttributes<CanBeValidatedForDeclaredClassOnlyAttribute>().All(x => x.Value));

    public static bool CanSetIsModified(this PropertyInfo property, object? obj = null)
    {
        if (property is { CanWrite: false, CanRead: false }) return false;

        var propertyType = obj is not null && property.GetValue(obj) is { } value ? value.GetType() : property.PropertyType;
        return property.GetCustomAttributes<CanSetIsModifiedAttribute>().Any(x => x.Value)
               || (property.GetCustomAttributes<CanSetIsModifiedAttribute>().All(x => x.Value) && propertyType.CanSetIsModified()
                                                                                               && property.ReflectedType.CanSetIsModified()
                                                                                               && property.DeclaringType.CanSetIsModified());
    }

    public static bool CanSetIsModified(this Type? type) => type == null || (type.GetCustomAttributes<CanSetIsModifiedAttribute>().All(x => x.Value) && type.GetCustomAttributes<CanSetIsModifiedAttributeForDeclaredClassOnlyAttribute>().All(x => x.Value));

    public static bool CanNotify(this PropertyInfo property) => (property.CanWrite || property.CanRead) &&
        (property.GetCustomAttributes<CanNotifyAttribute>().Any(x => x.Value) ||
         (property.GetCustomAttributes<CanNotifyAttribute>().All(x => x.Value) &&
          property.PropertyType.CanNotify() &&
          property.ReflectedType.CanNotify()));

    public static bool CanNotify(this Type? type) => type == null || type.GetCustomAttributes<CanNotifyAttribute>().All(x => x.Value);
}
