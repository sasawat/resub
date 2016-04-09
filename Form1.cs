using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace resubS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void buttonLoadMKV_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            tbxInFileName.Text = openFileDialog1.FileName;
        }

        private void buttonSaveTo_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
            tbxOutFileName.Text = saveFileDialog1.FileName;
        }
    }
}
