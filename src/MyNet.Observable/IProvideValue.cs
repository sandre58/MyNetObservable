// -----------------------------------------------------------------------
// <copyright file="IProvideValue.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;

namespace MyNet.Observable;

public interface IProvideValue<out T> : INotifyPropertyChanged
{
    T? Value { get; }
}
