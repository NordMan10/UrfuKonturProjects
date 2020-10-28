namespace Mazes
{
	public static class SnakeMazeTask
	{
        static void MoveLeft(Robot robot, int width) {
            for (var i = width - 2; i > 1; i--) {
                robot.MoveTo(Direction.Left);
            }
        }

        static void MoveRight(Robot robot, int width) {
            for (var i = 1; i < width - 2; i++) {
                robot.MoveTo(Direction.Right);
            } 
        }
        static void MoveDown(Robot robot) {
            for (var i = 1; i < 3; i++) {
                robot.MoveTo(Direction.Down);
            } 
        }

        static void MoveToDestination(Robot robot, int width, int height) {
            while (!(robot.Finished)) { 
                MoveRight(robot, width);
                MoveDown(robot);
                MoveLeft(robot, width);
                if (robot.Y != height - 2) MoveDown(robot);
            }
        }

		public static void MoveOut(Robot robot, int width, int height)
		{
            MoveToDestination(robot, width, height);
            
		}

	}
}