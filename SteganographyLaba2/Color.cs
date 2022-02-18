using System;
using System.Collections.Generic;
using System.Text;

namespace SteganographyLaba2
{
    public class Color
    {
        //public enum Colors : short
        //{
        //    Red,
        //    Blue,
        //    Green
        //}
        public byte red;
        public byte green;
        public byte blue;
        public Color(byte r, byte g, byte b)
        {
            red = r;
            green = g;
            blue = b;
        }
    }
    
}
