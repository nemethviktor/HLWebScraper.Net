using System.Text.RegularExpressions;

namespace HLWebScraper.Net.Helpers;

public static class HelperStringUtils
{
    /// <summary>
    ///     Clears the input text from/of html utf stuff.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string ClearUTFChars(string input)
    {
        return input
              .Replace(oldValue: "&#32;", newValue: "")
              .Replace(oldValue: "&#33;", newValue: "!")
              .Replace(oldValue: "&#34;", newValue: "\"")
              .Replace(oldValue: "&#35;", newValue: "#")
              .Replace(oldValue: "&#36;", newValue: "$")
              .Replace(oldValue: "&#37;", newValue: "%")
              .Replace(oldValue: "&amp;", newValue: "&")
              .Replace(oldValue: "&#38;", newValue: "&")
              .Replace(oldValue: "&#39;", newValue: "'")
              .Replace(oldValue: "&#40;", newValue: "(")
              .Replace(oldValue: "&#41;", newValue: ")")
              .Replace(oldValue: "&#42;", newValue: "*")
              .Replace(oldValue: "&#43;", newValue: "+")
              .Replace(oldValue: "&#44;", newValue: ",")
              .Replace(oldValue: "&#45;", newValue: "-")
              .Replace(oldValue: "&#46;", newValue: ".")
              .Replace(oldValue: "&#47;", newValue: "/")
              .Replace(oldValue: "&#48;", newValue: "0")
              .Replace(oldValue: "&#49;", newValue: "1")
              .Replace(oldValue: "&#50;", newValue: "2")
              .Replace(oldValue: "&#51;", newValue: "3")
              .Replace(oldValue: "&#52;", newValue: "4")
              .Replace(oldValue: "&#53;", newValue: "5")
              .Replace(oldValue: "&#54;", newValue: "6")
              .Replace(oldValue: "&#55;", newValue: "7")
              .Replace(oldValue: "&#56;", newValue: "8")
              .Replace(oldValue: "&#57;", newValue: "9")
              .Replace(oldValue: "&#58;", newValue: ":")
              .Replace(oldValue: "&#59;", newValue: ";")
              .Replace(oldValue: "&lt;", newValue: "<")
              .Replace(oldValue: "&#60;", newValue: "<")
              .Replace(oldValue: "&#61;", newValue: "=")
              .Replace(oldValue: "&gt;", newValue: ">")
              .Replace(oldValue: "&#62;", newValue: ">")
              .Replace(oldValue: "&#63;", newValue: "?")
              .Replace(oldValue: "&#64;", newValue: "@")
              .Replace(oldValue: "&#65;", newValue: "A")
              .Replace(oldValue: "&#66;", newValue: "B")
              .Replace(oldValue: "&#67;", newValue: "C")
              .Replace(oldValue: "&#68;", newValue: "D")
              .Replace(oldValue: "&#69;", newValue: "E")
              .Replace(oldValue: "&#70;", newValue: "F")
              .Replace(oldValue: "&#71;", newValue: "G")
              .Replace(oldValue: "&#72;", newValue: "H")
              .Replace(oldValue: "&#73;", newValue: "I")
              .Replace(oldValue: "&#74;", newValue: "J")
              .Replace(oldValue: "&#75;", newValue: "K")
              .Replace(oldValue: "&#76;", newValue: "L")
              .Replace(oldValue: "&#77;", newValue: "M")
              .Replace(oldValue: "&#78;", newValue: "N")
              .Replace(oldValue: "&#79;", newValue: "O")
              .Replace(oldValue: "&#80;", newValue: "P")
              .Replace(oldValue: "&#81;", newValue: "Q")
              .Replace(oldValue: "&#82;", newValue: "R")
              .Replace(oldValue: "&#83;", newValue: "S")
              .Replace(oldValue: "&#84;", newValue: "T")
              .Replace(oldValue: "&#85;", newValue: "U")
              .Replace(oldValue: "&#86;", newValue: "V")
              .Replace(oldValue: "&#87;", newValue: "W")
              .Replace(oldValue: "&#88;", newValue: "X")
              .Replace(oldValue: "&#89;", newValue: "Y")
              .Replace(oldValue: "&#90;", newValue: "Z")
              .Replace(oldValue: "&#91;", newValue: "[")
              .Replace(oldValue: "&#92;", newValue: "\\")
              .Replace(oldValue: "&#93;", newValue: "]")
              .Replace(oldValue: "&#94;", newValue: "^")
              .Replace(oldValue: "&#95;", newValue: "_")
              .Replace(oldValue: "&#96;", newValue: "`")
              .Replace(oldValue: "&#97;", newValue: "a")
              .Replace(oldValue: "&#98;", newValue: "b")
              .Replace(oldValue: "&#99;", newValue: "c")
              .Replace(oldValue: "&#100;", newValue: "d")
              .Replace(oldValue: "&#101;", newValue: "e")
              .Replace(oldValue: "&#102;", newValue: "f")
              .Replace(oldValue: "&#103;", newValue: "g")
              .Replace(oldValue: "&#104;", newValue: "h")
              .Replace(oldValue: "&#105;", newValue: "i")
              .Replace(oldValue: "&#106;", newValue: "j")
              .Replace(oldValue: "&#107;", newValue: "k")
              .Replace(oldValue: "&#108;", newValue: "l")
              .Replace(oldValue: "&#109;", newValue: "m")
              .Replace(oldValue: "&#110;", newValue: "n")
              .Replace(oldValue: "&#111;", newValue: "o")
              .Replace(oldValue: "&#112;", newValue: "p")
              .Replace(oldValue: "&#113;", newValue: "q")
              .Replace(oldValue: "&#114;", newValue: "r")
              .Replace(oldValue: "&#115;", newValue: "s")
              .Replace(oldValue: "&#116;", newValue: "t")
              .Replace(oldValue: "&#117;", newValue: "u")
              .Replace(oldValue: "&#118;", newValue: "v")
              .Replace(oldValue: "&#119;", newValue: "w")
              .Replace(oldValue: "&#120;", newValue: "x")
              .Replace(oldValue: "&#121;", newValue: "y")
              .Replace(oldValue: "&#122;", newValue: "z")
              .Replace(oldValue: "&#123;", newValue: "{")
              .Replace(oldValue: "&#124;", newValue: "|")
              .Replace(oldValue: "&#125;", newValue: "}")
              .Replace(oldValue: "&#126;", newValue: "~")
              .Replace(oldValue: "&#32;", newValue: "")
              .Replace(oldValue: "&#33;", newValue: "!")
              .Replace(oldValue: "&#34;", newValue: "\"")
              .Replace(oldValue: "&#35;", newValue: "#")
              .Replace(oldValue: "&#36;", newValue: "$")
              .Replace(oldValue: "&#37;", newValue: "%")
              .Replace(oldValue: "&amp;", newValue: "&")
              .Replace(oldValue: "&#38;", newValue: "&")
              .Replace(oldValue: "&#39;", newValue: "'")
              .Replace(oldValue: "&#40;", newValue: "(")
              .Replace(oldValue: "&#41;", newValue: ")")
              .Replace(oldValue: "&#42;", newValue: "*")
              .Replace(oldValue: "&#43;", newValue: "+")
              .Replace(oldValue: "&#44;", newValue: ",")
              .Replace(oldValue: "&#45;", newValue: "-")
              .Replace(oldValue: "&#46;", newValue: ".")
              .Replace(oldValue: "&#47;", newValue: "/")
              .Replace(oldValue: "&#48;", newValue: "0")
              .Replace(oldValue: "&#49;", newValue: "1")
              .Replace(oldValue: "&#50;", newValue: "2")
              .Replace(oldValue: "&#51;", newValue: "3")
              .Replace(oldValue: "&#52;", newValue: "4")
              .Replace(oldValue: "&#53;", newValue: "5")
              .Replace(oldValue: "&#54;", newValue: "6")
              .Replace(oldValue: "&#55;", newValue: "7")
              .Replace(oldValue: "&#56;", newValue: "8")
              .Replace(oldValue: "&#57;", newValue: "9")
              .Replace(oldValue: "&#58;", newValue: ":")
              .Replace(oldValue: "&#59;", newValue: ";")
              .Replace(oldValue: "&lt;", newValue: "<")
              .Replace(oldValue: "&#60;", newValue: "<")
              .Replace(oldValue: "&#61;", newValue: "=")
              .Replace(oldValue: "&gt;", newValue: ">")
              .Replace(oldValue: "&#62;", newValue: ">")
              .Replace(oldValue: "&#63;", newValue: "?")
              .Replace(oldValue: "&#64;", newValue: "@")
              .Replace(oldValue: "&#65;", newValue: "A")
              .Replace(oldValue: "&#66;", newValue: "B")
              .Replace(oldValue: "&#67;", newValue: "C")
              .Replace(oldValue: "&#68;", newValue: "D")
              .Replace(oldValue: "&#69;", newValue: "E")
              .Replace(oldValue: "&#70;", newValue: "F")
              .Replace(oldValue: "&#71;", newValue: "G")
              .Replace(oldValue: "&#72;", newValue: "H")
              .Replace(oldValue: "&#73;", newValue: "I")
              .Replace(oldValue: "&#74;", newValue: "J")
              .Replace(oldValue: "&#75;", newValue: "K")
              .Replace(oldValue: "&#76;", newValue: "L")
              .Replace(oldValue: "&#77;", newValue: "M")
              .Replace(oldValue: "&#78;", newValue: "N")
              .Replace(oldValue: "&#79;", newValue: "O")
              .Replace(oldValue: "&#80;", newValue: "P")
              .Replace(oldValue: "&#81;", newValue: "Q")
              .Replace(oldValue: "&#82;", newValue: "R")
              .Replace(oldValue: "&#83;", newValue: "S")
              .Replace(oldValue: "&#84;", newValue: "T")
              .Replace(oldValue: "&#85;", newValue: "U")
              .Replace(oldValue: "&#86;", newValue: "V")
              .Replace(oldValue: "&#87;", newValue: "W")
              .Replace(oldValue: "&#88;", newValue: "X")
              .Replace(oldValue: "&#89;", newValue: "Y")
              .Replace(oldValue: "&#90;", newValue: "Z")
              .Replace(oldValue: "&#91;", newValue: "[")
              .Replace(oldValue: "&#92;", newValue: "\\")
              .Replace(oldValue: "&#93;", newValue: "]")
              .Replace(oldValue: "&#94;", newValue: "^")
              .Replace(oldValue: "&#95;", newValue: "_")
              .Replace(oldValue: "&#96;", newValue: "`")
              .Replace(oldValue: "&#97;", newValue: "a")
              .Replace(oldValue: "&#98;", newValue: "b")
              .Replace(oldValue: "&#99;", newValue: "c")
              .Replace(oldValue: "&#100;", newValue: "d")
              .Replace(oldValue: "&#101;", newValue: "e")
              .Replace(oldValue: "&#102;", newValue: "f")
              .Replace(oldValue: "&#103;", newValue: "g")
              .Replace(oldValue: "&#104;", newValue: "h")
              .Replace(oldValue: "&#105;", newValue: "i")
              .Replace(oldValue: "&#106;", newValue: "j")
              .Replace(oldValue: "&#107;", newValue: "k")
              .Replace(oldValue: "&#108;", newValue: "l")
              .Replace(oldValue: "&#109;", newValue: "m")
              .Replace(oldValue: "&#110;", newValue: "n")
              .Replace(oldValue: "&#111;", newValue: "o")
              .Replace(oldValue: "&#112;", newValue: "p")
              .Replace(oldValue: "&#113;", newValue: "q")
              .Replace(oldValue: "&#114;", newValue: "r")
              .Replace(oldValue: "&#115;", newValue: "s")
              .Replace(oldValue: "&#116;", newValue: "t")
              .Replace(oldValue: "&#117;", newValue: "u")
              .Replace(oldValue: "&#118;", newValue: "v")
              .Replace(oldValue: "&#119;", newValue: "w")
              .Replace(oldValue: "&#120;", newValue: "x")
              .Replace(oldValue: "&#121;", newValue: "y")
              .Replace(oldValue: "&#122;", newValue: "z")
              .Replace(oldValue: "&#123;", newValue: "{")
              .Replace(oldValue: "&#124;", newValue: "|")
              .Replace(oldValue: "&#125;", newValue: "}")
              .Replace(oldValue: "&#126;", newValue: "~")
              .Replace(oldValue: "&nbsp;", newValue: "")
              .Replace(oldValue: "&#160;", newValue: "")
              .Replace(oldValue: "&iexcl;", newValue: "¡")
              .Replace(oldValue: "&#161;", newValue: "¡")
              .Replace(oldValue: "&cent;", newValue: "¢")
              .Replace(oldValue: "&#162;", newValue: "¢")
              .Replace(oldValue: "&pound;", newValue: "£")
              .Replace(oldValue: "&#163;", newValue: "£")
              .Replace(oldValue: "&curren;", newValue: "¤")
              .Replace(oldValue: "&#164;", newValue: "¤")
              .Replace(oldValue: "&yen;", newValue: "¥")
              .Replace(oldValue: "&#165;", newValue: "¥")
              .Replace(oldValue: "&brvbar;", newValue: "¦")
              .Replace(oldValue: "&#166;", newValue: "¦")
              .Replace(oldValue: "&sect;", newValue: "§")
              .Replace(oldValue: "&#167;", newValue: "§")
              .Replace(oldValue: "&uml;", newValue: "¨")
              .Replace(oldValue: "&#168;", newValue: "¨")
              .Replace(oldValue: "&copy;", newValue: "©")
              .Replace(oldValue: "&#169;", newValue: "©")
              .Replace(oldValue: "&ordf;", newValue: "ª")
              .Replace(oldValue: "&#170;", newValue: "ª")
              .Replace(oldValue: "&laquo;", newValue: "«")
              .Replace(oldValue: "&#171;", newValue: "«")
              .Replace(oldValue: "&not;", newValue: "¬")
              .Replace(oldValue: "&#172;", newValue: "¬")
              .Replace(oldValue: "&shy;", newValue: "­")
              .Replace(oldValue: "&#173;", newValue: "­")
              .Replace(oldValue: "&reg;", newValue: "®")
              .Replace(oldValue: "&#174;", newValue: "®")
              .Replace(oldValue: "&macr;", newValue: "¯")
              .Replace(oldValue: "&#175;", newValue: "¯")
              .Replace(oldValue: "&deg;", newValue: "°")
              .Replace(oldValue: "&#176;", newValue: "°")
              .Replace(oldValue: "&plusmn;", newValue: "±")
              .Replace(oldValue: "&#177;", newValue: "±")
              .Replace(oldValue: "&sup2;", newValue: "²")
              .Replace(oldValue: "&#178;", newValue: "²")
              .Replace(oldValue: "&sup3;", newValue: "³")
              .Replace(oldValue: "&#179;", newValue: "³")
              .Replace(oldValue: "&acute;", newValue: "´")
              .Replace(oldValue: "&#180;", newValue: "´")
              .Replace(oldValue: "&micro;", newValue: "µ")
              .Replace(oldValue: "&#181;", newValue: "µ")
              .Replace(oldValue: "&frac14;", newValue: "¼")
              .Replace(oldValue: "&#188;", newValue: "¼")
              .Replace(oldValue: "&frac12;", newValue: "½")
              .Replace(oldValue: "&#189;", newValue: "½")
              .Replace(oldValue: "&frac34;", newValue: "¾")
              .Replace(oldValue: "&#190;", newValue: "¾")
              .Replace(oldValue: "&Scaron;", newValue: "Š")
              .Replace(oldValue: "&#352;", newValue: "Š")
              .Replace(oldValue: "&scaron;", newValue: "š")
              .Replace(oldValue: "&#353;", newValue: "š")
              .Replace(oldValue: "&Yuml;", newValue: "Ÿ")
              .Replace(oldValue: "&#376;", newValue: "Ÿ")
              .Replace(oldValue: "&fnof;", newValue: "ƒ")
              .Replace(oldValue: "&#402;", newValue: "ƒ")
              .Replace(oldValue: "&circ;", newValue: "ˆ")
              .Replace(oldValue: "&#710;", newValue: "ˆ")
              .Replace(oldValue: "&tilde;", newValue: "˜")
              .Replace(oldValue: "&#732;", newValue: "˜")
              .Replace(oldValue: "&ensp;", newValue: " ")
              .Replace(oldValue: "&#8194;", newValue: " ")
              .Replace(oldValue: "&emsp;", newValue: " ")
              .Replace(oldValue: "&#8195;", newValue: " ")
              .Replace(oldValue: "&thinsp;", newValue: " ")
              .Replace(oldValue: "&#8201;", newValue: " ")
              .Replace(oldValue: "&zwnj;", newValue: "‌")
              .Replace(oldValue: "&#8204;", newValue: "‌")
              .Replace(oldValue: "&zwj;", newValue: "‍")
              .Replace(oldValue: "&#8205;", newValue: "‍")
              .Replace(oldValue: "&lrm;", newValue: "‎")
              .Replace(oldValue: "&#8206;", newValue: "‎")
              .Replace(oldValue: "&rlm;", newValue: "‏")
              .Replace(oldValue: "&#8207;", newValue: "‏")
              .Replace(oldValue: "&ndash;", newValue: "–")
              .Replace(oldValue: "&#8211;", newValue: "–")
              .Replace(oldValue: "&mdash;", newValue: "—")
              .Replace(oldValue: "&#8212;", newValue: "—")
              .Replace(oldValue: "&lsquo;", newValue: "‘")
              .Replace(oldValue: "&#8216;", newValue: "‘")
              .Replace(oldValue: "&rsquo;", newValue: "’")
              .Replace(oldValue: "&#8217;", newValue: "’")
              .Replace(oldValue: "&sbquo;", newValue: "‚")
              .Replace(oldValue: "&#8218;", newValue: "‚")
              .Replace(oldValue: "&ldquo;", newValue: "“")
              .Replace(oldValue: "&#8220;", newValue: "“")
              .Replace(oldValue: "&rdquo;", newValue: "”")
              .Replace(oldValue: "&#8221;", newValue: "”")
              .Replace(oldValue: "&bdquo;", newValue: "„")
              .Replace(oldValue: "&#8222;", newValue: "„")
              .Replace(oldValue: "&dagger;", newValue: "†")
              .Replace(oldValue: "&#8224;", newValue: "†")
              .Replace(oldValue: "&Dagger;", newValue: "‡")
              .Replace(oldValue: "&#8225;", newValue: "‡")
              .Replace(oldValue: "&bull;", newValue: "•")
              .Replace(oldValue: "&#8226;", newValue: "•")
              .Replace(oldValue: "&hellip;", newValue: "…")
              .Replace(oldValue: "&#8230;", newValue: "…")
              .Replace(oldValue: "&permil;", newValue: "‰")
              .Replace(oldValue: "&#8240;", newValue: "‰")
              .Replace(oldValue: "&prime;", newValue: "′")
              .Replace(oldValue: "&#8242;", newValue: "′")
              .Replace(oldValue: "&Prime;", newValue: "″")
              .Replace(oldValue: "&#8243;", newValue: "″")
              .Replace(oldValue: "&lsaquo;", newValue: "‹")
              .Replace(oldValue: "&#8249;", newValue: "‹")
              .Replace(oldValue: "&rsaquo;", newValue: "›")
              .Replace(oldValue: "&#8250;", newValue: "›")
              .Replace(oldValue: "&oline;", newValue: "‾")
              .Replace(oldValue: "&#8254;", newValue: "‾")
              .Replace(oldValue: "&euro;", newValue: "€")
              .Replace(oldValue: "&#8364;", newValue: "€")
              .Replace(oldValue: "&trade;", newValue: "™")
              .Replace(oldValue: "&#8482;", newValue: "™");
    }

