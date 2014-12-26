namespace DLCustomMapInstaller
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
            this.tbBackupFolder = new System.Windows.Forms.TextBox();
            this.buttBrowseBackup = new System.Windows.Forms.Button();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.buttAddMaps = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.buttDLMaps = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbBackupFolder
            // 
            this.tbBackupFolder.Location = new System.Drawing.Point(12, 12);
            this.tbBackupFolder.Name = "tbBackupFolder";
            this.tbBackupFolder.Size = new System.Drawing.Size(351, 20);
            this.tbBackupFolder.TabIndex = 0;
            // 
            // buttBrowseBackup
            // 
            this.buttBrowseBackup.Location = new System.Drawing.Point(369, 9);
            this.buttBrowseBackup.Name = "buttBrowseBackup";
            this.buttBrowseBackup.Size = new System.Drawing.Size(75, 23);
            this.buttBrowseBackup.TabIndex = 1;
            this.buttBrowseBackup.Text = "Browse";
            this.buttBrowseBackup.UseVisualStyleBackColor = true;
            this.buttBrowseBackup.Click += new System.EventHandler(this.buttBrowseBackup_Click);
            // 
            // tbLog
            // 
            this.tbLog.Location = new System.Drawing.Point(12, 38);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.Size = new System.Drawing.Size(432, 232);
            this.tbLog.TabIndex = 2;
            // 
            // buttAddMaps
            // 
            this.buttAddMaps.Location = new System.Drawing.Point(369, 274);
            this.buttAddMaps.Name = "buttAddMaps";
            this.buttAddMaps.Size = new System.Drawing.Size(75, 23);
            this.buttAddMaps.TabIndex = 3;
            this.buttAddMaps.Text = "Add Maps";
            this.buttAddMaps.UseVisualStyleBackColor = true;
            this.buttAddMaps.Click += new System.EventHandler(this.buttAddMaps_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(12, 280);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(129, 17);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "Save Original Backup";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // buttDLMaps
            // 
            this.buttDLMaps.Location = new System.Drawing.Point(252, 274);
            this.buttDLMaps.Name = "buttDLMaps";
            this.buttDLMaps.Size = new System.Drawing.Size(111, 23);
            this.buttDLMaps.TabIndex = 5;
            this.buttDLMaps.Text = "Check for Update";
            this.buttDLMaps.UseVisualStyleBackColor = true;
            this.buttDLMaps.Click += new System.EventHandler(this.buttDLMaps_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 311);
            this.Controls.Add(this.buttDLMaps);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.buttAddMaps);
            this.Controls.Add(this.tbLog);
            this.Controls.Add(this.buttBrowseBackup);
            this.Controls.Add(this.tbBackupFolder);
            this.Name = "Form1";
            this.Text = "Deadlocked HD Custom Map Backup Installer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbBackupFolder;
        private System.Windows.Forms.Button buttBrowseBackup;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.Button buttAddMaps;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button buttDLMaps;
    }
}

