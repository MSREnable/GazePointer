using Microsoft.HandsFree.Filters;
using Microsoft.HandsFree.Sensors;
using Microsoft.HandsFree.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using FormsScreen = System.Windows.Forms.Screen;

namespace Microsoft.HandsFree.Mouse
{
    class GazeHistoryItem
    {
        public FrameworkElement HitTarget;
        public long Timestamp;
        public int Duration;
    }

    public class GazeMouse
    {
        public const uint DefaultMouseDownDelay = 250;

        public delegate GazeClickParameters GetGazeClickParameters(FrameworkElement element);
        private delegate void GazeMouseDelegate();

        public event EventHandler EyesOff;
        public event EventHandler EyesOn;

        public GazeStats OriginalSignal { get; private set; }
        public GazeStats FilteredSignal { get; private set; }

        const long InvokeEffectiveTickCounts = 250;
        static long _lastInvokeTickCount = Environment.TickCount - InvokeEffectiveTickCounts;

        /// <summary>
        /// Are we currently invoking an action from a hands free device.
        /// </summary>
        public static bool IsHandsFreeInvoked
        {
            get { return (Environment.TickCount - _lastInvokeTickCount) < InvokeEffectiveTickCounts; }
            set { if (value) { _lastInvokeTickCount = Environment.TickCount; } }
        }

        private const int MinLastMouseTime = 1000;

        private static readonly TraceSource _trace = new TraceSource("GazeMouse", SourceLevels.Information);
        private static IGazeDataProvider _gazeDataProvider;
        private static readonly LogFilter _logFilter;

        private static GazeCursorElement _gazeCursor;
        private static Window _cursorWindow;
        private static Cursor _mouseCursor;

        private static readonly MouseInputListener _mouseListener;

        // _offScreenElement is a pseudo-element that represents the area outside
        // the screen so we can track how long the user has been looking outside
        // the screen and appropriately trigger the EyesOff event
        private static FrameworkElement _offScreenElement;
        private static GazeClickParameters _offScreenElementClickParams;
        private Matrix _transform;
        private Matrix _reverseTransform;

        private Point _topLeft;
        private Point _bottomRight;
        private bool _wasTracking;

        private IFilter _xyFilter;
        private GazeClickParameters _defaultClickParams;
        private GetGazeClickParameters _getGazeClickParams;

        private FrameworkElement _hitTarget;
        private DependencyObject _nextVisualHit = null;
        private GazeMouseState _gazeMouseState;

        private List<GazeHistoryItem> _gazeHistory;
        private Dictionary<FrameworkElement, int> _hitTargetTimes;

        long _maxHistoryTime = 1000; // in milliseconds
        readonly IdleDetector _idleDetector = new IdleDetector(TimeSpan.FromSeconds(0.25));

        public static readonly DependencyProperty GazeElementProperty = DependencyProperty.RegisterAttached("GazeElement", typeof(FrameworkElement), typeof(GazeMouse));

        public static void SetGazeElement(DependencyObject d, FrameworkElement value)
        {
            d.SetValue(GazeElementProperty, value);
        }

        public static FrameworkElement GetGazeElement(DependencyObject d)
        {
            return (FrameworkElement)d.GetValue(GazeElementProperty);
        }

        /// <summary>
        /// Attach GazeMouse behavior to this window
        /// </summary>
        public static GazeMouse Attach(Window window, GazeClickParameters clickParams = null, GetGazeClickParameters getGazeClickParams = null,
            Settings settings = null, bool forceMouseCursor = false)
        {
            if (settings == null)
            {
                settings = new Settings();
                settings.Sensor.Sensor = GazeDataProvider.Detect();
            }

            if (_gazeDataProvider == null)
            {
                _gazeDataProvider = GazeDataProvider.InitializeGazeDataProvider(settings.Sensor);
            }

            return new GazeMouse(
                window,
                clickParams ?? new GazeClickParameters
                {
                    MouseDownDelay = 1 * DefaultMouseDownDelay,
                    MouseUpDelay = 2 * DefaultMouseDownDelay,
                    RepeatMouseDownDelay = 3 * DefaultMouseDownDelay
                },
                getGazeClickParams,
                settings,
                forceMouseCursor
                );
        }

