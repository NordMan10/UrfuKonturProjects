using System;

namespace Digger
{
    class Terrain : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            return new CreatureCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return conflictedObject is Player; 
        }

        public int GetDrawingPriority()
        {
            return 10;
        }

        public string GetImageFileName()
        {
            return "Terrain.png";
        }
    }

    class Player : ICreature
    {
        public int CheckMovePlayer(char dir, int shift, int x, int y)
        {
            if (dir == 'x')
                if (x + shift >= 0 && x + shift < Game.MapWidth)
                    if (Game.Map[x + shift, y] is Sack) return 0;
                    else return shift;
            if (dir == 'y')
                if (y + shift >= 0 && y + shift < Game.MapHeight)
                    if (Game.Map[x, y + shift] is Sack) return 0;
                    else return shift;
            return 0;
        }

        public CreatureCommand Act(int x, int y)
        {
            var keyPressed = Game.KeyPressed;
            if (keyPressed == System.Windows.Forms.Keys.Left)
                return new CreatureCommand { DeltaX = CheckMovePlayer('x', -1, x, y) };
            else if (keyPressed == System.Windows.Forms.Keys.Right)
                return new CreatureCommand { DeltaX = CheckMovePlayer('x', 1, x, y) };
            else if (keyPressed == System.Windows.Forms.Keys.Up)
                return new CreatureCommand { DeltaY = CheckMovePlayer('y', -1, x, y) };
            else if (keyPressed == System.Windows.Forms.Keys.Down)
                return new CreatureCommand { DeltaY = CheckMovePlayer('y', 1, x, y) };
            return new CreatureCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if (conflictedObject is Sack|| conflictedObject is Monster)
            {
                Game.IsOver = true;
                return true;
            }
            return false;
        }

        public int GetDrawingPriority()
        {
            return 5;
        }

        public string GetImageFileName()
        {
            return "Digger.png";
        }
    }

    class Sack : ICreature
    {
        public static bool Flag = false;
        public int CountDown = 0;

        public bool CheckMoveSack(int x, int y)
        {
            if (y + 1 < Game.MapHeight)
            {
                return (Game.Map[x, y + 1] == null) || ((Game.Map[x, y + 1] is Player || Game.Map[x, y + 1] is Monster) && CountDown >= 1);
            }
            else return false;
        }

        public CreatureCommand Act(int x, int y)
        {
            ICreature transform = new Sack();
            if (CheckMoveSack(x, y))
            {
                CountDown++;
                return new CreatureCommand { DeltaY = 1 };
            }
            else
            {
                if (CountDown > 1)
                {
                    return new CreatureCommand { TransformTo = new Gold() };
                }
                else
                {
                    CountDown = 0;
                    return new CreatureCommand();
                }
            }
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return false;
        } 

        public int GetDrawingPriority()
        {
            return 3;
        }

        public string GetImageFileName()
        {
            return "Sack.png";
        }
    }

    class Gold : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            return new CreatureCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if (conflictedObject is Player)
            {
                Game.Scores += 10;
                return true;
            }
            if (conflictedObject is Monster) return true;
            return false;
        }

        public int GetDrawingPriority()
        {
            return 2;
        }

        public string GetImageFileName()
        {
            return "Gold.png";
        }
    }

    class Monster : ICreature
    {
        public bool CheckMoveMonster(char dir, int offset, int x, int y)
        {
            if (dir == 'x')
                if (x + offset >= 0 && x + offset < Game.MapWidth)
                    if (Game.Map[x + offset, y] == null || Game.Map[x + offset, y] is Player || Game.Map[x + offset, y] is Gold) return true;
                    else return false;
            if (dir == 'y')
                if (y + offset >= 0 && y + offset < Game.MapHeight)
                    if (Game.Map[x, y + offset] == null || Game.Map[x, y + offset] is Player || Game.Map[x + offset, y] is Gold) return true;
                    else return false;
            return false;
        }

        public int[] CheckPlayerLocation(int x, int y)
        {
            for (var i = 0; i < Game.MapWidth; i++)
                for (var j = 0; j < Game.MapHeight; j++)
                {
                    if (Game.Map[i, j] is Player) return new int[] { i, j };
                }
            return null;
        }

        public int[] GetMonsterOffset(int playerX, int playerY, int monsterX, int monsterY)
        {
            var monsterOffset = new int[2];
            if (playerX - monsterX < 0) monsterOffset[0] = -1;
            else if (playerX - monsterX > 0) monsterOffset[0] = 1;
            else monsterOffset[0] = 0;
            if (playerY - monsterY < 0) monsterOffset[1] = -1;
            else if (playerY - monsterY > 0) monsterOffset[1] = 1;
            else monsterOffset[1] = 0;
            return null;
        }

        public CreatureCommand Act(int x, int y)
        {
            var playerLocation = CheckPlayerLocation(x, y);
            if (playerLocation != null) 
            {
                var offsetX = GetMonsterOffset(playerLocation[0], playerLocation[1], x, y)[0];
                var offsetY = GetMonsterOffset(playerLocation[0], playerLocation[1], x, y)[1];
                if (CheckMoveMonster('x', offsetX, x, y)) 
                    return new CreatureCommand { DeltaX = offsetX };
                if (CheckMoveMonster('y', offsetY, x, y))
                    return new CreatureCommand { DeltaY = offsetY };
            }
            return new CreatureCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if (conflictedObject is Monster || conflictedObject is Sack) return true;
            return false;
        }

        public int GetDrawingPriority()
        {
            return 4;
        }

        public string GetImageFileName()
        {
            return "Monster.png";
        }
    }
}
