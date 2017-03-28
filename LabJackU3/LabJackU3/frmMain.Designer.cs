namespace LabJackU3 {
    partial class frmMain {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
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
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lbConsole = new System.Windows.Forms.ListBox();
            this.btnChan0 = new System.Windows.Forms.Button();
            this.btnChan1 = new System.Windows.Forms.Button();
            this.cbDebugLevel = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(14, 232);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 13);
            this.lblStatus.TabIndex = 1;
            // 
            // lbConsole
            // 
            this.lbConsole.FormattingEnabled = true;
            this.lbConsole.Location = new System.Drawing.Point(12, 42);
            this.lbConsole.Name = "lbConsole";
            this.lbConsole.Size = new System.Drawing.Size(418, 186);
            this.lbConsole.TabIndex = 2;
            // 
            // btnChan0
            // 
            this.btnChan0.BackColor = System.Drawing.Color.Black;
            this.btnChan0.Enabled = false;
            this.btnChan0.Location = new System.Drawing.Point(718, 189);
            this.btnChan0.Name = "btnChan0";
            this.btnChan0.Size = new System.Drawing.Size(46, 39);
            this.btnChan0.TabIndex = 3;
            this.btnChan0.Text = "-";
            this.btnChan0.UseVisualStyleBackColor = false;
            // 
            // btnChan1
            // 
            this.btnChan1.BackColor = System.Drawing.Color.Black;
            this.btnChan1.Enabled = false;
            this.btnChan1.Location = new System.Drawing.Point(666, 189);
            this.btnChan1.Name = "btnChan1";
            this.btnChan1.Size = new System.Drawing.Size(46, 39);
            this.btnChan1.TabIndex = 4;
            this.btnChan1.Text = "-";
            this.btnChan1.UseVisualStyleBackColor = false;
            // 
            // cbDebugLevel
            // 
            this.cbDebugLevel.FormattingEnabled = true;
            this.cbDebugLevel.Location = new System.Drawing.Point(649, 13);
            this.cbDebugLevel.Name = "cbDebugLevel";
            this.cbDebugLevel.Size = new System.Drawing.Size(121, 21);
            this.cbDebugLevel.TabIndex = 5;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(355, 13);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(776, 261);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.cbDebugLevel);
            this.Controls.Add(this.btnChan1);
            this.Controls.Add(this.btnChan0);
            this.Controls.Add(this.lbConsole);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.button1);
            this.Name = "frmMain";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ListBox lbConsole;
        private System.Windows.Forms.Button btnChan0;
        private System.Windows.Forms.Button btnChan1;
        private System.Windows.Forms.ComboBox cbDebugLevel;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.BindingSource bindingSource1;
    }
}