        public static void DetachAll()
        {
            _cursorWindow.Close();
            _mouseListener.Terminate();
            _logFilter.Terminate();
            _gazeDataProvider.Terminate();
        }

        static GazeMouse()
        {
            _mouseListener = new MouseInputListener();

            _logFilter = new LogFilter();
            _logFilter.Initialize();

            CreateCursorWindow();
            _offScreenElement = new FrameworkElement();
            _offScreenElementClickParams = new GazeClickParameters
            {
                MouseDownDelay = 1 * DefaultMouseDownDelay,
                MouseUpDelay = 2 * DefaultMouseDownDelay,
                RepeatMouseDownDelay = uint.MaxValue
            };
        }

        private readonly Settings _settings;
        private readonly Sensors.Settings _sensorSettings;
        private readonly Filters.Settings _filterSettings;
        private readonly LoggingSettings _loggingSettings;
        private readonly bool _forceMouseCursor;

        private readonly Window _window;
        private Window _mainWindow;
        private IntPtr _hwndMain;
        private bool _windowClosed;

        private GazeMouse(Window window, GazeClickParameters clickParameters, GetGazeClickParameters getGazeClickParameters,
            Settings settings, bool forceMouseCursor)
        {
            // TODO: Addition of loggingSettings with default as parameter WILL have introduced logging configuration bug.

            _settings = settings;
            _sensorSettings = settings.Sensor;
            _filterSettings = settings.Filter;
            _loggingSettings = settings.Logging;
            _forceMouseCursor = forceMouseCursor;

            if (_sensorSettings.Sensor == Sensors.Sensors.Mouse)
            {
                _filterSettings.ActiveFilter = FilterType.NullFilter;
            }

            _settings.PropertyChanged += (o, args) => _window.Dispatcher.BeginInvoke(new GazeMouseDelegate(UpdateCursorStyle));
            _filterSettings.PropertyChanged += (o, args) => _logFilter.Settings = _filterSettings;

            _window = window;
            _windowClosed = false;

            _defaultClickParams = clickParameters;
            _getGazeClickParams = getGazeClickParameters;

            FindMainWindow();

            window.LocationChanged += OnWindowLocationOrSizeChanged;
            window.SizeChanged += OnWindowLocationOrSizeChanged;
            window.Closed += OnWindowClosed;

            UpdateCursorStyle();

            var source = PresentationSource.FromVisual(window);

            _transform = source.CompositionTarget.TransformFromDevice;
            _reverseTransform = source.CompositionTarget.TransformToDevice;

            //
            // By the time we get here, the window may already be positioned correctly and
            // may receive no further location/size changed events. If that doesn't happen
            // _topLeft and _bottomRight don't get initialized correctly.
            // So force a call here
            //
            OnWindowLocationOrSizeChanged(window, null);

            if (source == null)
            {
                throw new NullReferenceException("Window does not contain a PresentationSource, may not be properly initialized");
            }
            if (source.CompositionTarget == null)
            {
                throw new NullReferenceException("Window Source does not contain a CompositionTarget, may not be properly initialized");
            }

            OriginalSignal = new GazeStats();
            FilteredSignal = new GazeStats();

            _xyFilter = InitializeFilter();

            _gazeHistory = new List<GazeHistoryItem>();
            _hitTargetTimes = new Dictionary<FrameworkElement, int>();
            _hitTarget = _offScreenElement;

            _gazeDataProvider.GazeEvent += OnGazeData;

            _idleDetector.GoneIdle += OnEyeGazeGoneIdle;
        }

        void OnEyeGazeGoneIdle(object sender, EventArgs e)
        {
            FireEyesOff();
        }

