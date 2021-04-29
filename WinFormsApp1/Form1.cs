using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private List<string> files = new List<string>();

        private void button1_Click(object sender, EventArgs e)
        {
            files = new List<string>();
            using var fbd = new FolderBrowserDialog();
            var result = fbd.ShowDialog();

            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                label2.Text = fbd.SelectedPath;
                SearchInFolders(fbd.SelectedPath);
            }
        }

        private void SearchInFolders(string folder)
        {
            if (folder.Contains("node_modules")) return;

            files.AddRange(Directory.GetFiles(folder));
            var newFolders = Directory.GetDirectories(folder);

            if (newFolders.Length > 0)
            {
                newFolders
                    .ToList()
                    .ForEach(newFolder => SearchInFolders(newFolder));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var resultString = "";

            var extensions = textBox1.Text.Split(" ");

            var validatedFiles = extensions
                .SelectMany(ex => files
                    .Where(x => x.EndsWith(ex))
                ).ToList();

            validatedFiles.ForEach(file =>
            {
                var text = File.ReadAllText(file);
                resultString += $"\n{file.Split(@"\").LastOrDefault()}\n\n{text}\n";
            });

            richTextBox1.Text = resultString;
        }
    }
}
