using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace SmarthutPOC.Helpers
{
    public class RegexMatcher
    {
        /// <summary>
        /// Searches and return the first occurance of an Guid inside a string. 
        /// </summary>
        /// <param name="input"></param>
        /// <returns>The guid contained in the string</returns>
        public static Guid CheckAndGetGuid(string input)
        {
            Match match = Regex.Match(input, @"(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}",
                RegexOptions.IgnoreCase);
            if (match.Success) return Guid.Parse(match.Value);

            return Guid.Empty;
        }
        
        /// <summary>
        /// Searches and return the first occurance of an email inside a string. 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string CheckAndGetEmail(string input)
        {
            Match match = Regex.Match(input, @"([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})",
                RegexOptions.IgnoreCase);
            if (match.Success) return match.Value;

            return null;
        }
    }
}