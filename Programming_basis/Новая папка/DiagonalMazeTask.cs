namespace Mazes
{
	public static class DiagonalMazeTask
	{
        static void MoveRight(Robot robot, int stepCount) {
            for (var i = 1; i < stepCount + 1; i++) {
                robot.MoveTo(Direction.Right);
            } 
        }
		
        static void MoveDown(Robot robot, int stepCount) {
            for (var i = 1; i < stepCount + 1; i++) {
                robot.MoveTo(Direction.Down);
            } 
        }
            
        static void MoveToDestination(Robot robot, int width, int height) {
            while (!(robot.Finished)) {
                if (height > width) {
                    MoveDown(robot, (height - 3) / (width - 2));
                    if (robot.X != width - 2) MoveRight(robot, 1);
                } else {
                    MoveRight(robot, (width - 3) / (height - 2));
                    if (robot.X != width - 2) MoveDown(robot, 1);
                }
            }
        }

		public static void MoveOut(Robot robot, int width, int height)
		{
            MoveToDestination(robot, width, height);
		}
	}
}