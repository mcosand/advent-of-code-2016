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
  }


  class Program
  {
    static void Main(string[] args)
    {
      int size = 3017957;
      //size = 5;

      Elf root = new Elf { id = 1, presents = 1 };
      Elf elf = root;
      for (int i=1;i<size+1;i++)
      {
        elf.nextElf = (i == size) ? root : new Elf { id = i + 1, presents = 1 };
   //     Console.WriteLine(elf.id + " " + elf.nextElf.id);
        elf = elf.nextElf;
      }
      //elf.nextElf = root;

      Console.WriteLine("built");
      while (elf.nextElf != elf)
      {
    //    Console.WriteLine("{0} steals {1}'s {2} presents", elf.id, elf.nextElf.id, elf.nextElf.presents);
        elf.presents += elf.nextElf.presents;
        elf.nextElf = elf.nextElf.nextElf;
    //    Console.WriteLine("{0} is now next to {1}", elf.id, elf.nextElf.id);
        elf = elf.nextElf;
      }
      
      Console.WriteLine(elf.id);
      int answer = elf.id;
    }
  }
}
