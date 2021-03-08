using System.Diagnostics;

namespace Ezfx.Dui
{
    public static class CodeOpener
    {
        public static void Open(string exePath, string file)
        {
            Process p = new Process();
            p.StartInfo.FileName = exePath;
            p.StartInfo.Arguments = file;
            p.Start();
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

        public static void OpenBySublime(string file, int line = 0)
        {
            string exePath = ApplicationFinder.FindSublime();
            Process p = new Process();
            p.StartInfo.FileName = exePath;
            p.StartInfo.Arguments = $"\"{file}\":{line}";
            p.Start();
        }

        /// According to 
        /// https://docs.spyder-ide.org/current/options.html
        /// You can only open a folder, not a file in spyder.
        /// In face, that does not help much.
        /// We will wait spyder to provide that option,
        /// although, I do not think it is going to happen.
        public static void OpenBySpyder(string file)
        {
            string exePath = ApplicationFinder.FindSpyder();
            Process p = new Process();
            p.StartInfo.FileName = "Spyder";
            p.StartInfo.Arguments = $"-p \"{file}\"";
            p.Start();
        }
    }
}
