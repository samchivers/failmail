using System.Linq;

namespace FailMail.FailMail.Helpers
{
    public static class StringHelpers
    {
        /// <summary>
        /// Check a string haystack for an array
        /// of possible needles - will check literally
        /// not just for whole worlds so a haystack of "contains"
        /// will return true for a "in" needle
        /// </summary>
        /// <param name="haystack"></param>
        /// <param name="needles"></param>
        /// <returns>bool</returns>
        public static bool ContainsAny(this string haystack, params string[] needles)
        {
            return needles.Any(haystack.Contains);
        }
    }
}
