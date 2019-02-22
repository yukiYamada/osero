using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Osero.GameMain
{
    public partial class FConfig : Form
    {
        private int mP1Mode;
        public int _P1Mode
        {
            set
            {
                mP1Mode = value;
            }
            get
            {
                return mP1Mode;
            }
        }

        private int mP2Mode;
        public int _P2Mode
        {
            set
            {
                mP2Mode = value;
            }
            get
            {
                return mP2Mode;
            }
        }

        private string mstrKifuPass;
        public string _KifuPass
        {
            set
            {
                mstrKifuPass = value;
            }
            get
            {
                return mstrKifuPass;
            }
        }
        private string mstrP1AI;
        public string _P1AI
        {
            get
            {
                return mstrP1AI;
            }
        }
        private string mstrP2AI;
        public string _P2AI
        {
            get
            {
                return mstrP2AI;
            }
        }
        public System.Windows.Forms.DialogResult mFormResult;
        public System.Windows.Forms.DialogResult _FormResult
        {
            get
            {
                return mFormResult;
            }
        }

        public FConfig()
        {
            InitializeComponent();
            this.SetAIFiles();
        }
        public void Display(IWin32Window owner)
        {
            if (mP1Mode == 0)
            {
                this.rdbPCFirst.Checked = true;
            }
            else
            {
                this.rdbAIFirst.Checked = true;
            }
            if(mP2Mode==0)
            {
                this.rdbPCSecond.Checked = true;
            }
            else
            {
                this.rdbAISecond.Checked = true;
            }
            this.txtKifuPass.Text = mstrKifuPass;

            this.ShowDialog(owner);
        }
        private void btnExec_Click(object sender, EventArgs e)
        {
            if (!ChkInputValue())
            {
                return;
            }
            this.mFormResult = System.Windows.Forms.DialogResult.OK;
            if (this.rdbPCFirst.Checked)
            {
                mP1Mode = 0;
            }
            else
            {
                mP1Mode = 1;
            }
            if (this.rdbPCSecond.Checked)
            {
                mP2Mode = 0;
            }
            else
            {
                mP2Mode = 1;
            }

            mstrKifuPass = this.txtKifuPass.Text;
            mstrP1AI = this.cmbFirst.Text;
            mstrP2AI = this.cmbSecond.Text;

            this.Close();
        }
        private void SetAIFiles()
        {
            string strAIDirectory = Application.StartupPath + "\\AI\\";

            this.cmbFirst.Items.Clear();
            this.cmbSecond.Items.Clear();

            if (!System.IO.Directory.Exists(strAIDirectory))
            {
                return;
            }
            foreach (string strFileName in System.IO.Directory.GetFiles(strAIDirectory,"*.ini"))
            {
                this.cmbFirst.Items.Add(System.IO.Path.GetFileName(strFileName));
                this.cmbSecond.Items.Add(System.IO.Path.GetFileName(strFileName));
            }
        }
        private bool ChkInputValue()
        {
            try
            {
                if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(this.txtKifuPass.Text)))
                {
                    MessageBox.Show("でぃれくとり不正");
                    return false;
                }
            }catch
            {
                MessageBox.Show("なんかだめ");
                return false;
            }

            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.mFormResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void rdbAIFirst_CheckedChanged(object sender, EventArgs e)
        {
             this.cmbFirst.Enabled = this.rdbAIFirst.Checked;
        }

        private void rdbAISecond_CheckedChanged(object sender, EventArgs e)
        {
            this.cmbSecond.Enabled = this.rdbAISecond.Checked;
        }
    }
}
