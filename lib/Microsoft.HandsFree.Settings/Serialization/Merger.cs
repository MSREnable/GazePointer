using Microsoft.HandsFree.Settings.Nudgers;
using System;
using System.ComponentModel;
using System.Xml;

namespace Microsoft.HandsFree.Settings.Serialization
{
    class Merger : SettingsSerializationContext
    {
        readonly XmlDocument _document;
        readonly XmlElement _element;

        internal Merger(XmlDocument document, XmlElement element, INotifyPropertyChanged settings)
            : base(settings)
        {
            _document = document;
            _element = element;
        }

        public override void Serialize(string name, ValueNudgerFactory factory)
        {
            var property = GetProperty(name);
            var attribute = _element.Attributes[name]?.Value;

            if (attribute == null)
            {
                var elements = _element.GetElementsByTagName(name);
                if (elements.Count != 0)
                {
                    attribute = elements[0].InnerText;

                    while (elements.Count != 0)
                    {
                        _element.RemoveChild(elements[0]);
                    }
                }
            }

            if (attribute != null)
            {
                object value;

                try
                {
                    var propertyType = property.PropertyType;
                    if (propertyType.IsAssignableFrom(typeof(string)))
                    {
                        value = attribute;
                    }
                    else if (propertyType.IsAssignableFrom(typeof(int)))
                    {
                        value = int.Parse(attribute);
                    }
                    else if (propertyType.IsAssignableFrom(typeof(double)))
                    {
                        value = double.Parse(attribute);
                    }
                    else if (propertyType.IsAssignableFrom(typeof(float)))
                    {
                        value = float.Parse(attribute);
                    }
                    else if (propertyType.IsAssignableFrom(typeof(bool)))
                    {
                        value = bool.Parse(attribute);
                    }
                    else if (propertyType.IsEnum)
                    {
                        value = Enum.Parse(propertyType, attribute);
                    }
                    else
                    {
                        value = new NotImplementedException("Property type not implemented");
                    }
                }
                catch
                {
                    // Value didn't parse, stick with default value.
                    value = null;
                }

                if (value != null)
                {
                    SettingsSerializer.SetProperty(property, Settings, value);
                }
            }
        }

        public override void SerializeChild(string name)
        {
            var property = GetProperty(name);
            var child = (INotifyPropertyChanged)property.GetValue(Settings);
            if (child == null)
            {
                child = CreateChildValue(property);
            }

            var childElements = _element.GetElementsByTagName(name);

            switch (childElements.Count)
            {
                case 0:
                    // Nothing to do, accept default values.
                    break;

                case 1:
                    var childElement = (XmlElement)childElements[0];
                    var childContext = new Merger(_document, childElement, child);
                    child.Serialize(childContext);
                    break;

                default:
                    throw new NotImplementedException();
            }

            SetPropertyValue(name, child);
        }
    }
}
