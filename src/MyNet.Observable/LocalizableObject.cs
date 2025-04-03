// -----------------------------------------------------------------------
// <copyright file="LocalizableObject.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using MyNet.Observable.Attributes;
using MyNet.Utilities;
using MyNet.Utilities.Localization;
using PropertyChanged;

namespace MyNet.Observable;

public class LocalizableObject : ObservableObject
{
    public LocalizableObject()
    {
        GlobalizationService.Current.CultureChanged += OnCultureChangedCallback;
        GlobalizationService.Current.TimeZoneChanged += OnTimeZoneChangedCallback;
    }

    private void OnCultureChangedCallback(object? sender, EventArgs e)
    {
        GetType().GetPublicPropertiesWithAttribute<UpdateOnCultureChangedAttribute>().ForEach(x => OnPropertyChanged(x.Name));
        OnCultureChanged();
    }

    private void OnTimeZoneChangedCallback(object? sender, EventArgs e)
    {
        GetType().GetPublicPropertiesWithAttribute<UpdateOnTimeZoneChangedAttribute>().ForEach(x => OnPropertyChanged(x.Name));
        OnTimeZoneChanged();
    }

    [SuppressPropertyChangedWarnings]
    protected virtual void OnCultureChanged() { }

    [SuppressPropertyChangedWarnings]
    protected virtual void OnTimeZoneChanged() { }

    protected override void Cleanup()
    {
        base.Cleanup();
        GlobalizationService.Current.CultureChanged -= OnCultureChangedCallback;
        GlobalizationService.Current.TimeZoneChanged -= OnTimeZoneChangedCallback;
    }
}
