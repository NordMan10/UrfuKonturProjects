using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using PhotoshopWithoutReflection.Filters.Transform;
using PhotoshopWithoutReflection.Filters;


namespace PhotoshopWithoutReflection
{
    static class MainClass
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            var window = new MainWindow();
            window.AddFilter(new PixelFilter<LightningParameters>(
                "1Осветление/затемнение",
                (pixel, parameters) => pixel * parameters.Coefficient
                ));

            window.AddFilter(new PixelFilter<EmptyParameters>(
                "Оттенки серого",
                (original, parameters) =>
                {
                    var lightness = original.R + original.G + original.B;
                    lightness /= 3;
                    return new Pixel(lightness, lightness, lightness);
                }
                ));

            window.AddFilter(new TransformFilter(
                "Отразить по горизонтали",
                (size) => size,
                (point, size) => new Point(size.Width - point.X - 1, point.Y)
                ));

            window.AddFilter(new TransformFilter(
                "Повернуть против ч.с",
                (size) => new Size(size.Height, size.Width),
                (point, size) => new Point(size.Width - point.Y - 1, point.X)
                ));

            window.AddFilter(new TransformFilter<RotationParameters>(
                "Свободное вращение", new RotateTransformer()
                ));

            Application.Run(window);
        }
    }
}
