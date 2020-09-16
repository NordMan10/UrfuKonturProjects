using System;

namespace MyPhotoshop
{
	public class Photo
	{
		public Photo(int width_, int height_)
        {
			width = width_;
			height = height_;
			InitData();
        }

		public int width;
		public int height;
		public Pixel[,] data;

		public void InitData()
        {
			data = new Pixel[width, height];
			for (var x = 0; x < data.GetLength(0); x++)
				for (var y = 0; y < data.GetLength(1); y++)
                {
					data[x, y] = new Pixel();
                }
        }
	}
}

