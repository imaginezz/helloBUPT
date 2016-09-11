using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;


class RegLib {
    public static string RegexMatch(string regStr, ref string input) {
        Regex reg = new Regex(regStr);
        Match match = reg.Match(input);
        return match.Value;
    }
}
