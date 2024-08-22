// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;

namespace MyNet.Observable.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public sealed class UpdateOnTimeZoneChangedAttribute : Attribute
    {
    }
}
