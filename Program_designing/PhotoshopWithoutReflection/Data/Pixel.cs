using System;

namespace PhotoshopWithoutReflection
{
    public struct Pixel
    {
        public Pixel(double red, double green, double blue)
        {
            r = g = b = 0;
            R = red;
            G = green;
            B = blue;
        }

        public static double Trim(double value)
        {
            if (value < 0) return 0;
            if (value > 1) return 1;
            return value;
        }


        private double r;
        public double R
        {
            get { return r; }
            set { r = GetCheckResult(value, 1); }
        }


        private double g;
        public double G
        {
            get { return g; }
            set { g = GetCheckResult(value, 1); }
        }

        private double b;
        public double B
        {
            get { return b; }
            set { b = GetCheckResult(value, 1); }
        }

        private bool IsValueMoreThan(double value, double border)
        {
            return value > border;
        }

        private double GetCheckResult(double value, double border)
        {
            if (IsValueMoreThan(value, border)) throw new ArgumentException();
            return value;
        }

        public static Pixel operator *(Pixel pixel, double value)
        {
            return new Pixel(Trim(pixel.R * value),
                Trim(pixel.G * value), Trim(pixel.B * value));
        }

        public static Pixel operator *(double value, Pixel pixel)
        {
            return pixel * value;
        }

        public double Average
        {
            get { return (R + G + B) / 3; }
        }
    }
}
