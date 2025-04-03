// -----------------------------------------------------------------------
// <copyright file="ValidatePropertySuspender.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Utilities.Suspending;

namespace MyNet.Observable.Suspenders;

public static class ValidatePropertySuspender
{
    public static Suspender Default { get; } = new();
}
