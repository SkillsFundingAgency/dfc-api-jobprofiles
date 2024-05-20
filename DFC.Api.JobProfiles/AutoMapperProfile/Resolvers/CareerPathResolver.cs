using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using DFC.Api.JobProfiles.AutoMapperProfile.Utilities;
using DFC.Api.JobProfiles.Data.ApiModels.CareerPathAndProgression;
using DFC.Api.JobProfiles.Data.ApiModels.HowToBecome;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;
using HtmlAgilityPack;

namespace DFC.Api.JobProfiles.AutoMapperProfile.Resolvers
{
    public class CareerPathResolver : IValueResolver<JobProfileCareerPathAndProgressionResponse, CareerPathAndProgressionApiModel, List<string>>
    {
        public List<string> Resolve(
            JobProfileCareerPathAndProgressionResponse source,
            CareerPathAndProgressionApiModel destination,
            List<string> destMember,
            ResolutionContext context)
        {
            var careerPath = new List<string>();

            if (source != null && source.JobProileCareerPath.IsAny())
            {
                var responseData = source.JobProileCareerPath.FirstOrDefault();

                if (responseData.Content.Html != null)
                {
                   var test = HtmltoText(responseData.Content.Html);
                    careerPath.Add(test);
                    return careerPath;
                }
            }
            return careerPath;
        }

        private static string HtmltoText(string html)
        {
            var doc = new HtmlDocument();
            string result = string.Empty;
            doc.LoadHtml(html);
            List<string> strings = new List<string>();
            HtmlNodeCollection textNodes = doc.DocumentNode.SelectNodes("//text()");

            if (textNodes != null)
            {
                foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//text()"))
                {
                    strings.Add(node.InnerText);
                    result = string.Join("", strings);
                    Console.WriteLine("text=" + node.InnerText);
                }
                return result;
            }
            else
            {
                return html;
            }

        }
    }

}
