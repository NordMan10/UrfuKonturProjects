namespace Mazes
{
	public static class EmptyMazeTask
	{
		static void MoveRight(Robot robot, int width) {
            for (var i = 1; i < width - 2; i++) {
                robot.MoveTo(Direction.Right);
            } 
        }
		
        static void MoveDown(Robot robot, int height) {
            for (var i = 1; i < height - 2; i++) {
                robot.MoveTo(Direction.Down);
            } 
        }
		
		public static void MoveOut(Robot robot, int width, int height)
		{
            MoveRight(robot, width);
            MoveDown(robot, height);
		}
	}
}