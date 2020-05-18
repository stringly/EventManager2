using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Extensions
{
    public static class ExtensionMethods
    {
        public static string ToLowerRemoveSymbols(this String str)
        {
            if (string.IsNullOrEmpty(str)) { return str; }
            char[] arr = str.ToCharArray();
            arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c)
                              || char.IsWhiteSpace(c)
                              || c == '-')));
            string lowerString = new string(arr);
            return lowerString.ToLower();
        }
    }
}
