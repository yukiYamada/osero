namespace オセロ君
{
    partial class FConfig
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
            this.btnExeｃ = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.rdbPCFirst = new System.Windows.Forms.RadioButton();
            this.rdbAIFirst = new System.Windows.Forms.RadioButton();
            this.grpFirst = new System.Windows.Forms.GroupBox();
            this.cmbFirst = new System.Windows.Forms.ComboBox();
            this.grpSecond = new System.Windows.Forms.GroupBox();
            this.cmbSecond = new System.Windows.Forms.ComboBox();
            this.rdbPCSecond = new System.Windows.Forms.RadioButton();
            this.rdbAISecond = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.txtKifuPass = new System.Windows.Forms.TextBox();
            this.fileBrowserButton2 = new オセロ君.FileBrowserButton();
            this.grpFirst.SuspendLayout();
            this.grpSecond.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExeｃ
            // 
            this.btnExeｃ.Location = new System.Drawing.Point(12, 238);
            this.btnExeｃ.Name = "btnExeｃ";
            this.btnExeｃ.Size = new System.Drawing.Size(223, 23);
            this.btnExeｃ.TabIndex = 0;
            this.btnExeｃ.Text = "決定";
            this.btnExeｃ.UseVisualStyleBackColor = true;
            this.btnExeｃ.Click += new System.EventHandler(this.btnExec_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(241, 238);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(248, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "キャンセル";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // rdbPCFirst
            // 
            this.rdbPCFirst.AutoSize = true;
            this.rdbPCFirst.Location = new System.Drawing.Point(6, 18);
            this.rdbPCFirst.Name = "rdbPCFirst";
            this.rdbPCFirst.Size = new System.Drawing.Size(47, 16);
            this.rdbPCFirst.TabIndex = 2;
            this.rdbPCFirst.TabStop = true;
            this.rdbPCFirst.Text = "人間";
            this.rdbPCFirst.UseVisualStyleBackColor = true;
            // 
            // rdbAIFirst
            // 
            this.rdbAIFirst.AutoSize = true;
            this.rdbAIFirst.Location = new System.Drawing.Point(59, 17);
            this.rdbAIFirst.Name = "rdbAIFirst";
            this.rdbAIFirst.Size = new System.Drawing.Size(86, 16);
            this.rdbAIFirst.TabIndex = 2;
            this.rdbAIFirst.TabStop = true;
            this.rdbAIFirst.Text = "オセロ君（AI）";
            this.rdbAIFirst.UseVisualStyleBackColor = true;
            this.rdbAIFirst.CheckedChanged += new System.EventHandler(this.rdbAIFirst_CheckedChanged);
            // 
            // grpFirst
            // 
            this.grpFirst.Controls.Add(this.cmbFirst);
            this.grpFirst.Controls.Add(this.rdbPCFirst);
            this.grpFirst.Controls.Add(this.rdbAIFirst);
            this.grpFirst.Location = new System.Drawing.Point(14, 12);
            this.grpFirst.Name = "grpFirst";
            this.grpFirst.Size = new System.Drawing.Size(475, 42);
            this.grpFirst.TabIndex = 3;
            this.grpFirst.TabStop = false;
            this.grpFirst.Text = "先攻";
            // 
            // cmbFirst
            // 
            this.cmbFirst.FormattingEnabled = true;
            this.cmbFirst.Location = new System.Drawing.Point(151, 16);
            this.cmbFirst.Name = "cmbFirst";
            this.cmbFirst.Size = new System.Drawing.Size(317, 20);
            this.cmbFirst.TabIndex = 3;
            // 
            // grpSecond
            // 
            this.grpSecond.Controls.Add(this.cmbSecond);
            this.grpSecond.Controls.Add(this.rdbPCSecond);
            this.grpSecond.Controls.Add(this.rdbAISecond);
            this.grpSecond.Location = new System.Drawing.Point(14, 60);
            this.grpSecond.Name = "grpSecond";
            this.grpSecond.Size = new System.Drawing.Size(475, 42);
            this.grpSecond.TabIndex = 3;
            this.grpSecond.TabStop = false;
            this.grpSecond.Text = "後攻";
            // 
            // cmbSecond
            // 
            this.cmbSecond.FormattingEnabled = true;
            this.cmbSecond.Location = new System.Drawing.Point(151, 14);
            this.cmbSecond.Name = "cmbSecond";
            this.cmbSecond.Size = new System.Drawing.Size(317, 20);
            this.cmbSecond.TabIndex = 3;
            // 
            // rdbPCSecond
            // 
            this.rdbPCSecond.AutoSize = true;
            this.rdbPCSecond.Location = new System.Drawing.Point(6, 18);
            this.rdbPCSecond.Name = "rdbPCSecond";
            this.rdbPCSecond.Size = new System.Drawing.Size(47, 16);
            this.rdbPCSecond.TabIndex = 2;
            this.rdbPCSecond.TabStop = true;
            this.rdbPCSecond.Text = "人間";
            this.rdbPCSecond.UseVisualStyleBackColor = true;
            // 
            // rdbAISecond
            // 
            this.rdbAISecond.AutoSize = true;
            this.rdbAISecond.Location = new System.Drawing.Point(59, 18);
            this.rdbAISecond.Name = "rdbAISecond";
            this.rdbAISecond.Size = new System.Drawing.Size(86, 16);
            this.rdbAISecond.TabIndex = 2;
            this.rdbAISecond.TabStop = true;
            this.rdbAISecond.Text = "オセロ君（AI）";
            this.rdbAISecond.UseVisualStyleBackColor = true;
            this.rdbAISecond.CheckedChanged += new System.EventHandler(this.rdbAISecond_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 116);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "棋譜の出力先";
            // 
            // txtKifuPass
            // 
            this.txtKifuPass.Location = new System.Drawing.Point(20, 131);
            this.txtKifuPass.Name = "txtKifuPass";
            this.txtKifuPass.Size = new System.Drawing.Size(194, 19);
            this.txtKifuPass.TabIndex = 5;
            // 
            // fileBrowserButton2
            // 
            this.fileBrowserButton2._strDialogTitle = "モジュール選択";
            this.fileBrowserButton2._strFilter = "ログファイル(*.txt;*.log)|*.txt;*.log;";
            this.fileBrowserButton2._txtLink = this.txtKifuPass;
            this.fileBrowserButton2.Location = new System.Drawing.Point(220, 131);
            this.fileBrowserButton2.Name = "fileBrowserButton2";
            this.fileBrowserButton2.Size = new System.Drawing.Size(43, 19);
            this.fileBrowserButton2.TabIndex = 6;
            // 
            // FConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 273);
            this.Controls.Add(this.fileBrowserButton2);
            this.Controls.Add(this.txtKifuPass);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.grpSecond);
            this.Controls.Add(this.grpFirst);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnExeｃ);
            this.Name = "FConfig";
            this.Text = "設定";
            this.grpFirst.ResumeLayout(false);
            this.grpFirst.PerformLayout();
            this.grpSecond.ResumeLayout(false);
            this.grpSecond.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExeｃ;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.RadioButton rdbPCFirst;
        private System.Windows.Forms.RadioButton rdbAIFirst;
        private System.Windows.Forms.GroupBox grpFirst;
        private System.Windows.Forms.GroupBox grpSecond;
        private System.Windows.Forms.RadioButton rdbPCSecond;
        private System.Windows.Forms.RadioButton rdbAISecond;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtKifuPass;
        private FileBrowserButton fileBrowserButton2;
        private System.Windows.Forms.ComboBox cmbFirst;
        private System.Windows.Forms.ComboBox cmbSecond;
    }
}