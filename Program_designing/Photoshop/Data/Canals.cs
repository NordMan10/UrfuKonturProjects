

namespace MyPhotoshop
{
    public class Canals
    {
        public Canals(double red, double green, double blue)
        {
            R = red;
            G = green;
            B = blue;
        }

        public double R { get; set; }

        public double G { get; set; }

        public double B { get; set; }

        public void SetValues(double red, double green, double blue)
        {
            R = red;
            G = green;
            B = blue;
        }
    }
}
