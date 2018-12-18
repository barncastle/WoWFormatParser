/*
* Copyright (c) <2011> <by Xalcon @ mmowned.com-Forum>
* Permission is hereby granted, free of charge, to any person obtaining
* a copy of this software and associated documentation files (the "Software"),
* to deal in the Software without restriction, including without limitation
* the rights to use, copy, modify, merge, publish, distribute, sublicense,
* and/or sell copies of the Software, and to permit persons to whom the
* Software is furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included
* in all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
* FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
* COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
* WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Common;

namespace WoWFormatParser.Structures.BLP
{
    public sealed class BLP : Format, IDisposable
    {
        public uint Version;
        public ColorFileFormat ColorEncoding; // 1 = Uncompressed, 2 = DirectX Compressed
        public byte AlphaSize; // 0 = no alpha, 1 = 1 Bit, 4 = Bit (only DXT3), 8 = 8 Bit Alpha
        public BLPPixelFormat PreferredFormat; // 0: DXT1 alpha (0 or 1 Bit alpha), 1 = DXT2/3 alpha (4 Bit), 7: DXT4/5 (interpolated alpha)
        public byte HasMips; // If true (1), then there are Mipmaps
        public int Width; // X Resolution of the biggest Mipmap
        public int Height; // Y Resolution of the biggest Mipmap
        public readonly uint[] MipOffsets; // Offset for every Mipmap level. If 0 = no more mitmap level
        public readonly uint[] MipSizes; // Size for every level

        private byte[] _buffer;
        private CRGBA[] _paletteBGRA;

        public BLP(BinaryReader br)
        {
            _buffer = br.ReadBytes((int)br.BaseStream.Length);
            br.BaseStream.Position = 0;

            // Checking for correct Magic-Code
            if (br.ReadUInt32() != 0x32504c42)
                throw new Exception("Invalid BLP Format.");

            // Reading type
            Version = br.ReadUInt32();
            if (Version != 1)
                throw new Exception($"Invalid version.");

            ColorEncoding = br.ReadEnum<ColorFileFormat>();
            AlphaSize = br.ReadByte();
            PreferredFormat = br.ReadEnum<BLPPixelFormat>();
            HasMips = br.ReadByte();
            Width = br.ReadInt32();
            Height = br.ReadInt32();
            MipOffsets = br.ReadStructArray<uint>(16);
            MipSizes = br.ReadStructArray<uint>(16);

            // fix miptable sizes
            int mipCount = Math.Max(MipOffsets.Count(x => x != 0) - 1, 0);
            Array.Resize(ref MipOffsets, mipCount);
            Array.Resize(ref MipSizes, mipCount);

            // When Encoding is 1, there is no image compression and we have to read a color palette
            if (ColorEncoding == ColorFileFormat.Palette)
                _paletteBGRA = br.ReadStructArray<CRGBA>(256);
            else
                _paletteBGRA = new CRGBA[256];
        }


        /// <summary>
        /// Converts the BLP to a System.Drawing.Bitmap
        /// </summary>
        /// <param name="mipmapLevel">The desired Mipmap-Level. If the given level is invalid, the smallest available level is choosen</param>
        /// <returns>The Bitmap</returns>
        public Bitmap GetBitmap(int mipmapLevel)
        {
            if (mipmapLevel >= MipSizes.Length)
                mipmapLevel = MipSizes.Length - 1;

            if (mipmapLevel < 0)
                mipmapLevel = 0;

            int scale = (int)Math.Pow(2, mipmapLevel);
            int w = Width / scale;
            int h = Height / scale;
            Bitmap bmp = new Bitmap(w, h);

            byte[] data = GetPictureData(mipmapLevel);
            byte[] pic = GetImageBytes(w, h, data); // This bytearray stores the Pixel-Data

            BitmapData bmpdata = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            ConvertToBGRA(pic);
            Marshal.Copy(pic, 0, bmpdata.Scan0, pic.Length); // copy! :D
            bmp.UnlockBits(bmpdata);

            return bmp;
        }


        /// <summary>
        /// Returns the uncompressed image as a bytarray in the 32pppRGBA-Format
        /// </summary>
        private byte[] GetImageBytes(int w, int h, byte[] data)
        {
            switch (ColorEncoding)
            {
                case ColorFileFormat.Palette:
                    return GetPictureUncompressedByteArray(w, h, data);
                case ColorFileFormat.DXT:
                    DXTDecompression.DXTFlags flag = (AlphaSize > 1) ? ((PreferredFormat == BLPPixelFormat.DXT5) ? DXTDecompression.DXTFlags.DXT5 : DXTDecompression.DXTFlags.DXT3) : DXTDecompression.DXTFlags.DXT1;
                    return DXTDecompression.DecompressImage(w, h, data, flag);
                case ColorFileFormat.ARGB8888:
                    return data;
                default:
                    return new byte[0];
            }
        }

        /// <summary>
        /// Extracts the palettized Image-Data from the given Mipmap and returns a byte-Array in the 32Bit RGBA-Format
        /// </summary>
        /// <param name="mipmap">The desired Mipmap-Level. If the given level is invalid, the smallest available level is choosen</param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="data"></param>
        /// <returns>Pixel-data</returns>
        private byte[] GetPictureUncompressedByteArray(int w, int h, byte[] data)
        {
            int length = w * h;
            byte[] pic = new byte[length * 4];
            for (int i = 0; i < length; i++)
            {
                pic[i * 4] = _paletteBGRA[data[i]].r;
                pic[i * 4 + 1] = _paletteBGRA[data[i]].g;
                pic[i * 4 + 2] = _paletteBGRA[data[i]].b;
                pic[i * 4 + 3] = GetAlpha(data, i, length);
            }
            return pic;
        }

        private byte GetAlpha(byte[] data, int index, int alphaStart)
        {
            byte b;
            switch (AlphaSize)
            {
                case 1:
                    b = data[alphaStart + (index / 8)];
                    return (byte)((b & (0x01 << (index % 8))) == 0 ? 0x00 : 0xFF);
                case 4:
                    b = data[alphaStart + (index / 2)];
                    return (byte)(index % 2 == 0 ? (b & 0x0F) << 4 : b & 0xF0);
                case 8:
                    return data[alphaStart + index];
                default:
                    return 0xFF;
            }
        }

        /// <summary>
        /// Returns the raw Mipmap-Image Data. This data can either be compressed or uncompressed, depending on the Header-Data
        /// </summary>
        /// <param name="mipmapLevel"></param>
        /// <returns></returns>
        private byte[] GetPictureData(int mipmapLevel)
        {
            if (_buffer.Length > 0)
            {
                byte[] data = new byte[MipSizes[mipmapLevel]];
                Buffer.BlockCopy(_buffer, (int)MipOffsets[mipmapLevel], data, 0, data.Length);
                return data;
            }

            return new byte[0];
        }

        private void ConvertToBGRA(byte[] pixel)
        {
            byte tmp;
            for (int i = 0; i < pixel.Length; i += 4)
            {
                tmp = pixel[i]; // store red
                pixel[i] = pixel[i + 2]; // Write blue into red
                pixel[i + 2] = tmp; // write stored red into blue
            }
        }

        public void Dispose()
        {
            Array.Resize(ref _buffer, 0);
            Array.Resize(ref _paletteBGRA, 0);
        }
    }

    public enum BLPPixelFormat : byte
    {
        DXT1,
        DXT3,
        ARGB8888,
        ARGB1555,
        ARGB4444,
        RGB565,
        A8,
        DXT5,
        Unspecified,
        ARGB2565
    }

    public enum ColorFileFormat : byte
    {
        JPEG,
        Palette,
        DXT,
        ARGB8888,
        Unknown_0x4
    }
}
