using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day_23
{
  class Program
  {
    static int[] registers = new int[4];
    static int pc = 0;

    static void Main(string[] args)
    {
      var program = File.ReadAllLines("input.txt");
      registers[0] = 7;

      while (pc < program.Length)
      {
        Console.WriteLine(string.Join("  ", registers) + " " + pc + " " + program[pc]);
    //    Console.WriteLine(program[pc]);

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
        match = Regex.Match(program[pc], "jnz ([a-d]|\\-?\\d+) ([a-d]|\\-?\\d+)");
        if (match.Success)
        {
          var v = GetValue(match.Groups[1].Value);
          //     Console.WriteLine("jnz {0}", v);
          if (v != 0)
          {
            pc += GetValue(match.Groups[2].Value) - 1;
          }
          pc++;
          continue;
        }

        match = Regex.Match(program[pc], "tgl ([a-d]|\\-?\\d+)");
        if (match.Success)
        {
          char c = match.Groups[1].Value[0];
          int offset = (c >= 'a' && c <= 'd') ? registers[c - 'a'] : int.Parse(match.Groups[1].Value);

          if (pc + offset < 0 || pc + offset >= program.Length)
          {
            pc++;
            continue;
          }

          var instruction = program[pc + offset];
          var parts = instruction.Split(' ');
          if (parts.Length == 2)
          {
            parts[0] = parts[0] == "inc" ? "dec" : "inc";
          } else if (parts.Length == 3)
          {
            parts[0] = parts[0] == "jnz" ? "cpy" : "jnz";
          }

          program[pc + offset] = string.Join(" ", parts);
          pc++;
          continue;
        }

        Console.WriteLine("Not a known instruction: " + program[pc]);
        pc++;
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
