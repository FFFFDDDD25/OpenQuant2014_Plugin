using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using IF;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace IF
{
    public class IniFile  
    {
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        ConcurrentDictionary<string, string> _readStored = new ConcurrentDictionary<string, string>();


        public string _path_ini;
        public string _path_folder;


        static HashSet<string> AllPathes = new HashSet<string>();

        public IniFile(string folder_name)
        {
            string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            _path_folder = path_desktop + "\\" + folder_name;
            _path_ini = _path_folder + "\\init.ini";

            if (AllPathes.Contains(_path_ini))
            {
                throw new Exception("two instances use same ini file");
            }
            else
            {
                AllPathes.Add(_path_ini);
            }

            DirectoryInfo TargetIniFile = new DirectoryInfo(_path_folder);

            TargetIniFile.Refresh();
            if (!TargetIniFile.Exists)
                TargetIniFile.Create();
        }


        public static string ReadOrWrite(string Key, string Value, string className)
        {
            string rtn = Read(Key, className);

            if (rtn == "")
                Write(Key, Value, className);
            else
                return rtn;

            return Read(Key, className);
        }

         
        public static string CallingClass()
        {
            string fullName;
            Type declaringType;
            int skipFrames = 1;
            do
            {
                MethodBase method = new StackFrame(skipFrames, false).GetMethod();
                declaringType = method.DeclaringType;
                if (declaringType == null)
                {
                    return method.Name;
                }
                skipFrames++;
                fullName = declaringType.FullName;
            }
            while (declaringType.Module.Name.Equals("mscorlib.dll", StringComparison.OrdinalIgnoreCase));

            return fullName.Split('.').Last();
        }


        public string ReadOrWrite(string Key, string Value)
        {
            string rtn = Read(Key);

            if (rtn == "")
                Write(Key, Value);
            else
                return rtn;

            return Read(Key);
        }


        public static void Write(string Key, string Value, string folder_name)
        {
            string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string path_folder = path_desktop + "\\" + folder_name;
            string path_ini = path_folder + "\\init.ini";

            DirectoryInfo TargetIniFile = new DirectoryInfo(path_folder);

            TargetIniFile.Refresh();
            if (!TargetIniFile.Exists)
                TargetIniFile.Create();



            WritePrivateProfileString("", Key, Value, path_ini);
        }
        public static string Read(string Key,string folder_name)
        {
            string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string path_folder = path_desktop + "\\" + folder_name;
            string path_ini = path_folder + "\\init.ini";

            DirectoryInfo TargetIniFile = new DirectoryInfo(path_folder);

            TargetIniFile.Refresh();
            if (!TargetIniFile.Exists)
                TargetIniFile.Create();




            string section = "";
            //string dicKey = Path + section + Key;


            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(section, Key, "", RetVal, 255, path_ini);

            string rtn = RetVal.ToString();

            return rtn;
        }


        public string Read(string Key)
        {
            string section = "";
            //string dicKey = Path + section + Key;

			string target;
            if (_readStored.TryGetValue(Key, out  target))
                return target;

            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(section, Key, "", RetVal, 255, _path_ini);

            string rtn = RetVal.ToString();

            if (rtn != "")
                _readStored.TryAdd(Key, rtn);
            return rtn;
        }

        public void Write(string Key, string Value)
        {
            _readStored[Key] = Value;// add or update
            WritePrivateProfileString("", Key, Value, _path_ini);
        }


#if false
        public void DeleteKey(string Key, string Section = null)
        {
            Write(Key, null, "");
        }

        public void DeleteSection(string Section = null)
        {
            Write(null, null, "");
        }
#endif

        public bool KeyExists(string Key, string Section = null)
        {
            return Read(Key).Length > 0;
        }
    }
}

