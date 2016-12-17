using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace day_17
{
  class Program
  {
    //static string input = "ulqzkmiv";
    static string input = "dmypynyp";
    static MD5 md5 = MD5.Create();

    static void Main(string[] args)
    {
      string whichOpen = GetWhichOpen("");

      var states = new [] { string.Format("0:0::{0}", whichOpen) };

      int count = 0;
      while (!states.Any(f => IsFinished(f)))
      {
        count++;
        Console.WriteLine("{0} {1}", count, states.Length);
        states = states.SelectMany(f => {
          return NextStates(f);
        }).ToArray();
      }

      var state = states.First(f => IsFinished(f));
      Console.Write(state);
    }
    
    private static bool IsFinished(string state)
    {
      return state.StartsWith("3:3");
    }

    private static IEnumerable<string> NextStates(string state)
    {
      string[] parts = state.Split(':');
      int x = int.Parse(parts[0]);
      int y = int.Parse(parts[1]);
      string path = parts[2];
      
      string whichOpen = parts[3];

      string fmt = "{0}:{1}:{2}:{3}";
      if (x > 0 && whichOpen[2] == '1') yield return string.Format(fmt, x - 1, y, path + 'L', GetWhichOpen(path + 'L'));
      if (x < 3 && whichOpen[3] == '1') yield return string.Format(fmt, x + 1, y, path + 'R', GetWhichOpen(path + 'R'));
      if (y > 0 && whichOpen[0] == '1') yield return string.Format(fmt, x, y - 1, path + 'U', GetWhichOpen(path + 'U'));
      if (y < 3 && whichOpen[1] == '1') yield return string.Format(fmt, x, y + 1, path + 'D', GetWhichOpen(path + 'D'));
    }

    static string GetWhichOpen(string path)
    {
      var hash = BitConverter.ToString(md5.ComputeHash(Encoding.ASCII.GetBytes(input + path))).Replace("-", string.Empty).ToLowerInvariant();
      string opens = new string(hash.Take(4).Select(f => f > 'a' ? '1' : '0').ToArray());
      return opens;
    }
  }
}
