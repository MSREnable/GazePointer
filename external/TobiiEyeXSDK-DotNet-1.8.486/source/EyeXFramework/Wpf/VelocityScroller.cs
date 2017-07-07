//-----------------------------------------------------------------------
// Copyright 2015 Tobii AB (publ). All rights reserved.
//-----------------------------------------------------------------------

namespace EyeXFramework.Wpf
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Threading;

    public static partial class Behavior
    {
        #region VelocityScroller Property

        private static readonly DependencyProperty VelocityScrollerProperty =
            DependencyProperty.RegisterAttached("VelocityScroller",
            typeof(VelocityScroller), typeof(Behavior),
            new FrameworkPropertyMetadata(null));

        private static void SetVelocityScroller(DependencyObject obj, VelocityScroller handler)
        {
            obj.SetValue(VelocityScrollerProperty, handler);
        }

        private static VelocityScroller GetVelocityScroller(DependencyObject obj)
        {
            return obj.GetValue(VelocityScrollerProperty) as VelocityScroller;
        }

        #endregion

        internal sealed class VelocityScroller : IDisposable
        {
            private readonly ScrollViewer _scrollViewer;
            private readonly DispatcherTimer _timer;
            private readonly Stopwatch _stopwatch;
            private Vector _velocity;
            private float _elapsedMs;

            public Vector Velocity
            {
                get { return _velocity; }
                set { _velocity = value; }
            }

            public VelocityScroller(ScrollViewer viewer)
            {
                _scrollViewer = viewer;

                // Create and start the dispatch timer.
                _timer = new DispatcherTimer();
                _timer.Interval = TimeSpan.FromMilliseconds(10);
                _timer.Tick += OnTick;
                _timer.Start();

                // Start the stop watch.
                _stopwatch = new Stopwatch();
                _stopwatch.Start();
            }

            ~VelocityScroller()
            {
                Dispose(false);
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            private void Dispose(bool disposing)
            {
                _timer.Stop();
                _stopwatch.Stop();
            }

            private void OnTick(object sender, EventArgs eventArgs)
            {
                // Get the elapsed time.
                var elapsedMs = _stopwatch.ElapsedMilliseconds;
                var currentElapsedSeconds = (elapsedMs - _elapsedMs) / 1000;
                _elapsedMs = elapsedMs;

                // Perform update.
                Update(currentElapsedSeconds);
            }

            private void Update(float elapsed)
            {
                if (_velocity.Length < 1f)
                {
                    return;
                }

                if (_scrollViewer.CanContentScroll)
                {
                    // Make sure that the scroll viewer isn't set to use integral height. 
                    // This will make the smooth scrolling impossible.
                    _scrollViewer.CanContentScroll = false;   
                }                

                // Calculate the velocity for this tick.
                var tickVelocity = _velocity * elapsed;

                // Update scroll offset.
                _scrollViewer.ScrollToHorizontalOffset(_scrollViewer.HorizontalOffset + -tickVelocity.X);
                _scrollViewer.ScrollToVerticalOffset(_scrollViewer.VerticalOffset + -tickVelocity.Y);
            }
        }
    }
}
