using System;

namespace PhotoshopWithoutReflection
{
	public class Photo
	{
		public Photo(int width_, int height_)
        {
			Width = width_;
			Height = height_;
			data = new Pixel[Width, Height];
        }

		public int Width { get; }

		public int Height { get; }

		private Pixel[,] data;


		public Pixel this[int x, int y]
        {
			get { return data[x, y]; }
			set { data[x, y] = value; }
        }
	}
}

