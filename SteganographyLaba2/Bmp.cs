using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SteganographyLaba2
{
    class Bmp
    {
        public BITMAPFILEHEADER fh;
        public BITMAPINFOHEADER ih;
        string path;
        public Color[,] image { get; private set; }
        public Bmp(string fileName)
        {
            path = fileName;
            fh = new BITMAPFILEHEADER(path);
            ih = new BITMAPINFOHEADER(path);
            image = new Color[ih.biHeight,ih.biWidth];
        }

        public Bmp(Bmp bmp, string pathImage)
        {
            fh = new BITMAPFILEHEADER(bmp.fh);
            ih = new BITMAPINFOHEADER(bmp.ih);
            image = new Color[ih.biHeight, ih.biWidth];
            path = pathImage;
        }

        public void ImageRead()
        {
            using(BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                reader.BaseStream.Seek(fh.bfOffBits,SeekOrigin.Begin);
                for (uint y = 0; y< ih.biHeight; y++)
                {
                    for (int x = 0; x < ih.biWidth; x++)
                    {
                        byte b = reader.ReadByte();
                        byte g = reader.ReadByte();
                        byte r = reader.ReadByte();
                        image[y, x] = new Color(r,g,b);
                    }
                }
            }
        }
        public void ImageWrite( string outpath)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(outpath, FileMode.Create)))
            {
                WriteHeader(writer);
                writer.Seek((int)fh.bfOffBits, SeekOrigin.Begin);
                for (uint y = 0; y < ih.biHeight; y++)
                {
                    for (int x = 0; x < ih.biWidth; x++)
                    {
                        writer.Write(image[y,x].blue);
                        writer.Write(image[y, x].green);
                        writer.Write(image[y, x].red);
                    }
                }
            }
        }

        private void WriteHeader(BinaryWriter writer)
        {
            writer.Write(fh.bfType);
            writer.Write(fh.bfSize);
            writer.Write(fh.bfReserved1);
            writer.Write(fh.bfReserved2);
            writer.Write(fh.bfOffBits);
            writer.Seek(14, SeekOrigin.Begin);
            writer.Write(ih.biSize);
            writer.Write(ih.biWidth);
            writer.Write(ih.biHeight);
            writer.Write(ih.biPlanes);
            writer.Write(ih.biBitCount);
            writer.Write(ih.biCompression);
            writer.Write(ih.biSizeImage);
            writer.Write(ih.biXPelsPerMeter);
            writer.Write(ih.biYPelsPerMeter);
            writer.Write(ih.biClrUsed);
            writer.Write(ih.biClrImportant);
        }

        public class BITMAPFILEHEADER
        {
            public ushort bfType;
            public uint bfSize;
            public ushort bfReserved1;
            public ushort bfReserved2;
            public uint bfOffBits;

            public BITMAPFILEHEADER(string fileName)
            {
                using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
                {
                    bfType = reader.ReadUInt16();
                    bfSize = reader.ReadUInt32();
                    bfReserved1 = reader.ReadUInt16();
                    bfReserved2 = reader.ReadUInt16();
                    bfOffBits = reader.ReadUInt32();
                }
            }
            public BITMAPFILEHEADER(BITMAPFILEHEADER fh)
            {
                bfType = fh.bfType;
                bfSize = fh.bfSize;
                bfReserved1 = fh.bfReserved1;
                bfReserved2 = fh.bfReserved2;
                bfOffBits = fh.bfOffBits;
            }
        }
        public class BITMAPINFOHEADER
        {
            public uint biSize;
            public uint biWidth;
            public uint biHeight;
            public ushort biPlanes;
            public ushort biBitCount;
            public uint biCompression;
            public uint biSizeImage;
            public uint biXPelsPerMeter;
            public uint biYPelsPerMeter;
            public uint biClrUsed;
            public uint biClrImportant;

            public BITMAPINFOHEADER(string fileName)
            {
                using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
                {
                    reader.BaseStream.Seek(14, SeekOrigin.Begin);
                    biSize = reader.ReadUInt32();
                    biWidth = reader.ReadUInt32();
                    biHeight = reader.ReadUInt32();
                    biPlanes = reader.ReadUInt16();
                    biBitCount = reader.ReadUInt16();
                    biCompression = reader.ReadUInt32();
                    biSizeImage = reader.ReadUInt32();
                    biXPelsPerMeter = reader.ReadUInt32();
                    biYPelsPerMeter = reader.ReadUInt32();
                    biClrUsed = reader.ReadUInt32();
                    biClrImportant = reader.ReadUInt32();
                }
            }
            public BITMAPINFOHEADER(BITMAPINFOHEADER ih)
            {
                biSize = ih.biSize;
                biWidth = ih.biWidth;
                biHeight = ih.biHeight;
                biPlanes = ih.biPlanes;
                biBitCount = ih.biBitCount;
                biCompression = ih.biCompression;
                biSizeImage = ih.biSizeImage;
                biXPelsPerMeter = ih.biXPelsPerMeter;
                biYPelsPerMeter = ih.biYPelsPerMeter;
                biClrUsed = ih.biClrUsed;
                biClrImportant = ih.biClrImportant;
            }
        }
            

    }
}
