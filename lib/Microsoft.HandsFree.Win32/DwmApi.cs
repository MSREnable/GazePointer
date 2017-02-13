// ReSharper disable InconsistentNaming

using System;
using System.Runtime.InteropServices;

namespace Microsoft.HandsFree.Win32
{
    public class DwmApi
    {
        [Flags]
        public enum DWMWINDOWATTRIBUTE
        {
            DWMA_NCRENDERING_ENABLED = 1,
            DWMA_NCRENDERING_POLICY = 2,
            DWMA_TRANSITIONS_FORCEDISABLED = 3,
            DWMA_ALLOW_NCPAINT = 4,
            DWMA_CPATION_BUTTON_BOUNDS = 5,
            DWMA_NONCLIENT_RTL_LAYOUT = 6,
            DWMA_FORCE_ICONIC_REPRESENTATION = 7,
            DWMA_FLIP3D_POLICY = 8,
            DWMA_EXTENDED_FRAME_BOUNDS = 9,
            DWMA_HAS_ICONIC_BITMAP = 10,
            DWMA_DISALLOW_PEEK = 11,
            DWMA_EXCLUDED_FROM_PEEK = 12,
            DWMA_LAST = 13
        }

        [Flags]
        public enum DWMNCRenderingPolicy
        {
            UseWindowStyle = 0,
            Disabled = 1,
            Enabled = 2,
            Last = 3
        }

        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hWnd, int attr, ref int attrValue, int attrSize);
    }
}
