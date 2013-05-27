using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HandyFunctions
{
    public static class DataValidation
    {
        public static Boolean ValidateInteger(string input)
        {
            int notUsed = 0;
            return int.TryParse(input, out notUsed);
        }
        public static Boolean ValidateDouble(string input, bool includeZero = false)
        {
            double notUsed = 0;
            return double.TryParse(input, out notUsed);
        }
        public static Boolean ValidateBooleanInteger(string input)
        {
            return ValidateInteger(input) && new int[] { 0, 1 }.Contains<int>(int.Parse(input));
        }

        public static Boolean ValidatePositiveInteger(string input, bool includeZero = false)
        {
            return ValidateInteger(input) && (includeZero ? Double.Parse(input) >= 0 : Double.Parse(input) > 0);
        }
        public static Boolean ValidateNegativeInteger(string input, bool includeZero = false)
        {
            return ValidateInteger(input) && (includeZero ? Double.Parse(input) <= 0 : Double.Parse(input) < 0);
        }
        
        public static Boolean ValidatePositiveDouble(string input, bool includeZero = false)
        {
            return ValidateDouble(input) && (includeZero ? Double.Parse(input) >= 0 : Double.Parse(input) > 0);
        }
        public static Boolean ValidateNegativeDouble(string input, bool includeZero = false)
        {
            return ValidateDouble(input) && (includeZero ? Double.Parse(input) <= 0 : Double.Parse(input) < 0);
        }

        public static Boolean ValidateBasedOnRegex(string input, string regex)
        {
            return Regex.IsMatch(input, regex);
        }
        public static Boolean ValidateBasedOnRegexList(string input, List<string> regexPatterns)
        {
            foreach (string regexPattern in regexPatterns)
            {
                if(!ValidateBasedOnRegex(input, regexPattern)){return false;}
            }

            // All regexes match
            return true;
        }
    }
}