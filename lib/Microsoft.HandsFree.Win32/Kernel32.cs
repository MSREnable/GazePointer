using System;
using System.Runtime.InteropServices;

namespace Microsoft.HandsFree.Win32
{
    public static class Kernel32
    {
        private const string DllName = "kernel32.dll";

        [DllImport(DllName)]
        public static extern uint GetCurrentThreadId();

        [DllImport(DllName, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport(DllName, SetLastError = true)]
        public static extern IntPtr LocalFree(IntPtr hMem);

    }
}
