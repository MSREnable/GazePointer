using System.Windows;
using Microsoft.HandsFree.Sensors;

namespace Microsoft.HandsFree.Filters
{
    /// <summary>
    /// See http://bilgin.esme.org/BitsBytes/KalmanFilterforDummies.aspx
    /// </summary>
    public class SimpleKalman : IFilter
    {
        // Adjusted to 0.03 from 0.01 in docs since our measurement noise seems to be high
        // This should start at the approximate value of where _estimationErrorCovarianceP evens out over time
        Point _measurementCovariance;
        Point _estimateCovariance;
        Point _kalmanGain;

        private double _filteredX;
        private double _filteredY;

        public void Initialize()
        {
            _measurementCovariance = new Point(0.03, 0.03);
            _estimateCovariance = new Point(1, 1);
            _kalmanGain = new Point(0, 0);
            _kalmanGain.X = _estimateCovariance.X / (_estimateCovariance.X + _measurementCovariance.X);
            _kalmanGain.Y = _estimateCovariance.Y / (_estimateCovariance.Y + _measurementCovariance.Y);
            _filteredX = 0.0;
            _filteredY = 0.0;
        }

        public void Terminate()
        {

        }

        public GazeEventArgs Update(GazeEventArgs gazeArgs)
        {
            double measuredX = gazeArgs.Scaled.X;
            double measuredY = gazeArgs.Scaled.Y;

            if ((double.IsNaN(measuredX)) || (double.IsNaN(measuredY)))
            {
                return new GazeEventArgs(_filteredX, _filteredY, gazeArgs, true);
            }

            _kalmanGain.X = _estimateCovariance.X / (_estimateCovariance.X + _measurementCovariance.X);
            _kalmanGain.Y = _estimateCovariance.Y / (_estimateCovariance.Y + _measurementCovariance.Y);
            _filteredX = _filteredX + _kalmanGain.X * (measuredX - _filteredX);
            _filteredY = _filteredY + _kalmanGain.Y * (measuredY - _filteredY);
            _estimateCovariance.X = (1 - _kalmanGain.X) * _estimateCovariance.X;
            _estimateCovariance.Y = (1 - _kalmanGain.Y) * _estimateCovariance.Y;
            return new GazeEventArgs(_filteredX, _filteredY, gazeArgs, true);
        }
    }
}
