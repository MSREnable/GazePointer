using Microsoft.HandsFree.Settings.Nudgers;
using System;
using System.ComponentModel;

namespace Microsoft.HandsFree.Settings.Serialization
{
    class Resetter : SettingsSerializationContext
    {
        Type _type;
        object _defaultSettings;

        internal Resetter(INotifyPropertyChanged settings)
            : base(settings)
        {
            _type = settings.GetType();
            var constructor = _type.GetConstructor(Type.EmptyTypes);
            _defaultSettings = constructor.Invoke(new object[0]);
        }

        public override void Serialize(string name, ValueNudgerFactory factory)
        {
            var property = _type.GetProperty(name);
            var value = property.GetValue(_defaultSettings);
            property.SetValue(Settings, value);
        }

        public override void SerializeChild(string name)
        {
            var property = GetProperty(name);
            var child = (INotifyPropertyChanged)property.GetValue(Settings);
            if (child == null)
            {
                child = CreateChildValue(property);
            }
            var childContext = new Resetter(child);
            child.Serialize(childContext);

            SetPropertyValue(name, child);
        }
    }
}
