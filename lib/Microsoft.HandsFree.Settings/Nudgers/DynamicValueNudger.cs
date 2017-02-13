using System;
using System.ComponentModel;

namespace Microsoft.HandsFree.Settings.Nudgers
{
    /// <summary>
    /// Nudger for environmentally defined values.
    /// </summary>
    public class DynamicValueNudger : ValueNudger<string>, IDynamicValueNudger
    {
        readonly string _unknownValue;

        /// <summary>
        /// Constuctor.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="propertyName"></param>
        /// <param name="description"></param>
        /// <param name="unknownValue"></param>
        public DynamicValueNudger(INotifyPropertyChanged container, string propertyName, string description, string unknownValue)
            : base(container, propertyName, description)
        {
            _unknownValue = unknownValue;

            Values = new DynamicValueSetting[0];
        }

        int Index
        {
            get
            {
                var value = Value;
                return Array.FindIndex(Values, (s) => s.Key == value);
            }
        }

        /// <summary>
        /// The display value.
        /// </summary>
        public override string ValueString
        {
            get
            {
                var index = Index;
                return 0 <= index ? Values[index].ValueString : string.Format(_unknownValue, Value);
            }
        }

        /// <summary>
        /// Is there a next greater value.
        /// </summary>
        protected override bool CanNudgeUp => Values.Length != 0;

        /// <summary>
        /// The next lesser value.
        /// </summary>
        protected override string NudgeUpValue
        {
            get
            {
                var index = Index + 1;
                if (Values.Length <= index)
                {
                    index = 0;
                }
                return Values[index].Key;
            }
        }

        /// <summary>
        /// Is there a next lesser value.
        /// </summary>
        protected override bool CanNudgeDown => Values.Length != 0;

        /// <summary>
        /// The next lesser value.
        /// </summary>
        protected override string NudgeDownValue
        {
            get
            {
                var index = Index - 1;
                if (index < 0)
                {
                    index = Values.Length - 1;
                }
                return Values[index].Key;
            }
        }

        /// <summary>
        /// The available values.
        /// </summary>
        public DynamicValueSetting[] Values
        {
            get;
            set;
        }

        public override bool Equals(object obj)
        {
            var isEqual = base.Equals(obj);

            if (isEqual)
            {
                var peer = (DynamicValueNudger)obj;

                isEqual = _unknownValue == peer._unknownValue;
            }
            return isEqual;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
