using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day_12
{
  class Program
  {
    static int[] registers = new int[4];
    static int pc = 0;

    static void Main(string[] args)
    {
      var program = File.ReadAllLines("input.txt");

      
      while (pc < program.Length)
      {
     //   Console.WriteLine(program[pc]);

        var match = Regex.Match(program[pc], "cpy ([a-d]|-?\\d+) ([a-d])");
        if (match.Success)
        {
          var v = GetValue(match.Groups[1].Value);
          registers[match.Groups[2].Value[0] - 'a'] = v;
          pc++;
          continue;
        }
        match = Regex.Match(program[pc], "(inc|dec) ([a-d])");
        if (match.Success)
        {
          registers[match.Groups[2].Value[0] - 'a'] += match.Groups[1].Value == "inc" ? 1 : -1;
          pc++;
          continue;
        }
        match = Regex.Match(program[pc], "jnz ([a-d]|\\-?\\d+) (\\-?\\d+)");
        if (match.Success)
        {
          var v = GetValue(match.Groups[1].Value);
     //     Console.WriteLine("jnz {0}", v);
          if (v != 0)
          {
            pc += int.Parse(match.Groups[2].Value) - 1;
          }
          pc++;
          continue;
        }

        Console.WriteLine("Not a known instruction: " + program[pc]);
      }

      Console.WriteLine(registers[0]);
    }

    private static int GetValue(string value)
    {
      int v;
      if (!int.TryParse(value, out v))
      {
        v = registers[value[0] - 'a'];
      }
      return v;
    }
  }
}
