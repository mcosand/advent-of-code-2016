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
      var input = File.ReadAllText(args.Length > 0 ? args[0] : "input.txt");
      var parts = input.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

      int x = 0;
      int y = 0;

      int direction = 0;

      HashSet<string> stops = new HashSet<string> { "0, 0" };
      List<int> firstDupe = null;
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

        string stop = "";
        for (int j=0; j < length; j++)
        {
          x += multipliers[direction][0];
          y += multipliers[direction][1];
          stop = string.Format("{0}, {1}", x, y);

          if (firstDupe == null && stops.Contains(stop))
          {
            Console.WriteLine("Found HQ");
            firstDupe = new List<int> { x, y };
          }
          else
          {
            stops.Add(stop);
          }
        }

        Console.WriteLine("{0}: {1} ({2})", parts[i], direction, stop);

      }

      Console.WriteLine("({0}, {1}) ==> {2}", firstDupe[0], firstDupe[1], Math.Abs(firstDupe[0]) + Math.Abs(firstDupe[1]));
    }
  }
}
