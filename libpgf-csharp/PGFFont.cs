using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.IO;

namespace libpgf_csharp
{
    public class PGFFont : IDisposable
    {

        private PGFFontRaw rawFont;
        private string originalFileName;
        private IntPtr fontPtr;

        private PGFGlyph[] glyphs;
        private PGFGlyph[] shadowGlyphs;
        private Dictionary<int, PGFGlyph> ucsDict;

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

            LoadGlyphs();

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
            SaveGlyphs();

            Marshal.StructureToPtr<PGFFontRaw>(rawFont, ptrToFont, false);
            return DLLInterface.SaveFont(fontPtr, fileName);
        }

        /// <summary>
        /// Attempt to get glyph by index - returns null on failure
        /// </summary>
        public PGFGlyph GetGlyphByIndex(int index)
        {
            if (index < glyphs.Length)
            {
                return glyphs[index];
            }

            return null;
        }

        /// <summary>
        /// Attempt to get glyph by index - returns null on failure
        /// </summary>
        public PGFGlyph GetGlyphByUcs(int ucs)
        {
            PGFGlyph glyph;
            bool success = ucsDict.TryGetValue(ucs, out glyph);

            if (success)
            {
                return ucsDict[ucs];
            }

            return null;
        }

        private void LoadGlyphs()
        {
            LinkedList<PGFGlyph> glyphList = new LinkedList<PGFGlyph>();
            LinkedList<PGFGlyph> shadowGlyphList = new LinkedList<PGFGlyph>();
            ucsDict = new Dictionary<int, PGFGlyph>();

            for (int i = 0; i < PGFRawConsts.GLYPH_NUM; i++)
            {
                IntPtr ptrToGlyph = rawFont.char_glpyh[i];

                if (ptrToGlyph == IntPtr.Zero)
                {
                    glyphList.AddLast((PGFGlyph)null);
                }
                else
                {
                    PGFGlyph newGlyph = new PGFGlyph(ptrToGlyph);
                    glyphList.AddLast(newGlyph);
                    ucsDict[i] = newGlyph;
                }

            }

            glyphs = glyphList.ToArray();

            for (int i = 0; i < PGFRawConsts.SHADOW_GLYPH_NUM; i++)
            {
                IntPtr ptrToGlyph = rawFont.shadow_glpyh[i];

                if (ptrToGlyph == IntPtr.Zero)
                {
                    shadowGlyphList.AddLast((PGFGlyph)null);
                }
                else
                {
                    PGFGlyph newGlyph = new PGFGlyph(ptrToGlyph);
                    shadowGlyphList.AddLast(newGlyph);
                }

            }

            shadowGlyphs = shadowGlyphList.ToArray();

        }

        private void SaveGlyphs()
        {
            foreach (PGFGlyph glyph in glyphs)
            {
                if (glyph != null)
                {
                    glyph.SaveGylph();
                }
            }

            foreach (PGFGlyph glyph in shadowGlyphs)
            {
                if (glyph != null)
                {
                    glyph.SaveGylph();
                }
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                System.Diagnostics.Debug.WriteLine("Disposing of font");
                DLLInterface.FreeFont(fontPtr);
                
                disposedValue = true;
            }
        }

        ~PGFFont()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }

    internal static class PGFRawConsts
    {
        public const int GLYPH_NUM = 65536;
        public const int SHADOW_GLYPH_NUM = 512;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct PGFFontRaw
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
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = PGFRawConsts.GLYPH_NUM)]
        public IntPtr[] char_glpyh;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = PGFRawConsts.SHADOW_GLYPH_NUM)]
        public IntPtr[] shadow_glpyh;
    }

    internal struct F26_Pair
    {
        int h;
        int v;
    }
}