using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace DevelopmentTracker.Model.Extensions;

public static partial class StringExtensions
{
    public static string ToSentenceCase(this string str)
    {
        return MatchSmallCapital().Replace(str, m => $"{m.Value[0]} {char.ToLower(m.Value[1])}");
    }

    public static string ToTitleCase(this string str)
    {
        return MatchSmallCapital().Replace(str, m => $"{m.Value[0]} {m.Value[1]}");
    }

    public static string ToPascalCase(this string str)
    {
        var invalidCharsRgx = MatchInvalidCharsRegex();
        var whiteSpace = MatchWhiteSpace();
        var startsWithLowerCaseChar = MatchStartsWithLowerCaseChar();
        var firstCharFollowedByUpperCasesOnly = MatchFirstCharFollowedByUpperCasesOnly();
        var lowerCaseNextToNumber = MatchLowerCaseNextToNumber();
        var upperCaseInside = MatchUpperCaseInside();

        // replace white spaces with undescore, then replace all invalid chars with empty string
        var pascalCase = invalidCharsRgx.Replace(whiteSpace.Replace(str, "_"), string.Empty)
            // split by underscores
            .Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries)
            // set first letter to uppercase
            .Select(w => startsWithLowerCaseChar.Replace(w, m => m.Value.ToUpper()))
            // replace second and all following upper case letters to lower if there is no next lower (ABC -> Abc)
            .Select(w => firstCharFollowedByUpperCasesOnly.Replace(w, m => m.Value.ToLower()))
            // set upper case the first lower case following a number (Ab9cd -> Ab9Cd)
            .Select(w => lowerCaseNextToNumber.Replace(w, m => m.Value.ToUpper()))
            // lower second and next upper case letters except the last if it follows by any lower (ABcDEf -> AbcDef)
            .Select(w => upperCaseInside.Replace(w, m => m.Value.ToLower()));

        return string.Concat(pascalCase);
    }

    [GeneratedRegex("[a-z][A-Z]")]
    private static partial Regex MatchSmallCapital();

    [GeneratedRegex("[^_a-zA-Z0-9]")]
    private static partial Regex MatchInvalidCharsRegex();

    [GeneratedRegex("(?<=\\s)")]
    private static partial Regex MatchWhiteSpace();

    [GeneratedRegex("^[a-z]")]
    private static partial Regex MatchStartsWithLowerCaseChar();
    [GeneratedRegex("(?<=[A-Z])[A-Z0-9]+$")]
    private static partial Regex MatchFirstCharFollowedByUpperCasesOnly();

    [GeneratedRegex("(?<=[0-9])[a-z]")]
    private static partial Regex MatchLowerCaseNextToNumber();

    [GeneratedRegex("(?<=[A-Z])[A-Z]+?((?=[A-Z][a-z])|(?=[0-9]))")]
    private static partial Regex MatchUpperCaseInside();
}