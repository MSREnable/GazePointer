//-----------------------------------------------------------------------
// Copyright 2015 Tobii AB (publ). All rights reserved.
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace EyeXFramework
{
    using System;
    using System.Runtime.CompilerServices;
    using Tobii.EyeX.Client;
    using Tobii.EyeX.Framework;

    /// <summary>
    /// Maps the pannable behavior to an interactor.
    /// Exposes EyeX behavior parameters and events as .NET properties and events.
    /// </summary>
    public class PannableBehavior : IEyeXBehavior
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PannableBehavior"/> class.
        /// </summary>
        public PannableBehavior()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PannableBehavior"/> class.
        /// </summary>
        /// <param name="panningHandler">Event handler for the <see cref="Panning"/> event.</param>
        public PannableBehavior(EventHandler<PannablePanEventArgs> panningHandler)
        {
            if (panningHandler != null)
            {
                Panning += panningHandler;
            }

            PanDirectionsAvailable = PanDirection.All;
            Profile = PanningProfile.Vertical;
        }

        /// <summary>
        /// The panning event.
        /// </summary>
        public event EventHandler<PannablePanEventArgs> Panning;

        /// <summary>
        /// Gets or sets the available panning directions.
        /// </summary>
        public PanDirection PanDirectionsAvailable { get; set; }

        /// <summary>
        /// Gets or sets the panning profile.
        /// </summary>
        public PanningProfile Profile { get; set; }

        /// <summary>
        /// Gets the type of behavior provided by the adapter.
        /// </summary>
        public BehaviorType BehaviorType
        {
            get { return BehaviorType.Pannable; }
        }

        /// <summary>
        /// Assigns the panning behavior to an interactor.
        /// </summary>
        /// <param name="interactor">The interactor.</param>
        public void AssignBehavior(Interactor interactor)
        {
            var parameters = new PannableParams
            {
                IsHandsFreeEnabled = EyeXBoolean.False,
                PanDirectionsAvailable = PanDirectionsAvailable,
                Profile = Profile
            };
            interactor.CreatePannableBehavior(ref parameters);
        }

        public void HandleEvent(object sender, IEnumerable<Behavior> behaviors)
        {
            foreach (var behavior in behaviors
                .Where(b => b.BehaviorType == BehaviorType.Pannable))
            {
                PannableEventType eventType;
                if (behavior.TryGetPannableEventType(out eventType))
                {
                    switch (eventType)
                    {
                        case PannableEventType.Pan:
                            HandlePanningEvent(sender, behavior);
                            break;
                    }
                }
            }
        }

        private void HandlePanningEvent(object sender, Behavior behavior)
        {
            PannablePanEventParams param;
            if (behavior.TryGetPannablePanEventParams(out param))
            {
                var handler = Panning;
                if (handler != null)
                {
                    handler(sender, new PannablePanEventArgs(param.PanVelocityX, param.PanVelocityY));
                }
            }
        }
    }

    /// <summary>
    /// Event arguments for the <see cref="PannableBehavior.Panning"/> event.
    /// </summary>
    public sealed class PannablePanEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PannablePanEventArgs"/> class.
        /// </summary>
        /// <param name="velocityX">Horizontal panning velocity</param>
        /// <param name="velocityY">Vertical panning velocity</param>
        public PannablePanEventArgs(double velocityX, double velocityY)
        {
            PanVelocityX = velocityX;
            PanVelocityY = velocityY;
        }

        /// <summary>
        /// Gets the horizontal panning velocity (pixels/second).
        /// </summary>
        public double PanVelocityX { get; private set; }

        /// <summary>
        /// Gets the vertical panning velocity (pixels/second).
        /// </summary>
        public double PanVelocityY { get; private set; }
    }
}
