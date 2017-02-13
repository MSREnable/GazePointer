using System.ComponentModel;

namespace Microsoft.HandsFree.Settings.Nudgers
{
    /// <summary>
    /// Value nudger for integers.
    /// </summary>
    class IntValueNudger : ValueNudger<int>
    {
        readonly int _increment;

        readonly int _minimum;

        readonly int _maximum;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="propertyName"></param>
        /// <param name="description"></param>
        /// <param name="increment"></param>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        internal IntValueNudger(INotifyPropertyChanged container, string propertyName, string description, int increment, int minimum, int maximum)
            : base(container, propertyName, description)
        {
            _increment = increment;
            _minimum = minimum;
            _maximum = maximum;
        }

        /// <summary>
        /// Next larger value.
        /// </summary>
        protected override int NudgeUpValue
        {
            get
            {
                var value = Value;
                value += _increment;
                if (_maximum <= value)
                {
                    value = _maximum;
                }
                return value;
            }
        }

        /// <summary>
        /// Next lower value.
        /// </summary>
        protected override int NudgeDownValue
        {
            get
            {
                var value = Value;
                value -= _increment;
                if (value <= _minimum)
                {
                    value = _minimum;
                }
                return value;
            }
        }

        /// <summary>
        /// Can nudge upwards.
        /// </summary>
        protected override bool CanNudgeUp
        {
            get
            {
                return Value < _maximum;
            }
        }

        /// <summary>
        /// Can nudge downwards.
        /// </summary>
        protected override bool CanNudgeDown
        {
            get
            {
                return _minimum < Value;
            }
        }

        public override bool Equals(object obj)
        {
            var isEqual = base.Equals(obj);

            if (isEqual)
            {
                var peer = (IntValueNudger)obj;

                isEqual = _minimum == peer._minimum &&
                    _maximum == peer._maximum &&
                    _increment == peer._increment;
            }
            return isEqual;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
