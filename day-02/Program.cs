using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day_2
{
  class Program
  {
    static void Main(string[] args)
    {
      var steps = File.ReadAllLines(args.Length > 0 ? args[0] : "input.txt");

      int x = 1;
      int y = 1;

      StringBuilder code = new StringBuilder();

      for (var i = 0; i < steps.Length; i++)
      {
        for (var j = 0; j < steps[i].Length; j++)
        {
          switch (steps[i][j])
          {
            case 'U':
              y = Math.Max(0, y - 1);
              break;
            case 'D':
              y = Math.Min(2, y + 1);
              break;
            case 'L':
              x = Math.Max(0, x - 1);
              break;
            case 'R':
              x = Math.Min(2, x + 1);
              break;
          }
        }

        Console.WriteLine("{0},{1} : {2}", x, y, y * 3 + x + 1);
        code.Append(y * 3 + x + 1);
      }
      Console.WriteLine(code);
    }
  }
}
