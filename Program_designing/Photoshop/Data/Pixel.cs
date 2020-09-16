using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPhotoshop.Data
{
    public class Pixel
    {
        public Pixel(double x, double y)
        {
            X = x;
            Y = y;
            Canals = new Canals(0, 0, 0);
        }

        public Pixel(double x, double y, double red, double green, double blue)
        {
            X = x;
            Y = y;
            Canals = new Canals(red, green, blue);
        }

        public double X { get; }

        public double Y { get; }

        public Canals Canals { get; set; }


    }
}
