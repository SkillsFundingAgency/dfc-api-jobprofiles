using System.Collections.Generic;

namespace DFC.Api.JobProfiles.AutoMapperProfile.Utilities
{
    public interface IFormatContentService
    {
        string GetParagraphText(string openingText, IEnumerable<string> dataItems, string separator);
    }
}