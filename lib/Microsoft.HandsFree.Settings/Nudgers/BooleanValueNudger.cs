using System.ComponentModel;
using System.Windows;

namespace Microsoft.HandsFree.Settings.Nudgers
{
    /// <summary>
    /// Nudger for Booleans.
    /// </summary>
    public class BooleanValueNudger : ValueNudger
    {
        public BooleanValueNudger(INotifyPropertyChanged container, string propertyName, string description)
            : base(container, propertyName, description)
        {
        }

        public override Visibility UpDownInterfaceVisibility
        {
            get
            {
                return Visibility.Collapsed;
            }
        }

        public override Visibility BooleanInterfaceVisibility
        {
            get
            {
                return Visibility.Visible;
            }
        }

        /// <summary>
        /// Up value.
        /// </summary>
        protected override object NudgeDownObjectValue
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Down value.
        /// </summary>
        protected override object NudgeUpObjectValue
        {
            get
            {
                return true;
            }
        }
    }
}
