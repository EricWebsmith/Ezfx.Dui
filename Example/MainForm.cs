using Ezfx.Dui;
using System;
using System.Windows.Forms;


namespace Example
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            var settings = Properties.Settings.Default;
            categoryComboBox.DataSource = System.Enum.GetNames(typeof(Category));
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var settings = Properties.Settings.Default;

            settings.Bind("package", packageTextBox.TextBox);
            settings.Bind("category", categoryTextBox.TextBox);
            settings.Bind("algorithm", algorithmTextBox.TextBox);
            settings.Bind("dataset", datasetTextBox.TextBox);
            InitMenu();

            categoryComboBox.SelectedIndex = this.categoryComboBox.FindString(settings.category);

            settings.BindRadios("category", categoryGroupBox);
            settings.BindRadios("package", scikitLearnRadioButton, sparkRadioButton);

            //RadioButtonGroupUI.Bind(settings, "category", categoryGroupBox);
            //RadioButtonGroupUI.Bind(settings, "package", scikitLearnRadioButton, sparkRadioButton);
        }

        private void InitMenu()
        {
            HistoryUI.AddMenuItems(historyButton, 20);
            HistoryUI.AddMenuItems(hisotryToolStripMenuItem, 20);
            BookmarkUI.AddMenuItems(bookmarkButton, 20);
            BookmarkUI.AddMenuItems(bookmarkMenuItem, 20);
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            var settings = Properties.Settings.Default;
            settings.Save();
            settings.Save2Json();
            string batContent = "cd py \n";
            batContent += "python script.py \n";
            batContent += "pause \n";
            ExternalProcess.Bat(batContent);
            InitMenu();
        }

        private void vsCodeButton_Click(object sender, EventArgs e)
        {
            CodeOpener.OpenByVSCode("py/script.py", 10, 5);
        }

        private void pyCharmButton_Click(object sender, EventArgs e)
        {
            CodeOpener.OpenByPyCharm("py/script.py");
        }

        private void sublimeButton_Click(object sender, EventArgs e)
        {
            CodeOpener.OpenBySublime("py/script.py", 10);
        }


        private void spyderButton_Click(object sender, EventArgs e)
        {
            CodeOpener.OpenBySpyder("py");
        }
    }
}
