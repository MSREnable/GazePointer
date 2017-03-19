using System;
using System.Windows.Threading;

namespace Microsoft.HandsFree.GazePointer
{
    class IdleDetector
    {
        readonly DispatcherTimer timer = new DispatcherTimer();

        internal event EventHandler GoneIdle;

        internal IdleDetector(TimeSpan inactivityLimit)
        {
            timer.Interval = inactivityLimit;

            timer.Tick += OnGoneIdle;
        }

        internal void Tick()
        {
            timer.Stop();
            timer.Start();
        }

        void OnGoneIdle(object sender, EventArgs e)
        {
            timer.Stop();

            GoneIdle?.Invoke(this, EventArgs.Empty);
        }
    }
}
