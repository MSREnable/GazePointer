using Microsoft.HandsFree.Settings.Nudgers;
using System;
using System.ComponentModel;
using System.Xml;

namespace Microsoft.HandsFree.Settings.Serialization
{
    class Rewriter : SettingsSerializationContext
    {
        readonly XmlDocument _document;
        readonly XmlElement _element;

        internal Rewriter(XmlDocument document, XmlElement element, INotifyPropertyChanged settings)
            : base(settings)
        {
            _document = document;
            _element = element;
        }

        public override void Serialize(string name, ValueNudgerFactory factory)
        {
            var value = GetPropertyValue(name);
            var valueString = value.ToString();
            _element.SetAttribute(name, valueString);
        }

        public override void SerializeChild(string name)
        {
            var child = (INotifyPropertyChanged)GetPropertyValue(name);

            var childElements = _element.GetElementsByTagName(name);

            switch (childElements.Count)
            {
                case 0:
                    if (child != null)
                    {
                        var childElement = _document.CreateElement(name);
                        _element.AppendChild(childElement);

                        var childWriter = new Writer(_document, childElement, child);
                        child.Serialize(childWriter);
                    }
                    else
                    {
                        // No child, nothing to do.
                    }
                    break;

                case 1:
                    if (child != null)
                    {
                        var childElement = (XmlElement)childElements[0];

                        var childRewriter = new Rewriter(_document, childElement, child);
                        child.Serialize(childRewriter);
                    }
                    else
                    {
                        // Remove unwanted child.
                        _element.RemoveChild(childElements[0]);
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
