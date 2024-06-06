// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using MyNet.Observable.Attributes;
using MyNet.Observable.Validation;
using MyNet.Utilities;
using MyNet.Utilities.Suspending;
using PropertyChanged;
using IEditableObject = MyNet.Observable.IEditableObject;

namespace MyNet.Observable
{
    /// <inheritdoc cref="ObservableObject" />
    /// <summary>
    /// The model base.
    /// </summary>
    [CanBeValidatedForDeclaredClassOnly(false)]
    [CanSetIsModifiedAttributeForDeclaredClassOnly(false)]
    public abstract class EditableObject : ObservableObject, IEditableObject
    {
        #region Fields

        private bool _isModified;

        #endregion Fields

        protected EditableObject()
        {
            var observableCollectionProperties = GetType().GetPublicProperties().Where(x => x.CanSetIsModified() && typeof(ICollection).IsAssignableFrom(x.PropertyType)).ToList();
            var observableCollections = observableCollectionProperties.GetValuesOfType<INotifyCollectionChanged>(this).NotNull().ToList();
            observableCollections.ForEach(x => x.CollectionChanged += CollectionChanged);
        }

        private void CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) => _isModified = true;

        [CanBeValidated(false)]
        [CanSetIsModified(false)]
        public override bool IsDisposed => base.IsDisposed;

        #region Members

        /// <summary>
        /// Gets the validation errors.
        /// </summary>
        private Dictionary<string, List<SeverityValidationResult>> ValidationErrors { get; } = [];

        /// <summary>
        /// Gets the rules which provide the errors.
        /// </summary>
        /// <value>The rules this instance must satisfy.</value>
        public ValidationRuleCollection ValidationRules { get; } = [];

        protected ISuspender IsModifiedSuspender { get; set; } = Suspenders.IsModifiedSuspender.Default;

        protected ISuspender ValidatePropertySuspender { get; set; } = Suspenders.ValidatePropertySuspender.Default;

        #endregion Members

        #region INotifyDataErrorInfo

        /// <summary>
        /// Occurs when the validation errors have changed for a property or for the entire object.
        /// </summary>
        protected event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        /// <inheritdoc />
        /// <summary>
        /// Occurs when the validation errors have changed for a property or for the entire object.
        /// </summary>
        event EventHandler<DataErrorsChangedEventArgs>? INotifyDataErrorInfo.ErrorsChanged
        {
            add => ErrorsChanged += value;
            remove => ErrorsChanged -= value;
        }

        /// <summary>
        /// Gets a value indicating whether the object has validation errors.
        /// </summary>
        /// <value><c>true</c> if this instance has errors, otherwise <c>false</c>.</value>
        public virtual IEnumerable<string> Errors => GetValidationMessages(ValidationRuleSeverity.Error);

        /// <summary>
        /// Gets a value indicating whether the object has validation errors.
        /// </summary>
        /// <value><c>true</c> if this instance has errors, otherwise <c>false</c>.</value>
        public virtual IEnumerable<string> Warnings => GetValidationMessages(ValidationRuleSeverity.Warning);

        /// <summary>
        /// Gets a value indicating whether the object has validation errors.
        /// </summary>
        /// <value><c>true</c> if this instance has errors, otherwise <c>false</c>.</value>
        public virtual IEnumerable<string> Informations => GetValidationMessages(ValidationRuleSeverity.Information);

        /// <summary>
        /// Gets a value indicating whether the object has validation errors.
        /// </summary>
        /// <value><c>true</c> if this instance has errors, otherwise <c>false</c>.</value>
        public virtual bool HasErrors => Errors.Any();

        /// <summary>
        /// Gets a value indicating whether the object has validation errors.
        /// </summary>
        /// <value><c>true</c> if this instance has errors, otherwise <c>false</c>.</value>
        public virtual bool HasWarnings => Warnings.Any();

        /// <summary>
        /// Gets a value indicating whether the object has validation errors.
        /// </summary>
        /// <value><c>true</c> if this instance has errors, otherwise <c>false</c>.</value>
        public virtual bool HasInformations => Informations.Any();

