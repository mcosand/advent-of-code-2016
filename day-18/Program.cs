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

      List<string> rows = new List<string> { input };
      int safe = GetSafe(input);
      Console.WriteLine(input);
      
      for (int i=1;i<count;i++)
      {
        rows.Add(NextRow(rows[i - 1]));
        safe += GetSafe(rows[i]);
    //    Console.WriteLine(rows[i]);
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
