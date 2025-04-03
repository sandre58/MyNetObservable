// -----------------------------------------------------------------------
// <copyright file="EditableWrapper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using MyNet.Utilities;

namespace MyNet.Observable;

public class EditableWrapper<T>(T item) : EditableObject, ICloneable, ISettable, IIdentifiable<Guid>, IWrapper<T>
{
    public Guid Id { get; } = Guid.NewGuid();

    public T Item { get; protected set; } = item;

    protected virtual void OnItemChanged()
    {
        if (Item is INotifyPropertyChanged notifyPropertyChanged)
        {
            notifyPropertyChanged.PropertyChanged += Item_PropertyChanged;
        }
    }

    protected virtual void OnItemChanging()
    {
        if (Item is INotifyPropertyChanged notifyPropertyChanged)
        {
            notifyPropertyChanged.PropertyChanged -= Item_PropertyChanged;
        }

        if (Item is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    private void Item_PropertyChanged(object? sender, PropertyChangedEventArgs e) => OnPropertyChanged(e.PropertyName);

    public virtual object Clone()
    {
        var item = Item is ICloneable clonable ? (T)clonable.Clone() : Item;
        return CreateCloneInstance(item);
    }

    protected virtual EditableWrapper<T> CreateCloneInstance(T item) => new(item);

    public void SetFrom(object? from)
    {
        if (Item is ISettable settable)
        {
            switch (from)
            {
                case T newItem:
                    settable.SetFrom(newItem);
                    break;
                case EditableWrapper<T> newWrapper:
                    settable.SetFrom(newWrapper.Item);
                    break;
            }
        }
        else
        {
            switch (from)
            {
                case T newItem:
                    Item?.DeepSet(newItem);
                    break;
                case EditableWrapper<T> newWrapper:
                    Item?.DeepSet(newWrapper.Item);
                    break;
            }
        }
    }

    public override bool Equals(object? obj) => obj != null && GetType() == obj.GetType() && ReferenceEquals(Item, ((EditableWrapper<T>)obj).Item);

    public override int GetHashCode() => Item?.GetHashCode() ?? 0;

    protected override void Cleanup()
    {
        if (Item is INotifyPropertyChanged notifyPropertyChanged)
        {
            notifyPropertyChanged.PropertyChanged -= Item_PropertyChanged;
        }

        base.Cleanup();
    }
}
