// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using MyNet.Observable.Attributes;
using MyNet.Observable.Resources;
using MyNet.Utilities;
using MyNet.Utilities.Deferring;
using MyNet.Utilities.Extensions;
using MyNet.Utilities.Localization;
using PropertyChanged;

namespace MyNet.Observable
{
    public class EditableDateTime : EditableObject
    {
        private TimeZoneInfo _currentTimeZone = GlobalizationService.Current.TimeZone;
        private readonly Deferrer _dateTimeChangedDeferrer;

        public EditableDateTime(bool isRequired = true)
        {
            _dateTimeChangedDeferrer = new Deferrer(() => RaisePropertyChanged(nameof(DateTime)));

            if (isRequired)
            {
                ValidationRules.Add<EditableDateTime, DateOnly?>(x => x.Date, () => ValidationResources.FieldXIsRequiredError.FormatWith(nameof(Date).Translate()), x => x is not null);
                ValidationRules.Add<EditableDateTime, TimeOnly?>(x => x.Time, () => ValidationResources.FieldXIsRequiredError.FormatWith(nameof(Time).Translate()), x => x is not null);
            }
        }

        public DateOnly? Date { get; set; }

        public TimeOnly? Time { get; set; }

        [CanSetIsModified(false)]
        [CanBeValidated(false)]
        [AlsoNotifyFor(nameof(Date), nameof(Time))]
        public bool HasValue => Date.HasValue && Time.HasValue;

        [CanSetIsModified(false)]
        [CanBeValidated(false)]
        public DateTime? DateTime => Date.HasValue && Time.HasValue ? Date.Value.At(Time.Value) : null;

        public DateTime? ToUtc() => DateTime.HasValue ? GlobalizationService.Current.ConvertToUtc(DateTime.Value) : null;

        public DateTime? ToLocal() => DateTime.HasValue ? GlobalizationService.Current.ConvertToTimeZone(DateTime.Value, TimeZoneInfo.Local) : null;

        public DateTime? ToTimeZone(TimeZoneInfo timezone) => DateTime.HasValue ? GlobalizationService.Current.ConvertToTimeZone(DateTime.Value, timezone) : null;

        public DateTime ToUtcOrDefault(DateTime defaultValue = default) => ToUtc() ?? defaultValue;

        public DateTime ToLocalOrDefault(DateTime defaultValue = default) => ToLocal() ?? defaultValue;

        public void Load(DateTime dateTime)
        {
            var date = GlobalizationService.Current.Convert(dateTime);

            using (_dateTimeChangedDeferrer.Defer())
            {
                Date = date.Date.ToDate();
                Time = date.ToTime();
            }
        }

        public void Clear()
        {
            using (_dateTimeChangedDeferrer.Defer())
            {
                Date = null;
                Time = null;
            }
        }

        [SuppressPropertyChangedWarnings]
        protected override void OnTimeZoneChanged()
        {
            base.OnTimeZoneChanged();

            if (DateTime.HasValue)
            {
                var date = TimeZoneInfo.ConvertTime(DateTime.Value, _currentTimeZone, GlobalizationService.Current.TimeZone);
                using (_dateTimeChangedDeferrer.Defer())
                {
                    Date = date.ToDate();
                    Time = date.ToTime();
                }
            }
            _currentTimeZone = GlobalizationService.Current.TimeZone;
        }

        protected virtual void OnDateChanged() => _dateTimeChangedDeferrer.IsDeferred.IfFalse(() => RaisePropertyChanged(nameof(DateTime)));

        protected virtual void OnTimeChanged() => _dateTimeChangedDeferrer.IsDeferred.IfFalse(() => RaisePropertyChanged(nameof(DateTime)));

        public override string? ToString() => Date?.At(Time ?? TimeOnly.MinValue).ToString();
    }
}
