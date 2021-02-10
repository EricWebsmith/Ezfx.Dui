using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Ezfx.Dui
{
    public static  class HistoryDropdown
    {
        public static void Apply(ToolStripDropDownButton button)
        {
            string appName = Assembly.GetEntryAssembly().GetName().Name;
            DirectoryInfo di = new DirectoryInfo($"{appName}.History");
            if (!di.Exists)
            {
                return;
            }

            foreach(FileInfo fi in di.GetFiles())
            {
                ToolStripMenuItem menuItem = new ToolStripMenuItem(fi.Name.Replace(".json", ""));
                menuItem.Click += MenuItem_Click;
                button.DropDownItems.Add(menuItem);
            }
        }

        private static void MenuItem_Click(object sender, EventArgs e)
        {
            string appName = Assembly.GetEntryAssembly().GetName().Name;
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            string timeStamp = menuItem.Text.Split(' ')[0];


            ApplicationSettingsBase settings = SettingsExtensions.GetSettings();
            settings.LoadHistory(timeStamp);
        }
    }
}
