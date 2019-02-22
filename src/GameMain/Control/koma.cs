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
    public partial class koma : UserControl
    {

        private int mcolor;
        public int _color
            {
            set
            {
                mcolor = value;
                this.ChangeColor();
            }
            get
            {
                return mcolor;
            }
        }

        public koma()
        {
            InitializeComponent();

            mcolor = 0;
            this.ChangeColor();
        }

        private void ChangeColor()
        {
            if (mcolor == 1)
            {
                this.pnlBtn.BackColor = System.Drawing.Color.White;
            }
            else if(mcolor==-1)
            {
                this.pnlBtn.BackColor = System.Drawing.Color.Black;
            }
            else
            {
                this.pnlBtn.BackColor = System.Drawing.Color.Gray;
            }
        }

        private void pnlBtn_Click(object sender, EventArgs e)
        {
            this.OnClick(e);
        }
    }
}
