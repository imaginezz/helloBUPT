using System.Text.RegularExpressions;


class RegLib {
    public static string RegexMatch(string regStr, ref string input) {
        Regex reg = new Regex(regStr);
        Match match = reg.Match(input);
        return match.Value;
    }
}
