using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day_11
{
  class Program
  {
    class Plan
    {
      string[][] _state;
      int _floor;

      public Plan(string startState)
      {
        var foo = startState.Split(':');
        int floor = int.Parse(foo[0]);

        var floors = foo[1].Split('|');
        _state = floors.Select(f => f.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)).ToArray();
        _floor = floor;
      }



      private string RenderState(string[][] _state, List<string> keep, List<string> move, int _floor, int delta)
      {
        return string.Format("{0}:{1}", _floor + delta, string.Join("|", _state.Select((f, i) =>
        {
          if (i == _floor) return string.Join(" ", keep);
          if (i == _floor + delta) return string.Join(" ", f.Concat(move).OrderBy(g => g).ToArray());
          return string.Join(" ", f);
        })));
      }

      public IEnumerable<string> BuildPlans()
      {
        for (var first = 0; first < _state[_floor].Length; first++)
        {
          for (var second = first; second < _state[_floor].Length; second++)
          {
            List<string> keep = new List<string>(_state[_floor]);
            List<string> move = new List<string>();

            move.Add(keep[first]);
            if (first != second)
            {
              move.Add(keep[second]);
              keep.RemoveAt(second);
            }
            keep.RemoveAt(first);

            if (_floor > 0)
            {
              string state = RenderState(_state, keep, move, _floor, -1);
              if (IsGoodState(state)) yield return state;
            }
            if (_floor < 3)
            {
              string state = RenderState(_state, keep, move, _floor, 1);
              if (IsGoodState(state)) yield return state;
            }
          }
        }
      }
    }

    static void Main(string[] args)
    {
       string state = "0:EG EM DG DM SG SM PG PM | TG RG RM CG CM | TM |";
      //string state = "0:HM LM | HG | LG | ";

      List<string> previousStates = new List<string>();

      int steps = 0;
      string[] plans = new[] { state };
      while (!plans.Any(f => IsFinished(f)))
      {
        plans = plans.AsParallel().SelectMany(f => new Plan(f).BuildPlans()).AsEnumerable().Distinct().Except(previousStates).ToArray();
        steps++;
        Console.WriteLine("- {0}", plans.Length);
        previousStates.AddRange(plans);
      }

      Console.WriteLine(steps);
    }

    public static bool IsFinished(string state)
    {
      return state.Split(':')[1].Split('|').Take(3).Sum(f => f.Trim().Length) == 0;
    }

    static bool IsGoodState(string state)
    {
      string[] floors = state.Split(':')[1].Split('|');
      for (var i = 0; i < floors.Length; i++)
      {
        var parts = floors[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        var noGenerators = !parts.Any(f => f[1] == 'G');
        var noParts = parts.Length == 0;
        var chipsNoGenerator = parts.Any(f => f[1] == 'M' && !parts.Any(g => g[0] == f[0] && g[1] == 'G'));

        if (!(noGenerators || noParts || !chipsNoGenerator))
          return false;
      }
      return true;
    }
  }
}