        private IFilter InitializeFilter()
        {
            IFilter xyFilter;
            switch (_filterSettings.ActiveFilter)
            {
                case FilterType.GainFilter:
                    xyFilter = new GainFilter(_filterSettings);
                    break;
                case FilterType.StampeFilter:
                    xyFilter = new StampeFilter(_filterSettings);
                    break;
                case FilterType.NullFilter:
                    xyFilter = new NullFilter();
                    break;
                case FilterType.OneEuroFilter:
                    xyFilter = new OneEuroFilter(_filterSettings);
                    break;
                default:
                    throw new ArgumentException("Unknown filter type");
            }
            xyFilter.Initialize();
            return xyFilter;
        }

        private static void CreateCursorWindow()
        {
            _gazeCursor = new GazeCursorElement
            {
                Radius = 5,
                Brush = new SolidColorBrush(Color.FromArgb(255, Colors.IndianRed.R, Colors.IndianRed.G, Colors.IndianRed.B)),
                Pen = new Pen(new SolidColorBrush(Colors.BlanchedAlmond), 2),
                MaxTrackCount = new Settings().MaxTrackCount
            };

            _cursorWindow = new Window
            {
                Left = FormsScreen.PrimaryScreen.Bounds.Left,
                Top = FormsScreen.PrimaryScreen.Bounds.Top,
                Width = FormsScreen.PrimaryScreen.Bounds.Width,
                Height = FormsScreen.PrimaryScreen.Bounds.Height,
                AllowsTransparency = true,
                Background = null,
                Cursor = Cursors.None,
                WindowStyle = WindowStyle.None,
                ShowInTaskbar = false,
                Topmost = true,
                ResizeMode = ResizeMode.NoResize,
                Content = _gazeCursor,
                IsEnabled = false,
                ShowActivated = false,
                IsHitTestVisible = false,
                Owner = Application.Current.MainWindow
            };

            var cursorUri = new Uri("Microsoft.HandsFree.Mouse;component/GazeCursor.cur", UriKind.Relative);
            Stream cursorStream = Application.GetResourceStream(cursorUri).Stream;

            _mouseCursor = new Cursor(cursorStream);

            _cursorWindow.Show();
        }

        public void ClearCursorTracks()
        {
            _gazeCursor.ClearTracks();
        }

        public void UpdateCursorStyle()
        {
            _gazeCursor.MaxTrackCount = _settings.MaxTrackCount;

            if (_gazeDataProvider.Sensor == Sensors.Sensors.Mouse)
            {
                _window.Cursor = _mouseCursor;
                _gazeCursor.HideCursor();
            }
            else if (_settings.ShowCursor || _forceMouseCursor)
            {
                _gazeCursor.IsVisible = true;
            }
            else
            {
                _window.Cursor = Cursors.None;
                _gazeCursor.HideCursor();
            }
        }

        private void UpdateCursorPosition(Point screenPoint)
        {
            Point cursorPoint = _transform.Transform(screenPoint);
            _gazeCursor.DrawCursorThumbnail(cursorPoint);
            if (_settings.ShowCursorTracks)
            {
                _gazeCursor.AddCursorPosition(cursorPoint.X, cursorPoint.Y, true);
            }
            else if ((_settings.ShowCursor || _forceMouseCursor) && _gazeDataProvider.Sensor != Sensors.Sensors.Mouse)
            {
                _gazeCursor.SetCursor(cursorPoint.X, cursorPoint.Y);
            }
        }

        private void FindMainWindow()
        {
            Debug.Assert(_window != null);
            Window window = _window;
            while ((window != null) && (window.Parent != null))
            {
                window = window.Parent as Window;
            }
            _mainWindow = window;
            _hwndMain = new WindowInteropHelper(window).Handle;
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            _windowClosed = true;
            _gazeDataProvider.GazeEvent -= OnGazeData;
        }

        private void OnWindowLocationOrSizeChanged(object sender, EventArgs e)
        {
            var window = (Window)sender;

            _topLeft = _reverseTransform.Transform(new Point(window.Left, window.Top));
            _bottomRight = _reverseTransform.Transform(new Point(window.Left + window.ActualWidth, window.Top + window.ActualHeight));
        }

