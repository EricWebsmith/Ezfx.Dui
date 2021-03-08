using Microsoft.Win32;
using System;
using System.IO;

namespace Ezfx.Dui
{
    public static class ApplicationFinder
    {
        private static string Null2Empty(object o)
        {
            return o == null ? string.Empty : (string)o;
        }

        private static string ReadKey(this RegistryKey key, string name)
        {
            object o = key.GetValue(name);
            return o == null ? string.Empty : (string)o;
        }

        public static string Find(string defaultPath, string registryDisplayName, string bindingKey = null)
        {
            string user = Environment.UserName;
            string path = defaultPath.Replace("{user}", user);
            if (File.Exists(path))
            {
                return path;
            }

            FileInfo fileInfo = new FileInfo(path);
            string shortName = fileInfo.Name;

            // the following code is from stackoverflow: 
            // https://stackoverflow.com/questions/908850/get-installed-applications-in-a-system

            string registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (Microsoft.Win32.RegistryKey key = Registry.LocalMachine.OpenSubKey(registry_key))
            {
                foreach (string subkey_name in key.GetSubKeyNames())
                {
                    using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                    {
                        string displayName = ReadKey(subkey, "DisplayName");

                        if (displayName.ToLower() == registryDisplayName.ToLower())
                        {
                            string displayIcon = ReadKey(subkey, "DisplayIcon");
                            if (displayIcon.EndsWith(".exe"))
                            {
                                return displayIcon;
                            }

                            string installLocation = ReadKey(subkey, "InstallLocation");
                            string regPath = Path.Combine(installLocation, shortName);
                            if (File.Exists(regPath))
                            {
                                return regPath;
                            }

                            break;
                        }
                    }
                }
            }

            return string.Empty;
        }

        public static string FindPyCharm(string bindingKey = null)
        {
            string defaultPath = @"C:\Program Files\JetBrains\PyCharm Community Edition 2020.3.3\bin\pycharm64.exe";
            return Find(defaultPath, "PyCharm Community Edition 2020.3.3", bindingKey);
        }

        public static string FindVSCode(string bindingKey = null)
        {
            string defaultPath = @"C:\Users\{user}\AppData\Local\Programs\Microsoft VS Code\Code.exe";
            return Find(defaultPath, "Microsoft Visual Studio Code (User)", bindingKey);
        }

        /// <summary>
        /// There is no default path for sublime.
        /// because it is portable.
        /// And you cannot find the location of sublime by register.
        /// </summary>
        /// <param name="bindingKey"></param>
        /// <returns></returns>
        public static string FindSublime(string bindingKey = null)
        {
            string defaultPath = @"C:\Sublime Text Build 3211 x64\sublime_text.exe";
            return Find(defaultPath, "", bindingKey);
        }

        public static string FindSpyder(string bindingKey = null)
        {
            string defaultPath = @"C:\ProgramData\Anaconda3\pythonw.exe";
            return Find(defaultPath, "", bindingKey);
        }
    }
}
