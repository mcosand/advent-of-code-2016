using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day_09
{
  class Program
  {
    static void Main(string[] args)
    {
      StringBuilder output = new StringBuilder();
      string input = File.ReadAllText("input.txt");

      var i = 0;
      while (i < input.Length)
      {
        if (input[i] == '(')
        {
          var match = Regex.Match(input.Substring(i), "\\((\\d+)x(\\d+)\\)");
          if (!match.Success)
          {
            output.Append('(');
            continue;
          }

          i += match.Length;
          var repeatLength = int.Parse(match.Groups[1].Value);
          var repeat = input.Substring(i, repeatLength);
          for (int j=0;j<int.Parse(match.Groups[2].Value);j++)
          {
            output.Append(repeat);
          }
          i += repeatLength;
        }
        else
        {
          output.Append(input[i]);
          i++;
        }
      }

      int length = output.Length;
      Console.WriteLine(length);
    }
  }
}
