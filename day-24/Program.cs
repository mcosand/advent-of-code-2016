using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace day_24
{
  class Program
  {
    static bool showWork = true;

    class Point
    {
      public Point(int id, int x, int y) { Id = id; X = x; Y = y; }
      public int Id { get; set; }
      public int X { get; set; }
      public int Y { get; set; }
      public Dictionary<int, uint> Distances { get; set; } = new Dictionary<int, uint>();

      public override string ToString()
      {
        return string.Format("{0}: {1},{2}", Id, X, Y);
      }
    }

    static List<bool[]> space = new List<bool[]>();
    static List<Point> coords = new List<Point>();


    static void Main(string[] args)
    {


      int y = 0;
      if (showWork) Console.SetWindowSize(185, 50);
      foreach (var line in File.ReadAllLines("input.txt"))
      {
        if (showWork) Console.WriteLine(line);
        bool[] b = new bool[line.Length];

        for (int i = 0; i < line.Length; i++)
        {
          b[i] = false;
          char c = line[i];
          if (c >= '0' && c <= '9')
          {
            coords.Add(new Point((int)(c - '0'), (int)i, (int)y));
          }
          else if (c == '#')
          {
            b[i] = true;
          }
        }
        space.Add(b);
        y++;
      }

      coords = coords.OrderBy(f => f.Id).ToList();


      if (showWork) {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.White;
        Console.SetCursorPosition(0, 0);
      }

      for (int fromId=0;fromId<coords.Count;fromId++)
      {
        Point from = coords.Single(f => f.Id == fromId);
        for (int toId = 0; toId < fromId; toId++)
        {
          Point to = coords.Single(f => f.Id == toId);

          if (showWork)
          {
            Thread.Sleep(2000);
            PrintSpace();
            PrintCoordLocation(from);
            PrintCoordLocation(to);
            Console.SetCursorPosition(0, 0);
            Thread.Sleep(1000);
          }

          var finder = new PathFinder2(from, to);
          var distance = finder.GetDistance();
          if (showWork) finder.ReconstructPath();

          coords.Single(f => f.Id == fromId).Distances.Add(toId, distance);
       //   Console.WriteLine("{0} -> {1}: {2}", fromId, toId, distance);

          if (fromId != toId)
          {
        //    Console.WriteLine("{0} -> {1}: {2}", toId, fromId, distance);

            coords.Single(f => f.Id == toId).Distances.Add(fromId, distance);
          }
        }
      }

      Console.WriteLine("Built graph");

      int answer = new RouteFinder(coords).BestRoute();

      Console.WriteLine(answer);
    }

    private static void PrintSpace()
    {
      Console.Clear();
      Console.ForegroundColor = ConsoleColor.Gray;
      foreach (var row in space)
      {
        foreach (var bit in row)
        {
          Console.Write(bit ? '▒' : ' ');
        }
        Console.WriteLine();
      }
    }

    private static void PrintCoordLocation(Point from)
    {
      Console.SetCursorPosition((int)from.X, (int)from.Y);
      Console.ForegroundColor = ConsoleColor.Magenta;
      Console.Write(from.Id);
    }

    class RouteFinder
    {
      Queue<string> queue = new Queue<string>();
      readonly List<Point> coords;

      public RouteFinder(List<Point> coords)
      {
        this.coords = coords;
      }

      public int BestRoute()
      {
        Queue<string> steps = new Queue<string>();
        int shortest = int.MaxValue;
        // 0,3,2,1:4
        steps.Enqueue("0:0");
        while (steps.Count > 0)
        {
          var step = steps.Dequeue();

          var parts = step.Split(':');
          var distance = int.Parse(parts[1]);
          var visited = parts[0].Split(',').Select(f => int.Parse(f)).ToArray();

          Point position = coords.Single(f => f.Id == visited.Last());

          if (visited.Length == coords.Count + 1 && distance < shortest)
          {
            shortest = distance;
            continue;
          } else if (distance > shortest)
          {
            continue;
          }


          if (visited.Length == coords.Count)
          {
            AddStep(steps, parts[0], distance, position, 0);
          }
          else
          {
            for (int nextId = 0; nextId < coords.Count; nextId++)
            {
              if (visited.Contains(nextId)) continue;

              AddStep(steps, parts[0], distance, position, nextId);
            }
          }
        }

        return shortest;
      }

      private static void AddStep(Queue<string> steps, string soFar, int distance, Point position, int nextId)
      {
        string nextState = soFar + "," + nextId + ":" + (distance + position.Distances[nextId]);
        if (showWork) Console.WriteLine(nextState);
        steps.Enqueue(nextState);
      }
    }

    class PathFinder2 : AStarFinder
    {
      public PathFinder2(Point start, Point goal) : base(start, goal) { }

    }

    class AStarFinder : PathFinder
    {
      List<Point> closedSet = new List<Point>();
      readonly List<Point> openSet;
      Dictionary<string, Point> lookup = new Dictionary<string, Point>();
      Dictionary<Point, Point> cameFrom = new Dictionary<Point, Point>();
      Dictionary<Point, uint> gScore = new Dictionary<Point, uint>();
      Dictionary<Point, uint> fScore = new Dictionary<Point, uint>();

      public AStarFinder(Point start, Point goal) : base(start, goal)
      {
        openSet = new List<Point> { base.start };
      }

      private string GetKey(Point node) { return string.Format("{0},{1}", node.X, node.Y); }

      protected virtual uint GuessDistance(Point from, Point to)
      {
        int diffX = from.X - to.X;
        int diffY = from.Y - to.Y;

        return (uint)Math.Ceiling(Math.Sqrt(diffX * diffX + diffY * diffY));
      }

      public override uint GetDistance()
      {
        lookup.Add(GetKey(start), start);
        lookup.Add(GetKey(goal), goal);

        gScore.Add(start, 0);

        fScore.Add(start, GuessDistance(start, goal));

        while (openSet.Count > 0)
        {
          Point current = openSet.OrderBy(f => fScore.ContainsKey(f)).ThenBy(f => fScore[f]).First();

          //      Console.WriteLine(string.Format("{0},{1}", current.X, current.Y));

          if (current == goal)
            return gScore[current];

          openSet.Remove(current);
          closedSet.Add(current);
          if (current != start && current != goal)PutChar(current.X, current.Y, ConsoleColor.White, '∙');

          foreach (var neighbor in GetNeighbors(current))
          {
            UpdateForNeigbor(current, neighbor);
          }

        }
        return int.MaxValue;
      }

      protected virtual IEnumerable<Point> GetNeighbors(Point current)
      {
        if (current.X > 0 && !space[current.Y][current.X - 1]) yield return GetOrCreateNeighbor(lookup, current.X - 1, current.Y);
        if (current.Y > 0 && !space[current.Y - 1][current.X]) yield return GetOrCreateNeighbor(lookup, current.X, current.Y - 1);
        if (current.X < space[0].Length && !space[current.Y][current.X + 1]) yield return GetOrCreateNeighbor(lookup, current.X + 1, current.Y);
        if (current.Y < space.Count && !space[current.Y + 1][current.X]) yield return GetOrCreateNeighbor(lookup, current.X, current.Y + 1);
      }

      private bool UpdateForNeigbor(Point current, Point neighbor)
      {
        if (closedSet.Contains(neighbor))
          return true;

        uint tentativeScore = GetScore(gScore, current) + DistanceFromNeighbor(current, neighbor);
        if (!openSet.Contains(neighbor))
        {
          if (showWork && neighbor != goal && neighbor != start) PutChar(neighbor.X, neighbor.Y, ConsoleColor.Cyan, '░');
          openSet.Add(neighbor);
        }
        else if (tentativeScore >= gScore[neighbor])
          return true;

        cameFrom[neighbor] = current;
        gScore[neighbor] = tentativeScore;
        fScore[neighbor] = GetScore(gScore, neighbor) + GuessDistance(neighbor, goal);
        return false;
      }

      protected virtual uint DistanceFromNeighbor(Point current, Point neighbor)
      {
        return 1;
      }

      private uint GetScore(Dictionary<Point, uint> list, Point node)
      {
        uint score;
        if (!list.TryGetValue(node, out score))
        {
          score = uint.MaxValue;
          list.Add(node, score);
        }
        return score;
      }

      private static void PutChar(int x, int y, ConsoleColor color, char c)
      {
        if (showWork)
        {
          Console.SetCursorPosition(x, y);
          Console.ForegroundColor = color;
          Console.Write(c);
        }
      }

      private static Point GetOrCreateNeighbor(Dictionary<string, Point> lookup, int nextX, int nextY)
      {
        Point neighbor;
        string key = string.Format("{0},{1}", nextX, nextY);
        if (!lookup.TryGetValue(key, out neighbor))
        {
          neighbor = new Point(0, nextX, nextY);
          lookup.Add(key, neighbor);
        }
        return neighbor;
      }



      public List<Point> ReconstructPath()
      {
        List<Point> path = new List<Point> { goal };
        Point current = goal;
        while (cameFrom.TryGetValue(current, out current))
        {
          if (showWork && current != goal && current != start)
          {
            Thread.Sleep(2);
            PutChar(current.X, current.Y, ConsoleColor.Yellow, '█');
          }
          path.Insert(0, current);
        }
        return path;
      }
    }

    abstract class PathFinder
    {
      protected Point start;
      protected Point goal;

      protected PathFinder(Point from, Point to)
      {
        this.start = from;
        this.goal = to;
        _goalX = to.X;
        _goalY = to.Y;
      }
      protected readonly int _goalX;
      protected readonly int _goalY;

      public abstract uint GetDistance();
    }

    class PathFinder1 : PathFinder
    {
      uint[][] spaces = new uint[space[0].Length][];
      protected uint bestPath = int.MaxValue;

      public PathFinder1(Point from, Point to) : base(from, to)
      {
      }
      public override uint GetDistance()
      {
        Expand(start.X, start.Y, 0);
        return bestPath;
      }

      private void Expand(int x, int y, uint distance)
      {
        Console.SetCursorPosition((int)x, (int)y);

        if (distance > bestPath)
        {
          //Console.ForegroundColor = ConsoleColor.Yellow;
          //Console.Write('█');
          return;
        }
        //Thread.Sleep(10); // to make it pretty.

        //Console.ForegroundColor = (ConsoleColor)(((distance / 50 + 2) % 5) + 8);
        if (x == _goalX && y == _goalY)
        {
          if (distance < bestPath)
            bestPath = distance;
          return;
        }

        if (spaces[x] == null) spaces[x] = new uint[space.Count];

        if (space[(int)y][(int)x])
        {
          spaces[x][y] = uint.MaxValue;
          //Console.Write('#');
          return;
        }
        if (spaces[x][y] != 0 && distance >= spaces[x][y])
          return;


        spaces[x][y] = distance;
        if (distance > 0) Console.Write(distance % 10);

        int[] plan;
        if (_goalY - y < 0 && _goalX - x < 0) plan = new int[] { 1, 2, 0, 3 };
        else if (_goalY - y < 0) plan = new int[] { 0, 1, 2, 3 };
        else if (_goalX - x < 0) plan = new int[] { 2, 3, 0, 1 };
        else plan = new int[] { 0, 3, 1, 2 };

        Action[] funcs = new[]
        {
          (Action)(() => { if (x < space[0].Length - 2) Expand(x + 1, y, distance + 1); }), // right
                   () => { if (y > 0) Expand(x, y - 1, distance + 1); }, // up
                   () => { if (x > 0) Expand(x - 1, y, distance + 1); }, // left
                   () => { if (y < space.Count - 2) Expand(x, y + 1, distance + 1); } // down
        };


        int offset = 0;
        //double bearing = Math.Atan((double)(y - _goalY) / (_goalX - x)) * 180 / Math.PI;
        //int offset =(int)(Math.Round((bearing - 45) / 90));
        //offset += 0;
        for (int i=0;i<funcs.Length;i++)
        {
          funcs[plan[i]]();
          //funcs[(i+offset) % funcs.Length]();
        }
      }

      private void TryLeft(int x, int y, int distance)
      {
        
      }
    }
  }
}
