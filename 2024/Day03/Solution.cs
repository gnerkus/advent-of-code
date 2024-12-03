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
        var instructions = IntermediateInputParser(input);
        var pairs = new List<int[]>();
        var isDisabled = false;

        foreach (var instr in instructions) {
            var instructionType = GetInstruction(instr);
            switch (instructionType)
            {
                case "don't":
                    isDisabled = true;
                    continue;
                case "do":
                    isDisabled = false;
                    continue;
                case "mul": {
                    if (!isDisabled) {
                        var pair = GetPair(instr);
                        pairs.Add(pair);
                    }

                    break;
                }
            }
        }
        
        return pairs.Select(pair => pair[0] * pair[1]).Sum();
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

    private static IEnumerable<string> IntermediateInputParser(string input) {
        string delimiter = @"mul\(\d{1,3},\d{1,3}\)|do\(\)|don't\(\)";
        var matchCollection = Regex.Matches(input, delimiter);
	
        return matchCollection.Select(match => match.Value);
    }
    
    private static string GetInstruction(string input) {
        string delimiter = @"(.+)\(.*\)";
        var matches = Regex.Match(input, delimiter);
        return matches.Groups[1].Value;
    }

    private static int[] GetPair(string input) {
        string instrDelimiter = @"\d{1,3}";
        var matches = Regex.Matches(input, instrDelimiter);
        return matches.Select(match => int.Parse(match.Value)).ToArray();
    }
}
