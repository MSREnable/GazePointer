using Microsoft.HandsFree.Settings.Nudgers;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Microsoft.HandsFree.Settings.Serialization
{
    /// <summary>
    /// Class implementing basic serialization of settings objects.
    /// </summary>
    public static class SettingsSerializer
    {
        public static void Serialize(this INotifyPropertyChanged content, ISettingsSerializationContext context)
        {
            var serializer = SettingSerializer.GetSerializer(content.GetType());
            serializer.Serialize(context);
        }

        /// <summary>
        /// Create a default settings object.
        /// </summary>
        /// <typeparam name="T">The type of the settings object.</typeparam>
        /// <returns>An settings object with all default values.</returns>
        public static T CreateDefault<T>()
            where T : INotifyPropertyChanged, new()
        {
            var root = new T();
            var context = new Creator(root);
            root.Serialize(context);
            return root;
        }

        /// <summary>
        /// Reset settings object to default values.
        /// </summary>
        /// <typeparam name="T">The type of the settings object.</typeparam>
        /// <param name="settings">The settings object to reset.</param>
        public static void ResetDefault<T>(T settings)
            where T : INotifyPropertyChanged, new()
        {
            var context = new Resetter(settings);
            settings.Serialize(context);
        }

        internal static XmlDocument ToXmlDocument<T>(T settings, string name)
            where T : INotifyPropertyChanged, new()
        {
            var xmlDocument = new XmlDocument();
            var xmlElement = xmlDocument.CreateElement(name);
            xmlDocument.AppendChild(xmlElement);

            var context = new Writer(xmlDocument, xmlElement, settings);
            settings.Serialize(context);

            return xmlDocument;
        }

        /// <summary>
        /// Convert object to an XML settings string.
        /// </summary>
        /// <typeparam name="T">Type of settings object.</typeparam>
        /// <param name="settings">The settings to convert to XML.</param>
        /// <param name="name">The name of the root element.</param>
        /// <returns>The XML representing the settings.</returns>
        public static string ToXmlString<T>(T settings, string name)
            where T : INotifyPropertyChanged, new()
        {
            var xmlDocument = ToXmlDocument(settings, name);

            var builder = new StringBuilder();
            var writerSettings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                Indent = true
            };
            using (var writer = XmlWriter.Create(builder, writerSettings))
            {
                xmlDocument.WriteTo(writer);
            }
            var xml = builder.ToString();

            return xml;
        }

        internal static T FromXmlDocument<T>(XmlDocument xmlDocument)
            where T : INotifyPropertyChanged, new()
        {
            var ob = new T();

            var element = xmlDocument.DocumentElement;

            var context = new Merger(xmlDocument, element, ob);
            ob.Serialize(context);

            return ob;
        }

        internal static void MergeXmlDocument<T>(XmlDocument xmlDocument, T settings)
            where T : INotifyPropertyChanged, new()
        {
            var element = xmlDocument.DocumentElement;

            var context = new Merger(xmlDocument, element, settings);
            settings.Serialize(context);
        }

        /// <summary>
        /// Convert settings XML into a settings object.
        /// </summary>
        /// <typeparam name="T">Type of settings object.</typeparam>
        /// <param name="xmlString">The XML string to be converted.</param>
        /// <returns>The settings object.</returns>
        public static T FromXmlString<T>(string xmlString)
            where T : INotifyPropertyChanged, new()
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlString);

            var ob = FromXmlDocument<T>(xmlDocument);

            return ob;
        }

        /// <summary>
        /// Find nudger for property.
        /// </summary>
        /// <param name="settings">The settings object.</param>
        /// <param name="name">The property name.</param>
        /// <returns>Nudger for property or null if cannot be found.</returns>
        public static IValueNudger FindNudger(INotifyPropertyChanged settings, string name)
        {
            var finder = new NudgerFinder(settings, name);
            settings.Serialize(finder);
            return finder.Nudger;
        }

        internal static void SetProperty(PropertyInfo property, INotifyPropertyChanged ob, object value)
        {
            var currentValue = property.GetValue(ob);

            if (!object.Equals(currentValue, value))
            {
                var isSet = false;
                PropertyChangedEventHandler handler = (s, e) => { if (e.PropertyName == null || e.PropertyName == property.Name) { isSet = true; } };

                ob.PropertyChanged += handler;
                property.SetValue(ob, value);
                ob.PropertyChanged -= handler;

                if (!isSet)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}