        private IEnumerable<string> GetValidationMessages(ValidationRuleSeverity severity)
            => ValidationErrors.SelectMany(x => x.Value.Where(y => y.Severity == severity && !string.IsNullOrEmpty(y.ErrorMessage)).Select(y => y.ErrorMessage!)).Distinct();

        /// <summary>
        /// Gets the validation errors for a specified property or for the entire object.
        /// </summary>
        /// <param name="propertyName">Name of the property to retrieve errors for. <c>null</c> to
        /// retrieve all errors for this instance.</param>
        /// <returns>A collection of errors.</returns>
        public IEnumerable GetErrors(string? propertyName) => string.IsNullOrEmpty(propertyName)
                ? GetValidationMessages(ValidationRuleSeverity.Error).ToList()
                : ValidationErrors.TryGetValue(propertyName!, out var errors) ? errors.Where(y => y.Severity == ValidationRuleSeverity.Error).Select(x => x.ErrorMessage).ToList() : Array.Empty<string?>();

        /// <summary>
        /// Gets the validation errors for the entire object.
        /// </summary>
        /// <returns>A collection of errors.</returns>
        public virtual IEnumerable<string> GetErrors()
        {
            var result = new List<string>();
            result.AddRange((IEnumerable<string>)GetErrors(string.Empty));

            var complexValidationErrors = GetType()
                .GetPublicProperties()
                .Where(x => x.CanBeValidated(this))
                .GetValuesOfType<IValidatable>(this)
                .SelectMany(x => x?.GetErrors() ?? [])
                .ToList();
            result.AddRange(complexValidationErrors);

            var collectionValidationErrors =
                GetType().GetPublicProperties()
                .Where(x => x.CanBeValidated(this))
                .Where(x => !typeof(IValidatable).IsAssignableFrom(x.PropertyType))
                .GetValuesOfType<IEnumerable>(this)
                .SelectMany(x => x?.OfType<IValidatable>() ?? [])
                .SelectMany(x => x.GetErrors()).ToList();
            result.AddRange(collectionValidationErrors);

            return result;
        }

