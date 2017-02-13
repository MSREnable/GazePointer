using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PannableForms
{
    public sealed class ScrollTextBox : TextBox
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetScrollInfo(IntPtr hwnd, int fnBar, ref SCROLLINFO lpsi);

        [DllImport("user32.dll")]
        static extern int SetScrollInfo(IntPtr hwnd, int fnBar, [In] ref SCROLLINFO lpsi, bool fRedraw);

        [DllImport("User32.dll", CharSet = CharSet.Auto, EntryPoint = "SendMessage")]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        const int WM_VSCROLL = 277;
        const int SB_THUMBTRACK = 5;

        struct SCROLLINFO
        {
            public uint cbSize;
            public uint fMask;
            public int nMin;
            public int nMax;
            public uint nPage;
            public int nPos;
            public int nTrackPos;
        }

        enum ScrollBarDirection
        {
            SB_VERT = 1
        }

        enum ScrollInfoMask
        {
            SIF_RANGE = 0x1,
            SIF_PAGE = 0x2,
            SIF_POS = 0x4,
            SIF_TRACKPOS = 0x10,
            SIF_ALL = SIF_RANGE + SIF_PAGE + SIF_POS + SIF_TRACKPOS
        }

        public void Scroll(int pixels)
        {
            var si = new SCROLLINFO();
            si.cbSize = (uint)Marshal.SizeOf(si);
            si.fMask = (uint)ScrollInfoMask.SIF_ALL;
            GetScrollInfo(Handle, (int)ScrollBarDirection.SB_VERT, ref si);
            si.nPos += pixels;
            if (si.nPos < 0)
            {
                si.nPos = 0; // stop scrolling from wrapping around at the top
            }
            SetScrollInfo(Handle, (int)ScrollBarDirection.SB_VERT, ref si, true);
            var ptrWparam = new IntPtr(SB_THUMBTRACK + 0x10000 * si.nPos);
            var ptrLparam = new IntPtr(0);
            SendMessage(Handle, WM_VSCROLL, ptrWparam, ptrLparam);
        }
    }
}
