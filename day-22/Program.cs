using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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
    public bool HasPayload { get; set; } = false;
  }

  class Program
  {
    static int payloadIndex = 0;
    static int emptyIndex = 0;
    static int lengthX = 0;
    static int lengthY = 0;
    static List<Node> nodes = new List<Node>();

    static Queue<int[]> steps = new Queue<int[]>();
    static Dictionary<string, int> previous = new Dictionary<string, int>();


    static void Main(string[] args)
    {
      Node payloadNode = null;
      Node emptyNode = null;

      foreach (var line in File.ReadAllLines("input.txt"))
      {
        var match = Regex.Match(line, "/dev/grid/node-x(\\d+)-y(\\d+) +(\\d+)T +(\\d+)T");
        if (!match.Success) throw new InvalidOperationException();

        int x = int.Parse(match.Groups[1].Value);
        int y = int.Parse(match.Groups[2].Value);

        int total = int.Parse(match.Groups[3].Value);
        int used = int.Parse(match.Groups[4].Value);
        var node = new Node(x, y, total, used);
        if (y == 0)
        {
          lengthX++;
          payloadNode = node;
        }
        if (y == 0 && x == 1) lengthY = nodes.Count;

        if (used == 0) emptyNode = node;
        nodes.Add(node);
      }

      emptyIndex = ToIndex(emptyNode);
      payloadIndex = ToIndex(payloadNode);

      Console.SetWindowSize(Console.BufferWidth, Math.Max(25, lengthY + 2));

      Console.WriteLine("built");


      steps.Enqueue(new[] { 0 });
      int count = 0;

      while (steps.Count > 0)
      {
     //   Console.WriteLine("==============");
        int[] copyFroms = steps.Dequeue();
        //   Console.WriteLine(string.Join(".", copyFroms));

        if (count++ == 10000)
        {
          Console.Write(" " + copyFroms[0]);
          count = 0;
        }


        int[] returns = new int[copyFroms.Length];
        for (int i=1;i<copyFroms.Length; i++)
        {
          returns[i] = CopyFrom(copyFroms[i]);
        }

        if (payloadIndex == 0)
        {
          int answer = copyFroms[0];
          Console.WriteLine("Found it! Distance=" + answer);
          return;
        }

        Node a = nodes[emptyIndex];
    //    Console.WriteLine("Current empty slot: {0},{1}", a.X, a.Y);
    //    PrintMap();
    //    Console.ReadLine();

        emptyNode = nodes[emptyIndex];
        if (emptyNode.X > 0) ConditionalPush(copyFroms, emptyNode, nodes[ToIndex(emptyNode.X - 1, emptyNode.Y)]);
        if (emptyNode.X < lengthX - 1) ConditionalPush(copyFroms, emptyNode, nodes[ToIndex(emptyNode.X + 1, emptyNode.Y)]);
        if (emptyNode.Y > 0) ConditionalPush(copyFroms, emptyNode, nodes[ToIndex(emptyNode.X, emptyNode.Y - 1)]);
        if (emptyNode.Y < lengthY - 1) ConditionalPush(copyFroms, emptyNode, nodes[ToIndex(emptyNode.X, emptyNode.Y + 1)]);

        for (int i=returns.Length-1; i> 0; i--)
        {
          CopyFrom(returns[i]);
        }
      }
    }

    private static void ConditionalPush(int[] copyFroms, Node empty, Node from)
    {
      if (empty.Total - empty.Used < from.Used) return;

      string key = string.Format("{0}/{1}/{2}", emptyIndex, payloadIndex, ToIndex(from)); //string.Join(".", copyFroms) + "." + ToIndex(from);
      if (previous.ContainsKey(key))
      {
        return;
      }

      var newCopies = new int[copyFroms.Length + 1];
      for (int i = 0; i < copyFroms.Length; i++) newCopies[i] = copyFroms[i];
      newCopies[copyFroms.Length] = ToIndex(from);
      newCopies[0] = copyFroms.Length;
      steps.Enqueue(newCopies);
      previous.Add(key, newCopies.Length);
    }

    private static int ToIndex(int x, int y)
    {
      return x * lengthY + y;
    }

    private static int ToIndex(Node n)
    {
      return ToIndex(n.X, n.Y);
    }

    private static int CopyFrom(int from)
    {
      var b = nodes[emptyIndex];
      var a = nodes[from];

   //   Console.WriteLine("Copy from {0},{1} to {2},{3}", a.X, a.Y, b.X, b.Y);
      nodes[emptyIndex].Used += nodes[from].Used;
      nodes[from].Used = 0;

      payloadIndex = payloadIndex != from ? payloadIndex : emptyIndex;

      int tmp = emptyIndex;
      emptyIndex = from;

      return tmp;
    }

    private static void PrintMap()
    {
      var colors = new[] { ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Cyan, ConsoleColor.Gray, ConsoleColor.DarkGray };

      for (int y = 0; y < lengthY; y++)
      {
        for (int x = 0; x < lengthX; x++)
        {
          var node = nodes[x * lengthY + y];
          var percent = (double)node.Used / node.Total;
          int transfers = 0;
          if (x > 0 && nodes[(x - 1) * lengthY + y].Used <= node.Total - node.Used) transfers++;
          if (x < lengthX - 1 && nodes[(x + 1) * lengthY + y].Used <= node.Total - node.Used) transfers++;
          if (y > 0 && nodes[x * lengthY + (y - 1)].Used <= node.Total - node.Used) transfers++;
          if (y < lengthY - 1 && nodes[x * lengthY + (y + 1)].Used <= node.Total - node.Used) transfers++;
          Console.ForegroundColor = colors[transfers];
          Console.Write(node.HasPayload ? "@" : (percent < .01 ? "_" : percent < .8 ? "." : "#") + " ");
        }
        Console.WriteLine();
      }
    }
  }
}
