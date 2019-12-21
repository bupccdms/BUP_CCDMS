using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace qms.Utility
{
    public class EnglishToBangla
    {

        public static string convertDigit(string input)
        {
            return input.Replace("0", "০")
            .Replace("1", "১")
            .Replace("2", "২")
            .Replace("3", "৩")
            .Replace("4", "৪")
            .Replace("5", "৫")
            .Replace("6", "৬")
            .Replace("7", "৭")
            .Replace("8", "৮")
            .Replace("9", "৯")
            .Replace("A", "এ")
            .Replace("B", "বি")
            .Replace("C", "সি")
            .Replace("D", "ডি")
            .Replace("E", "ই")
            .Replace("F", "এফ")
            .Replace("G", "জি")
            .Replace("H", "এইচ")
            .Replace("I", "আই")
            .Replace("J", "জে")
            .Replace("K", "কে")
            .Replace("L", "এল")
            .Replace("M", "এম")
            .Replace("N", "এন")
            .Replace("O", "ও")
            .Replace("P", "পি")
            .Replace("Q", "কিউ")
            .Replace("R", "আর")
            .Replace("S", "এস")
            .Replace("T", "টি")
            .Replace("U", "ইউ")
            .Replace("V", "ভি")
            .Replace("W", "ডব্লিও")
            .Replace("X", "এক্স")
            .Replace("Y", "ওয়াই")
            .Replace("Z", "জেড");
        }
    }
}