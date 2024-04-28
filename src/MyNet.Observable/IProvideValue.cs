// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;

namespace MyNet.Observable
{
    public interface IProvideValue<out T> : INotifyPropertyChanged
    {
        T? Value { get; }
    }
}
