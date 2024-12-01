namespace AdventOfCode.Y2024.Day01;

using System;
using System.Collections.Generic;
using System.Linq;

[ProblemName("Historian Hysteria")]
class Solution : Solver {

    public object PartOne(string input) {
        var left = new List<int>();
        var right = new List<int>();

        var rows = input
            .ReplaceLineEndings()
            .Split(Environment.NewLine, StringSplitOptions.None);

        foreach (var item in rows) {
            var items = item.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            left.Add(int.Parse(items[0]));
            right.Add(int.Parse(items[1]));
        }
        
        left.Sort();
        right.Sort();

        int result = left.Select((t, i) => Math.Abs(t - right[i])).Sum();

        return result;
    }

    public object PartTwo(string input) {
        return 0;
    }
}
