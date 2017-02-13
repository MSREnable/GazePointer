using System;
using System.ComponentModel;

namespace Microsoft.HandsFree.Settings.Nudgers
{
    /// <summary>
    /// Nudger for enumerates.
    /// </summary>
    class EnumValueNudger : ValueNudger
    {
        readonly Array _enumValues;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="description">THe property description.</param>
        public EnumValueNudger(INotifyPropertyChanged container, string propertyName, string description)
            : base(container, propertyName, description)
        {
            _enumValues = Enum.GetValues(Property.PropertyType);
        }

        /// <summary>
        /// Value as a string.
        /// </summary>
        public override string ValueString
        {
            get
            {
                var valueAsString = Value.ToString();
                var valueType = Property.PropertyType;
                var field = valueType.GetField(valueAsString);
                var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                var result = attribute != null ? attribute.Description : valueAsString;
                return result;
            }
        }

        /// <summary>
        /// The next value.
        /// </summary>
        protected override object NudgeUpObjectValue
        {
            get
            {
                var currentIndex = Array.IndexOf(_enumValues, Value);
                var replacementIndex = currentIndex + 1;
                if (_enumValues.Length <= replacementIndex)
                {
                    replacementIndex = 0;
                }
                return _enumValues.GetValue(replacementIndex);
            }
        }

        /// <summary>
        /// The previous value.
        /// </summary>
        protected override object NudgeDownObjectValue
        {
            get
            {
                var currentIndex = Array.IndexOf(_enumValues, Value);
                var replacementIndex = currentIndex - 1;
                if (replacementIndex < 0)
                {
                    replacementIndex = _enumValues.Length - 1; ;
                }
                return _enumValues.GetValue(replacementIndex);
            }
        }
    }
}
