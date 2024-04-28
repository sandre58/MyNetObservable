// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;

namespace MyNet.Observable
{
    public interface IAppointment : INotifyPropertyChanged
    {
        DateTime StartDate { get; }

        DateTime EndDate { get; }
    }
}