        /// <summary>
        /// Notifies when errors changed.
        /// </summary>
        /// <param name="propertyName"></param>
        [SuppressPropertyChangedWarnings]
        protected void OnErrorsChanged(string propertyName) => ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));

        #endregion INotifyDataErrorInfo

        #region IValidatable

        /// <summary>
        /// Test if property is valid.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        protected virtual void ValidateProperty(string propertyName, object? value)
        {
            var prop = GetType().GetProperty(propertyName);
            if (ValidatePropertySuspender.IsSuspended || prop == null || !prop.CanBeValidated(this)) return;

            var metadataValidationResults = new List<ValidationResult>();


            // Validation by Metadata
            _ = Validator.TryValidateProperty(value, new ValidationContext(this) { MemberName = propertyName }, metadataValidationResults);

            var validationResults = metadataValidationResults.Select(x => new SeverityValidationResult(x.ErrorMessage, x.MemberNames)).ToList();
            // Custom Validation
            var customValidations = ValidationRules.Apply(this, propertyName).Select(x => new SeverityValidationResult(x.Error, [propertyName], x.Severity)).ToList();
            if (customValidations.Count > 0)
                validationResults.AddRange(customValidations);

            // Is Valid
            if (validationResults.Count == 0)
            {
                if (!ValidationErrors.ContainsKey(propertyName)) return;

                _ = ValidationErrors.Remove(propertyName);
                RaiseValidation();
                OnErrorsChanged(propertyName);
                return;
            }

            // Is not valid
            if (!ValidationErrors.TryAdd(propertyName, validationResults))
                ValidationErrors[propertyName] = validationResults;
            RaiseValidation();
            OnErrorsChanged(propertyName);
        }

        public virtual bool ValidateProperties()
        {
            if (ValidatePropertySuspender.IsSuspended) return true;

            var result = true;
            var properties = GetType().GetPublicProperties().Where(x => x.CanBeValidated(this)).ToList();
            foreach (var property in properties)
            {
                // All Property
                ValidateProperty(property.Name, property.GetValue(this));
                result = result && !HasErrors;

                // Complex property
                if (property.GetValue(this) is IValidatable entity)
                    result = result && entity.ValidateProperties();

                // Collection property
                else if (property.GetValue(this) is ICollection collection)
                    result = result && collection.OfType<IValidatable>().All(validatable => validatable.ValidateProperties());
            }

            return result;
        }

        public void ResetValidation()
        {
            var propertiesNotValid = ValidationErrors.Keys;
            ValidationErrors.Clear();
            RaiseValidation();

            propertiesNotValid.ForEach(OnErrorsChanged);
        }

        private void RaiseValidation()
        {
            RaisePropertyChanged(nameof(Errors));
            RaisePropertyChanged(nameof(Warnings));
            RaisePropertyChanged(nameof(Informations));
            RaisePropertyChanged(nameof(HasErrors));
            RaisePropertyChanged(nameof(HasWarnings));
            RaisePropertyChanged(nameof(HasInformations));
        }

        #endregion IValidatable

        #region IPropertyChanged

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="before">The before value.</param>
        /// <param name="after">The after value.</param>
        protected virtual void OnPropertyChanged(string propertyName, object before, object after)
        {
            if (PropertyChangedSuspender.IsSuspended || GetType().GetCustomAttributes<CanNotifyAttribute>().Any(x => !x.Value)) return;

            var prop = GetType().GetPublicProperties().FirstOrDefault(x => x.Name == propertyName);
            if (prop == null || !prop.CanNotify()) return;

            // Validation
            if (prop.CanBeValidated(this))
            {
                ValidateProperty(propertyName, after);

                // Validate other property defined by [ValidateProperty(<PropertyName>)] attributes.
                prop.GetCustomAttributes().OfType<ValidatePropertyAttribute>().ForEach(x =>
                {
                    var property = GetType().GetProperty(x.PropertyName);
                    var value = property?.GetValue(this);
                    ValidateProperty(x.PropertyName, value);
                });
            }

            // Modification
            if (!IsModifiedSuspender.IsSuspended && prop.CanSetIsModified(this))
            {
                _isModified = true;
                OnPropertyIsModified(propertyName, before, after);
            }

            // Notification
            RaisePropertyChanged(propertyName);
        }

        protected virtual void OnPropertyIsModified(string propertyName, object before, object after) { }

        #endregion IPropertyChanged

        #region IModifiable

        /// <inheritdoc />
        /// <summary>
        /// Gets a value indicates if the entity has been modified.
        /// </summary>
        public virtual bool IsModified()
        {
            if (_isModified) return true;

            var propertiesToChecked = GetType().GetPublicProperties().Where(x => x.CanSetIsModified(this)).ToList();

            var complexIsModified =
                propertiesToChecked.GetValuesOfType<IModifiable>(this).Any(x => x != null && x.IsModified());
            if (complexIsModified) return true;

            var collectionIsModified =
                    propertiesToChecked.Where(x => !typeof(IModifiable).IsAssignableFrom(x.PropertyType)).GetValuesOfType<ICollection>(this)
                    .SelectMany(x => x?.OfType<IModifiable>() ?? [])
                    .Any(x => x.IsModified());
            return collectionIsModified;
        }

        /// <inheritdoc />
        /// <summary>
        /// Reset IsModified value.
        /// </summary>
        public virtual void ResetIsModified()
        {
            _isModified = false;
            var propertiesToChecked = GetType().GetPublicProperties().Where(x => x.CanSetIsModified(this)).ToList();

            // Complex properties
            propertiesToChecked.GetValuesOfType<IModifiable>(this).ToList().ForEach(x => x?.ResetIsModified());

            // Collection properties
            propertiesToChecked.Where(x => !typeof(IModifiable).IsAssignableFrom(x.PropertyType)).GetValuesOfType<ICollection>(this)
                .SelectMany(x => x?.OfType<IModifiable>() ?? [])
                .ToList().ForEach(x => x.ResetIsModified());
        }

        #endregion IModifiable

        protected override void Cleanup()
        {
            base.Cleanup();

            var observableCollections =
                    GetType().GetPublicProperties().Where(x => x.CanSetIsModified() && typeof(ICollection).IsAssignableFrom(x.PropertyType)).GetValuesOfType<INotifyCollectionChanged>(this).NotNull().ToList();
            observableCollections.ForEach(x => x.CollectionChanged -= CollectionChanged);
        }
    }
}
