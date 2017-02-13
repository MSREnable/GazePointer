using Microsoft.HandsFree.MVVM;
using Microsoft.HandsFree.Settings.Serialization;

namespace Microsoft.HandsFree.Settings
{
    /// <summary>
    /// Mouse emulation settings
    /// </summary>
    public class MouseEmulationSettings : NotifyingObject
    {
        bool _directInteraction;
        [SettingDescription("Direct Interaction")]
        public bool DirectInteraction
        {
            get { return _directInteraction; }
            set { SetProperty(ref _directInteraction, value); }
        }
    }
}
