using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace day_17
{
  class Program
  {
    //static string input = "ulqzkmiv";
    static string input = "dmypynyp";

    static void Main(string[] args)
    {
      int longest = 0;
      string longStr = null;

      Stack<string> states = new Stack<string>();
      states.Push("0:0:" + GetWhichOpen("")+":");

      while (states.Count > 0)
      {
        var s = states.Pop();

        string[] parts = s.Split(':');
        string path = parts[4];
        string whichOpen = parts[2];

        if (s.StartsWith("3:3"))
        {
          if (path.Length > longest)
          {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(path.Length);
            longest = path.Length;
            longStr = path;
          }
          Console.ForegroundColor = ConsoleColor.Gray;
          Console.Write('.');
          continue;
        }

        int x = int.Parse(parts[0]);
        int y = int.Parse(parts[1]);


        if (x > 0 && whichOpen[2] == '1') MaybePush(states, x - 1, y, path + 'L');
        if (x < 3 && whichOpen[3] == '1') MaybePush(states, x + 1, y, path + 'R');
        if (y > 0 && whichOpen[0] == '1') MaybePush(states, x, y - 1, path + 'U');
        if (y < 3 && whichOpen[1] == '1') MaybePush(states, x, y + 1, path + 'D');
      }

      Console.WriteLine(longest);
    }    

    private static void MaybePush(Stack<string> states, int x, int y, string path)
    {
      string which = GetWhichOpen(path);
      if (which != "0000:0") states.Push(string.Format("{0}:{1}:{2}:{3}", x, y, which, path));
    }

    static MD5 md5 = MD5.Create();

    static string GetWhichOpen(string path)
    {
      var hash = BitConverter.ToString(md5.ComputeHash(Encoding.ASCII.GetBytes(input + path))).Replace("-", string.Empty).ToLowerInvariant();
      int bits = 0;
      string opens = new string(hash.Take(4).Select(f =>
      {
        char answer = '0';
        bits = bits << 1;
        if (f > 'a')
        {
          bits++;
          answer = '1';
        }
        return answer;
      }).ToArray());
      return opens + ":" + bits.ToString("x");
    }
  }
}
