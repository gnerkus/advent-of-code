using System.Data;

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
        
        var fixedReport = false;
        
        // check if there are multiple diffs greater than 3
        var absCount = adjacencyDiffs.Count(item => Math.Abs(item) > 3);
        if (absCount > 1) {
            return false;
        }
        
        // check if strictly increasing or decreasing
        var positiveCount = adjacencyDiffs.Count(item => item > 0);
        if (positiveCount >= 1 && positiveCount < adjacencyDiffs.Count) {
            if (positiveCount == 1) {
                // decreasing, see if we can remove the positive number
                var posIndex = adjacencyDiffs.FindIndex(item => item > 0);
                if (posIndex != -1) {
                    adjacencyDiffs = Merge(adjacencyDiffs, posIndex);
                    // if there's still a positive diff
                    if (adjacencyDiffs.FindIndex(item => item > 0) >= 0) {
                        return false;
                    }
                    fixedReport = true;
                }
                
            } else if (positiveCount == adjacencyDiffs.Count - 1) {
                // increasing, see if we can remove the negative number
                var negIndex = adjacencyDiffs.FindIndex(item => item < 0);
                if (negIndex != -1) {
                    adjacencyDiffs = Merge(adjacencyDiffs, negIndex);
                    // if there's still a negative diff
                    if (adjacencyDiffs.FindIndex(item => item < 0) >= 0) {
                        return false;
                    }
                    fixedReport = true;
                }
                

            } else {
                return false;
            }
        }
        
        // check for the position of the diff greater than 3
        var absIndex = adjacencyDiffs.FindIndex(item => Math.Abs(item) > 3);
        if (absIndex > 0 && absIndex < adjacencyDiffs.Count - 1) {
            return false;
        }
        
        if (absIndex != -1) {
            if (fixedReport)
                return false;

            fixedReport = true;
        }
        
        // remove one 0
        var removeDuplicate = adjacencyDiffs.Remove(0);

        return !(fixedReport && removeDuplicate);
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

    private static List<int> Merge(List<int> source, int index) {
        var count = source.Count;

        if (index == count - 1) {
            var left = source[index] + source[index - 1];
            var right = source[index - 1];
            source.RemoveAt(index);
            var smaller = Math.Min(Math.Abs(left), Math.Abs(right));
            var newValue = smaller == Math.Abs(left) && smaller != 0 ? left : right;
            source[index - 1] = newValue;
            return source;
        }

        if (index == 0) {
            var left = source[1];
            var right = source[0] + source[1];
            source.RemoveAt(index);
            var smaller = Math.Min(Math.Abs(left), Math.Abs(right));
            var newValue = smaller == Math.Abs(left) && smaller != 0 ? left : right;
            source[index] = newValue;
            return source;
        }
        
        var l = source[index] + source[index - 1];
        var r = source[index] + source[index + 1];
        source.RemoveAt(index);
        var s = Math.Min(Math.Abs(l), Math.Abs(r));
        var n = s == Math.Abs(l) && s != 0 ? l : r;
        source[index] = n;
        return source;
    }
}
