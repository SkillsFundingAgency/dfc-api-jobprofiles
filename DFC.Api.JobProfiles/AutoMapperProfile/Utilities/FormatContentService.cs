using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.AutoMapperProfile.Utilities
{
    public class FormatContentService : IFormatContentService
    {
        public string GetParagraphText(string openingText, IEnumerable<string> wordList, string separator)
        {
            string formattedText;

            if (wordList == null || !wordList.Any())
            {
                return string.Empty;
            }

            if (wordList.Count() == 1)
            {
                formattedText = $"{openingText} {wordList.First()}.";
            }
            else
            {
                string itemsToJoin = string.Join(", ", wordList.Take(wordList.Count() - 1));
                formattedText = $"{openingText} {itemsToJoin} {separator} {wordList.Last()}.";
            }

            return formattedText;
        }
    }
}
