using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _700IQ
{
    class IniFile
    {
        public string path;

        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal,
            int size, string filePath);
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileSection(string lpAppName, byte[] lpszReturnBuffer, int nSize, string lpFileName);

        /// <summary>
        /// INIFile Constructor.
        /// </summary>
        /// <PARAM name="INIPath"></PARAM>
        public IniFile(string INIPath)
        {
            path = INIPath;
        }
        /// <summary>
        /// Write Data to the INI File
        /// </summary>
        /// <PARAM name="Section"></PARAM>
        /// Section name
        /// <PARAM name="Key"></PARAM>
        /// Key Name
        /// <PARAM name="Value"></PARAM>
        /// Value Name
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.path);
        }

        /// <summary>
        /// Read Data Value From the Ini File
        /// </summary>
        /// <PARAM name="Section"></PARAM>
        /// <PARAM name="Key"></PARAM>
        /// <PARAM name="Default"></PARAM>
        /// <returns></returns>
        public string IniReadValue(string Section, string Key, string Default)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, Default, temp,
                                            255, this.path);
            return temp.ToString();
        }

        /// <summary>
        /// Read All sections/keys of Ini File
        /// </summary>
        /// <PARAM name="Section"></PARAM>
        /// <returns></returns>
        public List<string> GetAllKeys(string Section)
        {
            byte[] buffer = new byte[2048];

            List<string> result = new List<string>();
            if (GetPrivateProfileSection(Section, buffer, 2048, this.path) > 0)
            {
                String[] tmp = Encoding.ASCII.GetString(buffer).Trim('\0').Split('\0');
                foreach (String entry in tmp)
                {
                    result.Add(entry.Substring(0, entry.IndexOf("=")));
                }
            }
            return result;
        }

        /// <summary> Return an entire INI section as a list of lines.  Blank lines are ignored and all spaces around the = are also removed. </summary>
        /// <param name="section">[Section]</param>
        /// <param name="file">INI File</param>
        /// <returns> List of lines </returns>
        public IEnumerable<KeyValuePair<string, string>> GetIniSection(string Section)
        {
            var result = new List<KeyValuePair<string, string>>();
            String[] iniLines;
            byte[] buffer = new byte[2048];
            if (GetPrivateProfileSection(Section, buffer, buffer.Length, this.path) > 0)
            {
                iniLines = Encoding.ASCII.GetString(buffer).Trim('\0').Split('\0');
                foreach (var line in iniLines)
                {
                    var m = System.Text.RegularExpressions.Regex.Match(line, @"^([^=]+)\s*=\s*(.*)");
                    result.Add(m.Success
                                   ? new KeyValuePair<string, string>(m.Groups[1].Value, m.Groups[2].Value)
                                   : new KeyValuePair<string, string>(line, ""));
                }
            }
            return result;
        }
    }
}
