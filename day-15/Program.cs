using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day_15
{
  public class Disc
  {
    public Disc(int positions, int start)
    {
      Positions = positions;
      Start = start;
    }
    public int Positions { get; set; }
    public int Start { get; set; }
  }
  class Program
  {
    static void Main(string[] args)
    {
      var discs = new[]
      {
        new Disc(5,2),
        new Disc(13, 7),
        new Disc(17, 10),
        new Disc(3, 2),
        new Disc(19, 9),
        new Disc(7, 0)
        /*new Disc(5, 4),
        new Disc(2, 1)*/
      };


      int time = 0;
      while (!IsGood(discs, time)) time++;

      Console.WriteLine(time);
    }

    static bool IsGood(Disc[] discs, int time)
    {
      for (int i=0;i<discs.Length;i++)
      {
        if ((time + i + 1 + discs[i].Start) % discs[i].Positions != 0) return false;
      }
      return true;
    }
  }
}
