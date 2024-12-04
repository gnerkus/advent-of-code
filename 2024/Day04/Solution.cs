namespace AdventOfCode.Y2024.Day04;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

[ProblemName("Ceres Search")]
class Solution : Solver {

    public object PartOne(string input) {
        var parsedInput = InputParser(input);
        return parsedInput.charArray.Select((item, pos) => {
            var cardinals = GetCardinals(parsedInput.charArray, pos, parsedInput.width);
            return cardinals.Count(cardinal => cardinal.Equals("xmas"));
        }).Sum();
    }

    public object PartTwo(string input) {
        var parsedInput = InputParser(input);

        return parsedInput.charArray.Select((item, pos) => {
            if (item != 'a') {
                return 0;
            }

            var cardinals = GetCrossCardinals(parsedInput.charArray, pos, parsedInput.width);
            return cardinals.Count(cardinal => cardinal.Equals("masmas"));
        }).Sum();
    }

    private static (int width, char[] charArray) InputParser(string input) {
        var rows = input
            .ReplaceLineEndings()
            .Split(Environment.NewLine, StringSplitOptions.None);
        var width = rows[0].Length;
        var charArray = Regex.Replace(input.ToLower(), @"\s+", "").ToArray();

        return (width, charArray);
    }

    private static IEnumerable<string> GetCardinals(char[] inputArray, int pos, int width) {
        var eastCheck = (pos + 3) % width < 3; // can't go east
        var westCheck = pos % width < 3;
        var northCheck = pos / width < 3; // can't go north
        var southCheck = (pos + (width * 3)) > (inputArray.Length - 1);
        
        var east = eastCheck ? "" : string.Join("", [
            inputArray[pos],
            inputArray[pos + 1],
            inputArray[pos + 2],
            inputArray[pos + 3],
        ]);
        
        var west = westCheck ? "" : string.Join("", [
            inputArray[pos],
            inputArray[pos - 1],
            inputArray[pos - 2],
            inputArray[pos - 3],
        ]);
        
        var north = northCheck ? "" : string.Join("", [
            inputArray[pos],
            inputArray[pos - width],
            inputArray[pos - (width * 2)],
            inputArray[pos - (width * 3)],
        ]);
        
        var south = southCheck ? "" : string.Join("", [
            inputArray[pos],
            inputArray[pos + width],
            inputArray[pos + (width * 2)],
            inputArray[pos + (width * 3)],
        ]);
        
        var northeast = northCheck || eastCheck ? "" : string.Join("", [
            inputArray[pos],
            inputArray[pos - width + 1],
            inputArray[pos - (width * 2) + 2],
            inputArray[pos - (width * 3) + 3],
        ]);
        
        var northwest = northCheck || westCheck ? "" : string.Join("", [
            inputArray[pos],
            inputArray[pos - width - 1],
            inputArray[pos - (width * 2) - 2],
            inputArray[pos - (width * 3) - 3],
        ]);
        
        var southeast = southCheck || eastCheck ? "" : string.Join("", [
            inputArray[pos],
            inputArray[pos + width + 1],
            inputArray[pos + (width * 2) + 2],
            inputArray[pos + (width * 3) + 3],
        ]);
        
        var southwest = southCheck || westCheck ? "" : string.Join("", [
            inputArray[pos],
            inputArray[pos + width - 1],
            inputArray[pos + (width * 2) - 2],
            inputArray[pos + (width * 3) - 3],
        ]);

        return [
            east,
            west,
            north,
            south,
            northeast,
            northwest,
            southeast,
            southwest
        ];
    }

    private static IEnumerable<string> GetCrossCardinals(char[] inputArray, int pos, int width) {
        var eastCheck = (pos + 1) % width < 1; // can't go east
        var westCheck = pos % width < 1;
        var northCheck = pos / width < 1; // can't go north
        var southCheck = (pos + (width * 1)) > (inputArray.Length - 1);
        var northEastCheck = northCheck || eastCheck;
        var southEastCheck = southCheck || eastCheck;
        var northWestCheck = northCheck || westCheck;
        var southWestCheck = southCheck || westCheck;

        var invalid = northEastCheck || northWestCheck || southEastCheck || southWestCheck;
        
        var down = invalid ? "" : string.Join("", [
            inputArray[pos - width - 1],
            inputArray[pos],
            inputArray[pos + width + 1],
            inputArray[pos - width + 1],
            inputArray[pos],
            inputArray[pos + width - 1]
        ]);
        
        var right = invalid ? "" : string.Join("", [
            inputArray[pos - width - 1],
            inputArray[pos],
            inputArray[pos + width + 1],
            inputArray[pos + width - 1],
            inputArray[pos],
            inputArray[pos - width + 1]
        ]);
        
        var up = invalid ? "" : string.Join("", [
            inputArray[pos + width - 1],
            inputArray[pos],
            inputArray[pos - width + 1],
            inputArray[pos + width + 1],
            inputArray[pos],
            inputArray[pos - width - 1],
        ]);
        
        var left = invalid ? "" : string.Join("", [
            inputArray[pos - width + 1],
            inputArray[pos],
            inputArray[pos + width - 1],
            inputArray[pos + width + 1],
            inputArray[pos],
            inputArray[pos - width - 1]
        ]);
        
        return [
            down,
            right,
            up,
            left
        ];
    }
}
