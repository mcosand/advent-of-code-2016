using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day_04
{
  class Program
  {
    static void Main(string[] args)
    {
      var lines = File.ReadAllLines(args.Length > 0 ? args[0] : "input.txt");
      long code = 0;

      foreach (var line in lines)
      {
        var match = Regex.Match(line, "(.*)\\-(\\d+)\\[([a-zA-Z]+)\\]");
        if (match.Success == false) Console.Write(line + " no match");
        Console.WriteLine(string.Format("{0} {1} {2}", match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value));

        var letters = string.Join("",
          match.Groups[1].Value
                        .ToLowerInvariant()
                        .Where(f => f != '-')
                        .GroupBy(f => f)
                        .Select(g => new { Count = g.Count(), Letter = g.Key })
                        .OrderByDescending(f => f.Count)
                        .ThenBy(f => f.Letter)
                        .Select(f => f.Letter)
                        .Take(5));
        Console.WriteLine(string.Format("{0} {1}", letters, match.Groups[3].Value));
        if (letters == match.Groups[3].Value) code += int.Parse(match.Groups[2].Value);
      }

      Console.WriteLine(code);
    }
  }
}
