using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using DFC.Api.JobProfiles.AutoMapperProfile.Utilities;
using DFC.Api.JobProfiles.Data.ApiModels.HowToBecome;
using DFC.Api.JobProfiles.Data.ApiModels.WhatYouWillDo;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;
using HtmlAgilityPack;

namespace DFC.Api.JobProfiles.AutoMapperProfile.Resolvers
{
    public class DayToDayResolver : IValueResolver<JobProfileWhatYoullDoResponse, WhatYouWillDoApiModel, List<string>>
    {
        public List<string> Resolve(
            JobProfileWhatYoullDoResponse source,
            WhatYouWillDoApiModel destination,
            List<string> destMember,
            ResolutionContext context)
        {
            var daytoDay = new List<string>();

            if (source != null && source.JobProfileWhatYoullDo.IsAny())
            {
                var responseData = source.JobProfileWhatYoullDo.FirstOrDefault();

                if (responseData.Daytodaytasks.Html != null)
                {
                    daytoDay = HtmltoList(responseData.Daytodaytasks.Html);
                    return daytoDay;
                }
            }
            return daytoDay;
        }

        private static List<string> HtmltoList(string html)
        {
            List<string> HtmlList = new List<string>();
            var prefix = html.Substring(0, html.IndexOf(":") + 1);
            HtmlList.Add(prefix);
            var matches = Regex.Matches(html, @"<li>(.*)</li>").ToList();


            var items = matches.Select(m => m.Groups[1].ToString()).ToList();
            HtmlList.AddRange(items);

            for (int i = 0; i < HtmlList.Count; i++)
            {
                HtmlList[i] = HtmltoText(HtmlList[i]);
            }

            return HtmlList;
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
