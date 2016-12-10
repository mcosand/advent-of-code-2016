using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day_10
{
  public abstract class Target
  {
    public string Id { get; set; }

    public string Name { get { return string.Format("{0} {1}", this.GetType().Name, Id); } }

    public virtual void GiveChip(int value)
    {
    }
  }

  public class OutputBin : Target
  {
    public int? lastValue = null;

    public override void GiveChip(int value)
    {
      if (lastValue.HasValue && lastValue.Value != value)
      {
        throw new NotImplementedException("not expected");
      }
      lastValue = value;

      base.GiveChip(value);
    }
  }

  public class Bot : Target
  {
    public Target Lower { get; set; }
    public Target Upper { get; set; }

    private List<int> chips = new List<int>();

    public override void GiveChip(int value)
    {
      chips.Add(value);
      if (chips.Count == 2)
      {
        var test = chips.OrderBy(f => f).ToArray();
        if (test[0] == 17 && test[1] == 61)
        {
          Console.WriteLine("FOUND IT: " + Id.ToString());
        }

        Lower.GiveChip(test[0]);
        Upper.GiveChip(test[1]);
        chips.Clear();
      }

      base.GiveChip(value);
    }
  }

  class Program
  {


    static void Main(string[] args)
    {
      var lines = File.ReadAllLines("input.txt");

      Dictionary<string, Target> targets = new Dictionary<string, Target>();

      //Dictionary<int, OutputBin> outputs = new Dictionary<int, OutputBin>();
      //Dictionary<int, Bot> bots = new Dictionary<int, Bot>();
      Dictionary<int, Bot> inputs = new Dictionary<int, Bot>();

      foreach (var line in lines)
      {
        var match = Regex.Match(line, "value (\\d+) goes to bot (\\d+)");
        if (match.Success)
        {
          int value = int.Parse(match.Groups[1].Value);
          int botId = int.Parse(match.Groups[2].Value);

          inputs.Add(value, AddOrGet<Bot>(targets, "b" + match.Groups[2].Value));
          continue;
        }

        match = Regex.Match(line, "bot (\\d+) gives low to (bot|output) (\\d+) and high to (bot|output) (\\d+)");
        if (match.Success)
        {
          var bot = AddOrGet<Bot>(targets, "b" + match.Groups[1].Value);

          bot.Lower = Factory(targets, match.Groups[2].Value.Substring(0, 1) + match.Groups[3].Value);
          bot.Upper = Factory(targets, match.Groups[4].Value.Substring(0, 1) + match.Groups[5].Value);
        }
      }      

      foreach (var a in inputs)
      {
        a.Value.GiveChip(a.Key);
      }

    }

    static Target Factory(Dictionary<string, Target> lookup, string key)
    {
      if (key[0] == 'b') return AddOrGet<Bot>(lookup, key);
      return AddOrGet<OutputBin>(lookup, key);
    }

    static T AddOrGet<T>(Dictionary<string, Target> lookup, string key) where T : Target, new()
    {
      if (!lookup.ContainsKey(key)) lookup.Add(key, new T() { Id = key });
      return (T)lookup[key];
    }
  }
}
