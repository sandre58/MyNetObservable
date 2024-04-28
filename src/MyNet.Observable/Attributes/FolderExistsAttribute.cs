// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using MyNet.Observable.Resources;

namespace MyNet.Observable.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class FolderExistsAttribute : ValidationAttribute
    {
        public bool AllowEmpty { get; set; }

        public FolderExistsAttribute(bool allowEmpty = true)
        {
            AllowEmpty = true;
            ErrorMessageResourceName = nameof(ValidationResources.FieldXMustContainExistingFolderError);
            ErrorMessageResourceType = typeof(ValidationResources);
        }

        public override bool IsValid(object? value) => AllowEmpty && string.IsNullOrEmpty(value?.ToString()) || !string.IsNullOrEmpty(value?.ToString()) && value is string filepath && (Directory.Exists(Path.GetDirectoryName(filepath)) || Directory.Exists(filepath));
    }
}
