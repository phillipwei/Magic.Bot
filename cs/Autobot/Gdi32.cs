using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Autobot
{
    [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "*", Justification = "Match Microsoft Specs")]
    public class Gdi32
    {
        /// <summary>
        /// This function creates a memory device context (DC) compatible with the specified device.
        /// </summary>
        /// <param name="hdc"> Handle to an existing device context.
        /// If this handle is NULL, the function creates a memory device context compatible with the application's current 
        /// screen.</param>
        /// <returns></returns>
        [DllImport("gdi32.dll", SetLastError = true)]
        internal static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        /// <summary>
        /// The CreateCompatibleBitmap function creates a bitmap compatible with the device that is associated with the 
        /// specified device context.
        /// </summary>
        /// <param name="hdc">A handle to a device context.</param>
        /// <param name="nWidth">The bitmap width, in pixels.</param>
        /// <param name="nHeight">The bitmap height, in pixels.</param>
        /// <returns>If the function succeeds, the return value is a handle to the compatible bitmap (DDB).
        /// If the function fails, the return value is NULL.</returns>
        [DllImport("gdi32.dll")]
        internal static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

        /// <summary>
        /// The SelectObject function selects an object into the specified device context (DC). The new object replaces 
        /// the previous object of the same type.
        /// </summary>
        /// <param name="hdc">A handle to the DC.</param>
        /// <param name="hgdiobj">A handle to the object to be selected. The specified object must have been created by 
        /// using one of the following functions.
        /// * Bitmap
        /// CreateBitmap, CreateBitmapIndirect, CreateCompatibleBitmap, CreateDIBitmap, CreateDIBSection
        /// Bitmaps can only be selected into memory DC's. A single bitmap cannot be selected into more than one DC at the 
        /// same time.
        /// * Brush
        /// CreateBrushIndirect, CreateDIBPatternBrush, CreateDIBPatternBrushPt, CreateHatchBrush, CreatePatternBrush, 
        /// CreateSolidBrush
        /// * Font
        /// CreateFont, CreateFontIndirect
        /// * Pen
        /// CreatePen, CreatePenIndirect
        /// * Region
        /// CombineRgn, CreateEllipticRgn, CreateEllipticRgnIndirect, CreatePolygonRgn, CreateRectRgn, CreateRectRgnIndirect
        /// </param>
        /// <returns></returns>
        [DllImport("gdi32.dll", ExactSpelling = true, PreserveSig = true, SetLastError = true)]
        internal static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        /// <summary>
        /// The DeleteObject function deletes a logical pen, brush, font, bitmap, region, or palette, freeing all system 
        /// resources associated with the object. After the object is deleted, the specified handle is no longer valid.
        /// </summary>
        /// <param name="hObject">A handle to a logical pen, brush, font, bitmap, region, or palette.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// If the specified handle is not valid or is currently selected into a DC, the return value is zero.</returns>
        [DllImport("gdi32.dll")]
        internal static extern bool DeleteObject(IntPtr hObject);
    }
}
