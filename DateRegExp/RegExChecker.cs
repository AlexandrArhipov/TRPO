using System.Text.RegularExpressions;

namespace DateRegExp
{
    public class RegExChecker
    {
        private const string DateRegex = @"^(((0[1-9]|[12]\d|3[01])\.(0[13578]|1[02])\.((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\.(0[13456789]|1[012])\.((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\.02\.((19|[2-9]\d)\d{2}))|(29\.02\.((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$";
        private const string DateAndTimeRegex = @"^(((0[1-9]|[12]\d|3[01])\.(0[13578]|1[02])\.((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\.(0[13456789]|1[012])\.((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\.02\.((19|[2-9]\d)\d{2}))|(29\.02\.((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))) (?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$";
        private const string EmailRegex = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                                          @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";
        
        public static bool CheckForDate(string input)
        {
            return Regex.IsMatch(input, DateRegex);
        }

        public static bool CheckForDateAndTime(string input)
        {
            return Regex.IsMatch(input, DateAndTimeRegex);
        }

        public static bool CheckForEmail(string input)
        {
            return Regex.IsMatch(input, EmailRegex, RegexOptions.IgnoreCase);
        }
    }
}