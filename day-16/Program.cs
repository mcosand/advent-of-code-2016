using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day_16
{
  class Program
  {
    static void Main(string[] args)
    {
      string input = "11011110011011101";
      int size = 35651584;

      while (input.Length <= size) input = GetNext(input);


      string checksum = input.Substring(0, size);

      do
      {
        checksum = ChecksumPass(checksum);
      } while (checksum.Length % 2 == 0);
    }

    public static string GetNext(string input)
    {
      StringBuilder b = new StringBuilder(input, input.Length * 2 + 1);
      b.Append('0');
      foreach (var c in input.Reverse().Select(f => f == '0' ? '1' : '0')) b.Append(c);
      return b.ToString();
    }

    public static string ChecksumPass(string input)
    {
      StringBuilder b = new StringBuilder(input.Length / 2);
      for (int i = 0; i < input.Length - 1; i += 2)
      {
        b.Append(input[i] == input[i + 1] ? '1' : '0');
      }

      return b.ToString();
    }
  }
}
