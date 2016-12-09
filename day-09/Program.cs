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
      string input = File.ReadAllText("input.txt");

      var length = Decompress(input);

    }

    private static long Decompress(string input)
    {
      long length = 0;
      var i = 0;
      while (i < input.Length)
      {
        if (input[i] == '(')
        {
          var match = Regex.Match(input.Substring(i), "\\((\\d+)x(\\d+)\\)");
          if (!match.Success)
          {
            length++;
            continue;
          }

          i += match.Length;
          var repeatLength = int.Parse(match.Groups[1].Value);
          var decompressedLength = Decompress(input.Substring(i, repeatLength));
          for (long j = 0; j < long.Parse(match.Groups[2].Value); j++)
          {
            length += decompressedLength;
          }
          i += repeatLength;
        }
        else
        {
          length++;
          i++;
        }
      }

      return length;
    }
  }
}
