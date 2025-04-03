// -----------------------------------------------------------------------
// <copyright file="IAppointment.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;

namespace MyNet.Observable;

public interface IAppointment : INotifyPropertyChanged
{
    DateTime StartDate { get; }

    DateTime EndDate { get; }
}
