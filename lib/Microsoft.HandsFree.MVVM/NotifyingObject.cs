// -------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All Rights Reserved.
// -------------------------------------------------------------------
namespace Microsoft.HandsFree.MVVM
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// An implementation of <see cref="INotifyPropertyChanged"/> that provides debug-only verification
    /// that properties for which change notifications are being raised actually exist on the object.
    /// This class also has a concise SetProperty method that sets a property only if it has changed and
    /// then raises a change notification for the calling property without having to pass the property name
    /// as a string.
    /// </summary>
    /// <remarks>
    /// For example, the following code...
    /// 
    ///		private string myString;
    ///		public string MyString
    ///		{
    ///			get { return this.myString; }
    ///			set
    ///			{
    ///				if (this.myString != value)
    ///				{
    ///					this.myString = value;
    ///					this.OnPropertyChanged("MyString");
    ///				}
    ///			}
    ///		}
    ///		
    /// ... can be simplified using this class.
    /// 
    /// 	public string MyString
    ///		{
    ///			get { return this.myString; }
    ///			set
    ///			{
    ///				this.SetProperty(ref this.myString, value);
    ///			}
    ///		}
    /// </remarks>
    public abstract class NotifyingObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">Name of the changed property.
        /// Can be null to indicate that all of the properties have changed.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            if (propertyName != null)
            {
                VerifyPropertyExists(propertyName, this);
            }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Checks if a property already matches a desired value.  Sets the property and
        /// notifies listeners only when necessary.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="target">Reference to a property with both getter and setter.</param>
        /// <param name="value">Desired value for the property.</param>
        /// <param name="propertyName">Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers that
        /// support <see cref="CallerMemberNameAttribute"/>.</param>
        /// <returns>True if the value was changed, false if the existing value matched the desired value.</returns>
        [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", Justification = "This is an established pattern. See BindableBase in the VS templates.")]
        protected bool SetProperty<T>(ref T target, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(target, value))
            {
                return false;
            }

            target = value;

            OnPropertyChanged(propertyName);
            return true;
        }

        [Conditional("DEBUG")]
        public static void VerifyPropertyExists(string propertyName, object sender)
        {
            if (string.IsNullOrEmpty(propertyName) || sender == null)
            {
                return;
            }

            var senderType = sender.GetType();

            var bracketsIndex = propertyName.LastIndexOf("[]", StringComparison.OrdinalIgnoreCase);
            var isIndexedProperty = bracketsIndex != -1;
            if (isIndexedProperty)
            {
                propertyName = propertyName.Substring(0, bracketsIndex);
            }

            var foundProperty = senderType.GetProperty(propertyName);
            if (isIndexedProperty && foundProperty != null)
            {
                Debug.Assert(foundProperty.GetIndexParameters().Length > 0,
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "{0} represents an indexed property, but no indexer property was found on type {1}.",
                        propertyName, senderType.FullName));
            }

            Debug.Assert(foundProperty != null,
                string.Format(CultureInfo.InvariantCulture, "Property {0} does not exist on type {1}.", propertyName, senderType.FullName));
        }
    }
}
