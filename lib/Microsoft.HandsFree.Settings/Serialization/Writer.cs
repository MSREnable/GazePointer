using Microsoft.HandsFree.Settings.Nudgers;
using System.ComponentModel;
using System.Xml;

namespace Microsoft.HandsFree.Settings.Serialization
{
    class Writer : SettingsSerializationContext
    {
        readonly XmlDocument _document;
        readonly XmlElement _element;

        internal Writer(XmlDocument document, XmlElement element, INotifyPropertyChanged settings)
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

            if (child != null)
            {
                var childElement = _document.CreateElement(name);
                _element.AppendChild(childElement);

                var childWriter = new Writer(_document, childElement, child);
                child.Serialize(childWriter);
            }
        }
    }
}
