// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using MyNet.Utilities;

namespace MyNet.Observable
{
    public interface IEditableObject : INotifyPropertyChanged, INotifyPropertyChanging, INotifyDataErrorInfo, IValidatable, IModifiable
    {
    }
}
