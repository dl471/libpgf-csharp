using System;
using System.Runtime.InteropServices;

namespace libpgf_csharp
{
    internal static class DLLInterface
    {
        [DllImport("libpgfglyph.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr LoadFont(string fileName);

        [DllImport("libpgfglyph.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool SaveFont(IntPtr fontPtr, string FileName);
    }
}