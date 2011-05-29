using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SlimDX.Windows;
using SlimDX;

namespace gameEditor2Client
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
//            Application.Run(new Form1());
            Form1 form1 = new Form1();
            form1.init();
            MessagePump.Run(form1, form1.drawFrame);

            form1.release();
            foreach (var item in ObjectTable.Objects)
                item.Dispose();

        }
    }
}
