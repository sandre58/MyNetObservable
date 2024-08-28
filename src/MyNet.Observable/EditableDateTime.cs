// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNet.Observable.Attributes;
using MyNet.Utilities.Deferring;
using MyNet.Utilities.Localization;
using MyNet.Utilities;
using PropertyChanged;
using MyNet.Observable.Resources;
using MyNet.Utilities.Extensions;

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
                ValidationRules.Add<EditableDateTime, DateTime?>(x => x.Date, () => ValidationResources.FieldXIsRequiredError.FormatWith(nameof(Date).Translate()), x => x is not null);
                ValidationRules.Add<EditableDateTime, TimeSpan?>(x => x.Time, () => ValidationResources.FieldXIsRequiredError.FormatWith(nameof(Time).Translate()), x => x is not null);
            }
        }

        public DateTime? Date { get; set; }

        public TimeSpan? Time { get; set; }

        [CanSetIsModified(false)]
        [CanBeValidated(false)]
        [AlsoNotifyFor(nameof(Date), nameof(Time))]
        public bool HasValue => Date.HasValue && Time.HasValue;

        [CanSetIsModified(false)]
        [CanBeValidated(false)]
        public DateTime? DateTime => Date.HasValue && Time.HasValue ? Date.Value.SetTime(Time.Value) : null;

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
                Date = date.Date;
                Time = date.TimeOfDay;
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
                    Date = date.Date;
                    Time = date.TimeOfDay;
                }
            }
            _currentTimeZone = GlobalizationService.Current.TimeZone;
        }

        protected virtual void OnDateChanged() => _dateTimeChangedDeferrer.IsDeferred.IfFalse(() => RaisePropertyChanged(nameof(DateTime)));

        protected virtual void OnTimeChanged() => _dateTimeChangedDeferrer.IsDeferred.IfFalse(() => RaisePropertyChanged(nameof(DateTime)));

        public override string? ToString() => Date?.SetTime(Time ?? TimeSpan.Zero).ToString();
    }
}
