using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace day_05
{
  class Program
  {
    static void Main(string[] args)
    {
      var input =  "wtnhxymk";
      var output = new string('_', input.Length).ToCharArray();
      var hits = 0;

      var md5 = MD5.Create();
      int j = 0;
      string hash = null;
      do
      {
        if (j % 10000 == 0) Console.Write(".");
        byte[] bytes = Encoding.ASCII.GetBytes(input + j.ToString());
        hash = string.Join("", md5.ComputeHash(bytes).Select(f => f.ToString("x2")).ToArray());

        int position = 0;
        if (hash.StartsWith("00000") && int.TryParse(hash[5].ToString(), out position) && position < 8 && output[position] == '_')
        {
          output[position] = hash[6];
          hits++;
          Console.WriteLine();
          Console.WriteLine(new string(output));
        }
        j++;

      } while (hits < input.Length);

      Console.WriteLine(new string(output));
    }
  }
}
