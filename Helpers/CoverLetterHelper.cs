using CraigslistSearcher.Helpers;
using CraigslistSearcher.Model;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CraigsListSearcher.Helpers
{
    class CoverLetterHelper
    {
        public Job job { get; set; }
        public CoverLetterHelper(Job job)
        {
            this.job = job;
        }
        private string ToOrdinal(int number)
        {
            switch (number % 100)
            {
                case 11:
                case 12:
                case 13:
                    return number.ToString() + "th";
            }

            switch (number % 10)
            {
                case 1:
                    return number.ToString() + "st";
                case 2:
                    return number.ToString() + "nd";
                case 3:
                    return number.ToString() + "rd";
                default:
                    return number.ToString() + "th";
            }
        }
        public Document Write()
        {
            Document doc = new Document();
            FileStream file = new FileStream("Cover Letter.pdf", FileMode.Create);
            PdfWriter.GetInstance(doc, file);
            doc.Open();
            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph(DateTime.Now.ToString("D")));
            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("To Whom It May Concern,"));
            doc.Add(new Paragraph("\n"));
            Font bold = new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD);
            doc.Add(new Paragraph("RE: " + job.jobTitle, bold));
            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("     I am writing to apply for the position of '" + job.jobTitle + "' at your company, which was advertised on Craigslist on " + job.datePosted.ToString("MMMM") + " " + ToOrdinal(job.datePosted.Day) + "."));
            doc.Add(new Paragraph("     My name is Dave Alton, and I am currently studying at British Columbia Institute of Technology. I am taking a program focused entirely on software development. Every day, we focus on learning many of the top languages and skills in the field."));
            doc.Add(new Paragraph("     I have enclosed my CV to support my application. It shows that I would bring important skills to the position, including:"));
            foreach (string jobSkill in job.jobSkills)
            {
                if (jobSkill != null)
                {
                    doc.Add(new Paragraph("          " + jobSkill));
                }
            }
            doc.Add(new Paragraph("     I would enjoy having the opportunity to talk with you more about this position, and how I could use my skills to benefit your organisation."));
            doc.Add(new Paragraph("     Thank you for considering my application. I look forward to hearing from you."));
            doc.Add(new Paragraph("     Yours faithfully,"));
            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("     Dave Alton"));
            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("     PS: This job was not manually applied for. I wrote a program to apply for jobs for me. It writes a cover letter based on your needs with my skills and sends it to you along with my resume. To see the code, go here: https://github.com/TheNewReborn/C-Sharp-CraigslistJobSearcher. I did however verify that this job was something I would be interested in before letting my computer apply on my behalf."));
            doc.Close();
            file.Close();
            return doc;
        }
        public void Send()
        {

        }
    }
}
