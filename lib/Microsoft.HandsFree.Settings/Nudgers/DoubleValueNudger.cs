using System;
using System.ComponentModel;

namespace Microsoft.HandsFree.Settings.Nudgers
{
    /// <summary>
    /// Double value nudger.
    /// </summary>
    public class DoubleValueNudger : ValueNudger<double>
    {
        readonly double _minimum;

        readonly double _maximum;

        readonly int _steps;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="propertyName"></param>
        /// <param name="description"></param>
        /// <param name="increment"></param>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        public DoubleValueNudger(INotifyPropertyChanged container, string propertyName, string description, double increment, double minimum, double maximum)
            : base(container, propertyName, description)
        {
            _minimum = minimum;
            _maximum = maximum;

            _steps = (int)Math.Round((_maximum - _minimum) / increment);
        }

        int CurrentStep { get { return (int)Math.Round(_steps * (Value - _minimum) / (_maximum - _minimum)); } }

        double GetSteppedValue(int step)
        {
            return _minimum + step * (_maximum - _minimum) / _steps;
        }

        /// <summary>
        /// Next larger value.
        /// </summary>
        protected override double NudgeUpValue
        {
            get
            {
                var step = CurrentStep;
                if (step < _steps)
                {
                    step++;
                }
                var value = GetSteppedValue(step);
                return value;
            }
        }

        /// <summary>
        /// Next lesser value.
        /// </summary>
        protected override double NudgeDownValue
        {
            get
            {
                var step = CurrentStep;
                if (0 < step)
                {
                    step--;
                }
                var value = GetSteppedValue(step);
                return value;
            }
        }

        /// <summary>
        /// Can we increment?
        /// </summary>
        protected override bool CanNudgeUp
        {
            get
            {
                return CurrentStep < _steps;
            }
        }

        /// <summary>
        /// Can we decrement.
        /// </summary>
        protected override bool CanNudgeDown
        {
            get
            {
                return 0 < CurrentStep;
            }
        }

        public override bool Equals(object obj)
        {
            var isEqual = base.Equals(obj);

            if (isEqual)
            {
                var peer = (DoubleValueNudger)obj;

                isEqual = _minimum == peer._minimum &&
                    _maximum == peer._maximum &&
                    _steps == peer._steps;
            }
            return isEqual;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
