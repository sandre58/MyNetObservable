// -----------------------------------------------------------------------
// <copyright file="IEditableObject.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using MyNet.Utilities;

namespace MyNet.Observable;

public interface IEditableObject : INotifyPropertyChanged, INotifyPropertyChanging, INotifyDataErrorInfo, IValidatable, IModifiable;
