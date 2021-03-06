﻿using AutoMapper;
using DFC.Api.JobProfiles.Data.AzureSearch.Models;
using DFC.Api.JobProfiles.SearchServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DFC.Api.JobProfiles.SearchServices
{
    public class DfcSearchQueryBuilder : ISearchQueryBuilder
    {
        private const string AzureSearchSpecialChar = @"[*+^""~?<>/]|( -)|[!]|[()]|[{}]|[\[\]]|&&|&|\|\|";
        private readonly IMapper mapper;

        public DfcSearchQueryBuilder()
        {
            var configure = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SearchProperties, SearchProperties>();
            });

            this.mapper = configure.CreateMapper();
        }

        public SearchProperties BuildExclusiveExactMatch(string searchTerm, SearchProperties properties)
        {
            var returnProperties = this.mapper.Map<SearchProperties>(properties ?? new SearchProperties());
            var cleanSearchTerm = EscapeSpecialCharactersInSearchTerm(searchTerm, returnProperties);

            //Escape single quotes
            cleanSearchTerm = cleanSearchTerm.Replace("'", "''", StringComparison.OrdinalIgnoreCase);

            if (string.IsNullOrWhiteSpace(returnProperties.FilterBy))
            {
                returnProperties.FilterBy =
                    $"(FilterableTitle eq '{cleanSearchTerm.TrimStart('\"').TrimEnd('\"')}' or FilterableAlternativeTitle eq '{cleanSearchTerm.TrimStart('\"').TrimEnd('\"')}')";
            }

            return returnProperties;
        }

        public SearchProperties BuildExclusivePartialMatch(string searchTerm, SearchProperties properties, int exactMatchCount)
        {
            var returnProperties = this.mapper.Map<SearchProperties>(properties ?? new SearchProperties());
            returnProperties.Count -= exactMatchCount;
            returnProperties.ExactMatchCount = exactMatchCount;

            var cleanSearchTerm = EscapeSpecialCharactersInSearchTerm(searchTerm, returnProperties);

            //Escape single quotes
            cleanSearchTerm = cleanSearchTerm.Replace("'", "''", StringComparison.OrdinalIgnoreCase);

            if (string.IsNullOrWhiteSpace(returnProperties.FilterBy))
            {
                returnProperties.FilterBy =
                    $"(FilterableTitle ne '{cleanSearchTerm.TrimStart('\"').TrimEnd('\"')}' and FilterableAlternativeTitle ne '{cleanSearchTerm.TrimStart('\"').TrimEnd('\"')}')";
            }

            return returnProperties;
        }

        public string BuildContainPartialSearch(string cleanedSearchTerm, SearchProperties properties)
        {
            if (properties?.UseRawSearchTerm == true)
            {
                return cleanedSearchTerm;
            }

            var newSearchTerm = string.Empty;
            var trimmedTerm = Regex.Replace(cleanedSearchTerm, @"\s+", " ").Trim();

            var computedContains = trimmedTerm.Any(char.IsWhiteSpace)
                ? trimmedTerm
                    .Split(' ')
                    .Aggregate(
                        newSearchTerm,
                        (current, term) => $"{current} " + (term.Contains("-", StringComparison.OrdinalIgnoreCase) ? term.Trim() : CreateContainTerm(term)))
                : trimmedTerm.Contains("-", StringComparison.OrdinalIgnoreCase) ? trimmedTerm : CreateContainTerm(trimmedTerm);

            return computedContains.Trim();
        }

        public string RemoveSpecialCharactersFromTheSearchTerm(string searchTerm, SearchProperties properties)
        {
            if (properties?.UseRawSearchTerm == true)
            {
                return searchTerm;
            }
            else
            {
                var regex = new Regex(AzureSearchSpecialChar);
                var result = regex.Replace(searchTerm, string.Empty);
                return $"{Regex.Replace(result.Trim(), @"\s+", " ")}";
            }
        }

        public string EscapeSpecialCharactersInSearchTerm(string searchTerm, SearchProperties properties)
        {
            if (properties?.UseRawSearchTerm == true)
            {
                return searchTerm;
            }
            else
            {
                var regex = new Regex(AzureSearchSpecialChar);
                return regex.Replace(searchTerm, (match) => $"\\{match.Value}");
            }
        }

        [Obsolete("This is a legacy search expression builder, use ISearchManipulator.BuildSearchExpression")]
        public string BuildExactMatchSearch(string searchTerm, string partialSearchTerm, SearchProperties properties)
        {
            if (properties?.UseRawSearchTerm == true)
            {
                return searchTerm;
            }
            else if (searchTerm?.Split(' ').Count() > 1)
            {
                return $"\"{searchTerm}\" {partialSearchTerm}".Trim();
            }
            else
            {
                return $"{searchTerm} {partialSearchTerm}".Trim();
            }
        }

        public string TrimCommonWordsAndSuffixes(string searchTerm, SearchProperties properties)
        {
            if (properties?.UseRawSearchTerm == true)
            {
                return searchTerm;
            }

            var result = searchTerm?.Any(char.IsWhiteSpace) == true
                ? searchTerm
                   .Split(' ')
                   .Aggregate(string.Empty, (current, term) =>
                   {
                       if (IsCommonCojoinginWord(term))
                       {
                           return $"{current}";
                       }
                       else
                       {
                           return $"{current} {TrimAndReplaceSuffixOnCurrentTerm(term)}";
                       }
                   })
                : TrimAndReplaceSuffixOnCurrentTerm(searchTerm);

            return result.Trim();
        }

        public string TrimAndReplaceSuffixOnCurrentTerm(string term)
        {
            var trimmedWord = TrimSuffixFromSingleWord(term);
            var replaceSuffix = ReplaceSuffixFromSingleWord(trimmedWord);
            return replaceSuffix;
        }

        [Obsolete("This functionality has been removed, will be removed in next release, if proven unuseful")]
        public string Specialologies(string term, string replacedSuffixTerm)
        {
            var indexOfOlogy = term?.LastIndexOf("ology", StringComparison.OrdinalIgnoreCase);
            return indexOfOlogy > -1
                ? replacedSuffixTerm?.IndexOf("ology", StringComparison.OrdinalIgnoreCase) > -1
                    ? $"{term?.Substring(0, indexOfOlogy.Value)}olo".Trim()
                    : $"{replacedSuffixTerm} {term?.Substring(0, indexOfOlogy.Value)}olo".Trim()
                : replacedSuffixTerm;
        }

        public bool IsCommonCojoinginWord(string term)
        {
            var commonWords = new[]
            {
                "and",
                "or",
                "as",
                "if",
                "also",
                "but",
                "not",
            };

            return commonWords.Any(w => w.Equals(term, StringComparison.OrdinalIgnoreCase));
        }

        public string CreateContainTerm(string trimmedTerm)
        {
            return $"/.*{trimmedTerm}.*/";
        }

        public string TrimSuffixFromSingleWord(string searchTerm)
        {
            var suffixes = new[]
            {
                "er",
                "ers",
                "ing",
                "ment",
                "ation",
                "or",
                "metry",
                "ics",
                "ette",
                "ance",
                "ies",
                "macy",
            };

            var suffixToBeTrimmed = suffixes.FirstOrDefault(s => searchTerm.EndsWith(s, StringComparison.OrdinalIgnoreCase));
            var trimmedResult = suffixToBeTrimmed is null ? searchTerm : searchTerm.Substring(0, searchTerm.LastIndexOf(suffixToBeTrimmed, StringComparison.OrdinalIgnoreCase));
            return trimmedResult.Length < 3 ? searchTerm : trimmedResult;
        }

        public string ReplaceSuffixFromSingleWord(string trimmedWord)
        {
            var replaceSuffixDictionary = new Dictionary<string, string>
            {
                ["therapy"] = "thera",
                ["ology"] = "olo",
            };

            var suffixToBeTrimmed = replaceSuffixDictionary.FirstOrDefault(s => trimmedWord.EndsWith(s.Key, StringComparison.OrdinalIgnoreCase));
            var trimmedResult = suffixToBeTrimmed.Key is null
                ? trimmedWord
                : $"{trimmedWord.Substring(0, trimmedWord.LastIndexOf(suffixToBeTrimmed.Key, StringComparison.OrdinalIgnoreCase))}{suffixToBeTrimmed.Value}";
            return trimmedResult.Length < 3 ? trimmedWord : trimmedResult;
        }
    }
}
