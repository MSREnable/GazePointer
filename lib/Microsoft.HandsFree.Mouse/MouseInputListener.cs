using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

using Microsoft.HandsFree.Win32;

namespace Microsoft.HandsFree.Mouse
{
    public class MouseInputListener
    {
        Window _listenerWindow;
        IntPtr _listenerHwnd;
        HwndSource _hwndSource;
        HwndSourceHook _wndprocHook;

        readonly Stopwatch _lastMouseActivityTimer;
        
        public MouseInputListener()
        {
            _listenerWindow = new Window()
            {
                Width = 0,
                Height = 0,
                WindowStyle = WindowStyle.None,
                ResizeMode = ResizeMode.NoResize,
                ShowInTaskbar = false,
                ShowActivated = false,
                Visibility = Visibility.Hidden
            };
            _listenerWindow.Show();

            InitializeRawInput();

            _lastMouseActivityTimer = new Stopwatch();
            _lastMouseActivityTimer.Start();
        }

        void InitializeRawInput()
        {
            _listenerHwnd = new WindowInteropHelper(_listenerWindow).Handle;

            User32.RAWINPUTDEVICE[] rawInputDevice = new User32.RAWINPUTDEVICE[1];
            rawInputDevice[0].UsagePage = User32.UsagePageGenericDesktop;
            rawInputDevice[0].Usage = User32.UsageMouse;
            rawInputDevice[0].Flags = User32.RawInputDeviceFlags.InputSink;
            rawInputDevice[0].WindowHandle = _listenerHwnd;

            bool success;
            success = User32.RegisterRawInputDevices(rawInputDevice, 1, Marshal.SizeOf(typeof(User32.RAWINPUTDEVICE)));

            _hwndSource = HwndSource.FromHwnd(_listenerHwnd);
            _wndprocHook = new HwndSourceHook(WndProc);
            _hwndSource.AddHook(_wndprocHook);
        }

        public void Terminate()
        {
            _hwndSource.RemoveHook(_wndprocHook);
            _listenerWindow.Close();
        }

        IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == User32.WM_INPUT)
            {
                User32.RAWINPUT rawInput = new User32.RAWINPUT();
                int size = Marshal.SizeOf(typeof(User32.RAWINPUT));
                if ((User32.GetRawInputData(lParam, User32.RawInputCommand.Input, out rawInput, ref size, Marshal.SizeOf(typeof(User32.RAWINPUTHEADER))) > 0) &&
                    (rawInput.header.hDevice != IntPtr.Zero) &&
                    (rawInput.header.dwType == User32.RIM_TYPEMOUSE))
                {
                    // Restart the mouse timer if the physical mouse was used 
                    _lastMouseActivityTimer.Restart();
                }
            }
            return IntPtr.Zero;
        }

        public long LastMouseActivityTime
        {
            get
            {
                return _lastMouseActivityTimer.IsRunning ? _lastMouseActivityTimer.ElapsedMilliseconds : long.MaxValue;
            }
        }
    }
}
