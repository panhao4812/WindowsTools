using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CleanCAAWorkSpace
{
    public class WhiteList
    {
        Global p;
        public List<string> _framework;
        public List<bool> s_framework;
        public List<string> _module;
        public List<bool> s_module;
        public List<string> _workspace;
        public List<bool> s_workspace;
        public List<string> _filetype;
        public List<bool> s_filetype;
        public void Clear()
        {
            _framework = new List<string>(); s_framework = new List<bool>();
            _module = new List<string>(); s_module = new List<bool>();
            _workspace = new List<string>(); s_workspace = new List<bool>();
            _filetype = new List<string>(); s_filetype = new List<bool>();
        }

        public WhiteList(Global _p)
        {
            Clear();
            p = _p;
            this._framework.Add("\\IdentityCard"); s_framework.Add(true);
            this._framework.Add("\\PublicInterfaces"); s_framework.Add(true);
            this._framework.Add("\\PrivateInterfaces"); s_framework.Add(true);
            this._framework.Add("\\ProtectedInterfaces"); s_framework.Add(true);
            this._framework.Add("\\CNext"); s_framework.Add(true);
            this._framework.Add("\\various"); s_framework.Add(true);

            this._module.Add("\\src"); s_module.Add(true);
            this._module.Add("\\LocalInterfaces"); s_module.Add(true);
            this._module.Add("\\Imakefile.mk"); s_module.Add(true);

            this._filetype.Add("CATIAV5Level"); s_filetype.Add(true);
            this._filetype.Add("Install_config_win_b64"); s_filetype.Add(true);
            this._filetype.Add(".doc"); s_filetype.Add(true);
            this._filetype.Add(".docx"); s_filetype.Add(true);
            this._filetype.Add(".gitattributes"); s_filetype.Add(true);
            this._filetype.Add(".gitignore"); s_filetype.Add(true);
            this._filetype.Add(".git"); s_filetype.Add(true);
            this._filetype.Add(".svn"); s_filetype.Add(true);
        }
        public List<string> ToPrint()
        {
            List<string> str = new List<string>();
            for (int i = 0; i < this._workspace.Count; i++)
            {
                str.Add("[workspace]" + _workspace[i]);
            }
            for (int i = 0; i < this._framework.Count; i++)
            {
                str.Add("[framework]" + _framework[i]);
            }
            for (int i = 0; i < this._module.Count; i++)
            {
                str.Add("[module]" + _module[i]);
            }
            for (int i = 0; i < this._filetype.Count; i++)
            {
                str.Add("[filetype]" + _filetype[i]);
            }
            return str;
        }
        public void setState(List<bool> state)
        {
            int j = 0;
            for (int i = 0; i < this._workspace.Count; i++)
            {
                s_workspace[i] = state[j]; j++;
            }
            for (int i = 0; i < this._framework.Count; i++)
            {
                s_framework[i] = state[j]; j++;
            }
            for (int i = 0; i < this._module.Count; i++)
            {
                s_module[i] = state[j]; j++;
            }
            for (int i = 0; i < this._filetype.Count; i++)
            {
                s_filetype[i] = state[j]; j++;
            }
        }
        public List<string> ToPath(string Workspace, List<string> Framework, List<string> Module)
        {

            List<string> str = new List<string>();
            if (!Directory.Exists(Workspace)) { return str; }

            for (int i = 0; i < this._workspace.Count; i++)
            {
                if (s_workspace[i]) str.Add(Workspace + _workspace[i]);
            }

            foreach (string framework in Framework)
            {
                if (Directory.Exists(framework))
                {
                    for (int i = 0; i < this._framework.Count; i++)
                    {
                        if (s_framework[i]) str.Add(framework + _framework[i]);
                    }
                }
            }
            foreach (string module in Module)
            {
                if (Directory.Exists(module))
                {
                    for (int i = 0; i < this._module.Count; i++)
                    {
                        if (s_module[i]) str.Add(module + _module[i]);
                    }
                }
            }
            List<string> temptype = new List<string>();
            for (int i = 0; i < _filetype.Count; i++)
            {
                if (s_filetype[i]) temptype.Add(_filetype[i]);
            }
            p.Print("_SearchFiles");
            Global._SearchFiles(Workspace, ref temptype, ref str);
            p.Print("_SearchFolders");
            Global._SearchFolders(Workspace, ref temptype, ref str);

            return str;

        }
    }
}
