using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day_24
{
  class Program
  {
    class Point
    {
      public Point(int id, uint x, uint y) { Id = id; X = x; Y = y; }
      public int Id { get; set; }
      public uint X { get; set; }
      public uint Y { get; set; }
      public Dictionary<int, int> Distances { get; set; } = new Dictionary<int, int>();
    }

    static List<bool[]> space = new List<bool[]>();
    static List<Point> coords = new List<Point>();


    static void Main(string[] args)
    {


      int y = 0;
      Console.SetWindowSize(185, 50);
      foreach (var line in File.ReadAllLines("input.txt"))
      {
        Console.WriteLine(line);
        bool[] b = new bool[line.Length];

        for (int i=0;i<line.Length;i++)
        {
          b[i] = false;
          char c = line[i];
          if (c >= '0' && c <= '9')
          {
            coords.Add(new Point((int)(c - '0'), (uint)i, (uint)y));
          }
          else if (c == '#')
          {
            b[i] = true;
          }
        }
        space.Add(b);
        y++;
      }


      Console.Clear();
      Console.ForegroundColor = ConsoleColor.White;
      Console.SetCursorPosition(0, 0);

      for (int fromId=0;fromId<coords.Count;fromId++)
      {  
        for (int toId=0;toId<fromId;toId++)
        {
          var distance = new PathFinder(coords.Single(f => f.Id == fromId), coords.Single(f => f.Id == toId)).GetDistance();
          coords.Single(f => f.Id == fromId).Distances.Add(toId, distance);
          Console.WriteLine("{0} -> {1}: {2}", fromId, toId, distance);

          if (fromId != toId)
          {
            Console.WriteLine("{0} -> {1}: {2}", toId, fromId, distance);

            coords.Single(f => f.Id == toId).Distances.Add(fromId, distance);
          }
        }
      }

      int answer = new RouteFinder(coords).BestRoute();
      Console.WriteLine(answer);
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
        Console.WriteLine(nextState);
        steps.Enqueue(nextState);
      }
    }

    class PathFinder
    {
      public Point from;
      public Point to;
      int[][] spaces = new int[space[0].Length][];

      public PathFinder(Point from, Point to)
      {
        this.from = from;
        this.to = to;
        _goalX = to.X;
        _goalY = to.Y;
      }
      private readonly uint _goalX;
      private readonly uint _goalY;
      private int bestPath = int.MaxValue;
      public int GetDistance()
      {
     //   Console.Clear();
        Expand(from.X, from.Y, 0);
        return bestPath;
      }

      private void Expand(uint x, uint y, int distance)
      {
        if (distance > bestPath) return;

        //Thread.Sleep(10); // to make it pretty.

   //     Console.SetCursorPosition((int)x, (int)y);
   //     Console.ForegroundColor = (ConsoleColor)(((distance / 10 + 2) % 15) + 1);
        if (x == _goalX && y == _goalY)
        {
          if (distance <= bestPath) bestPath = distance;
          return;
        }

        if (spaces[x] == null) spaces[x] = new int[space.Count];

        if (space[(int)y][(int)x])
        {
          spaces[x][y] = -1;
   //       Console.Write('#');
          return;
        }
        if (spaces[x][y] != 0 && distance >= spaces[x][y])
          return;


        spaces[x][y] = distance;
   //     Console.Write(distance % 10);

        if (x > 0) Expand(x - 1, y, distance + 1);
        if (y > 0) Expand(x, y - 1, distance + 1);
        if (x < space[0].Length - 2) Expand(x + 1, y, distance + 1);
        if (y < space.Count - 2) Expand(x, y + 1, distance + 1);
      }

    }
  }
}
