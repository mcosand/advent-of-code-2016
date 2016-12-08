using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace day_08
{
  class Program
  {
    static void Main(string[] args)
    {
      var lines = File.ReadAllLines("input.txt");
      //var w = 7;
      //var h = 3;
      var w = 50;
      var h = 6;

      var pixels = new bool[h][];
      for (int i = 0; i < h; i++) pixels[i] = new bool[w];
      
      foreach (var line in lines)
      {
        var foo = TryRect(line, pixels) ?? TryRotX(line, pixels) ?? TryRotY(line, pixels);
        PrintScreen(pixels);
      }

      int lit = Count(pixels);
      Console.WriteLine(lit);
    }

    static string TryRect(string line, bool[][] pixels)
    {
      var match = Regex.Match(line, "rect (\\d+)x(\\d+)");
      if (!match.Success) return null;

      var w = int.Parse(match.Groups[1].Value);
      var h = int.Parse(match.Groups[2].Value);

      for (int x=0;x<w;x++)
      {
        for (int y=0;y<h;y++)
        {
          pixels[y][x] = true;
        }
      }
      return ".";
    }

    static string TryRotX(string line, bool[][] pixels)
    {
      var match = Regex.Match(line, "rotate column x=(\\d+) by (\\d+)");
      if (!match.Success) return null;

      int col = int.Parse(match.Groups[1].Value);

      for (int i = 0; i < int.Parse(match.Groups[2].Value); i++)
      {
        bool bottom = pixels[pixels.Length - 1][col];
        for (int y = pixels.Length - 1; y > 0; y--)
        {
          pixels[y][col] = pixels[y - 1][col];
        }
        pixels[0][col] = bottom;
      }
      return "";
    }

    static string TryRotY(string line, bool[][] pixels)
    {
      var match = Regex.Match(line, "rotate row y=(\\d+) by (\\d+)");
      if (!match.Success) return null;

      int row = int.Parse(match.Groups[1].Value);

      for (int i = 0; i < int.Parse(match.Groups[2].Value); i++)
      {
        bool right = pixels[row][pixels[row].Length - 1];
        for (int x = pixels[row].Length - 1; x > 0; x--)
        {
          pixels[row][x] = pixels[row][x - 1];
        }
        pixels[row][0] = right;
      }
      return "";
    }

    static void PrintScreen(bool[][] pixels)
    {
      Console.WriteLine();
      for (int y=0;y<pixels.Length;y++)
      {
        Console.WriteLine(string.Join(" ", pixels[y].Select(f => f ? "X" : ".")));
      }
    }

    static int Count(bool[][] pixels)
    {
      int count = 0;
      for (var i=0;i<pixels.Length;i++)
      {
        for (var j=0;j<pixels[0].Length;j++)
        {
          count += pixels[i][j] ? 1 : 0;
        }
      }
      return count;
    }
  }
}
