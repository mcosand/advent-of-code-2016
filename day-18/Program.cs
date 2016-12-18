using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day_18
{
  class Program
  {

    static void Main(string[] args)
    {
      string input = ".^^.^.^^^^";
      input = ".^^^^^.^^.^^^.^...^..^^.^.^..^^^^^^^^^^..^...^^.^..^^^^..^^^^...^.^.^^^^^^^^....^..^^^^^^.^^^.^^^.^^";
      int count = 400000;


      string lastRow = input;
      int safe = GetSafe(lastRow);
//      Console.WriteLine(input);

      for (int i=1;i<count;i++)
      {
        string row = NextRow(lastRow);
        safe += GetSafe(row);
        //    Console.WriteLine(rows[i]);
        lastRow = row;
      }

      Console.WriteLine(safe);
    }


    private static string NextRow(string lastRow)
    {
      StringBuilder sb = new StringBuilder(lastRow.Length);
      for (int i=0;i<lastRow.Length;i++)
      {
        //^^.
        //.^^
        //^..
        //..^

        bool leftTrap = i > 0 && lastRow[i - 1] == '^';
        bool rightTrap = i < lastRow.Length - 1 && lastRow[i + 1] == '^';
        sb.Append(leftTrap == rightTrap ? '.' : '^');   
      }
      return sb.ToString();
    }

    private static int GetSafe(string row)
    {
      return row.Count(f => f == '.');
    }

  }
}
