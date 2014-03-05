using CraigslistSearcher.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CraigslistSearcher.Model
{
    class Job
    {
        public string url { get; set; }
        public DateTime datePosted { get; set; }
        public string replyEmail { get; set; }
        public string urlContents { get; set; }
        public string jobSummary { get; set; }
        public string jobTitle { get; set; }
        public string[] jobSkills { get; set; }
        public bool notEnoughExperience { get; set; }
        public bool hasSkill { get; set; }
        public bool isApplicableJob { get; set; }
        public Job(int skillNum, string jobSummary)
        {
            this.jobSkills = new string[skillNum];
            this.notEnoughExperience = false;
            this.hasSkill = false;
            this.isApplicableJob = false;
            this.jobSummary = jobSummary;
            this.jobTitle = getTitle();
            this.datePosted = getDate();
            this.url = getUrl();
            this.replyEmail = getReply();
        }
        private string getTitle()
        {
            string strStart = "";
            string strEnd = "(";
            int Start, End;
            if (this.jobSummary.Contains(strStart) && this.jobSummary.Contains(strEnd))
            {
                Start = this.jobSummary.IndexOf(strStart, 0) + strStart.Length;
                End = this.jobSummary.IndexOf(strEnd, Start);
                return this.jobSummary.Substring(Start + 1, End - Start - 1);
            }
            else
            {
                return "";
            }
        }
        private DateTime getDate()
        {
            string strStart = "[...]";
            string strEnd = "T";
            int Start, End;
            if (this.jobSummary.Contains(strStart) && this.jobSummary.Contains(strEnd))
            {
                Start = this.jobSummary.IndexOf(strStart, 0) + strStart.Length;
                End = this.jobSummary.IndexOf(strEnd, Start);
                string dateString = this.jobSummary.Substring(Start + 1, End - Start - 1);
                string[] dateArray = dateString.Split('-');
                DateTime date = new DateTime(Convert.ToInt32(dateArray[0]), Convert.ToInt32(dateArray[1]), Convert.ToInt32(dateArray[2]));
                return date;
            }
            else
            {
                return DateTime.Now;
            }
        }
        private string getUrl()
        {
            string strStart = "http";
            string strEnd = "html";
            int Start, End;
            if (this.jobSummary.Contains(strStart) && this.jobSummary.Contains(strEnd))
            {
                Start = this.jobSummary.IndexOf(strStart, 0) + strStart.Length;
                End = this.jobSummary.IndexOf(strEnd, Start);
                return strStart + this.jobSummary.Substring(Start, End - Start) + strEnd;
            }
            else
            {
                return "";
            }
        }
        public string getReply()
        {
            string strStart = "reply/";
            string strEnd = "reply";
            int Start, End;
            if (this.jobSummary.Contains(strStart) && this.jobSummary.Contains(strEnd))
            {
                Start = this.jobSummary.IndexOf(strStart, 0) + strStart.Length;
                End = this.jobSummary.IndexOf(strEnd, Start);
                try
                {
                    string replyCode = this.jobSummary.Substring(Start, End - Start - 2);
                    string replyUrl = "http://vancouver.en.craigslist.ca/reply/" + replyCode;
                    string replyContents = CraigslistHelper.getContents(replyUrl);
                    if (replyContents != "")
                    {
                        string email = CraigslistHelper.getSection(replyContents, "mailto:", "craigslist.org") + "craigslist.org";
                        return email.Replace("%40", "@");
                    }
                }
                catch (Exception e)
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
            return "";

        }
    }
}
