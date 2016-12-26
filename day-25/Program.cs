using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day_25
{
  class Program
  {
    static int[] registers = new int[4];
    static int pc = 0;

    static void Main(string[] args)
    {
      var program = File.ReadAllLines("input.txt");
      registers[0] = 2;
      for (int start = 1; start < int.MaxValue; start++)
      {
        registers[0] = start;
        int? line = null;
        int iterations = 0;
        pc = 0;

        while (pc < program.Length)
        {
          //     Console.WriteLine(string.Join("  ", registers) + " " + pc + " " + program[pc]);

          if (pc < program.Length - 2 && program[pc] == "inc a" && program[pc + 1] == "dec d" && program[pc + 2] == "jnz d -2")
          {
            Console.WriteLine("Shortcut A");
            registers[0] += registers[3];
            registers[3] = 0;
            pc += 3;
          }


          if (pc + 5 < program.Length && program[pc] == "cpy 282 b" && program[pc + 1] == "inc d" && program[pc + 2] == "dec b" && program[pc + 3] == "jnz b -2" && program[pc + 4] == "dec c" && program[pc + 5] == "jnz c -5")
          {
      //      Console.WriteLine("Shortcut B: 282 * " + registers[3]);
            registers[3] += 282 * registers[2];
            registers[1] = 0;
            registers[2] = 0;
            pc += 6;
       //     Console.ReadLine();
          }

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
            }
            else if (parts.Length == 3)
            {
              parts[0] = parts[0] == "jnz" ? "cpy" : "jnz";
            }

            program[pc + offset] = string.Join(" ", parts);
            pc++;
            continue;
          }

          match = Regex.Match(program[pc], "out ([a-d]|\\-?\\d+)");
          if (match.Success)
          {
            iterations++;
            if (iterations > 100)
              Console.WriteLine(start);
            //var old = Console.ForegroundColor;

            int v = GetValue(match.Groups[1].Value);
            //Console.ForegroundColor = ConsoleColor.Magenta;
            //Console.WriteLine(v + " " + start);
            //Console.ForegroundColor = old;


            if (line.HasValue && (!(v == 0 || v == 1) || v == line))
            {
              //Console.ForegroundColor = ConsoleColor.Yellow;
              //Console.WriteLine("Gerk. " + start);
              break;
            }
            line = v;
            pc++;
            continue;
          }
          Console.WriteLine("Not a known instruction: " + program[pc]);
          pc++;
        }
      }
  //    Console.WriteLine(registers[0]);
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
