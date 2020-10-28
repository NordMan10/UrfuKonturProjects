using System;
using System.Linq;
using System.Collections.Generic;

namespace func_rocket
{
    public enum Names
    {
        Zero,
        Heavy,
        Up,
        WhiteHole,
        BlackHole,
        BlackAndWhite
    }

    

    public class LevelsTask
	{
		static readonly Physics standardPhysics = new Physics();
        public static Func<string, Rocket, Gravity, Level> NewLevel = (name, r, g) =>
        {
            return new Level(name, r, new Vector(600, 200), g, standardPhysics);
        };

        private static List<Func<string, Rocket, Gravity, Level>> Levels = new List<Func<string, Rocket, Gravity, Level>>(6);

		public static IEnumerable<Level> CreateLevels()
		{
            
            yield return NewLevel(Names.Zero.ToString(),
                new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI),
                (size, v) => Vector.Zero
            );

            yield return NewLevel(Names.Heavy.ToString(),
                new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI),
                (size, v) => new Vector(0, 0.9)
            );

            yield return NewLevel(Names.Up.ToString(),
                new Rocket(new Vector(700, 500), Vector.Zero, -0.5 * Math.PI),
                (size, v) => new Vector(Math.Cos(v.Angle), Math.Sin(v.Angle)) * 300 / (size.Height - v.Y + 300.0)
            );

            yield return NewLevel(Names.WhiteHole.ToString(),
                new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI),
                (size, v) => new Vector(v.X, v.Y) * 140 * new Vector(v.X, v.Y).Length / (Math.Pow(new Vector(v.X, v.Y).Length, 2) + 1)
            );
            
        }
	}
}