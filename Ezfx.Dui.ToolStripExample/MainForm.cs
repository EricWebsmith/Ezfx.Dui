using Ezfx.Dui;
using System;
using System.Windows.Forms;


namespace ToolStripExample
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var settings = Properties.Settings.Default;
            settings.Bind("package", packageTextBox.TextBox);
            settings.Bind("category", categoryTextBox.TextBox);
            settings.Bind("algorithm", algorithmTextBox.TextBox);
            settings.Bind("dataset", datasetTextBox.TextBox);

            HistoryUI.Apply(historyButton, 20);
            HistoryUI.Apply(hisotryToolStripMenuItem, 20);
            BookmarkUI.Apply(bookmarkButton, 20);
            BookmarkUI.Apply(bookmarkMenuItem, 20);
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            var settings = Properties.Settings.Default;
            settings.Save2Json();
            string batContent = "cd py \n";
            batContent += "python script.py \n";
            batContent += "pause \n";
            ExternalProcess.Bat(batContent);
            HistoryUI.Apply(historyButton);
            HistoryUI.Apply(hisotryToolStripMenuItem);
            BookmarkUI.Apply(bookmarkButton, 20);
            BookmarkUI.Apply(bookmarkMenuItem, 20);
        }
    }
}
