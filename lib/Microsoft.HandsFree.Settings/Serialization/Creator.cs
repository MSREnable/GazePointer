using Microsoft.HandsFree.Settings.Nudgers;
using System.ComponentModel;

namespace Microsoft.HandsFree.Settings.Serialization
{
    class Creator : SettingsSerializationContext
    {
        internal Creator(INotifyPropertyChanged settings)
            : base(settings)
        {
        }

        public override void Serialize(string name, ValueNudgerFactory factory)
        {
        }

        public override void SerializeChild(string name)
        {
            var property = GetProperty(name);
            var child = CreateChildValue(property);

            var childContext = new Creator(child);
            child.Serialize(childContext);

            SetPropertyValue(name, child);
        }
    }
}
