using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CleanCAAWorkSpace
{
    public class Global
    {/// 启动控制台
        private bool _enable = false;
        public bool Enable
        {
            get
            {
                return _enable;
            }
            set
            {
                _enable = value;
                initConsole();
            }

        }
        public Global()
        {
            this.Enable = true;
        }
        public Global(bool enable)
        {
            this.Enable = enable;
        }
        public virtual void initConsole()
        {
            if (!_enable) return;
            AllocConsole();
            Console.WindowWidth = 136;
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();
        }
        public void Print(string str)
        {
            if (_enable) Console.WriteLine(str);
        }
        public void Print(double str)
        {
            if (_enable) Console.WriteLine(str.ToString());
        }
        public void Print(int str)
        {
            if (_enable) Console.WriteLine(str.ToString());
        }
        public void Print(float str)
        {
            if (_enable) Console.WriteLine(str.ToString());
        }
        public void Print(IEnumerable<string> collection)
        {
            if (!_enable) return;
            foreach (string str in collection)
            {
                Console.WriteLine(str);
            }

        }
        public void Print(IEnumerable<double> collection)
        {
            if (!_enable) return;
            foreach (double str in collection)
            {
                Console.WriteLine(str.ToString());
            }

        }
        public void Print(IEnumerable<float> collection)
        {
            if (!_enable) return;
            foreach (float str in collection)
            {
                Console.WriteLine(str.ToString());
            }

        }
        public void Print(IEnumerable<int> collection)
        {
            if (!_enable) return;
            foreach (int str in collection)
            {
                Console.WriteLine(str.ToString());
            }

        }

        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();
        /// 释放控制台
        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();

        public static bool _deletefolder_withoutroot(string dir)
        {
            if (Directory.Exists(dir))
            {
                foreach (string d in Directory.GetFileSystemEntries(dir))
                {
                    if (File.Exists(d))
                        File.Delete(d);
                    else
                        _deletefolder(d);
                }
            }
            else { return false; }
            return true;
        }
        public static bool _deletefolder(string dir)
        {
            if (Directory.Exists(dir))
            {
                foreach (string d in Directory.GetFileSystemEntries(dir))
                {
                    if (File.Exists(d))
                        File.Delete(d);
                    else
                        _deletefolder(d);
                }
                Directory.Delete(dir);
                return true;
            }
            return false;
        }
        public static bool _deletefiles(string dir)
        {
            if (Directory.Exists(dir))
            {
                foreach (string d in Directory.GetFileSystemEntries(dir))
                {
                    if (File.Exists(d))
                        File.Delete(d);
                    else
                        _deletefiles(d);
                }
                return true;
            }
            return false;
        }
        public static void _SearchFiles(string dir, ref List<string> type, ref List<string> output)
        {
            //通过文件名或者或者扩展名
            if (Directory.Exists(dir))
            {
                foreach (string d in Directory.GetFileSystemEntries(dir))
                {
                    if (File.Exists(d))
                    {
                        string name = Path.GetFileNameWithoutExtension(d);
                        string extension = Path.GetExtension(d);
                        for (int i = 0; i < type.Count; i++)
                        {
                            if (name == type[i]) { output.Add(d); break; }
                            if (extension == type[i]) { output.Add(d); break; }
                        }
                    }
                    else
                        _SearchFiles(d, ref type, ref output);
                }
            }

        }
        public static void _SearchFolders(string dir, ref List<string> type, ref List<string> output)
        {//文件夹名称
            if (Directory.Exists(dir))
            {
                string basefolder = Path.GetFileName(dir);

                for (int i = 0; i < type.Count; i++)
                {
                    if (basefolder == type[i]) { output.Add(dir); break; }
                }

                foreach (string d in Directory.GetFileSystemEntries(dir))
                {
                    _SearchFolders(d, ref type, ref output);
                }
            }
        }
        public static void _deletefiles_withWhitelist(string dir, ref List<string> _whitelist)
        {
            if (_whitelist.Contains(dir)) return;
            if (Directory.Exists(dir))
            {
                foreach (string d in Directory.GetFileSystemEntries(dir))
                {
                    if (!_whitelist.Contains(d))
                    {
                        if (File.Exists(d)) { File.Delete(d); }
                        else { _deletefiles_withWhitelist(d, ref _whitelist); }
                    }
                }
            }
        }
        public static List<string> From0Find1by2Name(string intput, string folder)
        {
            // For example "D:\0\1\2\..." "D:\0\4\2\..."
            // "D:\0"=intput "D:\0\1\2"=folder
            //{ "D:\0\1","D:\0\4"}=output
            List<string> output = new List<string>();
            foreach (string d1 in Directory.GetFileSystemEntries(intput))
            {
                if (Directory.Exists(d1))
                {
                    bool sign = false;
                    foreach (string d2 in Directory.GetFileSystemEntries(d1))
                    {
                        if (Directory.Exists(d2) && d2.Contains(folder))
                        {
                            sign = true;
                            break;
                        }
                    }

                    if (sign)
                    {
                        output.Add(d1);
                    }
                }
            }
            return output;
        }

    }
}