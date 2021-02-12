using System.Collections.Generic;
using System.Configuration;
using System.Windows.Forms;

namespace Ezfx.Dui
{
    public static class RadioButtonGroupUI
    {
        public static void Apply(ApplicationSettingsBase settings, string key, Control parentControl)
        {
            List<RadioButton> radios = new List<RadioButton>();
            foreach(Control control in parentControl.Controls)
            {
                switch (control)
                {
                    case RadioButton e:
                        radios.Add(e);
                        break;
                }
            }

            Apply(settings, key, radios.ToArray());
        }

        public static void Apply(ApplicationSettingsBase settings, string key, params RadioButton[] radios)
        {

            foreach (RadioButton radioButton in radios)
            {
                if (radioButton.Text == (string)settings[key])
                {
                    radioButton.Checked = true;
                }

                radioButton.CheckedChanged += (sender, e) => {
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
    }
}
