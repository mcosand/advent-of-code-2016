using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day_19
{
  public class Elf
  {
    public int id { get; set; }
    public int presents { get; set; }
    public Elf nextElf { get; set; }
    public Elf prevElf { get; set; }
  }


  class Program
  {
    static void Main(string[] args)
    {
      int size = 3017957;
      size = 5;

      Elf root = new Elf { id = 1, presents = 1 };
      Elf elf = root;
      Elf target = null;

      // build doubly-linked list so we can repair the ring when we remove the target
      for (int i=1;i<size+1;i++)
      {
        elf.nextElf = (i == size) ? root : new Elf { id = i + 1, presents = 1, prevElf = elf };
        elf = elf.nextElf;
        if (i == size / 2) target = elf;
      }
      root.prevElf = elf;

      int count = size;

      // We have two cursors on the ring, opposite each other.
      // March the two cursors around the shrinking ring.

      Console.WriteLine("built");
      while (elf.nextElf != elf)
      {
    //    Console.WriteLine("{0} steals {1}'s {2} presents", elf.id, target.id, target.presents);
        elf.presents += target.presents;


        target.prevElf.nextElf = target.nextElf;
        target.nextElf.prevElf = target.prevElf;
        target = count % 2 == 1 ? target.nextElf.nextElf : target.nextElf;
        count--;

        elf = elf.nextElf;
      }

      int answer = elf.id;
      Console.WriteLine(answer);
    }
  }
}
