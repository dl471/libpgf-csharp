using System;
using System.Runtime.InteropServices;

namespace libpgf_csharp
{
    public class PGFGlyph
    {

        private PGFGlyphRaw rawGlpyh;
        private IntPtr glyphPtr;

        public int width
        {
            get { return rawGlpyh.width; }
            set { rawGlpyh.width = value; }
        }

        public PGFGlyph(IntPtr ptrToGylph)
        {
            glyphPtr = ptrToGylph;
            rawGlpyh = Marshal.PtrToStructure<PGFGlyphRaw>(ptrToGylph);
        }

        public void SaveGylph()
        {
            Marshal.StructureToPtr<PGFGlyphRaw>(rawGlpyh, glyphPtr, false);
        }

    }

    [StructLayout(LayoutKind.Sequential)]
    internal class PGFGlyphRaw
    {
        int index;
        int ucs;
        int have_shadow;

        int size;       /* 14bits */
        public int width;      /* 7bits */
        int height;     /* 7bits */
        int left;       /* 7bits signed */
        int top;        /* 7bits signed */
        int flag;       /* 6bits: 2+1+1+1+1 */

        int shadow_flag;/* 7bits: 2+2+3 */
        int shadow_id;  /* 9bits */

        int dim_id;
        int bx_id;
        int by_id;
        int adv_id;

        F26_Pair dimension;
        F26_Pair bearingX;
        F26_Pair bearingY;
        F26_Pair advance;

        IntPtr data;
        IntPtr bmp;
    }

}