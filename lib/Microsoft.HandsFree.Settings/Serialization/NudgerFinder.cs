using Microsoft.HandsFree.Settings.Nudgers;
using System.ComponentModel;
using System.Diagnostics;

namespace Microsoft.HandsFree.Settings.Serialization
{
    public class NudgerFinder : SettingsSerializationContext
    {
        readonly string _targetName;

        public NudgerFinder(INotifyPropertyChanged settings, string targetName)
            : base(settings)
        {
            _targetName = targetName;
        }

        public IValueNudger Nudger { get; private set; }

        public override void Serialize(string name, ValueNudgerFactory factory)
        {
            if (factory != null && name == _targetName)
            {
                Debug.Assert(Nudger == null, "Should not have found a previous match");

                Nudger = factory();

                Debug.Assert(Nudger != null, "Should now have a match");
            }
        }

        public override void SerializeChild(string name)
        {
        }
    }
}
