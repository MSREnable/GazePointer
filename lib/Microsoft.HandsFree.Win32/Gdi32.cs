namespace Microsoft.HandsFree.Win32
{
    using System;
    using System.Runtime.InteropServices;

    public static class Gdi32
    {
        private const string DllName = "gdi32.dll";

        [DllImport(DllName)]
        public static extern IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

        public const int RGN_AND = 1;
        public const int RGN_OR = 2;
        public const int RGN_XOR = 3;
        public const int RGN_DIFF = 4;
        public const int RGN_COPY = 5;

        [DllImport(DllName)]
        public static extern int CombineRgn(IntPtr hrgnDest, IntPtr hrgnSrc1, IntPtr hrgnSrc2, int fnCombineMode);

        [DllImport(DllName)]
        public static extern bool DeleteObject(IntPtr hObject);
    }
}