        private void FireEyesOn()
        {
            if (!_wasTracking)
            {
                //_trace.TraceInformation("Firing EyesOn {0}", _window);
                if (EyesOn != null)
                {
                    EyesOn(this, null);
                }
                UpdateCursorStyle();
                _wasTracking = true;
            }
        }

        private void FireEyesOff()
        {
            if (_wasTracking)
            {
                //_trace.TraceInformation("Firing EyesOff {0}", _window);
                if (EyesOff != null)
                {
                    EyesOff(this, null);
                }
                _gazeCursor.HideCursor();
                _wasTracking = false;

                HandleDifferentTarget(_offScreenElement);
            }

            ResetHistory();
        }

        enum ButtonState
        {
            Inactive,
            Active,
            NearClick0,
            NearClick1,
            NearClick2,
            NearClick3,
            NearClick4,
            NearClick5,
            NearClick6,
            NearClick7,
            NearClick8,
            NearClick9,
            Click
        }

        static readonly ButtonState[] NearClickProgression =
            {
                ButtonState.NearClick0,
                ButtonState.NearClick1,
                ButtonState.NearClick2,
                ButtonState.NearClick3,
                ButtonState.NearClick4,
                ButtonState.NearClick5,
                ButtonState.NearClick6,
                ButtonState.NearClick7,
                ButtonState.NearClick8,
                ButtonState.NearClick9
            };
        static readonly int NearClickProgressionLength = NearClickProgression.Length;

        void GoToState(FrameworkElement control, ButtonState buttonState, bool useTransitions)
        {
            string description;
            var button = control as Button;
            if (button != null)
            {
                var content = button.Content;
                var textBlock = content as TextBlock;
                if (textBlock != null)
                {
                    description = textBlock.Text;
                }
                else
                {
                    description = content?.ToString();
                }
            }
            else
            {
                description = control.ToString();
            }
            //_trace.TraceInformation($"{description} => {buttonState}");

            VisualStateManager.GoToState(control, buttonState.ToString(), useTransitions);
        }

        void GoToStateNearClick(FrameworkElement control, long elapsed, long downTime, long upTime)
        {
            var position = downTime < upTime ? (NearClickProgressionLength * (elapsed - downTime)) / (upTime - downTime) : 0;
            var state = position < 0 ? NearClickProgression[0] :
                position < NearClickProgressionLength ? NearClickProgression[position] :
                NearClickProgression[NearClickProgressionLength - 1];

            GoToState(control, state, true);
        }

        void SendMouseInput(Point point, User32.MOUSEEVENTF flags, long timestamp)
        {
            User32.INPUT[] input = new User32.INPUT[1];
            input[0].type = User32.SendInputType.Mouse;
            input[0].U.mi.dx = (int)(point.X * 65535.0);
            input[0].U.mi.dy = (int)(point.Y * 65535.0);
            input[0].U.mi.dwFlags = flags;
            input[0].U.mi.time = 0; //(uint)timestamp / 1000;
            User32.SendInput(1, input, Marshal.SizeOf(typeof(User32.INPUT)));
        }

        delegate void GazeDataDelegate(GazeEventArgs e);

        void OnGazeData(object sender, GazeEventArgs e)
        {
            _window.Dispatcher.BeginInvoke(new GazeDataDelegate(GazeDataHandler), e);
        }

        public HitTestResultBehavior GazeHitTestResultFilter(HitTestResult hitResult)
        {
            FrameworkElement hitTarget = (hitResult != null) ? hitResult.VisualHit as FrameworkElement : null;
            if ((hitTarget != null) && (hitTarget.IsVisible) && (hitTarget.IsHitTestVisible))
            {
                _nextVisualHit = hitResult.VisualHit;
                return HitTestResultBehavior.Stop;
            }
            else
            {
                return HitTestResultBehavior.Continue;
            }
        }

