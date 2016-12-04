using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day_03
{
  class Program
  {
    static void Main(string[] args)
    {
      var groups = File.ReadAllLines(args.Length > 0 ? args[0] : "input.txt");
      var possible = 0;
      for (var i = 0; i < groups.Length; i += 3)
      {
        var set = new[]
        {
          ReadLine(groups[i]),
          ReadLine(groups[i + 1]),
          ReadLine(groups[i + 2])
        };

        for (var j = 0; j < 3; j++)
          possible += TestGroup(possible, set, j);

      }

      Console.WriteLine(possible);
    }

    private static int TestGroup(int possible, int[][] set, int j)
    {
      var sides = new[] { set[0][j], set[1][j], set[2][j] }.OrderBy(f => f).ToArray();
      Console.WriteLine(string.Format("{0}, {1} {2}", sides[0], sides[1], sides[2]));
      return (sides[0] >= 0 && sides[0] + sides[1] > sides[2]) ? 1 : 0;
    }

    private static int[] ReadLine(string group)
    {
      return group.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
        .Select(f => int.Parse(f))
        .ToArray();
    }
  }
}
