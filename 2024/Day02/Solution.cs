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
        var reportList = InputParser(input);
        return reportList.Count(IsSafeWithTolerance);

        // correct answer
        // return reportList.Count(item => Attenuate(item).Any(IsSafe));
    }

    private static List<List<int>> InputParser(string input) {
        var rows = input
            .ReplaceLineEndings()
            .Split(Environment.NewLine, StringSplitOptions.None);

        return rows.Select(row => row.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int
            .Parse).ToList()).ToList();
    }
    
    /**
     * When a problem involves checking if the input list is still valid if one or more items are removed,
     * try to generate multiple permutations of that input list
     */
    IEnumerable<int[]> Attenuate(int[] samples) =>
        from i in Enumerable.Range(0, samples.Length+1)
        let before = samples.Take(i - 1)
        let after = samples.Skip(i)
        select before.Concat(after).ToArray();

    private static bool IsSafeWithTolerance(List<int> report) {
        var adjacencyDiffs = new List<int>();
        
        for (int i = 1; i < report.Count; i++) {
            adjacencyDiffs.Add(report[i] - report[i - 1]);
        }
        
        // remove one 0
        var fixedReport = adjacencyDiffs.Remove(0);
        
        // check if strictly increasing
        var positiveCount = adjacencyDiffs.Count(item => item > 0);
        if (positiveCount < adjacencyDiffs.Count && positiveCount != 0) {
            if (fixedReport) {
                return false;
            }

            adjacencyDiffs = adjacencyDiffs.Where(item => item > 0).ToList();
            fixedReport = true;
        }
        
        // check if strictly decreasing
        var negativeCount = adjacencyDiffs.Count(item => item < 0);
        if (negativeCount < adjacencyDiffs.Count && negativeCount != 0) {
            if (fixedReport) {
                return false;
            }

            adjacencyDiffs = adjacencyDiffs.Where(item => item < 0).ToList();
            fixedReport = true;
        }

        // check if there are multiple diffs greater than 3
        var absCount = adjacencyDiffs.Count(item => Math.Abs(item) > 3);
        if (absCount > 1) {
            return false;
        }
        
        // check for the position of the diff greater than 3
        var absIndex = adjacencyDiffs.FindIndex(item => Math.Abs(item) > 3);
        if (absIndex > 0 && absIndex < adjacencyDiffs.Count - 1) {
            if (fixedReport) {
                return false;
            }

            var diff = adjacencyDiffs[absIndex] + adjacencyDiffs[absIndex - 1];
            if (Math.Abs(diff) > 3) {
                return false;
            }
        } else if (absIndex != -1) {
            if (fixedReport) {
                return false;
            }
        }

        return true;
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
