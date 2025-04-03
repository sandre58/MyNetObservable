// -----------------------------------------------------------------------
// <copyright file="IResetable.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Observable;

public interface IResetable<out T>
{
    T? DefaultValue { get; }

    void Reset();
}
