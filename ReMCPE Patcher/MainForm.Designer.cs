namespace ReMCPE_Patcher
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.disclaimerLbl = new System.Windows.Forms.Label();
            this.actionBtn = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.statusLbl = new System.Windows.Forms.Label();
            this.workingDirTextLbl = new System.Windows.Forms.Label();
            this.workingDirLbl = new System.Windows.Forms.Label();
            this.settingsLbl = new System.Windows.Forms.Button();
            this.aboutLbl = new System.Windows.Forms.Label();
            this.gitLinkLbl = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // disclaimerLbl
            // 
            this.disclaimerLbl.AutoSize = true;
            this.disclaimerLbl.Location = new System.Drawing.Point(12, 249);
            this.disclaimerLbl.Name = "disclaimerLbl";
            this.disclaimerLbl.Size = new System.Drawing.Size(475, 13);
            this.disclaimerLbl.TabIndex = 0;
            this.disclaimerLbl.Text = "DISCLAIMER: This Tool is not affiliated with MOJANG in any way. Requires an inter" +
    "net connection.";
            // 
            // actionBtn
            // 
            this.actionBtn.Location = new System.Drawing.Point(124, 103);
            this.actionBtn.Name = "actionBtn";
            this.actionBtn.Size = new System.Drawing.Size(235, 42);
            this.actionBtn.TabIndex = 1;
            this.actionBtn.Text = "Select Repo Directory";
            this.actionBtn.UseVisualStyleBackColor = true;
            this.actionBtn.Click += new System.EventHandler(this.selectDirBtn_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(90, 214);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(303, 23);
            this.progressBar.TabIndex = 2;
            // 
            // statusLbl
            // 
            this.statusLbl.AutoSize = true;
            this.statusLbl.Location = new System.Drawing.Point(87, 198);
            this.statusLbl.Name = "statusLbl";
            this.statusLbl.Size = new System.Drawing.Size(129, 13);
            this.statusLbl.TabIndex = 3;
            this.statusLbl.Text = "Status: Waiting for input...";
            // 
            // workingDirTextLbl
            // 
            this.workingDirTextLbl.AutoSize = true;
            this.workingDirTextLbl.Location = new System.Drawing.Point(12, 36);
            this.workingDirTextLbl.Name = "workingDirTextLbl";
            this.workingDirTextLbl.Size = new System.Drawing.Size(95, 13);
            this.workingDirTextLbl.TabIndex = 4;
            this.workingDirTextLbl.Text = "Working Directory:";
            // 
            // workingDirLbl
            // 
            this.workingDirLbl.AutoSize = true;
            this.workingDirLbl.Location = new System.Drawing.Point(121, 36);
            this.workingDirLbl.Name = "workingDirLbl";
            this.workingDirLbl.Size = new System.Drawing.Size(79, 13);
            this.workingDirLbl.TabIndex = 5;
            this.workingDirLbl.Text = "None selected.";
            // 
            // settingsLbl
            // 
            this.settingsLbl.Location = new System.Drawing.Point(445, 4);
            this.settingsLbl.Name = "settingsLbl";
            this.settingsLbl.Size = new System.Drawing.Size(46, 23);
            this.settingsLbl.TabIndex = 6;
            this.settingsLbl.Text = "...";
            this.settingsLbl.UseVisualStyleBackColor = true;
            this.settingsLbl.Click += new System.EventHandler(this.settingsLbl_Click);
            // 
            // aboutLbl
            // 
            this.aboutLbl.AutoSize = true;
            this.aboutLbl.Location = new System.Drawing.Point(12, 9);
            this.aboutLbl.Name = "aboutLbl";
            this.aboutLbl.Size = new System.Drawing.Size(216, 13);
            this.aboutLbl.TabIndex = 7;
            this.aboutLbl.Text = "Patcher for iProgramInCpp\'s ReMinecraftPE.";
            // 
            // gitLinkLbl
            // 
            this.gitLinkLbl.AutoSize = true;
            this.gitLinkLbl.Location = new System.Drawing.Point(234, 9);
            this.gitLinkLbl.Name = "gitLinkLbl";
            this.gitLinkLbl.Size = new System.Drawing.Size(40, 13);
            this.gitLinkLbl.TabIndex = 8;
            this.gitLinkLbl.TabStop = true;
            this.gitLinkLbl.Text = "GitHub";
            this.gitLinkLbl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.gitLinkLbl_LinkClicked);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 271);
            this.Controls.Add(this.gitLinkLbl);
            this.Controls.Add(this.aboutLbl);
            this.Controls.Add(this.settingsLbl);
            this.Controls.Add(this.workingDirLbl);
            this.Controls.Add(this.workingDirTextLbl);
            this.Controls.Add(this.statusLbl);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.actionBtn);
            this.Controls.Add(this.disclaimerLbl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(515, 310);
            this.MinimumSize = new System.Drawing.Size(515, 310);
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "ReMCPE Patcher";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label disclaimerLbl;
        private System.Windows.Forms.Button actionBtn;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label statusLbl;
        private System.Windows.Forms.Label workingDirTextLbl;
        private System.Windows.Forms.Label workingDirLbl;
        private System.Windows.Forms.Button settingsLbl;
        private System.Windows.Forms.Label aboutLbl;
        private System.Windows.Forms.LinkLabel gitLinkLbl;
    }
}

