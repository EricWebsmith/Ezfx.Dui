using Ezfx.Dui;
using System;
using System.Windows.Forms;


namespace ToolStripExample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BindingContext bc = new BindingContext();
            packageTextBox.TextBox.BindingContext = bc;
            packageTextBox.TextBox.DataBindings.Add(new Binding("Text", Properties.Settings.Default, "package", true, DataSourceUpdateMode.OnPropertyChanged));
            categoryTextBox.TextBox.BindingContext = bc;
            categoryTextBox.TextBox.DataBindings.Add(new Binding("Text", Properties.Settings.Default, "category", true, DataSourceUpdateMode.OnPropertyChanged));
            algorithmTextBox.TextBox.BindingContext = bc;
            algorithmTextBox.TextBox.DataBindings.Add(new Binding("Text", Properties.Settings.Default, "algorithm", true, DataSourceUpdateMode.OnPropertyChanged));
            datasetTextBox.TextBox.BindingContext = bc;
            datasetTextBox.TextBox.DataBindings.Add(new Binding("Text", Properties.Settings.Default, "dataset", true, DataSourceUpdateMode.OnPropertyChanged));

            HistoryDropdown.Apply(historyButton);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            var settings = Properties.Settings.Default;
            settings.Save2Local();
        }
    }
}
