using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;

namespace VoiceroidTimer
{
    static class main
    {
        //iniファイル読み込み用のdll定義
        [DllImport("kernel32.dll")]
        static extern int GetPrivateProfileSectionNames(
        IntPtr lpszReturnBuffer,
        uint nSize,
        string lpFileName);
        [DllImport("KERNEL32.DLL")]
        public static extern uint GetPrivateProfileString(
            string lpAppName,
            string lpKeyName,
            string lpDefault,
            StringBuilder lpReturnedString,
            uint nSize,
            string lpFileName);

        static mlogic mLogic = new mlogic();
        static string[][] inputList;
        static Timer timer = new Timer();
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            iniImport();
            Application.SetCompatibleTextRenderingDefault(false);
            timer.Tick += new EventHandler(timerProcess);
            timer.Interval = 1000;
            Application.Run(new Form1());
        }

        private static void timerProcess(object sender, EventArgs e)
        {
            mLogic.MLogic(inputList);
        }
        private static string GetIniValue(string path, string section, string key)
        {
            StringBuilder sb = new StringBuilder(256);
            GetPrivateProfileString(section, key, string.Empty, sb, Convert.ToUInt32(sb.Capacity), path);
            return sb.ToString();
        }
        private static void arrayImport(string[] input)
        {
            if (inputList == null)
            {
                inputList = new string[1][];
                inputList[0] = input;
            }
            else
            {
                Array.Resize(ref inputList, inputList.Length + 1);
                inputList[inputList.Length - 1] = input;
            }
        }
        public static void iniImport()
        {
            inputList = null;
            String path =
                Application.StartupPath +
                Path.DirectorySeparatorChar +
                "word.ini";
            if (File.Exists(path))
            {
                IntPtr ptr = Marshal.StringToHGlobalAnsi(new String('\0', 1024));
                int length = GetPrivateProfileSectionNames(ptr, 1024, path);

                if (0 < length)
                {
                    String result = Marshal.PtrToStringAnsi(ptr, length);
                    string[] results = result.Split('\0');
                    foreach (string str in results)
                    {
                        string[] input = new string[2];
                        Console.WriteLine(GetIniValue(path, str, "say"));
                        input[0] = str;
                        input[1] = GetIniValue(path, str, "say");
                        arrayImport(input);
                    }
                }

                Marshal.FreeHGlobal(ptr);
            }
        }
        public static bool voiceroidLoad()
        {
            if (!timer.Enabled)
            {
                if (mLogic.init())
                {
                    timer.Enabled = true;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
        public static string[] getCharacter()
        {
            return mLogic.getCharacter();
        }
        public static void setCharacter(int avatorIdx)
        {
            mLogic.setCharacter(avatorIdx);
        }
    }
}
