using System.Collections.Generic;
using Microsoft.HandsFree.Sensors;

namespace Microsoft.HandsFree.Filters
{
    public class AveragingFilter : IFilter
    {
        private Queue<double> _valuesX;
        private Queue<double> _valuesY;
        private double _sumX;
        private double _sumY;

        public int AverageCount = 10;

        public void Initialize()
        {
            _valuesX = new Queue<double>(AverageCount);
            _valuesY = new Queue<double>(AverageCount);
            _sumX = 0.0;
            _sumY = 0.0;
        }

        public void Terminate()
        {

        }

        public GazeEventArgs Update(GazeEventArgs gazeArgs)
        {
            _valuesX.Enqueue(gazeArgs.Scaled.X);
            _valuesY.Enqueue(gazeArgs.Scaled.Y);
            _sumX += gazeArgs.Scaled.X;
            _sumY += gazeArgs.Scaled.Y;
            if (_valuesX.Count > AverageCount)
            {
                _sumX -= _valuesX.Dequeue();
                _sumY -= _valuesY.Dequeue();
            }
            return new GazeEventArgs(_sumX / _valuesX.Count, _sumY / _valuesY.Count, gazeArgs, true);
        }
    }
}
