using MyPhotoshop.Filters;
using System;
using System.Windows.Forms;
using System.Drawing;
using MyPhotoshop.Filters.Transform;

namespace MyPhotoshop
{
	class MainClass
	{
        [STAThread]
		public static void Main (string[] args)
		{
			var window = new MainWindow();
			window.AddFilter(new PixelFilter<LightningParameters>(
				"����������/����������",
				(pixel, parameters) => pixel * parameters.Coefficient
				));

			window.AddFilter(new PixelFilter<EmptyParameters>(
				"������� ������",
				(original, parameters) =>
                {
					var lightness = original.R + original.G + original.B;
					lightness /= 3;
					return new Pixel(lightness, lightness, lightness);
				}
				));

            window.AddFilter(new TransformFilter(
                "�������� �� �����������",
                (size) => size,
                (point, size) => new Point(size.Width - point.X - 1, point.Y)
                ));

            window.AddFilter(new TransformFilter(
                "��������� ������ �.�",
                (size) => new Size(size.Height, size.Width),
                (point, size) => new Point(size.Width - point.Y - 1, point.X)
                ));

            window.AddFilter(new TransformFilter<RotationParameters>(
				"��������� ��������", new RotateTransformer()
				));

			Application.Run(window);
		}
	}
}
