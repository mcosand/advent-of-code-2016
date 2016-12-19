using System;

namespace day_19
{
  public class Elf
  {
    public int Id { get; set; }
    public int Presents { get; set; }
    public Elf NextElf { get; set; }
    public Elf PrevElf { get; set; }
  }


  class Program
  {
    static void Main(string[] args)
    {
      int size = 5;
      size = 3017957;

      Elf root = new Elf { Id = 1, Presents = 1 };
      Elf elf = root;
      Elf target = null;

      // build doubly-linked list so we can repair the ring when we remove the target
      for (int i = 1; i < size + 1; i++)
      {
        elf.NextElf = (i == size) ? root : new Elf { Id = i + 1, Presents = 1, PrevElf = elf };
        elf = elf.NextElf;
        if (i == size / 2) target = elf;
      }
      root.PrevElf = elf;

      int count = size;

      // We have two cursors on the ring, opposite each other.
      // March the two cursors around the shrinking ring.

      Console.WriteLine("built");
      while (elf.NextElf != elf)
      {
        //Console.WriteLine("{0} steals {1}'s {2} presents", elf.id, target.id, target.presents);

        // Ended up not being needed.
        elf.Presents += target.Presents;


        target.PrevElf.NextElf = target.NextElf;
        target.NextElf.PrevElf = target.PrevElf;
        target = count % 2 == 1 ? target.NextElf.NextElf : target.NextElf;
        count--;

        elf = elf.NextElf;
      }

      int answer = elf.Id;
      Console.WriteLine(answer);
    }
  }
}
