using Microsoft.HandsFree.Sensors;
using System;
using System.Windows;

//
// http://www.lifl.fr/~casiez/1euro/
// http://www.lifl.fr/~casiez/publications/CHI2012-casiez.pdf
//

namespace Microsoft.HandsFree.Filters
{
    class LowpassFilter
    {
        Point _previous;
        public Point Previous { get { return _previous; } }

        public LowpassFilter(Point initial)
        {
            _previous = initial;
        }

        public Point Update(Point point, Point alpha)
        {
            _previous.X = (alpha.X * point.X) + ((1 - alpha.X) * _previous.X);
            _previous.Y = (alpha.Y * point.Y) + ((1 - alpha.Y) * _previous.Y);
            return _previous;
        }
    }

    public class OneEuroFilter : IFilter
    {
        long _lastTimestamp;
        // Technically this is configurable. 
        // But this is not exposed to keep the configuration simple
        double _velocityCutoff;
        LowpassFilter _pointFilter;
        LowpassFilter _deltaFilter;

        private readonly Settings _settings;

        public OneEuroFilter(Settings settings)
        {
            _settings = settings;
        }


        public void Initialize()
        {
            _lastTimestamp = 0;
            _velocityCutoff = 1;
        }

        public void Terminate()
        {
        }

        public GazeEventArgs Update(GazeEventArgs gazeArgs)
        {
            if (_lastTimestamp == 0)
            {
                _lastTimestamp = gazeArgs.Timestamp;
                _pointFilter = new LowpassFilter(gazeArgs.Scaled);
                _deltaFilter = new LowpassFilter(new Point());
                return gazeArgs.Clone();
            }

            // Reducing _beta increases lag. Increasing beta decreases lag and improves response time
            // But a really high value of beta also contributes to jitter
            double beta = _settings.Beta;

            // This simply represents the cutoff frequency. A lower value reduces jiiter
            // and higher value increases jitter
            double cf = _settings.Cutoff;
            Point cutoff = new Point(cf, cf);

            // determine sampling frequency based on last time stamp
            double samplingFrequency = 1000.0 / (gazeArgs.Timestamp - _lastTimestamp);
            if (double.IsInfinity(samplingFrequency))
            {
                samplingFrequency = 60;
            }
            _lastTimestamp = gazeArgs.Timestamp;

            // calculate change in distance...
            Vector deltaDistance = gazeArgs.Scaled - _pointFilter.Previous;

            // ...and velocity
            Vector velocity = deltaDistance * samplingFrequency;

            // find the alpha to use for the velocity filter
            double velocityAlpha = Alpha(samplingFrequency, _velocityCutoff);
            Point velocityAlphaPoint = new Point(velocityAlpha, velocityAlpha);

            // find the filtered velocity
            Point filteredVelocity = _deltaFilter.Update((Point)velocity, velocityAlphaPoint);

            // ignore sign since it will be taken care of by deltaDistance
            filteredVelocity.X = Math.Abs(filteredVelocity.X);
            filteredVelocity.Y = Math.Abs(filteredVelocity.Y);

            // compute new cutoff to use based on velocity
            cutoff += (beta * (Vector)filteredVelocity);

            // find the new alpha to use to filter the points
            Point distanceAlpha = new Point(Alpha(samplingFrequency, cutoff.X),
                                            Alpha(samplingFrequency, cutoff.Y));

            // find the filtered point
            Point filteredPoint = _pointFilter.Update(gazeArgs.Scaled, distanceAlpha);

            // compute the new args
            var newArgs = new GazeEventArgs(filteredPoint.X, filteredPoint.Y, gazeArgs.Timestamp, Fixation.Unknown, true);
            return newArgs;
        }

        double Alpha(double rate, double cutoff)
        {
            double te = 1.0 / rate;
            double tau = 1.0 / (2 * Math.PI * cutoff);
            double alpha = te / (te + tau);
            return alpha;
        }
    }
}
