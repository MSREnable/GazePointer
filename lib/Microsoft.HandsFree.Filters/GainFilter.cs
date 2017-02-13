using Microsoft.HandsFree.Sensors;
using System;

namespace Microsoft.HandsFree.Filters
{
    public class GainFilter : IFilter
    {
        private double _filteredX;
        private double _filteredY;

        private readonly Settings _settings;

        public GainFilter(Settings settings)
        {
            _settings = settings;
        }

        public void Initialize()
        {
            _filteredX = 0;
            _filteredY = 0;
        }

        public void Terminate()
        {

        }

        public GazeEventArgs Update(GazeEventArgs gazeArgs)
        {
            var fixation = Fixation.Unknown;
            double measuredX = gazeArgs.Scaled.X;
            double measuredY = gazeArgs.Scaled.Y;

            if ((double.IsNaN(measuredX)) || (double.IsNaN(measuredY)))
            {
                return new GazeEventArgs(_filteredX, _filteredY, gazeArgs, true);
            }

            double distance = Math.Sqrt(((_filteredX - measuredX) * (_filteredX - measuredX)) + ((_filteredY - measuredY) * (_filteredY - measuredY)));

            if (distance > _settings.SaccadeDistance)
            {
                _filteredX = measuredX;
                _filteredY = measuredY;
                fixation = Fixation.False;
            }
            else
            {
                _filteredX = _filteredX + (_settings.Gain * (measuredX - _filteredX));
                _filteredY = _filteredY + (_settings.Gain * (measuredY - _filteredY));
                fixation = Fixation.True;
            }

            return new GazeEventArgs(_filteredX, _filteredY, gazeArgs.Timestamp, fixation, true);
        }
    }
}
