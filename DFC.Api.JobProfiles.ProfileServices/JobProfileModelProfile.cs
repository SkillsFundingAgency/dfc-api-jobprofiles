using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems.JobProfiles;
using RelatedSkill = DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems.JobProfiles.RelatedSkill;

namespace DFC.Api.JobProfiles.AutoMapperProfile
{
    public class JobProfileSkillsModel
    {
        public List<JobProfileApiSkills> JobSkills { get; set; }
    }

    public class JobProfileApiSkills
    {
        public Skills Skills { get; set; }

        public RelatedSkill JobProfileContextualisedSkills { get; set; }
    }
}
