using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Autobot
{
    public class All
    {
        public static string TextExtract(Bitmap bmp)
        {
            return string.Empty;
        }

        public static Process GetForegroundProcess()
        {
            IntPtr hwnd = User32.GetForegroundWindow();
            uint pid;
            User32.GetWindowThreadProcessId(hwnd, out pid);
            return Process.GetProcessById((int)pid);
        }

        public static Bitmap ScreenCaptureForeground()
        {
            return ScreenCaptureProcess(GetForegroundProcess());
        }

        public static Bitmap ScreenCaptureProcess(string processName)
        {
            Process process = Process.GetProcesses().Where(p => p.MainWindowTitle == processName).FirstOrDefault();
            if (process == null)
            {
                return null;
            }

            return ScreenCaptureProcess(process);
        }

        public static Bitmap ScreenCaptureProcess(Process process)
        {
            IntPtr hwnd = process.MainWindowHandle;
            User32.Rectangle rect;
            User32.GetWindowRect(hwnd, out rect);
            /// Todo: what if there is no valid screen region
            Bitmap image = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
            using (Graphics gfx = Graphics.FromImage(image))
            {
                gfx.CopyFromScreen(rect.Left, rect.Top, 0, 0, new Size(rect.Width, rect.Height), CopyPixelOperation.SourceCopy);
            }
            return image;
        }

        public static Bitmap ScreenCaptureDesktop()
        {
            Bitmap image = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            using (Graphics gfx = Graphics.FromImage(image))
            {
                gfx.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
            }
            return image;
        }

        public static bool CompareBitmap(Bitmap a, Bitmap b)
        {
            if (a != b && (a == null || b == null))
            {
                return false;
            }

            if (a == b)
            {
                return true;
            }

            if (a.Width != b.Width || a.Height != b.Height)
            {
                return false;
            }

            Rectangle rect = new Rectangle(0, 0, a.Width, a.Height);
            BitmapData a_data = a.LockBits(rect, ImageLockMode.ReadOnly, a.PixelFormat);
            BitmapData b_data = b.LockBits(rect, ImageLockMode.ReadOnly, b.PixelFormat);

            try
            {
                IntPtr a_scan = a_data.Scan0;
                IntPtr b_scan = b_data.Scan0;
                int stride = a_data.Stride;
                int len = stride * a_data.Height;
                return memcmp(a_scan, b_scan, len) == 0;
            }
            finally
            {
                a.UnlockBits(a_data);
                b.UnlockBits(b_data);
            }
        }

        [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "*", Justification = "Match Microsoft Specs")]
        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int memcmp(IntPtr ptr1, IntPtr ptr2, long count);

        private static IntPtr MakeMouseClickParam(int x, int y)
        {
            return (IntPtr)((y << 16) | (x & 0xffff));
        }

        public static void RefreshNotificationArea()
        {
            IntPtr startBarHandle = User32.FindWindowEx(IntPtr.Zero, IntPtr.Zero, "Shell_TrayWnd", string.Empty);
            IntPtr trayHandle = User32.FindWindowEx(startBarHandle, IntPtr.Zero, "TrayNotifyWnd", string.Empty);
            IntPtr notifyHandle = User32.FindWindowEx(trayHandle, IntPtr.Zero, "SysPager", string.Empty);
            IntPtr notifyIconsHandle = User32.FindWindowEx(notifyHandle, IntPtr.Zero, "ToolbarWindow32", "Notification Area");
            if (notifyIconsHandle == IntPtr.Zero)
            {
                notifyIconsHandle = User32.FindWindowEx(notifyHandle, IntPtr.Zero, "ToolbarWindow32", "User Promoted Notification Area");
            }
            
            User32.Rectangle notifyRectangle = new User32.Rectangle();
            User32.GetClientRect(notifyIconsHandle, ref notifyRectangle);
            for (int x = notifyRectangle.Left; x < notifyRectangle.Right; x += 5)
            {
                for (int y = notifyRectangle.Top; y < notifyRectangle.Bottom; y += 5)
                {
                    User32.SendMessage(notifyIconsHandle, User32.Messages.WM_MOUSEMOVE, IntPtr.Zero, MakeMouseClickParam(x, y));
                }
            }
        }
    }
}
