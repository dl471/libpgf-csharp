using System;
using System.Runtime.InteropServices;

namespace libpgf_csharp
{
    internal static class DLLInterface
    {
        [DllImport("libpgfdll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr LoadFont(string fileName);

        [DllImport("libpgfdll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool SaveFont(IntPtr fontPtr, string FileName);

        [DllImport("libpgfdll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void FreeFont(IntPtr fontPtr);
    }


}