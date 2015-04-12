using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Autobot
{
    [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "*", Justification = "Match Microsoft Specs")]
    public class User32
    {
        internal enum Messages
        {
            WM_ACTIVATE = 0x0006,
            WM_SETREDRAW = 0x000B,
            BM_GETCHECK = 0x00F0,
            BM_SETCHECK = 0x00F1,
            BM_GETSTATE = 0x00F2,
            BM_SETSTATE = 0x00F3,
            BM_SETSTYLE = 0x00F4,
            BM_CLICK = 0x00F5,
            BM_GETIMAGE = 0x00F6,
            BM_SETIMAGE = 0x00F7,
            WM_SYSCOMMAND = 0x0112,
            WM_MOUSEMOVE = 0x0200,
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205,
            WM_DRAWCLIPBOARD = 0x0308,
            WM_CHANGECBCHAIN = 0x030D,
            WM_USER = 0x0400,
            EM_GETEVENTMASK = 0x043B,
            EM_SETEVENTMASK = 0x0445
        };

        /// <summary>
        /// The GetWindowDC function retrieves the device context (DC) for the entire window, including title bar, menus, 
        /// and scroll bars. A window device context permits painting anywhere in a window, because the origin of the device 
        /// context is the upper-left corner of the window instead of the client area.
        /// 
        /// GetWindowDC assigns default attributes to the window device context each time it retrieves the device context. 
        /// Previous attributes are lost.
        /// </summary>
        /// <param name="hWnd">A handle to the window with a device context that is to be retrieved. If this value is NULL, 
        /// GetWindowDC retrieves the device context for the entire screen.  To get the device context for other display 
        /// monitors, use the EnumDisplayMonitors and CreateDC functions.
        /// </param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        internal static extern IntPtr GetWindowDC(IntPtr hWnd);

        /// <summary>
        /// Retrieves the dimensions of the bounding rectangle of the specified window. The dimensions are given in screen 
        /// coordinates that are relative to the upper-left corner of the screen.
        /// </summary>
        /// <param name="hwnd">A handle to the window.</param>
        /// <param name="lpRect">A pointer to a RECT structure that receives the screen coordinates of the upper-left and 
        /// lower-right corners of the window.</param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetWindowRect(IntPtr hwnd, out Rectangle lpRect);

        /// <summary>
        /// The PrintWindow function copies a visual window into the specified device context (DC), typically a printer DC.
        /// </summary>
        /// <param name="hwnd">A handle to the window that will be copied.</param>
        /// <param name="hdcBlt">A handle to the device context.</param>
        /// <param name="nFlags">The drawing options. It can be one of the following values:
        /// PW_CLIENTONLY:
        /// Only the client area of the window is copied to hdcBlt. By default, the entire window is copied.
        /// </param>
        /// <returns></returns>
        [DllImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool PrintWindow(IntPtr hwnd, IntPtr hdcBlt, uint nFlags);

        /// <summary>
        /// The ReleaseDC function releases a device context (DC), freeing it for use by other applications. The effect of 
        /// the ReleaseDC function depends on the type of DC. It frees only common and window DCs. It has no effect on class 
        /// or private DCs.
        /// </summary>
        /// <param name="hWnd">A handle to the window whose DC is to be released.</param>
        /// <param name="hDC">A handle to the DC to be released.</param>
        /// <returns>The return value indicates whether the DC was released. If the DC was released, the return value is 1.
        /// If the DC was not released, the return value is zero.</returns>
        [DllImport("user32.dll")]
        internal static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        internal static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint ProcessId);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetClientRect(IntPtr hWnd, ref Rectangle lpRect);
        
        [DllImport("User32.dll")]
        internal static extern IntPtr SendMessage(IntPtr hWnd, Messages Msg, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        public struct Rectangle
        {
            /// <summary>
            /// x position of upper-left corner
            /// </summary>
            public int Left;

            /// <summary>
            /// y position of upper-left corner
            /// </summary>
            public int Top;

            /// <summary>
            /// x position of lower-right corner
            /// </summary>
            public int Right;

            /// <summary>
            /// y position of lower-right corner
            /// </summary>
            public int Bottom;

            public int Width
            {
                get
                {
                    return this.Right - this.Left;
                }
            }

            public int Height
            {
                get
                {
                    return this.Bottom - this.Top;
                }
            }
        }
    }
}
