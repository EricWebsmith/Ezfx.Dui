using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ezfx.Dui
{
    public static class CodeOpener
    {
        public static void Open(string exePath, string file)
        {

        }

        public static void OpenByVSCode(string file, int line = 0, int column = 0)
        {
            string exePath = ApplicationFinder.FindVSCode();
            Process p = new Process();
            p.StartInfo.FileName = exePath;
            string arg = file;
            if (line > 0)
            {
                arg = $"--goto \"{file}\":{line}:{column}";
            }
            p.StartInfo.Arguments = arg;
            p.Start();
        }

        public static void OpenByPyCharm(string file)
        {
            string exePath = ApplicationFinder.FindPyCharm();
            Process p = new Process();
            p.StartInfo.FileName = exePath;
            p.StartInfo.Arguments = file;
            p.Start();
        }

        public static void OpenBySublime(string file, int line=0)
        {
            string exePath = ApplicationFinder.FindSublime();
            Process p = new Process();
            p.StartInfo.FileName = exePath;
            p.StartInfo.Arguments = $"\"{file}\":{line}";
            p.Start();
        }
    }
}
