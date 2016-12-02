using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day_1a
{
  class Program
  {
    static void Main(string[] args)
    {
      var input = File.ReadAllText("input.txt");
      var parts = input.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

      int x = 0;
      int y = 0;

      int direction = 0;


      /*
       0 : x + 0,  y + 1
       1 : x + 1, y + 0
       2 : x + 0, y - 1
       3 : x - 1, y + 0
       */
      int[][] multipliers = new[] { new[] { 0, 1 }, new[] { 1, 0 }, new[] { 0, -1 }, new[] { -1, 0 } };

      for (var i = 0; i < parts.Length; i++)
      {
        direction = (direction + 4 + (parts[i][0] == 'L' ? -1 : 1)) % 4;
        int length = int.Parse(parts[i].Substring(1));

        x += multipliers[direction][0] * length;
        y += multipliers[direction][1] * length;

        Console.WriteLine("{0}: {1} ({2}, {3})", parts[i], direction, x, y);
      }

      Console.WriteLine("({0}, {1}) ==> {2}", x, y, x + y);
    }
  }
}
