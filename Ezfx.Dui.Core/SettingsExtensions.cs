﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Ezfx.Dui
{
    public static class SettingsExtensions
    {
        public static void Bind(this ApplicationSettingsBase settings, string key, Control control, string controlProperty = "Text")
        {

            control.BindingContext = control.FindForm().BindingContext;
            control.DataBindings.Add(new Binding(controlProperty, settings, key, true, DataSourceUpdateMode.OnPropertyChanged));
        }

        public static void BindRadios(this ApplicationSettingsBase settings, string key, Control parentControl)
        {
            List<RadioButton> radios = new List<RadioButton>();
            foreach (Control control in parentControl.Controls)
            {
                switch (control)
                {
                    case RadioButton e:
                        radios.Add(e);
                        break;
                }
            }

            BindRadios(settings, key, radios.ToArray());
        }

        public static void BindRadios(this ApplicationSettingsBase settings, string key, params RadioButton[] radios)
        {

            foreach (RadioButton radioButton in radios)
            {
                if (radioButton.Text == (string)settings[key])
                {
                    radioButton.Checked = true;
                }

                radioButton.CheckedChanged += (sender, e) =>
                {
                    if (radioButton.Checked)
                    {
                        settings[key] = radioButton.Text;
                    }
                };
            }

            settings.PropertyChanged += (sender, e) =>
            {
                foreach (RadioButton radioButton in radios)
                {
                    if (radioButton.Text == (string)settings[key])
                    {
                        radioButton.Checked = true;
                    }
                    else
                    {
                        radioButton.Checked = false;
                    }
                }
            };
        }

        public static void Save2Json(this ApplicationSettingsBase settings, bool saveHistory = true)
        {
            string appName = settings.GetType().Assembly.GetName().Name;
            Dictionary<string, object> configDict = new Dictionary<string, object>();
            foreach (SettingsProperty sp in settings.Properties)
            {
                configDict[sp.Name] = settings[sp.Name];
            }

            Newtonsoft.Json.JsonSerializer jsonSerializer = new Newtonsoft.Json.JsonSerializer();
            jsonSerializer.Formatting = Newtonsoft.Json.Formatting.Indented;
            using (var sw = new StreamWriter($"{appName}.json"))
            {
                jsonSerializer.Serialize(sw, configDict);
            }

            if (saveHistory)
            {
                DirectoryInfo di = new DirectoryInfo($"{appName}.History");
                if (!di.Exists)
                {
                    di.Create();
                }

                string timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmm_ss");

                using (var sw = new StreamWriter($"{appName}.History/{timeStamp}.json"))
                {
                    jsonSerializer.Serialize(sw, configDict);
                }
            }
        }

        internal static ApplicationSettingsBase GetSettings()
        {
            Assembly entryAssembly = Assembly.GetEntryAssembly();
            Type settingType = entryAssembly.GetType($"{entryAssembly.GetName().Name}.Properties.Settings");
            PropertyInfo pi = settingType.GetProperty("Default");
            return (ApplicationSettingsBase)pi.GetValue(null, null);
        }

        public static void LoadJson(this ApplicationSettingsBase settings, string jsonPath)
        {
            Dictionary<string, object> configDict = new Dictionary<string, object>();
            Newtonsoft.Json.JsonSerializer jsonSerializer = new Newtonsoft.Json.JsonSerializer();
            using (StreamReader sw = new StreamReader(jsonPath))
            {
                using (JsonReader jsonReader = new JsonTextReader(sw))
                {
                    configDict = jsonSerializer.Deserialize<Dictionary<string, object>>(jsonReader);
                    jsonReader.Close();
                }
                sw.Close();
            }

            foreach (SettingsProperty sp in settings.Properties)
            {
                if (!configDict.ContainsKey(sp.Name))
                {
                    continue;
                }

                if (sp.PropertyType == typeof(string))
                {
                    // Unboxing is not needed here, 
                    // since both indexers return objects.
                    settings[sp.Name] = configDict[sp.Name];
                }
                else if (sp.PropertyType == typeof(long))
                {
                    settings[sp.Name] = configDict[sp.Name];
                }
                else if (sp.PropertyType == typeof(int))
                {
                    // first, unbox it to long.
                    // then convert long to int.
                    settings[sp.Name] = (int)(long)configDict[sp.Name];
                }
                else if (sp.PropertyType == typeof(float))
                {
                    // first, unbox it to double
                    // then convert is to float
                    settings[sp.Name] = (float)(double)configDict[sp.Name];
                }
                else if (sp.PropertyType == typeof(double))
                {
                    settings[sp.Name] = configDict[sp.Name];
                }
                else if (sp.PropertyType == typeof(bool))
                {
                    settings[sp.Name] = (bool)configDict[sp.Name];
                }
                else if (sp.PropertyType == typeof(decimal))
                {
                    settings[sp.Name] = (decimal)(long)configDict[sp.Name];
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
