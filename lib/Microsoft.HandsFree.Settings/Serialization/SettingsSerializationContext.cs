using Microsoft.HandsFree.Settings.Nudgers;
using System;
using System.ComponentModel;
using System.Reflection;

namespace Microsoft.HandsFree.Settings.Serialization
{
    /// <summary>
    /// Interface implemented by objects managing serialization.
    /// </summary>
    public abstract class SettingsSerializationContext : ISettingsSerializationContext
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="settings">Settings object</param>
        protected SettingsSerializationContext(INotifyPropertyChanged settings)
        {
            Settings = settings;
            SettingsType = settings.GetType();
        }

        /// <summary>
        /// The object being serialized.
        /// </summary>
        public INotifyPropertyChanged Settings { get; private set; }

        internal Type SettingsType { get; private set; }

        internal PropertyInfo GetProperty(string name)
        {
            return SettingsType.GetProperty(name);
        }

        internal object GetPropertyValue(string name)
        {
            var property = GetProperty(name);
            var value = property.GetValue(Settings);
            return value;
        }

        internal static INotifyPropertyChanged CreateChildValue(PropertyInfo property)
        {
            var constructor = property.PropertyType.GetConstructor(Type.EmptyTypes);
            var child = constructor.Invoke(new object[0]);
            return (INotifyPropertyChanged)child;
        }

        internal void SetPropertyValue(string name, object value)
        {
            var property = GetProperty(name);
            SettingsSerializer.SetProperty(property, Settings, value);
        }

        /// <summary>
        /// Basic CLR property/XML attribute serialization.
        /// </summary>
        /// <param name="name">Name of property and XML attribute.</param>
        /// <param name="factory">Factory for creating value nudger.</param>
        /// 
        public abstract void Serialize(string name, ValueNudgerFactory factory);

        /// <summary>
        /// Serialization of required and expected child object.
        /// </summary>
        /// <param name="name">Name of property containing child reference/XML element</param>
        public abstract void SerializeChild(string name);
    }
}
