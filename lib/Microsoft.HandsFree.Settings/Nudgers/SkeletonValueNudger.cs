using System;
using System.ComponentModel;

namespace Microsoft.HandsFree.Settings.Nudgers
{
    class SkeletonValueNudger : ValueNudger
    {
        internal const string PlaceholderDescription = "Not user setting";

        internal SkeletonValueNudger(INotifyPropertyChanged container, string propertyName)
            : base(container, propertyName, PlaceholderDescription)
        {
        }

        protected override object NudgeDownObjectValue
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        protected override object NudgeUpObjectValue
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        protected override bool CanNudgeDown => false;

        protected override bool CanNudgeUp => false;
    }
}
