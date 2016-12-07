using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day_07
{
  class Program
  {
    static void Main(string[] args)
    {
      var lines = File.ReadAllLines(args.Length > 0 ? args[0] : "input.txt");
      int count = 0;
      foreach (var line in lines)
      {
        if (isSsl(line))
        {
          Console.WriteLine("YES " + line);
          count++;
        }
        else Console.WriteLine("NO  " + line);
      }
      Console.WriteLine(count);
    }

    private static bool isTls(string line)
    {
      var bracketDepth = 0;
      var answer = false;

      for (var i=0;i<line.Length; i++)
      {
        if (i > 2 && line[i] == line[i-3] && line[i-1] == line[i-2] && line[i] != line[i-1])
        {
          if (bracketDepth > 0) return false;
          answer |= true;
        }
        else if (line[i] == '[')
        {
          bracketDepth++;
        }
        else if (line[i] == ']')
        {
          bracketDepth = Math.Max(0, bracketDepth - 1);
        }
      }


      return answer;
    }
    
    private static bool isSsl(string line)
    {
      var bracketDepth = 0;
      List<string> [] parsed = new[] { new List<string>(), new List<string>() };
      for (var i=0;i<line.Length;i++)
      {
        if (line[i] == '[')
        {
          bracketDepth++;
        }
        else if (line[i] == ']')
        {
          bracketDepth = Math.Max(0, bracketDepth - 1);
        }
        else if (i > 1 && line[i] == line[i - 2] && line[i] != line[i - 1])
        {
          parsed[bracketDepth].Add(string.Format("{0}{1}", line[i], line[i - 1]));
        }
      }

      foreach (var a in parsed[0])
      {
        if (parsed[1].Contains(string.Format("{0}{1}", a[1], a[0]))) return true;
      }
      return false;
    }    
  }
}
