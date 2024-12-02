namespace AdventOfCode.Y2024.Day02;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

[ProblemName("Red-Nosed Reports")]
class Solution : Solver {

    public object PartOne(string input) {
        var reportList = InputParser(input);

        return reportList.Count(IsSafe);
    }

    public object PartTwo(string input) {
        return 0;
    }

    private static List<List<int>> InputParser(string input) {
        var rows = input
            .ReplaceLineEndings()
            .Split(Environment.NewLine, StringSplitOptions.None);

        return rows.Select(row => row.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int
            .Parse).ToList()).ToList();
    }

    private static bool IsSafe(List<int> report) {
        if (!IsDiffSafe(report[1], report[0]))
            return false;
        
        var isIncreasing = report[1] > report[0];

        for (int i = 2; i < report.Count; i++) {
            if (!IsDiffSafe(report[i], report[i - 1]))
                return false;

            if ((report[i] > report[i - 1]) && !isIncreasing)
                return false;
            
            if ((report[i] < report[i - 1]) && isIncreasing)
                return false;
        }
        
        return true;
    }

    private static bool IsDiffSafe(int current, int prev) {
        if (current == prev)
            return false;

        return Math.Abs(current - prev) is <= 3 and >= 1;
    }
}
