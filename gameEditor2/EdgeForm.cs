using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using gameEditor2.model;

namespace gameEditor2
{
    public partial class EdgeForm : Form
    {

        private MapEdge mapEdge;
        internal MapEdge MapEdge {
            set { this.mapEdge = value; }
            get { return mapEdge; }
        }

        public EdgeForm()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void EdgeForm_Shown(object sender, EventArgs e)
        {
            this.closedCheckBox.Checked = !mapEdge.Enabled;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.mapEdge.Enabled = !this.closedCheckBox.Checked;
            this.Dispose();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
