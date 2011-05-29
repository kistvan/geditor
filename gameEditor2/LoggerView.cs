using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace gameEditor2
{
    public partial class LoggerView : Form
    {
        public LoggerView()
        {
            InitializeComponent();
        }

        public void appendLog(String text) {
            List<String> list = textBox1.Lines.ToList();
            list.Add(text);
            textBox1.Lines = list.ToArray();
            
        }

        public void setFpsText(String str) {
            fpsTextBox.Text = str;
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            textBox1.Lines = new String[0];
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
