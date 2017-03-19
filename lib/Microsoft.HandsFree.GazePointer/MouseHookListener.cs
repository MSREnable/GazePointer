using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.HandsFree.Win32;

namespace Microsoft.HandsFree.GazePointer
{
    public class MouseHookListener : IDisposable
    {
        private bool _disposed;
        private readonly IntPtr _mouseHook;
        private readonly Stopwatch _lastMouseActivityTimer;

        // Following Resharper's advice to move this to a local variable causes disposed delegate runtime errors
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly User32.MouseProcLL _mouseProcLl;

        public long LastMouseActivityTime
        {
            get
            {
                return _lastMouseActivityTimer.IsRunning ? _lastMouseActivityTimer.ElapsedMilliseconds : long.MaxValue;
            }
        }

        public MouseHookListener()
        {
            _lastMouseActivityTimer = new Stopwatch();
            var moduleHandle = Kernel32.GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName);
            _mouseProcLl = LowLevelMouseProc;
            _mouseHook = User32.SetWindowsHookEx(User32.HookType.WH_MOUSE_LL, _mouseProcLl, moduleHandle, 0);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    User32.UnhookWindowsHookEx(_mouseHook);
                }
                _disposed = true;
            }
        }

        ~MouseHookListener()
        {
            Dispose(false);
        }

        public IntPtr LowLevelMouseProc(int code, uint wParam, IntPtr lParam)
        {
            if (code >= 0)
            {
                var hookStruct = (User32.MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(User32.MSLLHOOKSTRUCT));
                if ((hookStruct.flags & User32.LLMHF_INJECTED) != User32.LLMHF_INJECTED)
                {
                    _lastMouseActivityTimer.Restart();
                }
            }
            return User32.CallNextHookEx(_mouseHook, code, wParam, lParam);
        }
    }
}