        public FrameworkElement GetRootElement(DependencyObject initial)
        {
            DependencyObject current = initial;
            DependencyObject result = null;

            FrameworkElement element = GetGazeElement(initial);
            if (element != null)
            {
                return element;
            }

            while (current != null)
            {
                result = current;
                if (current is Visual)
                {
                    current = VisualTreeHelper.GetParent(current);
                }
                else
                {
                    current = LogicalTreeHelper.GetParent(current);
                }

                // stop enumeration if we encounter a FrameworkElement type we care about
                if ((current is Button) || (current is ToggleButton) || (current is TextBox) || (current is TabItem))
                {
                    result = current;
                    break;
                }
            }

            element = result as FrameworkElement;
            SetGazeElement(initial, element);
            return element;
        }

        void InvokeTarget(FrameworkElement hitTarget)
        {
            if (hitTarget == _offScreenElement)
            {
                FireEyesOff();
                return;
            }

            if (hitTarget.IsEnabled)
            {
                _lastInvokeTickCount = Environment.TickCount;

                var button = hitTarget as Button;
                if (button != null)
                {
                    var peer = new ButtonAutomationPeer(button);
                    var provider = (IInvokeProvider)peer.GetPattern(PatternInterface.Invoke);
                    Debug.Assert(provider != null);
                    provider.Invoke();
                }
                else
                {
                    var toggleButton = hitTarget as ToggleButton;
                    if (toggleButton != null)
                    {
                        var peer = new ToggleButtonAutomationPeer(toggleButton);
                        var provider = (IToggleProvider)peer.GetPattern(PatternInterface.Toggle);
                        Debug.Assert(provider != null);
                        provider.Toggle();
                    }
                    else
                    {
                        var textbox = hitTarget as TextBox;
                        if (textbox != null)
                        {
                            textbox.Focus();
                        }
                        else
                        {
                            var tabItem = hitTarget as TabItem;
                            if (tabItem != null)
                            {
                                tabItem.IsSelected = true;
                            }
                        }
                    }
                }

                // Ensure WPF update's the UI with its new state.
                CommandManager.InvalidateRequerySuggested();
            }
        }

        bool HandleSameTarget(GazeEventArgs e, FrameworkElement hitTarget, long elapsed)
        {
            bool invokeTarget = false;
            Debug.Assert(hitTarget != null);

            GazeClickParameters clickParams;
            if (hitTarget == _offScreenElement)
            {
                clickParams = _offScreenElementClickParams;
            }
            else if (_getGazeClickParams != null)
            {
                clickParams = _getGazeClickParams(hitTarget);
            }
            else
            {
                clickParams = _defaultClickParams;
            }

            //_trace.TraceInformation("ClickParameters: {0}, Elapsed: {1}", clickParams, elapsed);

            // Update the max total time we are maintaining if we ever see a click param higher than what we have.
            if ((clickParams.RepeatMouseDownDelay > _maxHistoryTime) && (clickParams.RepeatMouseDownDelay != uint.MaxValue))
            {
                _maxHistoryTime = clickParams.RepeatMouseDownDelay;
            }

            GazeMouseState oldState = _gazeMouseState;
            switch (_gazeMouseState)
            {
                case GazeMouseState.MouseEnter:
                    if (elapsed >= clickParams.MouseDownDelay)
                    {
                        _gazeMouseState = GazeMouseState.MouseDown;
                        GoToStateNearClick(hitTarget, elapsed, clickParams.MouseDownDelay, clickParams.MouseUpDelay);
                    }
                    break;
                case GazeMouseState.MouseDown:
                    if (elapsed >= clickParams.MouseUpDelay)
                    {
                        GoToState(hitTarget, ButtonState.Active, true);
                        _gazeMouseState = GazeMouseState.MouseUp;
                        invokeTarget = true;
                    }
                    else
                    {
                        GoToStateNearClick(hitTarget, elapsed, clickParams.MouseDownDelay, clickParams.MouseUpDelay);
                    }
                    break;
                case GazeMouseState.MouseUp:
                    if (elapsed >= clickParams.RepeatMouseDownDelay)
                    {
                        _gazeMouseState = GazeMouseState.RepeatMouseUp;
                        // The history needs to be reset to ensure proper handling of 
                        // mousedown/mouseup logic after the RepeatMouseDownDelay
                        ResetHistory();
                    }
                    break;
                case GazeMouseState.RepeatMouseUp:
                    if (elapsed >= clickParams.MouseDownDelay)
                    {
                        _gazeMouseState = GazeMouseState.RepeatMouseDown;
                        GoToStateNearClick(hitTarget, elapsed, clickParams.MouseDownDelay, clickParams.MouseUpDelay);
                    }
                    break;
                case GazeMouseState.RepeatMouseDown:
                    if (elapsed >= clickParams.MouseUpDelay)
                    {
                        GoToState(hitTarget, ButtonState.Active, true);
                        _gazeMouseState = GazeMouseState.RepeatMouseUp;
                        invokeTarget = true;
                        ResetHistory();
                    }
                    else
                    {
                        GoToStateNearClick(hitTarget, elapsed, clickParams.MouseDownDelay, clickParams.MouseUpDelay);
                    }
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }

            //_trace.TraceInformation("GazeMouseState: Old={0}, New={1}", oldState, _gazeMouseState);
            return invokeTarget;
        }

