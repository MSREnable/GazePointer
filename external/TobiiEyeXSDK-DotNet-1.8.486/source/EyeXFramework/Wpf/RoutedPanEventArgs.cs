//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

namespace EyeXFramework.Wpf
{
    using System.Windows;

    /// <summary>
    /// Contains state information and event data associated with a routed panning event.
    /// </summary>
    public sealed class RoutedPanEventArgs : RoutedEventArgs
    {
        private readonly double _velocityX;
        private readonly double _velocityY;

        /// <summary>
        /// Gets the horizontal panning velocity.
        /// </summary>
        public double VelocityX
        {
            get { return _velocityX; }
        }

        /// <summary>
        /// Gets the vertical panning velocity.
        /// </summary>
        public double VelocityY
        {
            get { return _velocityY; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutedPanEventArgs"/> class.
        /// </summary>
        /// <param name="routedEvent">The routed event identifier for this instance of the <see cref="RoutedPanEventArgs"/> class.</param>
        /// <param name="source">An alternate source that will be reported when the event is handled. This pre-populates the <see cref="RoutedEventArgs.Source"/> property.</param>
        /// <param name="velocityX">The horizontal panning velocity.</param>
        /// <param name="velocityY">The vertical panning velocity.</param>
        public RoutedPanEventArgs(RoutedEvent routedEvent, object source, double velocityX, double velocityY)
            : base(routedEvent, source)
        {
            _velocityX = velocityX;
            _velocityY = velocityY;
        }
    }
}