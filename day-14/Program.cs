using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace day_14
{
  class Search
  {
    public Search(int index, char c, string key)
    {
      this.Index = index;
      this.C = c;
      this.Key = key;
    }
    public char C { get; set; }
    public string Key { get; set; }
    public int Index { get; set; }
    public int? FiveIndex { get; set; }
  }

  class Program
  {
    static void Main(string[] args)
    {
      Search[] searches = new Search[1000];
      List<int> keyIndicies = new List<int>();

      int index = 0;
      string salt = "jlmsuwbz";
      //string salt = "abc";

      var md5 = MD5.Create();

      while (keyIndicies.Count < 64)
      {
        var text = string.Format("{0}{1}", salt, index);
        var hash = BitConverter.ToString(md5.ComputeHash(Encoding.ASCII.GetBytes(text))).Replace("-", string.Empty);


        var modI = index % 1000;
        if (searches[modI] != null)
        {
          if (searches[modI].FiveIndex.HasValue)
          {
            keyIndicies.Add(searches[modI].Index);
            Console.WriteLine("Found key at index " + searches[modI].Index.ToString() + " " + index.ToString() + " " + keyIndicies.Count.ToString());
          }
        }
        searches[modI] = new Search(index, ContainsThree(hash), hash);

        for (var i=0;i<1000;i++)
        {
          if (i == modI) continue;
          if (searches[i] != null && !searches[i].FiveIndex.HasValue && searches[i].C != 0)
          {
            if (ContainsFive(hash, searches[i].C))
            {
              Console.WriteLine("Found string of 5 {0}'s at index {1}", searches[i].C, index);
              searches[i].FiveIndex = index;
            }
          }
        }

        index++;
        if (index % 1000 == 0) Console.WriteLine(index);
      }
      int answer = keyIndicies[63];
      Console.WriteLine(answer);
    }

    static char ContainsThree(string input)
    {
      char[] buffer = new char[3];
      for (int i=0; i<input.Length; i++)
      {
        buffer[i % 3] = input[i];
        if (buffer[0] == buffer[1] && buffer[1] == buffer[2]) return buffer[0];
      }
      return (char)0;
    }

    static bool ContainsFive(string input, char test)
    {
      int count = 0;
      for (int i=0;i<input.Length;i++)
      {
        if (input[i] == test)
        {
          count++;
        }
        else
        {
          count = 0;
        }
        if (count == 5) return true;
      }
      return false;
    }
  }
}
