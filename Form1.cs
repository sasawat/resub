using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using resub;

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
            foreach(DictFile x in Config.Dictlist)
            {
                lbxDict.Items.Add(x);
            }
            cbxIncludeOrigSubInOutput.Checked = true;
            cbxAllDict.Checked = true;
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

        private void btnNewDict_Click(object sender, EventArgs e)
        {
            //Get FileName
            var ofdresult = openFileDialog1.ShowDialog();
            if (ofdresult != DialogResult.OK) return;
            string filename = openFileDialog1.FileName;
            //Get Name
            var nameDialog = new NameDictionaryForm();
            var ndresult = nameDialog.ShowDialog();
            if (ndresult != DialogResult.OK) return;
            string name = nameDialog.ChosenName;
            //Add the DictFile
            Config.Dictlist.Add(new DictFile(filename, name));
            lbxDict.Items.Add(new DictFile(filename, name));
            //Refresh checkbox
            cbxAllDict.Checked = cbxAllDict.Checked;
        }

        private void btnRemoveDict_Click(object sender, EventArgs e)
        {
            //Remove from Config.Dictlist
            var toremove = lbxDict.SelectedItems;
            foreach(var x in toremove)
            {
                if(Config.Dictlist.Contains(x))
                {
                    Config.Dictlist.Remove((DictFile)x);
                }
            }
            //Remove from Listbox
            for (int i = lbxDict.SelectedIndices.Count - 1; i >= 0; i--)
            {
                lbxDict.Items.RemoveAt(lbxDict.SelectedIndices[i]);
            }
        }

        private void StatusPrint(string line)
        {
            tbxStatus.Text += line + Environment.NewLine;
            tbxStatus.Select(tbxStatus.Text.Length - 1, 0);
        }

        private void StatusClear()
        {
            tbxStatus.Text = "";
        }

        private void updateProgressBar(object sender, ProgressChangedEventArgs pceh)
        {
            progressBar.Value = pceh.ProgressPercentage;
            StatusPrint((string)pceh.UserState);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            //Reset the progress bar
            progressBar.Value = 0;
            //Clear the status box
            StatusClear();
            //Run!
            Program.ResubCore.printlnfunc = StatusPrint;
            if (cbxAllDict.Checked)
            {
                Program.ResubCore.runAsync(tbxInFileName.Text, tbxOutFileName.Text,
                    Config.Dictlist, !cbxIncludeOrigSubInOutput.Checked, 
                    new ProgressChangedEventHandler(updateProgressBar));
            }
            else
            {
                var tempdictlist = new List<DictFile>();
                foreach(var x in lbxDict.SelectedItems)
                {
                    tempdictlist.Add((DictFile)x);
                }
                Program.ResubCore.runAsync(tbxInFileName.Text, tbxOutFileName.Text,
                    tempdictlist, !cbxIncludeOrigSubInOutput.Checked,
                    new ProgressChangedEventHandler(updateProgressBar));
            }
        }

        private void disableDictionarySelection()
        {
            for (int i = 0; i < lbxDict.Items.Count; ++i) lbxDict.SetSelected(i, true);
            lbxDict.Enabled = false;
        }

        private void enableDictionarySelection()
        {
            lbxDict.Enabled = true;
            lbxDict.SelectionMode = SelectionMode.MultiExtended;
            lbxDict.ClearSelected();
        }

        private void cbxAllDict_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxAllDict.Checked) disableDictionarySelection();
            else enableDictionarySelection();
        }
    }
}
