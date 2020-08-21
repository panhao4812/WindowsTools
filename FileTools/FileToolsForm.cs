using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FileTools
{
    public partial class FileToolsForm : Form
    {
        public List<M_File> files = new List<M_File>();
        public FileToolsForm()
        {
            InitializeComponent();
        }
        private void FileToolsForm_Load(object sender, EventArgs e)
        {
            this.Height = 600;
            this.Width = 800;
            dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;//自动换行                                                                              
            this.dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells; //设置自动调整高度
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = "";
            FolderBrowserDialog ofd = new FolderBrowserDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                path = ofd.SelectedPath;
                files = M_File.Create(path); 
            }
            else
            {
                return;
            }
            if (files.Count < 1) return;
            dataGridView1.RowCount = files.Count ;
            for (int i = 0; i < files.Count; i++)
            {
                this.dataGridView1.Rows[i].Cells[0].Value = i;
                this.dataGridView1.Rows[i].Cells[1].Value = files[i].Name;
                this.dataGridView1.Rows[i].Cells[2].Value = files[i].Size;
                this.dataGridView1.Rows[i].Cells[3].Value = files[i].Path;
                this.dataGridView1.Rows[i].Cells[4].Value = files[i].MD5;
            }
        }
        private void mD5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (files.Count < 1) return;
            for (int i = 0; i < files.Count; i++)
            {
                this.dataGridView1.Rows[i].Cells[0].Value = i;
                this.dataGridView1.Rows[i].Cells[1].Value = files[i].Name;
                this.dataGridView1.Rows[i].Cells[2].Value = files[i].Size;
                this.dataGridView1.Rows[i].Cells[3].Value = files[i].Path;
                files[i].GetMD5HashFromFile();
                this.dataGridView1.Rows[i].Cells[4].Value = files[i].MD5;
            }
            if (files.Count < 2) return;
            for (int i = 0; i < files.Count-1; i++)
            {
                if(files[i].MD5== files[i + 1].MD5)
                {
                    this.dataGridView1.Rows[i].Cells[0].Style.BackColor = Color.LightPink;
                    this.dataGridView1.Rows[i+1].Cells[0].Style.BackColor = Color.LightPink;
                }
            }
        }
        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = "";
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            sfd.FilterIndex = 2;
            sfd.RestoreDirectory = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                path = sfd.FileName;
            }
            else
            {
                return;
            }
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
            StreamWriter stream = new StreamWriter(fs);
            string output = "";
            for (int i = 0; i < files.Count; i++)
            {
                output += i.ToString()+ "\r\n";
                output += files[i].Name + " | ";
                output += files[i].Size.ToString()  + "\r\n";
                output += files[i].Path +  "\r\n";
                files[i].GetMD5HashFromFile();
                output += files[i].MD5 + "\r\n";
            }
            stream.Write(output);
            stream.Flush();
            stream.Close();
        }
    }
}