        void HandleDifferentTarget(FrameworkElement hitTarget)
        {
            Debug.Assert(hitTarget != null);
            _gazeMouseState = GazeMouseState.MouseEnter;
            //_trace.TraceInformation("ENTER hit target: {0}", hitTarget);
            if (_hitTarget != _offScreenElement)
            {
                GoToState(_hitTarget, ButtonState.Inactive, false);
            }
        }

        FrameworkElement GetHitTarget(GazeEventArgs ev)
        {
            var pt = new User32.POINT() { X = (int)ev.Screen.X, Y = (int)ev.Screen.Y };
            var hwnd = User32.WindowFromPoint(pt);
            if ((_hwndMain != hwnd) && (!User32.IsChild(_hwndMain, hwnd)))
            {
                return _offScreenElement;
            }

            if ((ev.Scaled.X < 0) || (ev.Scaled.Y < 0) || (ev.Scaled.X > 1) || (ev.Scaled.Y > 1))
            {
                return _offScreenElement;
            }

            FrameworkElement hitTarget;
            try
            {
                _nextVisualHit = null;
                Point windowPoint = _window.PointFromScreen(ev.Screen);
                VisualTreeHelper.HitTest(_window, null, new HitTestResultCallback(GazeHitTestResultFilter), new PointHitTestParameters(windowPoint));
                hitTarget = (_nextVisualHit != null) ? GetRootElement(_nextVisualHit) : null;
            }
            catch (System.InvalidOperationException ex)
            {
                _trace.TraceInformation("EXCEPTION: {0}", ex.Message);
                hitTarget = null;
            }

            if (hitTarget == null)
            {
                hitTarget = _offScreenElement;
            }

            return hitTarget;
        }

        bool VerifyWindowContext()
        {
            if (_windowClosed)
            {
                return false;
            }

            if (_sensorSettings.Sensor != Sensors.Sensors.LogPlayback)
            {
                // NOTE: This is relying on the application disabling the main window before a modal dialog is launched.
                if (!_window.IsEnabled)
                {
                    _trace.TraceEvent(TraceEventType.Verbose, 0, "Window: {0} is not active", _window);
                    FireEyesOff();
                    return false;
                }

                if (_mouseListener.LastMouseActivityTime < MinLastMouseTime && _gazeDataProvider.Sensor != Sensors.Sensors.Mouse)
                {
                    // ignore the gaze stream if the mouse was used recently
                    return false;
                }
            }

            return true;
        }

        void ResetHistory()
        {
            _gazeHistory.Clear();
            _hitTargetTimes.Clear();
        }

