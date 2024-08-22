// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using MyNet.Observable.Attributes;
using MyNet.Utilities;
using MyNet.Utilities.Localization;
using PropertyChanged;

namespace MyNet.Observable
{
    public class LocalizableObject : ObservableObject
    {
        public LocalizableObject()
        {
            GlobalizationService.Current.CultureChanged += OnCultureChangedCallback;
            GlobalizationService.Current.TimeZoneChanged += OnTimeZoneChangedCallback;
        }

        private void OnCultureChangedCallback(object? sender, EventArgs e)
        {
            GetType().GetPublicPropertiesWithAttribute<UpdateOnCultureChangedAttribute>().ForEach(x => RaisePropertyChanged(x.Name));
            OnCultureChanged();
        }

        private void OnTimeZoneChangedCallback(object? sender, EventArgs e)
        {
            GetType().GetPublicPropertiesWithAttribute<UpdateOnTimeZoneChangedAttribute>().ForEach(x => RaisePropertyChanged(x.Name));
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
}
