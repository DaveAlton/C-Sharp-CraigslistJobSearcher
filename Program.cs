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

namespace Examples.System.Net
{
    public class WebRequestPostExample
    {
        private static void getJobs(string[] yourSkills, int yourExperience)
        {
            string[] skills = yourSkills;
            int experience = yourExperience;

            int skillsLength = skills.Length;
            string url = "http://vancouver.en.craigslist.ca/sof/index.rss";
            using (XmlReader reader = XmlReader.Create(url))
            {
                reader.MoveToContent();
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        Job job = new Job(skillsLength);
                        XElement el = XNode.ReadFrom(reader) as XElement;
                        if (el != null)
                        {
                            job.jobSummary = el.Value;
                            //looking for developer or programmer jobs
                            if (job.jobSummary.Contains("developer") || job.jobSummary.Contains("Developer") || job.jobSummary.Contains("Programmer") || job.jobSummary.Contains("programmer"))
                            {
                                //must contain these keywords
                                if (job.jobSummary.Contains("C#") || job.jobSummary.Contains("Java") || job.jobSummary.Contains("ASP.NET") || job.jobSummary.Contains("Android") || job.jobSummary.Contains("iOS") || job.jobSummary.Contains("mobile") || job.jobSummary.Contains("asp.net") || job.jobSummary.Contains(".NET") || job.jobSummary.Contains(".net") || job.jobSummary.Contains("Mobile"))
                                {
                                    //must NOT contain these keywords
                                    if (!job.jobSummary.Contains("UI") && !job.jobSummary.Contains("QA") && !job.jobSummary.Contains("sr.") && !job.jobSummary.Contains("Sr.") && !job.jobSummary.Contains("Senior") && !job.jobSummary.Contains("senior") && !job.jobSummary.Contains("Richmond") && !job.jobSummary.Contains("C++") && !job.jobSummary.Contains("Front-End") && !job.jobSummary.Contains("Front End") && !job.jobSummary.Contains("Victoria") && !job.jobSummary.Contains("Part-Time"))
                                    {
                                        //must NOT be too high in experience
                                        for (int j = experience + 4; j < 15; j++)
                                        {
                                            if (job.jobSummary.ToUpper().Contains(j + " YEARS") || job.jobSummary.ToUpper().Contains(j + "+ YEARS"))
                                            {
                                                job.notEnoughExperience = true;
                                            }
                                        }
                                        if (!job.notEnoughExperience)
                                        {
                                            job.url = HtmlHelper.getUrl(job.jobSummary);
                                            string contents = HtmlHelper.getContents(job.url);
                                            job.hasSkill = false;
                                            int i = 0;
                                            foreach (string skill in skills)
                                            {
                                                if (contents.ToUpper().Contains(skill.ToUpper()))
                                                {
                                                    job.jobSkills[i] = skill;
                                                    i++;
                                                    job.hasSkill = true;
                                                }
                                            }
                                            if (job.hasSkill)
                                            {
                                                job.jobTitle = HtmlHelper.getTitle(job.jobSummary);
                                                Console.WriteLine(job.jobTitle);
                                                foreach (string jobSkill in job.jobSkills)
                                                {
                                                    if (jobSkill != null)
                                                    {
                                                        Console.WriteLine(jobSkill);
                                                    }
                                                }
                                                string replyCode = HtmlHelper.getReply(contents, "reply/", "reply");
                                                string replyUrl = "http://vancouver.en.craigslist.ca/reply/" + replyCode;
                                                string replyContents = HtmlHelper.getContents(replyUrl);
                                                if (replyContents != "")
                                                {
                                                    string email = HtmlHelper.getSection(replyContents, "mailto:", "craigslist.org") + "craigslist.org";
                                                    job.replyEmail = email.Replace("%40", "@");
                                                    Console.WriteLine(job.replyEmail);
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
            Console.WriteLine();
            Console.WriteLine("PROGRAM FINISHED");
            Console.ReadLine();
        }
        public static void Main()
        {
            string[] skills = new string[] { "C#", "ASP.NET", ".NET", "PHP", "OBJECTIVE-C", "MYSQL", "JAVA", "JS", "JavaScript", "jQuery", "CSS", "HTML", "AJAX", "JSON", "Android", "SQL Server" };
            const int EXPERIENCE = 0;

            getJobs(skills, EXPERIENCE);
        }
    }
}
