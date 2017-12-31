namespace オセロ君
{
    partial class koma
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlBtn = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnlBtn
            // 
            this.pnlBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBtn.Location = new System.Drawing.Point(0, 0);
            this.pnlBtn.Name = "pnlBtn";
            this.pnlBtn.Size = new System.Drawing.Size(88, 88);
            this.pnlBtn.TabIndex = 0;
            this.pnlBtn.Click += new System.EventHandler(this.pnlBtn_Click);
            // 
            // koma
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pnlBtn);
            this.ForeColor = System.Drawing.Color.Transparent;
            this.Name = "koma";
            this.Size = new System.Drawing.Size(88, 88);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBtn;
    }
}
