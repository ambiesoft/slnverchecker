using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slnverchecker
{
    class SolutionVersionInfo
    {
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

                if(format_==null && line.StartsWith("Microsoft Visual Studio Solution File, Format Version "))
                {
                    format_ = line;
                    continue;
                }
                if(comment_==null && line.StartsWith("# Visual Studio "))
                {
                    comment_ = line;
                    continue;
                }
                if(vsversion_==null && line.StartsWith("VisualStudioVersion = "))
                {
                    vsversion_ = line;
                    continue;
                }
                if (minimumvsversion_ == null && line.StartsWith("MinimumVisualStudioVersion = "))
                {
                    minimumvsversion_ = line;
                    continue;
                }
                break;
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
