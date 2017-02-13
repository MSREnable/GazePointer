using Microsoft.HandsFree.Sensors;
using System;
using System.Collections.Generic;

namespace Microsoft.HandsFree.Filters
{
    //
    // Heuristic Filtering reliable calibration methods for video-based pupil-tracking systems
    // Dave M. Stampe
    // Behavior Research Methods, 1993, 25(2), 137-142
    //
    // This implementation is a generic version of the Stampe Filter that smooths out the 
    // noise over any given number of samples
    //

    // Creating a class (instead of struct) for the point class
    // so that the values can be modified in place using references

    class CustomPoint
    {
        public double X;
        public double Y;
        
        public CustomPoint(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

    public class StampeFilter : IFilter
    {
        private List<CustomPoint> _history;

        private const int STATS_HISTORY_LENGTH = 30;

        private readonly Settings _settings;

        void InitializeSampleHistory(int length)
        {
            _history = new List<CustomPoint>(length);
            for (int i = 0; i < length; i++)
            {
                _history.Add(new CustomPoint(0, 0));
            }
        }

        public StampeFilter(Settings settings)
        {
            _settings = settings;
        }

        public void Initialize()
        {
            InitializeSampleHistory(_settings.HistoryLength);
        }

        public void Terminate()
        {

        }

        public void SetParameter(string name, double value)
        {
            if (name == "SampleHistoryLength")
            {
                InitializeSampleHistory((int)value);
            }
        }

        public GazeEventArgs Update(GazeEventArgs gazeArgs)
        {
            double measuredX = gazeArgs.Scaled.X;
            double measuredY = gazeArgs.Scaled.Y;

            // the newest sample is inserted at the beginning
            var ptNewest = new CustomPoint(measuredX, measuredY);
            _history.Insert(0, ptNewest);

            for (int i = 1; i < _history.Count - 1; i++)
            {
                //
                // This block ensures that the entire history we are maintaining
                // is either steadily increasing, or steadily decreasing or all equal
                //
                if (_history[1].X == _history[i].X)
                {
                    if (((_history[i].X < _history[i + 1].X) && (_history[i].X < _history[0].X)) ||
                        ((_history[i].X > _history[i + 1].X) && (_history[i].X > _history[0].X)))
                    {
                        double val = Math.Abs(_history[i].X - _history[i + 1].X) < Math.Abs(_history[i].X - _history[0].X) ? _history[i + 1].X : _history[0].X;

                        for (int j = 1; j <= i; j++)
                        {
                            _history[j].X = val;
                        }
                    }
                }

                if (_history[1].Y == _history[i].Y)
                {
                    if (((_history[i].Y < _history[i + 1].Y) && (_history[i].Y < _history[0].Y)) ||
                        ((_history[i].Y > _history[i + 1].Y) && (_history[i].Y > _history[0].Y)))
                    {
                        double val = Math.Abs(_history[i].Y - _history[i + 1].Y) < Math.Abs(_history[i].Y - _history[0].Y) ? _history[i + 1].Y : _history[0].Y;

                        for (int j = 1; j <= i; j++)
                        {
                            _history[j].Y = val;
                        }
                    }
                }
            }

            CustomPoint ptOldest = _history[_history.Count - 1];
            _history.RemoveAt(_history.Count - 1);

            var saccadeThreshold = _settings.SaccadeDistance;
            var fixation = (Math.Abs(ptNewest.X - ptOldest.X) < saccadeThreshold) &&
                           (Math.Abs(ptNewest.Y - ptOldest.Y) < saccadeThreshold)
                ? Fixation.True
                : Fixation.False;

            return new GazeEventArgs(ptOldest.X, ptOldest.Y, gazeArgs.Timestamp, fixation, true);
        }
    }
}
