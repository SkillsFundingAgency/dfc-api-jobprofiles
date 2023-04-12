using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using DFC.Api.JobProfiles.Data.AzureSearch.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DFC.Api.JobProfiles.Data.AzureSearch.Models
{
    public class JobProfileIndex : IDigitalDataModel
    {
        [Key]
        public string IdentityField { get; set; }

        [SimpleField(IsFilterable= true, IsSortable = true, IsFacetable = true)]
        public string SocCode { get; set; }

        [SearchableField(IsFilterable = true, IsSortable = true, AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        [IsSuggestible]
        [AddWeighting(7)]
        public string Title { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.Keyword)]
        [AddWeighting(100)]
        public string TitleAsKeyword => Title.ToLowerInvariant();

        [SearchableField(IsFilterable = true, AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        [IsSuggestible]
        [AddWeighting(6)]
        public IEnumerable<string> AlternativeTitle { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.Keyword)]
        [AddWeighting(100)]
        public IEnumerable<string> AltTitleAsKeywords => AlternativeTitle?.Select(a => a.ToLowerInvariant());

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        [AddWeighting(5)]
        public string Overview { get; set; }

        [SimpleField(IsFilterable= true, IsSortable = true, IsFacetable = true)]
        public double SalaryStarter { get; set; }

        [SimpleField(IsFilterable= true, IsSortable = true, IsFacetable = true)]
        public double SalaryExperienced { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "This is an application field of type string, last segment and is not a complete uri")]
        [SimpleField(IsFilterable = true)]
        public string UrlName { get; set; }

        [SearchableField(IsFilterable= true, AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        [AddWeighting(4)]
        public IEnumerable<string> JobProfileCategories { get; set; }

        [SearchableField(IsFilterable= true, AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        [AddWeighting(3)]
        public IEnumerable<string> JobProfileSpecialism { get; set; }

        [SearchableField(IsFilterable= true, AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        [AddWeighting(3)]
        public IEnumerable<string> HiddenAlternativeTitle { get; set; }

        public IEnumerable<string> JobProfileCategoriesWithUrl { get; set; }

        [SimpleField(IsFilterable = true)]
        public IEnumerable<string> Interests { get; set; }

        [SimpleField(IsFilterable = true)]
        public IEnumerable<string> Enablers { get; set; }

        [SimpleField(IsFilterable = true)]
        public IEnumerable<string> EntryQualifications { get; set; }

        [SimpleField(IsFilterable = true)]
        public IEnumerable<string> TrainingRoutes { get; set; }

        [SimpleField(IsFilterable = true)]
        public IEnumerable<string> PreferredTaskTypes { get; set; }

        [SimpleField(IsFilterable = true)]
        public IEnumerable<string> JobAreas { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string CollegeRelevantSubjects { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string UniversityRelevantSubjects { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string ApprenticeshipRelevantSubjects { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string WYDDayToDayTasks { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string CareerPathAndProgression { get; set; }

        [SimpleField(IsFilterable = true)]
        public IEnumerable<string> WorkingPattern { get; set; }

        [SimpleField(IsFilterable = true)]
        public IEnumerable<string> WorkingPatternDetails { get; set; }

        [SimpleField(IsFilterable = true)]
        public IEnumerable<string> WorkingHoursDetails { get; set; }

        [SimpleField(IsFilterable= true, IsSortable = true, IsFacetable = true)]
        public double MinimumHours { get; set; }

        [SimpleField(IsFilterable= true, IsSortable = true, IsFacetable = true)]
        public double MaximumHours { get; set; }
    }
}