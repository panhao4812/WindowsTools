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

namespace CleanCAAWorkSpace
{
    public partial class CleanCAAWorkSpaceForm : Form
    {
        FolderBrowserDialog ofd;
        Global p;
        List<string> _framework;
        List<string> _module;
        WhiteList _whitelist;

        public CleanCAAWorkSpaceForm()
        {
            InitializeComponent();
            this.textBox1.Text = Environment.CurrentDirectory;
            this.ofd = new FolderBrowserDialog();
            this.p = new Global(false);
            this._framework = new List<string>();
            this._module = new List<string>();
            this._whitelist = new WhiteList(p);
            GetPath(ofd.SelectedPath, ref this._framework, ref this._module);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(textBox1.Text))
            {
                ofd.SelectedPath = textBox1.Text;
            }
            else
            {
                ofd.SelectedPath = Environment.CurrentDirectory;
            }
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (GetPath(ofd.SelectedPath, ref this._framework, ref this._module))
                {
                    textBox1.Text = ofd.SelectedPath;
                }
                else
                {
                    MessageBox.Show("Not a CAA WorkSpace");
                }
            }
            foreach (string Framework in this._framework)
            {
                p.Print(Framework);
            }
            foreach (string module in this._module)
            {
                p.Print(module);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("Explorer.exe", textBox1.Text);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string output = "";
            if (!GetPath(textBox1.Text))
            {
                MessageBox.Show("Select a CAA WorkSpace folder first");
                return;
            }
            if (this.checkBox1.Checked)
            {
                string path1 = textBox1.Text + "\\win_b64";
                if (Global._deletefolder_withoutroot(path1))
                { output += path1 + " Clear" + "\r\n"; }
                else { output += path1 + "==> Failed" + "\r\n"; }
            }
            if (this.checkBox2.Checked)
            {
                foreach (string Module in this._module)
                {
                    string path2 = Module + "\\Objects\\win_b64";
                    if (Global._deletefolder_withoutroot(path2))
                    { output += Module + " Clear" + "\r\n"; }
                    else { output += Module + "==> Failed" + "\r\n"; }
                }
            }
            if (this.checkBox3.Checked)
            {
                foreach (string Framework in this._framework)
                {
                    string path3 = Framework + "\\ImportedInterfaces";
                    if (Global._deletefolder_withoutroot(path3))
                    { output += Framework + " Clear" + "\r\n"; }
                    else { output += Framework + "==> Failed" + "\r\n"; }
                }
            }
            if (this.checkBox4.Checked)
            {
                List<bool> sign1=new List<bool>();
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (checkedListBox1.GetItemCheckState(i) == CheckState.Checked)
                    {
                        sign1.Add(true);
                    }
                    else
                    {
                        sign1.Add(false);
                    }
                }
                this._whitelist.setState(sign1);
                List<string> ls = this._whitelist.ToPath(textBox1.Text, this._framework, this._module);
                if (ls.Count > 0) { output += "White List Used"; } else { output += "White List Failed"; }
                Global._deletefiles_withWhitelist(textBox1.Text, ref ls);
            }
            MessageBox.Show(output);
        }
        private bool GetPath(string workspace)
        {
            if (!Directory.Exists(workspace)) return false;
            if (!(Directory.Exists(workspace + "/win_b64")
                  && Directory.Exists(workspace + "/ToolsData")
                  && Directory.Exists(workspace + "/CATEnv")
                  ))
            {
                return false;
            }
            return true;
        }
        private bool GetPath(string workspace, ref List<string> framework, ref List<string> moudle)
        {
            framework.Clear(); moudle.Clear();

            if (!Directory.Exists(workspace)) return false;
            if (!(Directory.Exists(workspace + "/win_b64")
                  && Directory.Exists(workspace + "/ToolsData")
                  && Directory.Exists(workspace + "/CATEnv")
                  ))
            {
                return false;
            }
            this._framework = Global.From0Find1by2Name(workspace, "CNext");
            for (int i = 0; i < this._framework.Count; i++)
            {
                this._module.AddRange(Global.From0Find1by2Name(_framework[i], "src"));
            }


            return true;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ofd.Dispose();
        }

        private void checkBox4_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox4.CheckState == CheckState.Checked)
            {
                CheckedListBox.ObjectCollection co = this.checkedListBox1.Items;

                for (int i = 0; i < this._whitelist._workspace.Count; i++)
                {
                    co.Add("[workspace]" + _whitelist._workspace[i], _whitelist.s_workspace[i]);
                }
                for (int i = 0; i < this._whitelist._framework.Count; i++)
                {
                    co.Add("[framework]" + _whitelist._framework[i], _whitelist.s_framework[i]);
                }
                for (int i = 0; i < this._whitelist._module.Count; i++)
                {
                    co.Add("[module]" + _whitelist._module[i], _whitelist.s_module[i]);
                }
                for (int i = 0; i < this._whitelist._filetype.Count; i++)
                {
                    co.Add("[filetype]" + _whitelist._filetype[i], _whitelist.s_filetype[i]);
                }
            }
            else
            {
                List<bool> sign1 = new List<bool>();
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (checkedListBox1.GetItemCheckState(i) == CheckState.Checked)
                    {
                        sign1.Add(true);
                    }
                    else
                    {
                        sign1.Add(false);
                    }
                }
                this._whitelist.setState(sign1);
                this.checkedListBox1.Items.Clear();

            }
        }
        //////////////////////////////////
    }
}
