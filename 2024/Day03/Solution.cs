namespace AdventOfCode.Y2024.Day03;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

[ProblemName("Mull It Over")]
class Solution : Solver {

    public object PartOne(string input) {
        var pairs = InputParser(input);

        return pairs.Select(pair => pair[0] * pair[1]).Sum();
    }

    public object PartTwo(string input) {
        return 0;
    }

    private static IEnumerable<int[]> InputParser(string input) {
        string delimiter = @"mul\(\d{1,3},\d{1,3}\)";
        var matchCollection = Regex.Matches(input, delimiter);

        string instrDelimiter = @"\d{1,3}";

        return matchCollection.Select(instruction => {
            
            var matches = Regex.Matches(instruction.Value, instrDelimiter);
            return matches.Select(match => int.Parse(match.Value)).ToArray();
        });
    }
}
