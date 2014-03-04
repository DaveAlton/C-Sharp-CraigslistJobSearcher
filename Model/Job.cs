using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CraigslistSearcher.Model
{
    class Job
    {
        public string url { get; set; }
        public string replyEmail { get; set; }
        public string jobTitle { get; set; }
        public string urlContents { get; set; }
        public string jobSummary { get; set; }
        public string[] jobSkills { get; set; }
        public bool notEnoughExperience { get; set; }
        public bool hasSkill { get; set; }
        public Job(int skillNum)
        {
            this.jobSkills = new string[skillNum];
            this.notEnoughExperience = false;
            this.hasSkill = false;
        }
    }
}
