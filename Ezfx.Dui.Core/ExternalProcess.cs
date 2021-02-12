using System.Diagnostics;
using System.IO;

namespace Ezfx.Dui
{
    public static class ExternalProcess
    {
        public static void Bat(string batContent)
        {
            string batFile = "run.bat";
            File.WriteAllText(batFile, batContent);

            Process p = new Process();
            p.StartInfo.FileName = batFile;
            p.Start();
            //do not delete it. if you do, the process will stop.
            //File.Delete(batFile);
        }
    }
}
