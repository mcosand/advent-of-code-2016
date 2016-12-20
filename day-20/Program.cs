using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day_20
{
  class Program
  {
    static void Main(string[] args)
    {
      uint minValue = 1;
      var input = File.ReadAllLines("input.txt");
      foreach (var range in input.Select(f =>
        {
          var pair = f.Split('-');
          return new Tuple<uint, uint>(uint.Parse(pair[0]), uint.Parse(pair[1]));
        })
        .OrderBy(f => f.Item1))
      {
        if (range.Item1 <= minValue && range.Item2 > minValue) minValue = range.Item2 + 1;
      }

      Console.WriteLine(minValue);
    }
  }
}
