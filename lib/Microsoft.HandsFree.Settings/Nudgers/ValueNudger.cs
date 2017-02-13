using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Microsoft.HandsFree.MVVM;

namespace Microsoft.HandsFree.Settings.Nudgers
{
    /// <summary>
    /// Abstract implementation of IValueNudger for general object property.
    /// </summary>
    public abstract class ValueNudger : IValueNudger
    {
        /// <summary>
        /// The object containing the property.
        /// </summary>
        readonly INotifyPropertyChanged _container;

        /// <summary>
        /// The name of the property within the object.
        /// </summary>
        readonly string _propertyName;

        /// <summary>
        /// The description of the property.
        /// </summary>
        readonly string _description;

        /// <summary>
        /// The reflected property information.
        /// </summary>
        readonly PropertyInfo _property;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="container">The object containing the property.</param>
        /// <param name="propertyName">he name of the property within the object.</param>
        /// <param name="description">The description of the property.</param>
        protected ValueNudger(INotifyPropertyChanged container, string propertyName, string description)
        {
            // Copy the parameters.
            _container = container;
            _propertyName = propertyName;
            _description = description;

            // Reflect the property information.
            var type = container.GetType();
            _property = type.GetProperty(propertyName);
            Debug.Assert(_property != null, "Property name must match an actual property");

            // Track external changes to the property.
            container.PropertyChanged += OnPropertyChanged;

            // Create the nudge up and down commands.
            NudgeUp = new RelayCommand(NudgeUpAction, CanNudgeUpPredicate);
            NudgeDown = new RelayCommand(NudgeDownAction, CanNudgeDownPredicate);
        }

        /// <summary>
        /// Description to display to user describing setting.
        /// </summary>
        public string Description
        {
            get { return _description; }
        }

        /// <summary>
        /// Display string for value.
        /// </summary>
        public virtual string ValueString
        {
            get { return Value.ToString(); }
        }

        /// <summary>
        /// Visibility of Up/Down interaction.
        /// </summary>
        public virtual Visibility UpDownInterfaceVisibility { get { return Visibility.Visible; } }

        /// <summary>
        /// Visibility of boolean interaction.
        /// </summary>
        public virtual Visibility BooleanInterfaceVisibility { get { return Visibility.Collapsed; } }

        /// <summary>
        /// Command for incrementing the value.
        /// </summary>
        public ICommand NudgeUp
        {
            get;
            private set;
        }

        /// <summary>
        /// Command for decrementing the value.
        /// </summary>
        public ICommand NudgeDown
        {
            get;
            private set;
        }

        /// <summary>
        /// Current value of the property.
        /// </summary>
        public object Value
        {
            get
            {
                return _property.GetValue(_container);
            }
            set
            {
                _property.SetValue(_container, value);
            }
        }

        /// <summary>
        /// Value as a Boolean.
        /// </summary>
        public bool ValueBool
        {
            get
            {
                return Value is bool ? (bool)Value : false;
            }
            set
            {
                if (Value is bool)
                {
                    Value = value;
                }
            }
        }

        /// <summary>
        /// Get the value to use if the property is nudged up.
        /// </summary>
        protected abstract object NudgeUpObjectValue { get; }

        /// <summary>
        /// Get the value to use if the property is nudged down.
        /// </summary>
        protected abstract object NudgeDownObjectValue { get; }

        /// <summary>
        /// Test whether the nudge up value is valid.
        /// </summary>
        protected virtual bool CanNudgeUp { get { return true; } }

        /// <summary>
        /// Test whether the nudge down value is valid.
        /// </summary>
        protected virtual bool CanNudgeDown { get { return true; } }

        /// <summary>
        /// Handler for performing nudge up.
        /// </summary>
        /// <param name="o">Unused.</param>
        void NudgeUpAction(object o)
        {
            Value = NudgeUpObjectValue;
        }

        /// <summary>
        /// Handler for performing nudge down.
        /// </summary>
        /// <param name="o">Unused.</param>
        void NudgeDownAction(object o)
        {
            Value = NudgeDownObjectValue;
        }

        /// <summary>
        /// Test whether we can nudge up.
        /// </summary>
        /// <param name="o">Unused.</param>
        /// <returns>Whether we can nudge up.</returns>
        bool CanNudgeUpPredicate(object o)
        {
            return CanNudgeUp;
        }

        /// <summary>
        /// Test whether we can nudge down.
        /// </summary>
        /// <param name="o">Unused.</param>
        /// <returns>Whether we can nudge down.</returns>
        bool CanNudgeDownPredicate(object o)
        {
            return CanNudgeDown;
        }

        /// <summary>
        /// The property information.
        /// </summary>
        protected PropertyInfo Property { get { return _property; } }

        /// <summary>
        /// The property changed handler.
        /// </summary>
        /// <param name="sender">Unused.</param>
        /// <param name="e">The event args.</param>
        void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Did our property change?
            if (e.PropertyName == _propertyName)
            {
                // Get the handler.
                var handler = PropertyChanged;

                // Is there a handler.
                if (handler != null)
                {
                    // Signal the value has changed.
                    handler(this, new PropertyChangedEventArgs(nameof(Value)));
                    handler(this, new PropertyChangedEventArgs(nameof(ValueString)));
                    handler(this, new PropertyChangedEventArgs(nameof(ValueBool)));

                    // TODO: An external change should re-evaluate CanNudgeUp and CanNudgeDown, etc.
                }
            }
        }

        public override bool Equals(object obj)
        {
            var peer = obj as ValueNudger;

            var isEqual = peer != null;
            if (isEqual)
            {
                isEqual = GetType() == peer.GetType() &&
                    _container == peer._container &&
                    _description == peer._description &&
                    _property == peer._property &&
                    _propertyName == peer._propertyName;
            }

            return isEqual;
        }

        public override int GetHashCode()
        {
            return _propertyName.GetHashCode();
        }

        /// <summary>
        /// The property changed event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }

    /// <summary>
    /// Abstract implementation of IValueNudger using ValueNudger for typed property.
    /// </summary>
    /// <typeparam name="T">The property type.</typeparam>
    public abstract class ValueNudger<T> : ValueNudger
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="container">The object containing the property.</param>
        /// <param name="propertyName">he name of the property within the object.</param>
        /// <param name="description">The description of the property.</param>
        protected ValueNudger(INotifyPropertyChanged container, string propertyName, string description)
            : base(container, propertyName, description)
        {
            Debug.Assert(Property.PropertyType == typeof(T), "Property type should match");
        }

        /// <summary>
        /// The value of the property.
        /// </summary>
        public new T Value
        {
            get
            {
                return (T)base.Value;
            }
        }

        /// <summary>
        /// Concrete implementation for nudge up object value.
        /// </summary>
        protected sealed override object NudgeUpObjectValue { get { return NudgeUpValue; } }

        /// <summary>
        /// Concrete implementation for nudge down object value.
        /// </summary>
        protected sealed override object NudgeDownObjectValue { get { return NudgeDownValue; } }

        /// <summary>
        /// The typed nudge up value.
        /// </summary>
        protected abstract T NudgeUpValue
        {
            get;
        }

        /// <summary>
        /// The typed nudge down value.
        /// </summary>
        protected abstract T NudgeDownValue
        {
            get;
        }
    }
}
