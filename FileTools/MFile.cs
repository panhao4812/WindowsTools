using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTools
{
    public class M_File
    {
        string _path = "";
        string _name = "";
        long _size = 0;
        string _MD5 = "";
        int _sign = 0;
        public string Path
        {
            get
            {
                return _path;
            }

            set
            {
                _path = value;
            }
        }
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }
        public long Size
        {
            get
            {
                return _size;
            }

            set
            {
                _size = value;
            }
        }
        public string MD5
        {
            get
            {
                return _MD5;
            }

            set
            {
                _MD5 = value;
            }
        }
        public int Sign
        {
            get
            {
                return _sign;
            }

            set
            {
                _sign = value;
            }
        }
        public M_File()
        {
            MD5 = "null";
            Path = ""; Name = "";
            Size = 0;
        }
        public M_File(string path)
        {
            MD5 = "null";
            CreateFromPath(path);
        }
        public void CreateFromPath(string path)
        {
            Path = path;
            Name = System.IO.Path.GetFileName(path);
            FileInfo MyFileInfo = new FileInfo(path);
            Size = MyFileInfo.Length;
        }
        public void GetMD5HashFromFile()
        {
            if (MD5 != "null") return;
            try
            {
                FileStream file = new FileStream(this.Path, FileMode.Open, FileAccess.Read);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                MD5 = sb.ToString();
            }
            catch
            {
                MD5 = "null";
                return;
            }
        }
        public bool equalto(M_File f1)
        {
            if (f1.Size != this.Size) return false;
            else
            {
                this.GetMD5HashFromFile();
                f1.GetMD5HashFromFile();
                if (this.MD5 == "null" || f1.MD5 == "null") return false;
                if (this.MD5 == f1.MD5) return true;
                return false;
            }

        }
        public static void ListAllFiles(string dir, ref List<string> output)
        {
            if (Directory.Exists(dir))
            {
                foreach (string d in Directory.GetFileSystemEntries(dir))
                {
                    if (File.Exists(d))
                    {
                        output.Add(d);
                    }
                    else
                        ListAllFiles(d, ref output);
                }
            }
        }
        public static List<M_File> Create(string Path)
        {
            List<M_File> output2 = new List<M_File>();
            List<string> output = new List<string>();
            ListAllFiles(Path, ref output);
            for (int i = 0; i < output.Count; i++)
            {
                output2.Add(new M_File(output[i]));
            }
            //  output2.Sort(SortMethod);
            return output2;
        }
        public static List<M_File> Read(string Path)
        {
            StreamReader sr;
            List<M_File> output = new List<M_File>();
            if (File.Exists(Path)) sr = File.OpenText(Path); else return output;
            try
            {
                while (sr.Peek() != -1)
                {
                    string str = sr.ReadLine();
                    string[] chara = str.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                    if (chara.Length == 2)
                    {
                        M_File f1 = new M_File(chara[0]);
                        f1.MD5 = chara[1];
                        output.Add(f1);
                    }
                }
                sr.Close();
                // output.Sort(SortMethod);
                return output;
            }
            catch
            {
                sr.Close();
                // output.Sort(SortMethod);
                return output;
            }
        }
        public static void Write(string Path, List<M_File> data)
        {
            string str = "";
            for (int i = 0; i < data.Count; i++)
            {
                M_File f1 = data[i];
                str += f1.Path + "|" + f1.MD5; str += "\r\n";
            }
            FileStream fs = new FileStream(Path, FileMode.OpenOrCreate);
            StreamWriter stream = new StreamWriter(fs);
            try
            {
                stream.Write(str);
                stream.Flush();
                stream.Close();
            }
            catch { stream.Close(); }
        }
        public static int SortMethod(M_File a, M_File b)
        {
            if (a == null && b != null) return -1;
            if (b == null && a != null) return 1;
            if (b == null && a == null) return 0;
            if (a.Size > b.Size)
            {
                return 1;
            }
            else if (a.Size < b.Size)
            {
                return -1;
            }
            return 0;
        }
        public static List<M_File> ComputeDuplicate(List<M_File> data)
        {
            List<M_File> output = new List<M_File>();
            data.Sort(SortMethod);
            for (int i = 1; i < data.Count; i++)
            {
                if (data[i - 1].equalto(data[i]) == false)
                {
                    data[i - 1].Sign = 1; data[i].Sign = 1;
                }
            }
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].Sign == 1) output.Add(data[i]);
            }
            return output;
        }
    }
}
