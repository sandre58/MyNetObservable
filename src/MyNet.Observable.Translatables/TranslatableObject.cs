// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using MyNet.Observable.Attributes;
using MyNet.Utilities;
using MyNet.Utilities.Localization;
using PropertyChanged;

namespace MyNet.Observable.Translatables
{
    public class TranslatableObject<T> : ObservableObject, IProvideValue<T>
    {
        private readonly Func<T?> _provideValue;

        public TranslatableObject(Func<T?> provideValue)
        {
            _provideValue = provideValue;
            CultureInfoService.Current.CultureChanged += LocalizationManager_CultureChanged;
        }

        [UpdateOnCultureChanged]
        public virtual T? Value => _provideValue != null ? _provideValue.Invoke() : default;

        public override string? ToString() => Value?.ToString();

        #region Culture

        private void LocalizationManager_CultureChanged(object? sender, EventArgs e)
        {
            GetType().GetPublicPropertiesWithAttribute<UpdateOnCultureChangedAttribute>().ForEach(x => RaisePropertyChanged(x.Name));
            OnCultureChanged();
        }

        [SuppressPropertyChangedWarnings]
        protected virtual void OnCultureChanged() { }

        protected override void Cleanup()
        {
            base.Cleanup();
            CultureInfoService.Current.CultureChanged -= LocalizationManager_CultureChanged;
        }

        #endregion
    }
}
