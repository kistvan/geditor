using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SlimDX;
using SlimDX.Direct3D9;
using SlimDX.Windows;
using System.IO;
using System.Text;
using Microsoft.Win32;
using System.Reflection;

namespace gameEditor2
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            StreamWriter st = null;
            try
            {
                st = new StreamWriter(@".\editor.log", false, Encoding.UTF8);
                checkDotNetVersion(st);
                //OS
                st.WriteLine("OS:" +  Environment.OSVersion.VersionString);
                string procArch = System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
                if (procArch == null || procArch == "x86")
                {
                    st.WriteLine("arch:x86");
                }
                else {
                    st.WriteLine("arch:" + procArch);
                }
                Assembly[] ass = AppDomain.CurrentDomain.GetAssemblies();
                foreach(Assembly asm in ass) {
                    st.WriteLine(asm.FullName);
                }
                Form1 form1 = new Form1();
                form1.init(st);
                MessagePump.Run(form1, form1.DrawFrame);

                form1.OnResourceUnload();

            }
            catch (Exception ex)
            {
                Console.WriteLine("error:" + ex.Message);
                Console.WriteLine("stack:" + ex.StackTrace);
                throw ex;
            }
            finally { 
                if(st != null) {
                    st.Close();
                }
            }
            foreach (var item in ObjectTable.Objects)
                item.Dispose();
        }

        private static void checkDotNetVersion(StreamWriter st) {
            st.WriteLine(".Net Frameworkのバージョンをチェックします");
            String val = getRegistry(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v2.0.50727", "Install", st);
            if (val.Equals("1"))
            {
                st.WriteLine("2.0");
            }
            val = getRegistry(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.0", "Install", st);
            if (val.Equals("1"))
            {
                st.WriteLine("3.0");
            }
            val = getRegistry(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5", "Install", st);
            if(val.Equals("1")) {
                st.WriteLine("3.5");
            }
            val = getRegistry(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Client", "Install", st);
            if (val.Equals("1"))
            {
                st.WriteLine("4.0 client");
            }
        }

        private static String getRegistry(String key, String name, StreamWriter st) {
            RegistryKey rk = null;
            try
            {
                rk = Registry.LocalMachine.OpenSubKey(key);
                String s = rk.GetValue(name).ToString();
                return s;
            }
            catch (Exception ex)
            {
                return "" + ex.Message;
            }
            finally { 
                if(rk != null) {
                    rk.Close();
                }
            }
        }


        delegate IntPtr GetHandleDelegate();
    }
}
