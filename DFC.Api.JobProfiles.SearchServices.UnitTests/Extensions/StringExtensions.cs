using System.Text.RegularExpressions;

namespace DFC.Api.JobProfiles.SearchServices.UnitTests.Extensions
{
    public static class StringExtensions
    {
        public static string ConvertToKey(this string rawKey)
        {
            Regex regex = new Regex(@"[ ()']");
            return regex.Replace(rawKey, match =>
            {
                return "-";
            }).TrimEnd('-').ToLowerInvariant();
        }
    }
}
