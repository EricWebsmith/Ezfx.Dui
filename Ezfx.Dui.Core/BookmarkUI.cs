using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Ezfx.Dui
{
    /// <summary>
    /// The Bookmark UI class.
    /// Add bookmark menu items to the UI.
    /// </summary>
    public static class BookmarkUI
    {
        const string BOOKMARKS_FOLDER = "Bookmarks";

        public static void Apply(ToolStripDropDownItem bookmarkMenuItem, int itemCount = 20)
        {
            string appName = Assembly.GetEntryAssembly().GetName().Name;
            ApplicationSettingsBase settings = SettingsExtensions.GetSettings();
            DirectoryInfo di = new DirectoryInfo($"{appName}.{BOOKMARKS_FOLDER}");
            if (!di.Exists)
            {
                di.Create();
            }

            bookmarkMenuItem.DropDownItems.Clear();
            var files = di.GetFiles().OrderByDescending(c => c.Name).Take(itemCount).ToArray();

            //edit button
            //we do not need the delete button, we let the developer delete the files themselves.
            //so we just open the folder.
            //this is DUI, not UI.

            ToolStripMenuItem editItem = new ToolStripMenuItem("Edit");
            editItem.Click += EditMenuItem_Click;
            bookmarkMenuItem.DropDownItems.Add(editItem);

            bookmarkMenuItem.DropDownItems.Add(new ToolStripSeparator());


            foreach (FileInfo fi in files)
            {
                Dictionary<string, object> configDict = new Dictionary<string, object>();
                Newtonsoft.Json.JsonSerializer jsonSerializer = new Newtonsoft.Json.JsonSerializer();
                using (StreamReader sw = new StreamReader(fi.FullName))
                {
                    using (JsonReader jsonReader = new JsonTextReader(sw))
                    {
                        configDict = jsonSerializer.Deserialize<Dictionary<string, object>>(jsonReader);
                        jsonReader.Close();
                    }
                    sw.Close();
                }

                string summary = string.Empty;
                foreach (object value in configDict.Values)
                {
                    if (value is string)
                    {
                        string svalue = value.ToString();
                        if (svalue.Length < 20)
                        {
                            summary += " " + svalue;
                        }
                    }
                }


                ToolStripMenuItem menuItem = new ToolStripMenuItem(fi.Name.Replace(".json", " ") + summary);
                menuItem.Click += (sender, e)=> {
                    settings.LoadJson(fi.FullName);
                };
                bookmarkMenuItem.DropDownItems.Add(menuItem);
            }
        }

        private static void EditMenuItem_Click(object sender, EventArgs e)
        {
            string appName = Assembly.GetEntryAssembly().GetName().Name;
            DirectoryInfo di = new DirectoryInfo($"{appName}.{BOOKMARKS_FOLDER}");
            if (!di.Exists)
            {
                di.Create();
            }
            Process.Start(di.FullName);
        }
    }
}
