﻿namespace resubS
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonLoadMKV = new System.Windows.Forms.Button();
            this.buttonSaveTo = new System.Windows.Forms.Button();
            this.tbxInFileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbxOutFileName = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.gbxOpt = new System.Windows.Forms.GroupBox();
            this.cbxIncludeOrigSubInOutput = new System.Windows.Forms.CheckBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnNewDict = new System.Windows.Forms.Button();
            this.btnRemoveDict = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.tbxStatus = new System.Windows.Forms.TextBox();
            this.gbxOpt.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonLoadMKV
            // 
            this.buttonLoadMKV.Location = new System.Drawing.Point(655, 4);
            this.buttonLoadMKV.Name = "buttonLoadMKV";
            this.buttonLoadMKV.Size = new System.Drawing.Size(117, 23);
            this.buttonLoadMKV.TabIndex = 0;
            this.buttonLoadMKV.Text = "Browse";
            this.buttonLoadMKV.UseVisualStyleBackColor = true;
            this.buttonLoadMKV.Click += new System.EventHandler(this.buttonLoadMKV_Click);
            // 
            // buttonSaveTo
            // 
            this.buttonSaveTo.Location = new System.Drawing.Point(655, 33);
            this.buttonSaveTo.Name = "buttonSaveTo";
            this.buttonSaveTo.Size = new System.Drawing.Size(117, 23);
            this.buttonSaveTo.TabIndex = 1;
            this.buttonSaveTo.Text = "Browse";
            this.buttonSaveTo.UseVisualStyleBackColor = true;
            this.buttonSaveTo.Click += new System.EventHandler(this.buttonSaveTo_Click);
            // 
            // tbxInFileName
            // 
            this.tbxInFileName.Location = new System.Drawing.Point(78, 6);
            this.tbxInFileName.Name = "tbxInFileName";
            this.tbxInFileName.Size = new System.Drawing.Size(571, 20);
            this.tbxInFileName.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Input File";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Output File";
            // 
            // tbxOutFileName
            // 
            this.tbxOutFileName.Location = new System.Drawing.Point(78, 33);
            this.tbxOutFileName.Name = "tbxOutFileName";
            this.tbxOutFileName.Size = new System.Drawing.Size(571, 20);
            this.tbxOutFileName.TabIndex = 5;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // gbxOpt
            // 
            this.gbxOpt.Controls.Add(this.btnRemoveDict);
            this.gbxOpt.Controls.Add(this.btnNewDict);
            this.gbxOpt.Controls.Add(this.label3);
            this.gbxOpt.Controls.Add(this.listBox1);
            this.gbxOpt.Controls.Add(this.cbxIncludeOrigSubInOutput);
            this.gbxOpt.Location = new System.Drawing.Point(15, 62);
            this.gbxOpt.Name = "gbxOpt";
            this.gbxOpt.Size = new System.Drawing.Size(480, 287);
            this.gbxOpt.TabIndex = 6;
            this.gbxOpt.TabStop = false;
            this.gbxOpt.Text = "Options";
            // 
            // cbxIncludeOrigSubInOutput
            // 
            this.cbxIncludeOrigSubInOutput.AutoSize = true;
            this.cbxIncludeOrigSubInOutput.Location = new System.Drawing.Point(7, 20);
            this.cbxIncludeOrigSubInOutput.Name = "cbxIncludeOrigSubInOutput";
            this.cbxIncludeOrigSubInOutput.Size = new System.Drawing.Size(198, 17);
            this.cbxIncludeOrigSubInOutput.TabIndex = 0;
            this.cbxIncludeOrigSubInOutput.Text = "Include original subtitles in output file";
            this.cbxIncludeOrigSubInOutput.UseVisualStyleBackColor = true;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(6, 74);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(468, 199);
            this.listBox1.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Choose Dictionary Files";
            // 
            // btnNewDict
            // 
            this.btnNewDict.Location = new System.Drawing.Point(130, 45);
            this.btnNewDict.Name = "btnNewDict";
            this.btnNewDict.Size = new System.Drawing.Size(159, 23);
            this.btnNewDict.TabIndex = 3;
            this.btnNewDict.Text = "Load new dictionary file";
            this.btnNewDict.UseVisualStyleBackColor = true;
            // 
            // btnRemoveDict
            // 
            this.btnRemoveDict.Location = new System.Drawing.Point(295, 45);
            this.btnRemoveDict.Name = "btnRemoveDict";
            this.btnRemoveDict.Size = new System.Drawing.Size(179, 23);
            this.btnRemoveDict.TabIndex = 4;
            this.btnRemoveDict.Text = "Remove selected dictionary files";
            this.btnRemoveDict.UseVisualStyleBackColor = true;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(501, 62);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(271, 23);
            this.btnStart.TabIndex = 7;
            this.btnStart.Text = "Start resub";
            this.btnStart.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(503, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Status";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(546, 92);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(226, 23);
            this.progressBar1.TabIndex = 9;
            // 
            // tbxStatus
            // 
            this.tbxStatus.Location = new System.Drawing.Point(502, 123);
            this.tbxStatus.Multiline = true;
            this.tbxStatus.Name = "tbxStatus";
            this.tbxStatus.ReadOnly = true;
            this.tbxStatus.Size = new System.Drawing.Size(270, 212);
            this.tbxStatus.TabIndex = 10;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 361);
            this.Controls.Add(this.tbxStatus);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.gbxOpt);
            this.Controls.Add(this.tbxOutFileName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbxInFileName);
            this.Controls.Add(this.buttonSaveTo);
            this.Controls.Add(this.buttonLoadMKV);
            this.MaximumSize = new System.Drawing.Size(800, 400);
            this.MinimumSize = new System.Drawing.Size(800, 400);
            this.Name = "Form1";
            this.Text = "resub S";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.gbxOpt.ResumeLayout(false);
            this.gbxOpt.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonLoadMKV;
        private System.Windows.Forms.Button buttonSaveTo;
        private System.Windows.Forms.TextBox tbxInFileName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbxOutFileName;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.GroupBox gbxOpt;
        private System.Windows.Forms.CheckBox cbxIncludeOrigSubInOutput;
        private System.Windows.Forms.Button btnNewDict;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button btnRemoveDict;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TextBox tbxStatus;
    }
}
