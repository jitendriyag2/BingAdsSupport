using Microsoft.Extensions.FileSystemGlobbing.Internal.Patterns;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BingAdSupport.CSSData.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] files = Directory.GetFiles(@"E:\Jitendriya\Project\BingAdsSupport", "TicketReportView.txt");
            StringBuilder str = new StringBuilder();
            string pattern = @"<\s*div\sclass=.*>(.*?)<\s*/\s*div>";

            

            foreach (var file in files)
            {
                str.Append(File.ReadAllText(file));

                Match matches = Regex.Match(str.ToString(), pattern, RegexOptions.IgnorePatternWhitespace);
                while(matches.Success)
                {
                    string n = matches.Groups[0].Value;
                }
            }
            
        }
    }
}
