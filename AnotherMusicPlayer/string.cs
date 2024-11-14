using System.Data;

namespace CustomExtensions
{
    //Extension methods must be defined in a static class
    public static class StringExtension
    {
        // This is the extension method.
        // The first parameter takes the "this" modifier
        public static string Capitalize(this string input)
        {
            if (input == null || input.Length == 0) { return ""; }
            if (input.Length == 1) { return "" + char.ToUpper(input[0]); }
            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }

        public static string CapitalizeWords(this string input, char separator = ' ')
        {
            if (input == null || input.Length == 0) { return ""; }
            if (input.Length == 1) { return "" + char.ToUpper(input[0]); }

            string[] sts = input.Split(separator);
            string end = "";

            for (int i = 0; i < sts.Length; i++)
            {
                if (i > 0) { end += separator; }
                end += sts[i].Capitalize();
            }

            return end;
        }
    }
}
