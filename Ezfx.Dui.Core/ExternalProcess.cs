using System.Diagnostics;
using System.IO;

namespace Ezfx.Dui
{
    public static class ExternalProcess
    {
        /// <summary>
        /// Call a BAT file. At the end of the bat content. 
        /// It is suggested to add a pause statement at the end.
        /// Because it is not guaranteed the external process will run to the end.
        /// </summary>
        /// <param name="batContent"></param>
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
