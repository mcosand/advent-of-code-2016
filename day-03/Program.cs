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
      foreach (var group in groups)
      {
        var sides = group.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
          .Select(f => int.Parse(f))
          .OrderBy(f => f)
          .ToArray();

        if (sides[0] <= 0) continue;
        if (sides[0] + sides[1] > sides[2]) possible++;
      }
      Console.WriteLine(possible);
    }
  }
}
