using System;
using System.Runtime.InteropServices;
using System.IO;

namespace libpgf_csharp
{
    public class PGFFont
    {

        private PGFFontRaw rawFont;
        private string originalFileName;
        private IntPtr fontPtr;

        public PGFHeader header { get; }

        public PGFFont(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException("Could not find file", fileName);
            }

            IntPtr ptrToFont = DLLInterface.LoadFont(fileName);
            originalFileName = fileName;

            if (ptrToFont == IntPtr.Zero)
            {
                throw new Exception("Failed to load font");
            }

            fontPtr = ptrToFont;
            rawFont = Marshal.PtrToStructure<PGFFontRaw>(ptrToFont);

            header = new PGFHeader(rawFont.header);
        }

        public bool SaveFont()
        {
            return SaveFont(fontPtr, originalFileName);
        }

        public bool SaveFont(string fileName)
        {
            return SaveFont(fontPtr, fileName);
        }

        public bool SaveFont(IntPtr ptrToFont, string fileName)
        {
            header.SaveHeader();

            Marshal.StructureToPtr<PGFFontRaw>(rawFont, ptrToFont, false);

            return DLLInterface.SaveFont(fontPtr, fileName);
        }

    }

    [StructLayout(LayoutKind.Sequential)]
    struct PGFFontRaw
    {
        public IntPtr header;

        public IntPtr dimension;
        public IntPtr bearingX;
        public IntPtr bearingY;
        public IntPtr advance;

        public IntPtr charmap;
        public IntPtr charptr;
        public IntPtr shadowmap;

        public IntPtr glyphdata;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 65536)]
        public IntPtr[] char_glpyh;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
        public IntPtr[] shadow_glpyh;
    }

    struct F26_Pair
    {
        int h;
        int v;
    }
}
