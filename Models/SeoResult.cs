using System.Collections.Generic;

namespace SeoOptimizerTool.Models
{
    public class SeoResult
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public int HeadingCountH1 { get; set; }
        public int HeadingCountH2 { get; set; }
        public int WordCount { get; set; }
        public string CanonicalUrl { get; set; }
        public string RobotsMeta { get; set; }
        public int ImagesMissingAlt { get; set; }
        public int InternalLinksCount { get; set; }
        public int ExternalLinksCount { get; set; }
        public List<string> Warnings { get; set; } = new List<string>();
    }
}
