using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace slnverchecker
{
    class SolutionVersionInfo
    {
        readonly string FORMAT_PREFIX = "Microsoft Visual Studio Solution File, Format Version ";
        readonly string FORMAT_PREFIX_REGEX = @"Microsoft Visual Studio Solution File, Format Version (?<ver>.*)$";
        readonly string COMMENT_PREFIX = "# Visual Studio ";
        readonly string COMMENT_PREFIX_REGEX = "# Visual Studio (?<ver>.*)$";
        readonly string VSVERSION_PREFIX = "VisualStudioVersion";
        readonly string VSVERSION_PREFIX_REGEX = @"VisualStudioVersion\s+=\s+(?<ver>.*)$";
        readonly string MINIMUMVSVERSION_PREFIX = "MinimumVisualStudioVersion";
        readonly string MINIMUMVSVERSION_PREFIX_REGEX = @"MinimumVisualStudioVersion\s+=\s+(?<ver>.*)$";
        string format_;
        string comment_;
        string vsversion_;
        string minimumvsversion_;
        public SolutionVersionInfo(string[] lines)
        {
            foreach(string lineorig in lines)
            {
                string line = lineorig.Trim();
                if (line.Length == 0)
                    continue;

                if (format_ == null && line.StartsWith(FORMAT_PREFIX))
                {
                    format_ = line;
                    continue;
                }
                if (comment_ == null && line.StartsWith(COMMENT_PREFIX))
                {
                    comment_ = line;
                    continue;
                }
                if (vsversion_ == null && line.StartsWith(VSVERSION_PREFIX))
                {
                    vsversion_ = line;
                    continue;
                }
                if (minimumvsversion_ == null && line.StartsWith(MINIMUMVSVERSION_PREFIX))
                {
                    minimumvsversion_ = line;
                    continue;
                }
                break;
            }
        }

        public int FormatMajor
        {
            get
            {
                if (string.IsNullOrEmpty(format_))
                    return -1;

                Match match = Regex.Match(format_, FORMAT_PREFIX_REGEX);
                if (!match.Success)
                    return -1;

                string ver = match.Groups["ver"].Value;
                string[] nums = ver.Split('.');
                int result;
                if (!int.TryParse(nums[0], out result))
                    return -1;
                return result;
            }
        }
        public int Comment
        {
            get
            {
                if (string.IsNullOrEmpty(comment_))
                    return -1;

                Match match = Regex.Match(comment_, COMMENT_PREFIX_REGEX);
                if (!match.Success)
                    return -1;
                int result;
                if (!int.TryParse(match.Groups["ver"].Value, out result))
                    return -1;
                return result;
            }
        }
        private static string equallineVersion(string text, string regex)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            Match match = Regex.Match(text, regex);
            if (!match.Success)
                return string.Empty;

            return match.Groups["ver"].Value;
        }
        public string VsVersion
        {
            get
            {
                return equallineVersion(vsversion_, VSVERSION_PREFIX_REGEX);
            }
        }
        public string MinVsVersion
        {
            get
            {
                return equallineVersion(minimumvsversion_, MINIMUMVSVERSION_PREFIX_REGEX);
            }
        }
        public override string ToString() 
        {
            if(string.IsNullOrEmpty(format_) ||
                string.IsNullOrEmpty(comment_) ||
                string.IsNullOrEmpty(vsversion_) ||
                string.IsNullOrEmpty(minimumvsversion_))
            {
                return "ILLEGAL SLN";
            }
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine(format_);
            sb.AppendLine(comment_);
            sb.AppendLine(vsversion_);
            sb.AppendLine(minimumvsversion_);

            return sb.ToString();
        }
    }
}
