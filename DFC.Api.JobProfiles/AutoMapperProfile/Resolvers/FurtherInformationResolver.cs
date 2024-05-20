using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DFC.Api.JobProfiles.AutoMapperProfile.Utilities;
using DFC.Api.JobProfiles.Data.ApiModels.HowToBecome;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;
using HtmlAgilityPack;

namespace DFC.Api.JobProfiles.AutoMapperProfile.Resolvers
{
    public class FurtherInformationResolver : IValueResolver<JobProfileHowToBecomeResponse, MoreInformationApiModel, List<string>>
    {
        public List<string> Resolve(
            JobProfileHowToBecomeResponse source,
            MoreInformationApiModel destination,
            List<string> destMember,
            ResolutionContext context)
        {
            var furtherInformation = new List<string>();

            if (source != null && source.JobProfileHowToBecome.IsAny())
            {
                var responseData = source.JobProfileHowToBecome.FirstOrDefault();

                if (responseData.FurtherInformation.Html != null)
                {
                    furtherInformation = HtmltoText(responseData.FurtherInformation.Html);
                    return furtherInformation;
                }
            }
            return furtherInformation;
        }

        private static List<string> HtmltoText(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            List<string> strings = new List<string>();
            HtmlNodeCollection textNodes = doc.DocumentNode.SelectNodes("//a[@href]");

            if (textNodes != null)
            {
                foreach (var node in textNodes)
                {
                    string hrefValue = string.Concat(node.InnerText, "|", node.GetAttributeValue("href", string.Empty));
                    //string hrefValue = node.GetAttributeValue("href", string.Empty);
                    strings.Add(hrefValue);
                }
                return strings;
            }
            else
            {
                return strings;
            }

        }

    }


}
