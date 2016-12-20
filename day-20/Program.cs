using System;
using System.IO;
using System.Linq;

namespace day_20
{
  class Program
  {
    static void Main(string[] args)
    {
      uint minValue = 1;
      uint count = 0;

      var input = File.ReadAllLines("input.txt");
      foreach (var range in input.Select(f =>
        {
          var pair = f.Split('-');
          return new Tuple<uint, uint>(uint.Parse(pair[0]), uint.Parse(pair[1]));
        })
        .OrderBy(f => f.Item1))
      {
        Console.WriteLine("{0}-{1}", range.Item1, range.Item2);
        if (range.Item1 <= minValue && range.Item2 > minValue)
        {
          minValue = range.Item2 + 1;
          Console.WriteLine("Move minValue to " + minValue);
        }
        else if (range.Item1 > minValue)
        {
          Console.WriteLine("Add " + (range.Item1 - minValue) + " to count " + count);
          count += range.Item1 - minValue;
        }

        if (range.Item2 == uint.MaxValue) break;
        if (range.Item2 > minValue) minValue = range.Item2 + 1;
      }


      Console.WriteLine(minValue);
    }
  }
}