        FrameworkElement GetHitTargetWithMaxTime(GazeEventArgs ev, out long elapsedTime)
        {
            elapsedTime = 0;
            FrameworkElement hitTarget = GetHitTarget(ev);
            Debug.Assert(hitTarget != null);

            // append this gaze history item and append to the list
            var historyItem = new GazeHistoryItem { HitTarget = hitTarget, Timestamp = ev.Timestamp };

            if (_gazeHistory.Count == 0)
            {
                _gazeHistory.Add(historyItem);
                _hitTargetTimes[hitTarget] = 0;
                return hitTarget;
            }

            // find elapsed time since we got the last hit target
            int elapsed = 0, elapsedPrev = 0;
            elapsed = (int)(ev.Timestamp - _gazeHistory[_gazeHistory.Count - 1].Timestamp);
            historyItem.Duration = elapsed;
            _gazeHistory.Add(historyItem);

            // update the time this particular hit target has accumulated
            _hitTargetTimes.TryGetValue(hitTarget, out elapsedPrev);
            _hitTargetTimes[hitTarget] = elapsedPrev + elapsed;

            // drop the oldest samples from the list until we have samples only 
            // within the window we are monitoring
            while (_gazeHistory[_gazeHistory.Count - 1].Timestamp - _gazeHistory[0].Timestamp > _maxHistoryTime)
            {
                var evOldest = _gazeHistory[0];
                _gazeHistory.RemoveAt(0);

                var elapsedOldest = _gazeHistory[0].Timestamp - evOldest.Timestamp;

                _hitTargetTimes[evOldest.HitTarget] -= evOldest.Duration;
                Debug.Assert(_hitTargetTimes[evOldest.HitTarget] >= 0);
            }


            // return the most recent hit target along with the 
            // time it has accumulated within the window of history 
            // we are maintaining
            elapsedTime = _hitTargetTimes[hitTarget];
            return hitTarget;
        }

        void GazeDataHandler(GazeEventArgs gazeEventArgs)
        {
            FrameworkElement hitTarget = null;
            long elapsedTime;

            _idleDetector.Tick();

            if (!VerifyWindowContext())
            {
                return;
            }

            // if logging is turned on, log the original data, not the filtered data
            if ((_sensorSettings.Sensor != Sensors.Sensors.LogPlayback) && (_loggingSettings.LogGazeData))
            {
                _logFilter.Update(gazeEventArgs);
            }

            GazeEventArgs ev = _xyFilter.Update(gazeEventArgs);

            UpdateCursorPosition(ev.Screen);

            if ((hitTarget = GetHitTargetWithMaxTime(ev, out elapsedTime)) == null)
            {
                return;
            }

            bool invokeTarget = false;
            if (hitTarget == _hitTarget)
            {
                invokeTarget = HandleSameTarget(ev, hitTarget, elapsedTime);
            }
            else
            {
                HandleDifferentTarget(hitTarget);
            }

            if (invokeTarget)
            {
                InvokeTarget(hitTarget);
            }

            _hitTarget = hitTarget;

            if (_hitTarget != _offScreenElement)
            {
                FireEyesOn();
            }
        }

        void UpdateStatistics(FrameworkElement target, Point original, Point filtered)
        {
            Point targetPointOrig = _window.TranslatePoint(original, target);
            Point targetPointFiltered = _window.TranslatePoint(filtered, target);
            Point targetCenter = new Point(target.ActualWidth / 2.0, target.ActualHeight / 2.0);

            OriginalSignal.Update((targetCenter.X - targetPointOrig.X) / target.ActualWidth,
                                  (targetCenter.Y - targetPointOrig.Y) / target.ActualHeight);

            FilteredSignal.Update((targetCenter.X - targetPointFiltered.X) / target.ActualWidth,
                                  (targetCenter.Y - targetPointFiltered.Y) / target.ActualHeight);
        }

        public static void LaunchRecalibration()
        {
            _gazeDataProvider.LaunchRecalibration();
        }
    }
}
