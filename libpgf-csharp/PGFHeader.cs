using System;
using System.Runtime.InteropServices;

namespace libpgf_csharp
{
    public class PGFHeader
    {
        private PGFHeaderRaw rawHeader;
        private IntPtr headerPtr;

        public string fontName
        {
            get { return rawHeader.font_name; }
            set { rawHeader.font_name = value; }
        }
        public string fontType
        {
            get { return rawHeader.font_type; }
            set { rawHeader.font_name = value; }
        }

        public PGFHeader(IntPtr ptrToHeader)
        {
            headerPtr = ptrToHeader;
            rawHeader = Marshal.PtrToStructure<PGFHeaderRaw>(ptrToHeader);
        }

        public void SaveHeader()
        {
            SaveHeader(headerPtr);
        }

        public void SaveHeader(IntPtr ptrToHeader)
        {
            Marshal.StructureToPtr<PGFHeaderRaw>(rawHeader, ptrToHeader, true);
        }

    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct PGFHeaderRaw
    {
        ushort header_start;
        ushort header_len;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        byte[] pgf_id;
        uint revision;
        uint version;

        /* 0x0010 */
        public uint charmap_len;
        uint charptr_len;
        uint charmap_bpe;
        uint charptr_bpe;

        /* 0x0020 */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        byte[] unk_20; /* 04 04 */
        byte bpp;       /* 04 */
        byte unk_23;        /* 00 */

        uint h_size;
        uint v_size;
        uint h_res;
        uint v_res;

        byte unk_34;        /* 00 */
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string font_name;  /* "FTT-NewRodin Pro DB" */
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string font_type; /* "Regular" */
        byte unk_B5;        /* 00 */

        ushort charmap_min;
        ushort charmap_max;

        /* 0x00BA */
        ushort unk_BA;      /* 0x0000 */
        uint unk_BC;        /* 0x00010000 */
        uint unk_C0;        /* 0x00000000 */
        uint unk_C4;        /* 0x00000000 */
        uint unk_C8;        /* 0x00010000 */
        uint unk_CC;        /* 0x00000000 */
        uint unk_D0;        /* 0x00000000 */

        int ascender;
        int descender;
        int max_h_bearingX;
        int max_h_bearingY;
        int min_v_bearingX;
        int max_v_bearingY;
        int max_h_advance;
        int max_v_advance;
        int max_h_dimension;
        int max_v_dimension;
        ushort max_glyph_w;
        ushort max_glyph_h;

        ///* 0x0100 */
        ushort charptr_scale;   /* 0004 */
        byte dimension_len;
        byte bearingX_len;
        byte bearingY_len;
        byte advance_len;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 102)]
        byte[] unk_106;

        uint shadowmap_len;
        uint shadowmap_bpe;
        uint unk_174;
        uint shadowscale_x;
        uint shadowscale_y;
        uint unk_180;
        uint unk_184;
    }

}
