using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.ServiceModel.Syndication;
using System.Collections.Generic;
using System.Xml.Linq;
using CraigslistSearcher.Helpers;
using CraigslistSearcher.Model;
using System.Diagnostics;
using iTextSharp.text;
using iTextSharp.text.pdf;
using CraigsListSearcher.Helpers;
using CraigsListSearcher.Models;

namespace CraigslistSearcher
{
    public class CraigslistSearcher
    {
        private static void getJobs(string[] yourSkills, int yourExperience, string[] mustHaveWords, string[] cantHaveWords)
        {
            string[] developerKeywords = new string[] { "developer", "programmer" };
            int skillsLength = yourSkills.Length;
            string[] urls = new string[] { "http://vancouver.en.craigslist.ca/sof/", "http://vancouver.en.craigslist.ca/web/",  };
            foreach(string url in urls){
                using (XmlReader reader = XmlReader.Create(url+"index.rss"))
                {
                    reader.MoveToContent();
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            XElement el = XNode.ReadFrom(reader) as XElement;
                            if (el != null)
                            {
                                Job job = new Job(skillsLength, el.Value);
                                //looking for developer or programmer jobs
                                foreach (string developerKeyword in developerKeywords)
                                {
                                    if (job.jobSummary.ToUpper().Contains(developerKeyword.ToUpper()))
                                    {
                                        job.isApplicableJob = true;
                                    }
                                }
                                foreach (string mustHaveWord in mustHaveWords)
                                {
                                    if (job.jobSummary.ToUpper().Contains(mustHaveWord.ToUpper()))
                                    {
                                        job.isApplicableJob = true;
                                    }
                                }
                                foreach (string cantHaveWord in cantHaveWords)
                                {
                                    if (job.jobSummary.ToUpper().Contains(cantHaveWord.ToUpper()))
                                    {
                                        job.isApplicableJob = false;
                                    }
                                }
                                if (job.isApplicableJob)
                                {
                                    //must be within 3 years of experience;
                                    for (int j = yourExperience + 3; j < 15; j++)
                                    {
                                        if (job.jobSummary.ToUpper().Contains(j + " YEARS") || job.jobSummary.ToUpper().Contains(j + "+ YEARS"))
                                        {
                                            job.notEnoughExperience = true;
                                        }
                                    }
                                    if (!job.notEnoughExperience)
                                    {
                                        job.htmlContents = CraigslistHelper.getContents(job.url);
                                        job.hasSkill = false;
                                        int i = 0;
                                        //Check if this job is requesting a skill you have
                                        foreach (string skill in yourSkills)
                                        {
                                            if (job.htmlContents.ToUpper().Contains(skill.ToUpper()))
                                            {
                                                job.jobSkills[i] = skill;
                                                i++;
                                                job.hasSkill = true;
                                            }
                                        }
                                        if (job.hasSkill)
                                        {
                                            Console.WriteLine(job.jobTitle);
                                            Console.WriteLine("Apply? Y/N");
                                            string response = Console.ReadLine();
                                            //ask if you want to apply to this job
                                            if (response.ToUpper() == "Y")
                                            {
                                                CoverLetterHelper coverLetter = new CoverLetterHelper(job);
                                                Document doc = coverLetter.Write();
                                                Email email = new Email(job.replyEmail, "RE: " + job.jobTitle);
                                                email.Send();
                                                Console.WriteLine();
                                            }
                                            else
                                            {
                                                Console.WriteLine();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public static void Main()
        {
            string[] skills = new string[] { "Angular", "Restful APIs", "SVN", "GIT", "MVC", "ADO.NET", "LINQ", "C#", "ASP.NET", ".NET", "PHP", "Objective-C", "MySQL", "Java", "JavaScript", "jQuery", "CSS", "HTML", "AJAX", "JSON", "Android", "SQL Server" };
            //the post must contain one of these words:
            string[] mustHaveWords = new string[] { "C#", "Java ", "ASP.NET", "Android", "iOS", "mobile", ".NET", "Mobile" };
            //the post can't contain any of these words:
            string[] cantHaveWords = new string[] { "UI ", "QA", "Sr.", "Senior", "Richmond", "C++", "Victoria", "Part-Time", "Perl", "Opportunity", "SalesForce", "Drupal", "Design", "Volunteer", "Manager", "Chinese", "Pure C", "Legacy C"};
            //my work experience in years:
            const int EXPERIENCE = 1;

            getJobs(skills, EXPERIENCE, mustHaveWords, cantHaveWords);
            Console.ReadLine();
        }
    }
}
