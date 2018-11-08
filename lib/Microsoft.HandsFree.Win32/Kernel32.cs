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

        // CreateHardLink(@"c:\temp\New Link", @"c:\temp\Original File",IntPtr.Zero);
        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool CreateHardLink(string lpFileName, string lpExistingFileName, IntPtr lpSecurityAttributes);

        // CreateSymbolicLink(symbolicLink, directoryName, SymbolicLink.Directory);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CreateSymbolicLink(string lpSymlinkFileName, string lpTargetFileName, SymbolicLink dwFlags);

        public enum SymbolicLink
        {
            File = 0,
            Directory = 1,
            FileUnprivileged = 2,
            DirectoryUnprivileged = 3
        }
    }
}
