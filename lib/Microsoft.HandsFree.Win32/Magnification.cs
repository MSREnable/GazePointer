using System;
using System.Runtime.InteropServices;

namespace Microsoft.HandsFree.Win32
{
    public class Magnification
    {
        private const string DllName = "Magnification.dll";

        public const string WindowClassMagnifier = "Magnifier";

        public enum MagnifierStyle : int
        {
            MS_SHOWMAGNIFIEDCURSOR = 0x0001,
            MS_CLIPAROUNDCURSOR = 0x0002,
            MS_INVERTCOLORS = 0x0004
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Transformation
        {
            public float m00;
            public float m10;
            public float m20;
            public float m01;
            public float m11;
            public float m21;
            public float m02;
            public float m12;
            public float m22;

            public Transformation(float magnificationFactor)
                : this()
            {
                m00 = magnificationFactor;
                m11 = magnificationFactor;
                m22 = 1.0f;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ColorEffect
        {
            public float transform00;
            public float transform10;
            public float transform20;
            public float transform30;
            public float transform40;
            public float transform01;
            public float transform02;
            public float transform03;
            public float transform04;
            public float transform11;
            public float transform12;
            public float transform13;
            public float transform14;
            public float transform21;
            public float transform22;
            public float transform23;
            public float transform24;
            public float transform31;
            public float transform32;
            public float transform33;
            public float transform34;
            public float transform41;
            public float transform42;
            public float transform43;
            public float transform44;
        }

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern bool MagInitialize();

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern bool MagUninitialize();

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern bool MagSetWindowSource(IntPtr hwnd, Shell32.RECT rect);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern bool MagGetWindowSource(IntPtr hwnd, ref Shell32.RECT pRect);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern bool MagSetWindowTransform(IntPtr hwnd, ref Transformation pTransform);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern bool MagGetWindowTransform(IntPtr hwnd, ref Transformation pTransform);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern bool MagSetWindowFilterList(IntPtr hwnd, int dwFilterMode, int count, IntPtr hwndPtr);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int MagGetWindowFilterList(IntPtr hwnd, IntPtr pdwFilterMode, int count, IntPtr hwndPtr);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern bool MagSetColorEffect(IntPtr hwnd, ref ColorEffect pEffect);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern bool MagGetColorEffect(IntPtr hwnd, ref ColorEffect pEffect);
    }
}
