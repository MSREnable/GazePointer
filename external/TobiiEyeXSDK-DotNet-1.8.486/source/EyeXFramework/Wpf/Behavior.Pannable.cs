//-----------------------------------------------------------------------
// Copyright 2015 Tobii AB (publ). All rights reserved.
//-----------------------------------------------------------------------

namespace EyeXFramework.Wpf
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;
    using Tobii.EyeX.Framework;

    /// <summary>
    /// Partial class with events and properties related to the Pannable behavior.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder", Justification = "More important to keep regions together.")]
    public static partial class Behavior
    {
        #region Pannable Property

        /// <summary>
        /// When set to a value other than None, the associated framework element becomes 
        /// an EyeX interactor with the Pannable behavior. 
        /// <para>
        /// -The routed event Pannable will be fired when the user interacts 
        /// with the scrollViewer by looking at it and triggering panning. The
        /// event will contain information about the horizontal and vertical panning velocity
        /// (in pixels/second).
        /// </para>
        /// </summary>
        public static readonly DependencyProperty PannableProperty = DependencyProperty.RegisterAttached(
           "Pannable", typeof(PannableType), typeof(Behavior), new FrameworkPropertyMetadata(PannableType.None, OnPannablePropertyChanged));

        /// <summary>
        /// Sets the <see cref="PannableType"/> for the given <see cref="FrameworkElement"/>.
        /// </summary>
        /// <param name="element">The <see cref="FrameworkElement"/> to set the property for.</param>
        /// <param name="value">The <see cref="PannableType"/> value of the property.</param>
        public static void SetPannable(this FrameworkElement element, PannableType value)
        {
            element.SetValue(PannableProperty, value);
        }

        /// <summary>
        /// Gets the <see cref="PannableType"/> for the given <see cref="FrameworkElement"/>.
        /// </summary>
        /// <param name="element">The <see cref="FrameworkElement"/> to get the property value from.</param>
        /// <returns>The <see cref="PannableType"/> for the given <see cref="FrameworkElement"/>.</returns>
        public static PannableType GetPannable(this FrameworkElement element)
        {
            return (PannableType)element.GetValue(PannableProperty);
        }

        #endregion

        #region PanDirection Property

        /// <summary>
        /// Indicates what directions that are currently pannable.
        /// </summary>
        public static readonly DependencyProperty AvailablePanDirectionsProperty = DependencyProperty.RegisterAttached(
            "AvailablePanDirections", typeof(PanDirectionType), typeof(Behavior), new FrameworkPropertyMetadata(PanDirectionType.None, OnPanDirectionPropertyChanged));

        /// <summary>
        /// Sets the currently available directions.
        /// <para>
        /// - Should be used to dynamically update the available pan directions when panning is 
        /// ongoing. 
        /// </para><para>
        /// - For example: if the contents of a vertically scrollable area has been scrolled to 
        /// the bottom, the down direction should temporarily be removed from the available 
        /// directions until the contents has been scrolled up again.
        /// </para>
        /// </summary>
        /// <param name="element">The <see cref="FrameworkElement"/> to set the property for.</param>
        /// <param name="direction">The <see cref="PanDirectionType"/> value of the property.</param>
        public static void SetAvailablePanDirections(this FrameworkElement element, PanDirectionType direction)
        {
            element.SetValue(AvailablePanDirectionsProperty, direction);
        }

        /// <summary>
        /// Gets the currently available directions.
        /// </summary>
        /// <param name="element">The <see cref="FrameworkElement"/> to get the property value from.</param>
        /// <returns>The <see cref="PanDirectionType"/> for the given <see cref="FrameworkElement"/>.</returns>
        public static PanDirectionType GetAvailablePanDirections(this FrameworkElement element)
        {
            return (PanDirectionType)element.GetValue(AvailablePanDirectionsProperty);
        }

        private static void OnPanDirectionPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var element = dependencyObject as FrameworkElement;
            if (null == element)
            {
                return;
            }

            // Get or create the behavior.
            var behavior = GetOrCreatePannableBehavior(element);

            // Update the pan profile.
            var direction = (PanDirectionType)e.NewValue;
            behavior.PanDirectionsAvailable = (PanDirection)direction;
        }

        #endregion

        #region PanningProfile Property

        /// <summary>
        /// The panning profile indicates how panning behaves when activated.
        /// </summary>
        public static readonly DependencyProperty PanningProfileProperty = DependencyProperty.RegisterAttached(
            "PanningProfile", typeof(PanningProfileType), typeof(Behavior), new FrameworkPropertyMetadata(PanningProfileType.Radial, OnPanningProfilePropertyChanged));

        /// <summary>
        /// Sets the current panning profile.
        /// </summary>
        /// <param name="element">The <see cref="FrameworkElement"/> to set the property for.</param>
        /// <param name="profile">The panning profile to set as the current one.</param>
        public static void SetPanningProfile(this FrameworkElement element, PanningProfileType profile)
        {
            element.SetValue(PanningProfileProperty, profile);
        }

        /// <summary>
        /// Gets the current panning profile.
        /// </summary>
        /// <param name="element">The <see cref="FrameworkElement"/> to get the property value from.</param>
        /// <returns>The current panning profile.</returns>
        public static PanningProfileType GetPanningProfile(this FrameworkElement element)
        {
            return (PanningProfileType)element.GetValue(PanningProfileProperty);
        }

        private static void OnPanningProfilePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var element = dependencyObject as FrameworkElement;
            if (null == element)
            {
                return;
            }

            // Get or create the behavior.
            var behavior = GetOrCreatePannableBehavior(element);

            // Update the pan profile.
            var profile = (PanningProfileType)e.NewValue;
            behavior.Profile = (PanningProfile)profile;
        }

        #endregion

        #region AutoPan Property

        /// <summary>
        /// When set to true, the associated framework element will auto pan.
        /// </summary>
        public static readonly DependencyProperty AutoPanProperty = DependencyProperty.RegisterAttached(
            "AutoPan", typeof(bool), typeof(Behavior), new FrameworkPropertyMetadata(false, OnAutoPanPropertyChanged));

        /// <summary>
        /// Sets whether or not the associated <see cref="FrameworkElement"/> is auto panning.
        /// </summary>
        /// <param name="element">The <see cref="FrameworkElement"/> to set the property for.</param>
        /// <param name="enabled">Whether or not automatic panning is enabled.</param>
        public static void SetAutoPan(this FrameworkElement element, bool enabled)
        {
            element.SetValue(AutoPanProperty, enabled);
        }

        /// <summary>
        /// Gets whether or not automatic panning is enabled..
        /// </summary>
        /// <param name="element">The <see cref="FrameworkElement"/> to get the property from.</param>
        /// <returns><c>true</c> it automatic panning is enabled; otherwise <c>false</c>.</returns>
        public static bool GetAutoPan(this FrameworkElement element)
        {
            return (bool)element.GetValue(AutoPanProperty);
        }

        private static void OnAutoPanPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var element = dependencyObject as FrameworkElement;
            if (null == element)
            {
                return;
            }

            // Make sure that the interactor and behavior 
            // exist for the element.
            GetOrCreatePannableBehavior(element);

            // Subscribe to the event.            
            AddPanningHandler(element, PerformAutoPan);
        }

        #endregion

        #region Panning Event

        /// <summary>
        /// Event that notifies that the associated element is being panned by the user.
        /// That is, the element the user is looking at while triggering panning.
        /// </summary>
        public static readonly RoutedEvent PanningEvent = EventManager.RegisterRoutedEvent(
            "Panning",
            RoutingStrategy.Bubble,
            typeof(EventHandler<RoutedPanEventArgs>),
            typeof(Behavior));

        /// <summary>
        /// Adds a handler for the <c>Panning</c> event to a given <see cref="DependencyObject"/>.
        /// </summary>
        /// <param name="dependencyObject">The <see cref="DependencyObject"/> where to add the event handler.</param>
        /// <param name="handler">The <see cref="RoutedEventHandler"/> to add.</param>
        public static void AddPanningHandler(this DependencyObject dependencyObject, EventHandler<RoutedPanEventArgs> handler)
        {
            var uie = dependencyObject as UIElement;
            if (uie == null)
            {
                return;
            }

            uie.RemoveHandler(PanningEvent, handler);
            uie.AddHandler(PanningEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the panning event from a given <see cref="DependencyObject"/>.
        /// </summary>
        /// <param name="dependencyObject">The <see cref="DependencyObject"/> from where to remove the event handler.</param>
        /// <param name="handler">The <see cref="RoutedEventHandler"/> to remove.</param>
        public static void RemovePanningHandler(this DependencyObject dependencyObject, EventHandler<RoutedPanEventArgs> handler)
        {
            var uie = dependencyObject as UIElement;
            if (uie == null)
            {
                return;
            }

            uie.RemoveHandler(PanningEvent, handler);
        }

        private static void OnPannablePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var element = dependencyObject as FrameworkElement;
            if (null == element)
            {
                return;
            }

            // Get or create the behavior.
            GetOrCreatePannableBehavior(element);
        }

        private static void OnPanning(object sender, PannablePanEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (null == element)
            {
                return;
            }

            // Raise the panning event.
            element.RaiseEvent(new RoutedPanEventArgs(PanningEvent, element, e.PanVelocityX, e.PanVelocityY));
        }

        #endregion

        private static PannableBehavior GetOrCreatePannableBehavior(FrameworkElement element)
        {
            // Try to get the behavior if it already exist.
            var interactor = element.GetWpfInteractor();
            if (interactor != null)
            {
                var existingBehavior = interactor.GetBehavior(BehaviorType.Pannable) as PannableBehavior;
                if (existingBehavior != null)
                {
                    return existingBehavior;
                }
            }

            // Create default pannable behavior.
            // The settings here are set via the corresponding dependency properties.
            var behavior = new PannableBehavior()
            {
                PanDirectionsAvailable = PanDirection.All,
                Profile = PanningProfile.Radial
            };

            // Subscribe to the panning event.
            behavior.Panning += OnPanning;

            // Add the behavior.
            AddBehavior(element, behavior);

            return behavior;
        }

        private static void PerformAutoPan(object sender, RoutedPanEventArgs e)
        {
            var element = sender as FrameworkElement;
            var scrollViewer = GetScrollViewer(element);
            if (scrollViewer == null)
            {
                throw new InvalidOperationException("Could not resolve scroll viewer from registered framework element.");
            }

            var handler = GetVelocityScroller(element);
            if (handler == null)
            {
                handler = new VelocityScroller(scrollViewer);
                SetVelocityScroller(element, handler);
            }

            // Update velocity.
            handler.Velocity = new Vector(e.VelocityX, e.VelocityY);
        }

        private static ScrollViewer GetScrollViewer(FrameworkElement element)
        {
            return element as ScrollViewer ?? WpfCrawler.GetFirstChildOfType<ScrollViewer>(element);
        }

        /// <summary>
        /// Represents the type of panning to use.
        /// </summary>
        public enum PannableType
        {
            /// <summary>
            /// No panning.
            /// </summary>
            None,

            /// <summary>
            /// The user pans by looking at a scroll viewer 
            /// and triggering panning.
            /// </summary>
            Default
        }

        /// <summary>
        /// Represents the available panning directions.
        /// </summary>
        [Flags]
        public enum PanDirectionType
        {
            /// <summary>
            /// No available panning directions.
            /// </summary>
            None = 0,

            /// <summary>
            /// Only panning to the left is available.
            /// </summary>
            Left = 1,

            /// <summary>
            /// Only panning to the right is available.
            /// </summary>
            Right = 2,

            /// <summary>
            /// Only upwards panning is available.
            /// </summary>
            Up = 4,

            /// <summary>
            /// Only downwards panning is available.
            /// </summary>
            Down = 8,

            /// <summary>
            /// All panning directions are available.
            /// </summary>
            All = Left | Right | Up | Down,
        }

        /// <summary>
        /// Represents a panning profile.
        /// A panning profile dictates how panning will behave. 
        /// </summary>
        public enum PanningProfileType
        {
            /// <summary>
            /// No panning.
            /// </summary>
            None = 1,

            /// <summary>
            /// Currently same as the vertical profile,
            /// but will be properly implemented in a future release.
            /// </summary>
            Reading = 2,

            /// <summary>
            /// Horizontal panning.
            /// </summary>
            Horizontal = 3,

            /// <summary>
            /// Vertical panning.
            /// </summary>
            Vertical = 4,

            /// <summary>
            /// Vertical and horizontal panning where the 
            /// vertical zone is bigger than the horizontal.
            /// </summary>
            VerticalFirstThenHorizontal = 5,

            /// <summary>
            /// Radial panning.
            /// </summary>
            Radial = 6,

            /// <summary>
            /// Vertical and horizontal panning where the 
            /// horizontal zone is bigger than the vertical.
            /// </summary>
            HorizontalFirstThenVertical = 7,
        }
    }
}