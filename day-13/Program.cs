using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace day_13
{
  class Program
  {
    private int[][] spaces = new int[1000][];
    

    private readonly uint _seed = 1364;
    private readonly uint _goalX = 31;
    private readonly uint _goalY = 39;
    private int bestPath = 50 + 1;
    private int count = 0;

    static void Main(string[] args)
    {
      new Program().Run();      
    }

    private void Run()
    {
      var foreColor = Console.ForegroundColor;
      Expand(1, 1, 1);
      Console.ForegroundColor = foreColor;
      Console.WriteLine();
      Console.WriteLine();
      Console.WriteLine("Spots: " + count.ToString());
    }

    private void Expand(uint x, uint y, int distance)
    {
      if (distance > bestPath) return;

      //Thread.Sleep(10); // to make it pretty.

      Console.SetCursorPosition((int)x, (int)y);
      Console.ForegroundColor = (ConsoleColor)(((distance / 10 + 2) % 15) + 1) ;
      if (x == _goalX && y == _goalY)
      {
       // if (distance <= bestPath) bestPath = distance - 1;
       // return;
      }

      if (spaces[x] == null) spaces[x] = new int[1000];

      if (NumberOfSetBits((x * x + 3 * x + 2 * x * y + y + y * y) +_seed) % 2 == 1 && !(x == 1 && y == 1))
      {
        spaces[x][y] = -1;
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.Write('#');
        return;
      }
      if (spaces[x][y] != 0 && distance >= spaces[x][y])
        return;

      if (spaces[x][y] == 0) count++;
      spaces[x][y] = distance;
      Console.Write(distance % 10);

      if (x > 0) Expand(x - 1, y, distance + 1);
      if (y > 0) Expand(x, y - 1, distance + 1);
      Expand(x + 1, y, distance + 1);
      Expand(x, y + 1, distance + 1);
    }

    int NumberOfSetBits(uint i)
    {
      uint q = i;
      int count = 0;
      for (int x=0;x<32;x++)
      {
        if (i % 2 == 1) count++;
        i = i / 2;
      }
      return count;
    }
    
  }
}
