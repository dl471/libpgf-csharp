using System;
using System.Runtime.InteropServices;

namespace libpgf_csharp
{
    class PGFFont
    {

        private PGFFontRaw rawFont;
        private IntPtr fontPtr;

        public PGFFont(IntPtr ptrToFont)
        {
            fontPtr = ptrToFont;
            rawFont = Marshal.PtrToStructure<PGFFontRaw>(ptrToFont);
        }

        public void SaveFont()
        {
            SaveFont(fontPtr);
        }

        public void SaveFont(IntPtr ptrToFont)
        {
            Marshal.StructureToPtr<PGFFontRaw>(rawFont, ptrToFont, false);
        }

    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct PGFFontRaw
    {
        IntPtr header;

        IntPtr dimension;
        IntPtr bearingX;
        IntPtr bearingY;
        IntPtr advance;

        IntPtr charmap;
        IntPtr charptr;
        IntPtr shadowmap;

        IntPtr glyphdata;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 65536)]
        IntPtr[] char_glpyh;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
        IntPtr[] shadow_glpyh;
    }

    internal struct F26_Pair
    {
        int h;
        int v;
    }
}