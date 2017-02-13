using System;
using System.Collections.Generic;
using System.Windows;

namespace Microsoft.HandsFree.Filters
{
    public class GazeStats
    {
        int _count;
        Point _sum;
        Point _sumSquared;

        public GazeStats()
        {
            _count = 0;
            _sum = new Point();
            _sumSquared = new Point();
        }

        public void Reset()
        {
            _count = 0;
            _sum.X = 0;
            _sum.Y = 0;
            _sumSquared.X = 0;
            _sumSquared.Y = 0;
        }

        public void Update(double x, double y)
        {
            _sum.X += x;
            _sum.Y += y;
            _sumSquared.X += x * x;
            _sumSquared.Y += y * y;
            _count++;
        }

        public int Count
        {
            get { return _count;  }
        }


        public Point Mean
        {
            get { return new Point(_sum.X / _count, _sum.Y / _count); }
        }

        //
        // StdDev = sqrt(Variance) = sqrt(E[X^2] – (E[X])^2)
        //
        public Point StandardDeviation
        {
            get
            {
                Point mean = this.Mean;
                return new Point(Math.Sqrt((_sumSquared.X / _count) - (mean.X * mean.X)),
                                 Math.Sqrt((_sumSquared.Y / _count) - (mean.Y * mean.Y)));
            }
        }
    }
}
