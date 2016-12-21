using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day_21
{
  class Program
  {
    static void Main(string[] args)
    {
      var startText = "decab";
      startText = "fbgdceah";

      List<char> text = new List<char>(startText.ToCharArray());

      Console.WriteLine(new string(text.ToArray()));
      RunRules(text, true);

      string cipher = new string(text.ToArray());
      Console.WriteLine(cipher);
    }

    private static void RunRules(List<char> text, bool inverse)
    {
      var lines = File.ReadAllLines("input.txt").AsEnumerable();
      if (inverse) lines = lines.Reverse();
      foreach (var line in lines)
      {
        Console.WriteLine(line);
        RunRule(text, line, inverse);
        Console.WriteLine("  " + new string(text.ToArray()));

      }
    }

    private static void RunRule(List<char> text, string line, bool inverse)
    {
      var match = Regex.Match(line, "swap position (\\d+) with position (\\d+)");
      if (match.Success)
      {
        char tmp = text[int.Parse(match.Groups[2].Value)];
        text[int.Parse(match.Groups[2].Value)] = text[int.Parse(match.Groups[1].Value)];
        text[int.Parse(match.Groups[1].Value)] = tmp;
        return;
      }

      match = Regex.Match(line, "swap letter ([a-z]) with letter ([a-z])");
      if (match.Success)
      {
        char a = match.Groups[1].Value[0];
        char b = match.Groups[2].Value[0];
        for (int i = 0; i < text.Count; i++)
        {
          if (text[i] == a) text[i] = b;
          else if (text[i] == b) text[i] = a;
        }
        return;
      }

      match = Regex.Match(line, "rotate (left|right) (\\d+) steps?");
      if (match.Success)
      {
        int rot = int.Parse(match.Groups[2].Value);
        string direction = match.Groups[1].Value;
        Rotate(text, rot, direction, inverse);
        return;
      }

      match = Regex.Match(line, "rotate based on position of letter ([a-z])");
      if (match.Success)
      {
        if (inverse) LetterPositionRotateBackward(text, match.Groups[1].Value[0]);
        else LetterPositionRotateForward(text, match.Groups[1].Value[0]);
        return;
      }

      match = Regex.Match(line, "reverse positions (\\d+) through (\\d+)");
      if (match.Success)
      {
        var start = int.Parse(match.Groups[1].Value);
        var finish = int.Parse(match.Groups[2].Value);
        while (start < finish)
        {
          char tmp = text[start];
          text[start] = text[finish];
          text[finish] = tmp;
          start++;
          finish--;
        }
        return;
      }

      match = Regex.Match(line, "move position (\\d+) to position (\\d+)");
      if (match.Success)
      {
        var x = int.Parse(match.Groups[1].Value);
        var y = int.Parse(match.Groups[2].Value);
        
        char tmp = text[inverse ? y : x];
        text.RemoveAt(inverse ? y : x);
        text.Insert(inverse ? x : y, tmp);
      }
    }

    private static void LetterPositionRotateBackward(List<char> text, char letter)
    {
      string result = new string(text.ToArray());
      List<char> dummy;
      do
      {
        Rotate(text, 1, "left", false);
        dummy = new List<char>(text);
        LetterPositionRotateForward(dummy, letter);
      } while (new string(dummy.ToArray()) != result);
    }

    private static void LetterPositionRotateForward(List<char> text, char letter)
    {
      int i = 0;
      for (; i < text.Count; i++)
      {
        if (text[i] ==letter) break;
      }
      Rotate(text, 1 + i + ((i >= 4) ? 1 : 0), "right", false);
    }

    private static void Rotate(List<char> text, int rot, string direction, bool inverse)
    {
      char[] source = text.ToArray();
      rot = rot % text.Count;
      int sign = direction == "left" ? 1 : -1;
      sign *= (inverse ? -1 : 1);
      for (int i = 0; i < text.Count; i++)
      {
        int from = (text.Count + i + (sign * rot)) % text.Count;
        text[i] = source[from];
      }
    }
  }
}
