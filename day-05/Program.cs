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
      var input = "wtnhxymk";
      var output = new char[input.Length];
      var hits = 0;

      var md5 = MD5.Create();
      int j = 0;
      string hash = null;
      do
      {
        if (j % 10000 == 0) Console.Write(".");
        byte[] bytes = Encoding.ASCII.GetBytes(input + j.ToString());
        hash = string.Join("", md5.ComputeHash(bytes).Select(f => f.ToString("x2")).ToArray());

        if (hash.StartsWith("00000"))
        {
          output[hits++] = hash[5];
          Console.WriteLine(hash[5]);
        }
        j++;

      } while (hits < input.Length);

      Console.WriteLine(new string(output));
    }
  }
}
