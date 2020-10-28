using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Dungeon
{
    public static class MapExtensions
    {
        public static IEnumerable<Point> GetNeighboursOf(this Map map, Point point, int maxWidth, int maxHeight)
        {
            for (var dx = -1; dx < 2; dx++)
                for (var dy = -1; dy < 2; dy++)
                    if (dx != 0 && dy != 0) continue;
                    else yield return new Point(point.X + dx, point.Y + dy);
        }
    }

    public class BfsTask
    {
        public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Point[] chests)
        {
            var listsQueue = new Queue<SinglyLinkedList<Point>>();
            var visitedPoints = new HashSet<Point>();
            listsQueue.Enqueue(new SinglyLinkedList<Point>(start, null));
            var maxWidth = map.Dungeon.GetLength(0);
            var maxHeight = map.Dungeon.GetLength(1);
            visitedPoints.Add(start);

            while (listsQueue.Count > 0)
            {
                var points = listsQueue.Dequeue();

                if (points.Value.X < 0 || points.Value.X >= maxWidth 
                    || points.Value.Y < 0 || points.Value.Y >= maxHeight) continue;
                if (map.Dungeon[points.Value.X, points.Value.Y] == MapCell.Wall) continue;
                if (chests.Contains(points.Value)) yield return points;

                foreach (var neighbour in map.GetNeighboursOf(points.Value, maxWidth, maxHeight))
                {
                    if (!visitedPoints.Contains(neighbour))
                        listsQueue.Enqueue(new SinglyLinkedList<Point>(neighbour, points));
                    visitedPoints.Add(neighbour);
                }
            }
            yield break;    
        }
    }
}