using Microsoft.HandsFree.Settings.Nudgers;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Microsoft.HandsFree.Settings.Serialization
{
    public class SettingDescriptionAttribute : Attribute
    {
        readonly Func<INotifyPropertyChanged, string, ValueNudgerFactory> _factoryFactory;

        public SettingDescriptionAttribute()
            : this(SkeletonValueNudger.PlaceholderDescription, null, null)
        {
            _factoryFactory = (c, p) => () => new SkeletonValueNudger(c, p);
        }

        public SettingDescriptionAttribute(SettingNudgerFactoryBehavior behavior)
            : this(SkeletonValueNudger.PlaceholderDescription, behavior)
        {
        }

        public SettingDescriptionAttribute(string description, SettingNudgerFactoryBehavior behavior)
            : this(description, null, null, null)
        {
            Debug.Assert(behavior == SettingNudgerFactoryBehavior.NameAssociatedMember);

            _factoryFactory = (c, p) => NameAssociationNonFactory(c, p, description);
        }

        public SettingDescriptionAttribute(string description)
        {
            _factoryFactory = (c, p) => AutoCreateFactory(c, p, description);
        }

        public SettingDescriptionAttribute(string description, object minimum, object maximum)
            : this(description, null, null, null)
        {
        }

        public SettingDescriptionAttribute(string description, int min, int max, int increment)
        {
            _factoryFactory = (c, p) => () => new IntValueNudger(c, p, description, increment, min, max);
        }

        public SettingDescriptionAttribute(string description, int min, int max)
            : this(description, min, max, 1)
        {
        }

        public SettingDescriptionAttribute(string description, double min, double max, double increment)
        {
            _factoryFactory = (c, p) => () => new DoubleValueNudger(c, p, description, increment, min, max);
        }

        public SettingDescriptionAttribute(string description, object minimum, object maximum, object step)
        {
            if (_factoryFactory == null)
            {
                _factoryFactory = (c, p) => () => new SkeletonValueNudger(c, p);
            }
            else
            {
                Debug.WriteLine("Already set");
            }
        }

        ValueNudgerFactory AutoCreateFactory(INotifyPropertyChanged container, string propertyName, string description)
        {
            IValueNudger nudger;

            var containerType = container.GetType();
            var property = containerType.GetProperty(propertyName);
            var propertyType = property.PropertyType;

            if (propertyType.IsEnum)
            {
                nudger = new EnumValueNudger(container, propertyName, description);
            }
            else if (propertyType == typeof(bool))
            {
                nudger = new BooleanValueNudger(container, propertyName, description);
            }
            else
            {
                nudger = new SkeletonValueNudger(container, propertyName);
            }

            return () => nudger;
        }

        ValueNudgerFactory NameAssociationNonFactory(INotifyPropertyChanged container, string propertyName, string description)
        {
            var containerType = container.GetType();
            var property = containerType.GetProperty(propertyName + "Nudger");
            var nudgerOb=property.GetValue(container);
            var nudger = (IValueNudger)nudgerOb;

            return () => nudger;
        }

        public ValueNudgerFactory CreateFactory(INotifyPropertyChanged container, string propertyName)
        {
            var factory = _factoryFactory(container, propertyName);
            return factory;
        }
    }
}