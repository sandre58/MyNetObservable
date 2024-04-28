﻿// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;

namespace MyNet.Observable.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface, Inherited = true)]
    public sealed class CanSetIsModifiedAttribute(bool value = true) : Attribute
    {
        public bool Value { get; } = value;
    }
}
