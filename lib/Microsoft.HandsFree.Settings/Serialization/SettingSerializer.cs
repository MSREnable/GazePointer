using Microsoft.HandsFree.Settings.Nudgers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Xml.Serialization;

namespace Microsoft.HandsFree.Settings.Serialization
{
    public class SettingSerializer
    {
        static Dictionary<Type, SettingSerializer> _serializerDictionary = new Dictionary<Type, SettingSerializer>();

        readonly Type _type;

        readonly Dictionary<string, SettingDescriptionAttribute> _properties = new Dictionary<string, SettingDescriptionAttribute>();

        readonly HashSet<string> _children = new HashSet<string>();

        public object CreateInstance()
        {
            var constructor = _type.GetConstructor(Type.EmptyTypes);
            var ob = constructor.Invoke(new object[0]);
            return ob;
        }

        /// <summary>
        /// Private constructor.
        /// </summary>
        /// <param name="type">The type the serializer manages.</param>
        SettingSerializer(Type type)
        {
            _type = type;

            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var property in properties)
            {
                var xmlIgnore = property.GetCustomAttribute<XmlIgnoreAttribute>();
                if (xmlIgnore == null)
                {
                    var propertyType = property.PropertyType;
                    if (typeof(INotifyPropertyChanged).IsAssignableFrom(propertyType))
                    {
                        var serializer = GetSerializer(propertyType);

                        _children.Add(property.Name);
                    }
                    else
                    {
                        var settingDescription = property.GetCustomAttribute<SettingDescriptionAttribute>();
                        if (settingDescription == null)
                        {
                            throw new Exception("Missing description");
                        }

                        if (propertyType.IsEnum)
                        {
                        }
                        if (propertyType == typeof(string))
                        {
                        }
                        else if (propertyType == typeof(int))
                        {
                        }
                        else if (propertyType == typeof(double))
                        {
                        }
                        else if (propertyType == typeof(bool))
                        {
                        }
                        else if (propertyType.IsEnum)
                        {
                        }
                        else
                        {
                            throw new NotImplementedException("Property type not implemented");
                        }

                        _properties.Add(property.Name, settingDescription);
                    }
                }
                else
                {
                    Debug.WriteLine($"Ignoring property {property.Name}");
                }
            }
        }

        public static SettingSerializer GetSerializer(Type type)
        {
            SettingSerializer serializer;
            if (!_serializerDictionary.TryGetValue(type, out serializer))
            {
                serializer = new SettingSerializer(type);
                _serializerDictionary.Add(type, serializer);
            }

            return serializer;
        }

        public static SettingSerializer GetSerializer<T>()
            where T : INotifyPropertyChanged, new()
        {
            var type = typeof(T);
            var serializer = GetSerializer(type);
            return serializer;
        }

        // TODO: Scafolding for validating old method against new.
        public IEnumerable<string> Properties => _properties.Keys;

        public IEnumerable<string> Children => _children;

        ValueNudgerFactory CreateFactory(INotifyPropertyChanged container, string propertyName)
        {
            var serializer = _properties[propertyName];
            var factory = serializer.CreateFactory(container, propertyName);
            return factory;
        }

        public IValueNudger CreateNudger(INotifyPropertyChanged container, string propertyName)
        {
            ValueNudgerFactory factory = CreateFactory(container, propertyName);
            var nudger = factory();
            return nudger;
        }

        internal void Serialize(ISettingsSerializationContext context)
        {
            foreach(var property in _properties)
            {
                context.Serialize(property.Key, property.Value.CreateFactory(context.Settings, property.Key));
            }

            foreach(var child in _children)
            {
                context.SerializeChild(child);
            }
        }
    }
}
