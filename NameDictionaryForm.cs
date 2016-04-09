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
    public partial class NameDictionaryForm : Form
    {
        public string ChosenName;

        public NameDictionaryForm()
        {
            InitializeComponent();
        }

        private void NameDictionaryForm_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (
                tbxName.Text.Contains(",") ||
                tbxName.Text.Contains(":")
                )
            {
                MessageBox.Show("Name contains illegal characters please choose a different name.");
                return;
            }
            ChosenName = tbxName.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
