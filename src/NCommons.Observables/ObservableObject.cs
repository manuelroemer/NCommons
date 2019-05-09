﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NCommons.Observables
{

    /// <summary>
    ///     An abstract base class for objects that need to implement the
    ///     <see cref="INotifyPropertyChanged"/> and/or <see cref="INotifyPropertyChanging"/>
    ///     interface(s).
    ///     
    ///     Apart from implementing the boilerplate code, this class also provides utility
    ///     methods for intuitively raising of these events.
    /// </summary>
    public abstract class ObservableObject : INotifyPropertyChanging, INotifyPropertyChanged
    {

        /// <summary>
        ///     Occurs when a property's value is about to change, but before the change
        ///     has actually been made yet.
        /// </summary>
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        ///     Occurs when a property's value has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ObservableObject"/> class.
        /// </summary>
        public ObservableObject() { }

        /// <summary>
        ///     Sets the value of the specified backing field of a property and raises the 
        ///     <see cref="PropertyChanging"/> and <see cref="PropertyChanged"/> events before
        ///     and after setting the property respectively.
        /// </summary>
        /// <typeparam name="T">The type of the property to be changed.</typeparam>
        /// <param name="property">
        ///     The property to be changed.
        /// </param>
        /// <param name="newValue">
        ///     The new value of the property.
        /// </param>
        /// <param name="propertyName">
        ///     The name of the property that is going to be changed.
        ///     If this is <c>null</c> or an empty string, no event will be raised.
        ///     
        ///     This parameter is marked with the <see cref="CallerMemberNameAttribute"/>.
        ///     As such, it will automatically be set by the compiler, if not specified.
        /// </param>
        /// <returns>
        ///     <c>true</c> if the property was actually changed;
        ///     <c>false</c> if not.
        /// </returns>
        protected bool Set<T>(
            ref T property,
            T newValue,
            [CallerMemberName]string? propertyName = "")
        {
            return Set(ref property, newValue, EqualityComparer<T>.Default, propertyName);
        }

        /// <summary>
        ///     Sets the value of the specified backing field of a property and raises the 
        ///     <see cref="PropertyChanging"/> and <see cref="PropertyChanged"/> events before
        ///     and after setting the property respectively.
        /// </summary>
        /// <typeparam name="T">The type of the property to be changed.</typeparam>
        /// <param name="property">
        ///     The property to be changed.
        /// </param>
        /// <param name="newValue">
        ///     The new value of the property.
        /// </param>
        /// <param name="equalityComparer">
        ///     An equality comparer which is used to compare the property's current and
        ///     new value.
        ///     If the two properties are considered equal, no events will be raised.
        ///     
        ///     If <c>null</c>, no value comparison will be done and the events will be raised,
        ///     even if the values are equal.
        /// </param>
        /// <param name="propertyName">
        ///     The name of the property that is going to be changed.
        ///     If this is <c>null</c> or an empty string, no event will be raised.
        ///     
        ///     This parameter is marked with the <see cref="CallerMemberNameAttribute"/>.
        ///     As such, it will automatically be set by the compiler, if not specified.
        /// </param>
        /// <returns>
        ///     <c>true</c> if the property was actually changed;
        ///     <c>false</c> if not.
        /// </returns>
        protected bool Set<T>(
            ref T property,
            T newValue,
            IEqualityComparer<T>? equalityComparer,
            [CallerMemberName]string? propertyName = "")
        {
            // No need to redundantly set the property (and raise events).
            if (equalityComparer != null && equalityComparer.Equals(property, newValue))
            {
                return false;
            }

            OnPropertyChanging(propertyName);
            property = newValue;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        ///     Sets a property via a user-provided delegate and raises the <see cref="PropertyChanging"/>
        ///     and <see cref="PropertyChanged"/> events before and after setting the property
        ///     respectively.
        /// </summary>
        /// <param name="setProperty">
        ///     An action which sets the property for which the change events should be raised.
        /// </param>
        /// <param name="propertyName">
        ///     The name of the property that is going to be changed.
        ///     If this is <c>null</c> or an empty string, no event will be raised.
        ///     
        ///     This parameter is marked with the <see cref="CallerMemberNameAttribute"/>.
        ///     As such, it will automatically be set by the compiler, if not specified.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="setProperty"/>
        /// </exception>
        protected void Set(Action setProperty, [CallerMemberName]string? propertyName = "")
        {
            if (setProperty is null) throw new ArgumentNullException(nameof(setProperty));

            OnPropertyChanging(propertyName);
            setProperty();
            OnPropertyChanged(propertyName);
        }

        /// <summary>
        ///     Raises the <see cref="PropertyChanging"/> event with the specified property name.
        /// </summary>
        /// <param name="propertyName">
        ///     The name of the property that is about to change.
        ///     If this is <c>null</c> or an empty string, the event will not be raised.
        ///     
        ///     This parameter is marked with the <see cref="CallerMemberNameAttribute"/>.
        ///     As such, it will automatically be set by the compiler, if not specified.
        /// </param>
        protected void OnPropertyChanging([CallerMemberName]string? propertyName = "")
        {
            if (string.IsNullOrEmpty(propertyName)) return;
            OnPropertyChanging(new PropertyChangingEventArgs(propertyName));
        }

        /// <summary>
        ///     Raises the <see cref="PropertyChanging"/> event with the specified event args.
        /// </summary>
        /// <param name="args">
        ///     The event args which provide detail about the property that is about to change.
        ///     If this is <c>null</c>, the event will not be raised.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="args"/>
        /// </exception>
        protected virtual void OnPropertyChanging(PropertyChangingEventArgs? args)
        {
            if (args != null)
            {
                PropertyChanging?.Invoke(this, args);
            }
        }

        /// <summary>
        ///     Raises the <see cref="PropertyChanged"/> event with the specified property name.
        /// </summary>
        /// <param name="propertyName">
        ///     The name of the property that changed.
        ///     If this is <c>null</c> or an empty string, the event will not be raised.
        ///     
        ///     This parameter is marked with the <see cref="CallerMemberNameAttribute"/>.
        ///     As such, it will automatically be set by the compiler, if not specified.
        /// </param>
        protected void OnPropertyChanged([CallerMemberName]string? propertyName = "")
        {
            if (string.IsNullOrEmpty(propertyName)) return;
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        ///     Raises the <see cref="PropertyChanged"/> event with the specified event args.
        /// </summary>
        /// <param name="args">
        ///     The event args which provide detail about the changed property.
        ///     If this is <c>null</c>, the event will not be raised.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="args"/>
        /// </exception>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs? args)
        {
            if (args != null)
            {
                PropertyChanged?.Invoke(this, args);
            }
        }

    }

}