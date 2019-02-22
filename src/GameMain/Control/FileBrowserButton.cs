using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Osero.GameMain.Control
{
    public partial class FileBrowserButton : UserControl
    {
        public FileBrowserButton()
        {
            InitializeComponent();
        }

        private System.Windows.Forms.TextBox mtxtLink;
        public System.Windows.Forms.TextBox _txtLink
        {
            set
            {
                mtxtLink = value;
            }
            get
            {
                return mtxtLink;
            }
        }
        private string mstrFilter;
        public string _strFilter
        {
            set
            {
                mstrFilter = value;
            }
            get
            {
                return mstrFilter;
            }
        }
        private string mstrDialogTitle;
        public string _strDialogTitle
        {
            set
            {
                mstrDialogTitle = value;
            }
            get
            {
                return mstrDialogTitle;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mtxtLink.Text = this.ShowFileBrowser(mstrDialogTitle, mtxtLink.Text, _strFilter);
        }
        private string ShowFileBrowser(string strTitle, string strDefaultPath, string strFilter)
        {
            string strRet = strDefaultPath;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = strFilter + "|すべてのファイル(*.*)|*.*";
            ofd.Title = strTitle;
            ofd.FileName = System.IO.Path.GetFileName(strDefaultPath);
            try
            { 
                if (System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(strDefaultPath)))
                {
                    ofd.InitialDirectory = System.IO.Path.GetDirectoryName(strDefaultPath);
                }
            }catch{}
            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                strRet = ofd.FileName;
            }
            return strRet;
        }
    }
}