    /// <summary>
    ///     Trims intra-spaces
    ///     via https://stackoverflow.com/a/58915116/3968494
    /// </summary>
    internal static string TrimInternalSpaces(string s)
    {
        char[] dst = new char[s.Length];
        uint end = 0;
        char prev = char.MinValue;
        for (int k = 0; k < s.Length; ++k)
        {
            char c = s[index: k];
            dst[end] = c;

            // We'll move forward if the current character is not ' ' or if prev char is not ' '
            // To avoid 'if' let's get diffs for c and prev and then use bitwise operatios to get 
            // 0 if n is 0 or 1 if n is non-zero
            uint x = (uint)(' ' - c) + (uint)(' ' - prev); // non zero if any non-zero

            end += ((x | (~x + 1)) >> 31) &
                   1; // https://stackoverflow.com/questions/3912112/check-if-a-number-is-non-zero-using-bitwise-operators-in-c by ruslik
            prev = c;
        }

        return new string(value: dst, startIndex: 0, length: (int)end);
    }

    /// <summary>
    ///     Finds text between two strings
    /// </summary>
    /// <param name="pageText"></param>
    /// <param name="textStart"></param>
    /// <param name="textEnd"></param>
    /// <returns></returns>
    public static string FindTextBetween(string pageText, string textStart, string textEnd)
    {
        int startIndex = pageText.IndexOf(value: textStart, comparisonType: StringComparison.Ordinal);
        if (startIndex == -1)
            // textStart not found
            return string.Empty;

        int endIndex = pageText.IndexOf(value: textEnd, startIndex: startIndex + textStart.Length,
            comparisonType: StringComparison.Ordinal);
        return endIndex == -1
            ?
            // textEnd not found or found before textStart
            string.Empty
            : pageText.Substring(startIndex: startIndex + textStart.Length,
                length: endIndex - (startIndex + textStart.Length)).Trim();
    }

    /// <summary>
    ///     Trims and removes extraneous spaces, newline and tab chars from a string.
    /// </summary>
    /// <param name="text">Text to clear up</param>
    /// <param name="removeBold">Whether to remove the '<strong></strong>' pairs.</param>
    /// <param name="removeDT">Whether to remove the '<dt></dt>' pairs.</param>
    /// <returns></returns>
    public static string TrimAndReplaceNewLinesAndTabs(string text, bool removeBold = true, bool removeDT = true)
    {
        string tmpStr = Regex.Replace(input: text, pattern: @"(\r\n?|\r?\n|\t)+", replacement: "").Trim();
        tmpStr = removeBold
            ? tmpStr.Replace(oldValue: "<strong>", newValue: "").Replace(oldValue: "</strong>", newValue: "")
            : tmpStr;
        tmpStr = removeDT
            ? tmpStr.Replace(oldValue: "<dt>", newValue: "").Replace(oldValue: "</dt>", newValue: "")
            : tmpStr;


        return tmpStr;
    }
}