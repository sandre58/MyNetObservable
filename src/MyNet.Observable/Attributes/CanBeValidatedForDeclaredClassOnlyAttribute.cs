// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;

namespace MyNet.Observable.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
    public sealed class CanBeValidatedForDeclaredClassOnlyAttribute(bool value = true) : Attribute
    {
        public bool Value { get; } = value;
    }
}
