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
      var keys = new[] { 'x', 'x', '1', 'x', 'x',
                         'x', '2', '3', '4', 'x',
                         '5', '6', '7', '8', '9',
                         'x', 'A', 'B', 'C', 'x',
                         'x', 'x', 'D', 'x', 'x' };
//    1
//  2 3 4
//5 6 7 8 9  -- put 7 at 0,0 then all keys are 2 or less steps away from origin
//  A B C
//    D

      int x = -2;
      int y = 0;

      StringBuilder code = new StringBuilder();

      for (var i = 0; i < steps.Length; i++)
      {
        for (var j = 0; j < steps[i].Length; j++)
        {
          int newX = x;
          int newY = y;
          switch (steps[i][j])
          {
            case 'U':
              newY = y - 1;
              break;
            case 'D':
              newY = y + 1;
              break;
            case 'L':
              newX = x - 1;
              break;
            case 'R':
              newX = x + 1;
              break;
          }
        //  Console.WriteLine(Math.Abs(newX) + Math.Abs(newY));
          if (Math.Abs(newX) + Math.Abs(newY) < 3)
          {
            x = newX;
            y = newY;
          }
          var k = keys[x + 2 + 5 * (y + 2)];
          Console.WriteLine("{0},{1} : {2}", x, y, k);
        }
        Console.WriteLine("==========");
        char key = keys[x + 2 + 5 * (y + 2)];
        Console.WriteLine("{0},{1} : {2}", x, y, key);
        code.Append(key);
      }
      Console.WriteLine(code);
    }
  }
}
