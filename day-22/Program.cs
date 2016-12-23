using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day_22
{
  class Node
  {
    public Node(int x, int y, int total, int used)
    {
      X = x;
      Y = y;
      Total = total;
      Used = used;
    }
    public int X { get; set; }
    public int Y { get; set; }
    public int Total { get; set; }
    public int Used { get; set; }
  }

  class Program
  {
    static void Main(string[] args)
    {
      List<Node> nodes = new List<Node>();
      foreach (var line in File.ReadAllLines("input.txt"))
      {
        var match = Regex.Match(line, "/dev/grid/node-x(\\d+)-y(\\d+) +(\\d+)T +(\\d+)T");
        if (!match.Success) throw new InvalidOperationException();

        int x = int.Parse(match.Groups[1].Value);
        int y = int.Parse(match.Groups[2].Value);
        int total = int.Parse(match.Groups[3].Value);
        int used = int.Parse(match.Groups[4].Value);
        nodes.Add(new Node(x, y, total, used));
      }
      Console.WriteLine("built");
      Part1(nodes);
    }

    private static void Part1(List<Node> nodes)
    {
      var numPairs = (from a in nodes
                      from b in nodes
                      where a.Used > 0 && !(a.X == b.X && a.Y == b.Y) && a.Used <= (b.Total - b.Used)
                      select 1)
                   .Count();
      Console.WriteLine(numPairs);
    }
  }
}
