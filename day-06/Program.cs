using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day_06
{
  class Program
  {
    static void Main(string[] args)
    {
      var lines = File.ReadAllLines(args.Length > 0 ? args[0] : "input.txt");

      var bits = new int[lines[0].Length][];

      foreach (var line in lines)
      {
        for (var i=0;i<line.Length;i++)
        {
          bits[i] = bits[i] ?? new int[26];
          bits[i][line[i] - 'a']++;
        }
      }

      StringBuilder sb = new StringBuilder();
      for(var i=0;i<lines[0].Length;i++)
      {
        var most = bits[i].Select((f, idx) => new { Count = f, Index = idx }).OrderByDescending(f => f.Count).Select(f => f.Index).First();
        sb.Append((char)(most + 'a'));
      }

      string answer = sb.ToString();
      Console.WriteLine(answer);
    }
  }
}
