using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using Osero.Common;
using Osero.GameMain;
using Osero.Interface;
using Osero.GameMain.Control;

namespace Osero.GameMain
{
    public partial class FBoad : Form
    {
        

        private readonly System.Collections.ObjectModel.ReadOnlyCollection<string> KifuConvertIntToString = Array.AsReadOnly(new string[]{ "", "a","b","c","d","e","f","g","h"});

        private int S_mp1Mode
        {
            set
            {
                Properties.Settings.Default.S_p1Mode = value;
            }
            get
            {
                return Properties.Settings.Default.S_p1Mode;
            }
        }
        private int S_mp2Mode
        {
            set
            {
                Properties.Settings.Default.S_p2Mode = value;
            }
            get
            {
                return Properties.Settings.Default.S_p2Mode;
            }
        }
        private string S_kifuPass
        {
            set
            {
                Properties.Settings.Default.S_kifuPass = value;
            }
            get
            {
                return Properties.Settings.Default.S_kifuPass;
            }
        }
        private string S_P1AI
        {
            set
            {
                Properties.Settings.Default.S_p1AIPath = value;
            }
            get
            {
                return Properties.Settings.Default.S_p1AIPath;
            }
        }
        private string S_P2AI
        {
            set
            {
                Properties.Settings.Default.S_p2AIPath = value;
            }
            get
            {
                return Properties.Settings.Default.S_p2AIPath;
            }
        }
        private koma[,] mkomas = null;
        private int[,] mBord = null;
        private string mstrKifu;
        private common.pclsXY mclsSelectedIdx = new common.pclsXY(0,0);
        private int mintTurn;
        private bool mblnSilent = false;
        private int mplayCount = 0;
        private int _intTurn
        {
            set
            {
                mintTurn = value;
                this.BoadCnt();
            }
            get
            {
                return mintTurn;
            }
        }
        private int mintBCnt;
        private int mintWCnt;
        private bool mblnOkClick;
        private CInterface AIP1;
        private CInterface AIP2;

        public FBoad()
        {
            InitializeComponent();

            Properties.Settings.Default.Reload();

            if(S_kifuPass.Trim()=="")
            {
                S_kifuPass = Application.StartupPath + "\\_oseroKifu.log";
            }

            this.loadKoma();
            this.init();


            //コマンドライン引数を配列で取得する
            string[] cmds = System.Environment.GetCommandLineArgs();
            if (cmds.Length > 1)
            {
                //回数の引継ぎ
                this.mplayCount = int.Parse(cmds[1]);
                //AIオートの実行（終了するまでまっておく）
                System.Threading.Thread.Sleep(10000);
                StartSilentMode();
            }
            else
            {
                mplayCount = 0;
            }
        }
        private void init()
        {
            mclsSelectedIdx = new common.pclsXY(0, 0);
            _intTurn = -1;
            mintBCnt = 2;
            mintWCnt = 2;
            mstrKifu = "";
            mblnOkClick = false;
            mplayCount = 0;

            this.InitKoma();
            this.mclsSelectedIdx.setIdx(0, 0);

        }
        private void AILoading()
        {
            string strModuelDirectory = Application.StartupPath + "\\AI\\";
            if (S_mp1Mode == 1)
            {
                this.AIP1 = null;
                this.AIP1 = new CInterface(S_P1AI, Application.StartupPath, -1);
            }
            if (S_mp2Mode == 1)
            {
                this.AIP2 = null;
                this.AIP2 = new CInterface(S_P2AI, Application.StartupPath, 1);
            }
        }
        private void GameOver()
        {
            //エンド
            if (!this.mblnSilent)
            {
                if (mintBCnt > mintWCnt)
                {
                    MessageBox.Show("先攻（黒）の勝ち");
                }
                else if (mintBCnt == mintWCnt)
                {
                    MessageBox.Show("引き分け");
                }
                else
                {
                    MessageBox.Show("後攻（白）の勝ち");
                }
            }


            mstrKifu += "," + (mintBCnt - mintWCnt).ToString();
            System.IO.StreamWriter sw = new System.IO.StreamWriter(S_kifuPass, true);
            sw.WriteLine(mstrKifu);
            sw.Close();
            sw.Dispose();
            //リスタート
            if (mblnSilent)
            {
                mplayCount += 1;
                switch (mplayCount)
                {
                    case 200:
                        //再起動させる（メモリがとぶっぽい）
                        //コマンドラインに実行回数を引き渡す
                        string cmd = mplayCount.ToString();
                        //新たにアプリケーションを起動する
                        System.Diagnostics.Process.Start(Application.ExecutablePath, cmd);

                        //現在のアプリケーションを終了する
                        Application.Exit();
                        break;
                    case 1000:
                        break;
                    default:
                        System.GC.Collect();
                        startGame();
                        break;
                }
            }
        }
        private void TurnSkip()
        {
            mstrKifu += "ts";
            _intTurn *= -1;
        }

        private void GameCtrl()
        {
            this.BoadCnt();

            bool blnGameOver = false;
            blnGameOver = this.ChkBord();
            if (blnGameOver)
            {
                this.TurnSkip();
                blnGameOver = this.ChkBord();
            }
            if (blnGameOver)
            {
                this.GameOver();
                return;
            }
            else
            {
                this.PlayerCtrl();
            }

        }

        private void PlayerCtrl()
        {
            int intPlayerMode = this.GetPlayerMode();

            this.mclsSelectedIdx.setIdx(0, 0);
            this.timExecKoma.Enabled = true;
            if (intPlayerMode == 0)
            {
                //ひと
                this.GUICtrl();
            }
            else
            {
                //AI
                this.AICtrl();
            }
        }
        private void BoadCnt()
        {
            mintBCnt =0;
            mintWCnt = 0;

            if(this.mkomas != null)
            {
                 
                for(int i = 1; i < 9; i++)
                {
                    for(int j= 1; j < 9; j++)
                    {
                        mBord[i, j] = this.mkomas[i, j]._color;
                        if(this.mkomas[i,j]._color==-1)
                        {
                            mintBCnt += 1;
                            continue;
                        }
                        if (this.mkomas[i, j]._color == 1)
                        {
                            mintWCnt += 1;
                            continue;
                        }
                    }
                }
            }
            this.lblBCnt.Text = mintBCnt.ToString();
            this.lblWCnt.Text = mintWCnt.ToString();

            lblTrunB.Visible = (_intTurn == -1);
            lblTrunWhite.Visible = !(_intTurn == -1);
        }

        private int GetPlayerMode()
        {
            int intRet=0;
            if(_intTurn == -1)
            {
                intRet = S_mp1Mode;
            }
            else
            {
                intRet = S_mp2Mode;
            }
            return intRet;
        }
        private void GUICtrl()
        {
            this.mblnOkClick = true;
        }
        private void AICtrl()
        {
            common.pclsXY clsAIResult = null;
            if (mintTurn == -1)
            {
                //先攻（黒）
                clsAIResult = AIP1.OnTurn(mintTurn, mBord);
            }
            else
            {
                //後攻（白）
                clsAIResult = AIP2.OnTurn(mintTurn, mBord);
            }
            this.mclsSelectedIdx.setIdx(clsAIResult.idxX, clsAIResult.idxY);
        }
        private bool ChkBord()
        {
            bool blnNoKoma=true;
            for(int idxX = 1; idxX < 9; idxX++)
            {
                for (int idxY = 1; idxY < 9; idxY++)
                {
                    if(this.mkomas[idxX,idxY]._color==0)
                    {
                        this.mkomas[idxX, idxY].Visible = false;
                    }
                    if(ChkAround(idxX,idxY,false))
                    {
                        this.mkomas[idxX, idxY].Visible = true;
                        if(this.mkomas[idxX,idxY]._color ==0)
                        {
                            blnNoKoma = false;
                        }
                    }
                }
            }
            return blnNoKoma;
        }
        private bool ChkAround(int idxX, int idxY, bool blnOnDraw)
        {
            bool blnRet=false;

            while (true)
            {
                //左上
                if(ChkKoma(-1,-1,idxX,idxY,blnOnDraw))
                {
                    blnRet = true;
                    if (!blnOnDraw) { break; }
                }
                //上
                if (ChkKoma(0, -1, idxX, idxY, blnOnDraw))
                {
                    blnRet = true;
                    if (!blnOnDraw) { break; }
                }
                //右上
                if (ChkKoma(1, -1, idxX, idxY, blnOnDraw))
                {
                    blnRet = true;
                    if (!blnOnDraw) { break; }
                }
                //左
                if (ChkKoma(-1, 0, idxX, idxY, blnOnDraw))
                {
                    blnRet = true;
                    if (!blnOnDraw) { break; }
                }
                //右
                if (ChkKoma(1, 0, idxX, idxY, blnOnDraw))
                {
                    blnRet = true;
                    if (!blnOnDraw) { break; }
                }
                //左下
                if (ChkKoma(-1, 1, idxX, idxY, blnOnDraw))
                {
                    blnRet = true;
                    if (!blnOnDraw) { break; }
                }
                //下
                if (ChkKoma(0, 1, idxX, idxY, blnOnDraw))
                {
                    blnRet = true;
                    if (!blnOnDraw) { break; }
                }
                //右下
                if (ChkKoma(1, 1, idxX, idxY, blnOnDraw))
                {
                    blnRet = true;
                    if (!blnOnDraw) { break; }
                }
                break;
            }
            return blnRet;
        }
        private bool ChkKoma(int vctX,int vctY,int idxX, int idxY, bool blnOnDraw)
        {
            bool blnRet = false;


            int intChkIdxX = idxX + vctX;
            int intChkIdxY = idxY + vctY;
     
            if(intChkIdxX > 8 || intChkIdxX < 1 || intChkIdxY > 8 || intChkIdxY < 1)
            {
                return false;
            }
            if (!this.mkomas[intChkIdxX, intChkIdxY].Visible || this.mkomas[intChkIdxX, intChkIdxY]._color == _intTurn)
            {
                return false;
            }
            common.pclsXY drawkomasTmp = new common.pclsXY(0,0);
            List<common.pclsXY> drawKomas = new List<common.pclsXY>();
            while (intChkIdxX < 9 && intChkIdxX > 0 && intChkIdxY < 9 && intChkIdxY > 0)
            {
                if (this.mkomas[intChkIdxX, intChkIdxY]._color == 0)
                {
                    break;
                }

                drawkomasTmp.setIdx(intChkIdxX, intChkIdxY);
                drawKomas.Add(drawkomasTmp.clone());

                if (this.mkomas[intChkIdxX, intChkIdxY].Visible && this.mkomas[intChkIdxX, intChkIdxY]._color == _intTurn)
                {
                    blnRet = true;
                    break;
                }
                intChkIdxX += vctX;
                intChkIdxY += vctY;
            }
            if (blnOnDraw && blnRet)
            {
                for(int i=0; i< drawKomas.Count; i++)
                {
                    this.mkomas[drawKomas[i].idxX, drawKomas[i].idxY]._color = _intTurn;
                }
            }
            return blnRet;
        }

        private void loadKoma()
        {
            mBord = null;
            mBord = new int[9, 9];
            

            mkomas = null;
            mkomas = new koma[9, 9];

            this.mkomas[1, 1] = this.koma11;
            this.mkomas[1, 2] = this.koma12;
            this.mkomas[1, 3] = this.koma13;
            this.mkomas[1, 4] = this.koma14;
            this.mkomas[1, 5] = this.koma15;
            this.mkomas[1, 6] = this.koma16;
            this.mkomas[1, 7] = this.koma17;
            this.mkomas[1, 8] = this.koma18;
            this.mkomas[2, 1] = this.koma21;
            this.mkomas[2, 2] = this.koma22;
            this.mkomas[2, 3] = this.koma23;
            this.mkomas[2, 4] = this.koma24;
            this.mkomas[2, 5] = this.koma25;
            this.mkomas[2, 6] = this.koma26;
            this.mkomas[2, 7] = this.koma27;
            this.mkomas[2, 8] = this.koma28;
            this.mkomas[3, 1] = this.koma31;
            this.mkomas[3, 2] = this.koma32;
            this.mkomas[3, 3] = this.koma33;
            this.mkomas[3, 4] = this.koma34;
            this.mkomas[3, 5] = this.koma35;
            this.mkomas[3, 6] = this.koma36;
            this.mkomas[3, 7] = this.koma37;
            this.mkomas[3, 8] = this.koma38;
            this.mkomas[4, 1] = this.koma41;
            this.mkomas[4, 2] = this.koma42;
            this.mkomas[4, 3] = this.koma43;
            this.mkomas[4, 4] = this.koma44;
            this.mkomas[4, 5] = this.koma45;
            this.mkomas[4, 6] = this.koma46;
            this.mkomas[4, 7] = this.koma47;
            this.mkomas[4, 8] = this.koma48;
            this.mkomas[5, 1] = this.koma51;
            this.mkomas[5, 2] = this.koma52;
            this.mkomas[5, 3] = this.koma53;
            this.mkomas[5, 4] = this.koma54;
            this.mkomas[5, 5] = this.koma55;
            this.mkomas[5, 6] = this.koma56;
            this.mkomas[5, 7] = this.koma57;
            this.mkomas[5, 8] = this.koma58;
            this.mkomas[6, 1] = this.koma61;
            this.mkomas[6, 2] = this.koma62;
            this.mkomas[6, 3] = this.koma63;
            this.mkomas[6, 4] = this.koma64;
            this.mkomas[6, 5] = this.koma65;
            this.mkomas[6, 6] = this.koma66;
            this.mkomas[6, 7] = this.koma67;
            this.mkomas[6, 8] = this.koma68;
            this.mkomas[7, 1] = this.koma71;
            this.mkomas[7, 2] = this.koma72;
            this.mkomas[7, 3] = this.koma73;
            this.mkomas[7, 4] = this.koma74;
            this.mkomas[7, 5] = this.koma75;
            this.mkomas[7, 6] = this.koma76;
            this.mkomas[7, 7] = this.koma77;
            this.mkomas[7, 8] = this.koma78;
            this.mkomas[8, 1] = this.koma81;
            this.mkomas[8, 2] = this.koma82;
            this.mkomas[8, 3] = this.koma83;
            this.mkomas[8, 4] = this.koma84;
            this.mkomas[8, 5] = this.koma85;
            this.mkomas[8, 6] = this.koma86;
            this.mkomas[8, 7] = this.koma87;
            this.mkomas[8, 8] = this.koma88;

            this.koma11.Click += new EventHandler(this.koma_Click);
            this.koma12.Click += new EventHandler(this.koma_Click);
            this.koma13.Click += new EventHandler(this.koma_Click);
            this.koma14.Click += new EventHandler(this.koma_Click);
            this.koma15.Click += new EventHandler(this.koma_Click);
            this.koma16.Click += new EventHandler(this.koma_Click);
            this.koma17.Click += new EventHandler(this.koma_Click);
            this.koma18.Click += new EventHandler(this.koma_Click);
            this.koma21.Click += new EventHandler(this.koma_Click);
            this.koma22.Click += new EventHandler(this.koma_Click);
            this.koma23.Click += new EventHandler(this.koma_Click);
            this.koma24.Click += new EventHandler(this.koma_Click);
            this.koma25.Click += new EventHandler(this.koma_Click);
            this.koma26.Click += new EventHandler(this.koma_Click);
            this.koma27.Click += new EventHandler(this.koma_Click);
            this.koma28.Click += new EventHandler(this.koma_Click);
            this.koma31.Click += new EventHandler(this.koma_Click);
            this.koma32.Click += new EventHandler(this.koma_Click);
            this.koma33.Click += new EventHandler(this.koma_Click);
            this.koma34.Click += new EventHandler(this.koma_Click);
            this.koma35.Click += new EventHandler(this.koma_Click);
            this.koma36.Click += new EventHandler(this.koma_Click);
            this.koma37.Click += new EventHandler(this.koma_Click);
            this.koma38.Click += new EventHandler(this.koma_Click);
            this.koma41.Click += new EventHandler(this.koma_Click);
            this.koma42.Click += new EventHandler(this.koma_Click);
            this.koma43.Click += new EventHandler(this.koma_Click);
            this.koma44.Click += new EventHandler(this.koma_Click);
            this.koma45.Click += new EventHandler(this.koma_Click);
            this.koma46.Click += new EventHandler(this.koma_Click);
            this.koma47.Click += new EventHandler(this.koma_Click);
            this.koma48.Click += new EventHandler(this.koma_Click);
            this.koma51.Click += new EventHandler(this.koma_Click);
            this.koma52.Click += new EventHandler(this.koma_Click);
            this.koma53.Click += new EventHandler(this.koma_Click);
            this.koma54.Click += new EventHandler(this.koma_Click);
            this.koma55.Click += new EventHandler(this.koma_Click);
            this.koma56.Click += new EventHandler(this.koma_Click);
            this.koma57.Click += new EventHandler(this.koma_Click);
            this.koma58.Click += new EventHandler(this.koma_Click);
            this.koma61.Click += new EventHandler(this.koma_Click);
            this.koma62.Click += new EventHandler(this.koma_Click);
            this.koma63.Click += new EventHandler(this.koma_Click);
            this.koma64.Click += new EventHandler(this.koma_Click);
            this.koma65.Click += new EventHandler(this.koma_Click);
            this.koma66.Click += new EventHandler(this.koma_Click);
            this.koma67.Click += new EventHandler(this.koma_Click);
            this.koma68.Click += new EventHandler(this.koma_Click);
            this.koma71.Click += new EventHandler(this.koma_Click);
            this.koma72.Click += new EventHandler(this.koma_Click);
            this.koma73.Click += new EventHandler(this.koma_Click);
            this.koma74.Click += new EventHandler(this.koma_Click);
            this.koma75.Click += new EventHandler(this.koma_Click);
            this.koma76.Click += new EventHandler(this.koma_Click);
            this.koma77.Click += new EventHandler(this.koma_Click);
            this.koma78.Click += new EventHandler(this.koma_Click);
            this.koma81.Click += new EventHandler(this.koma_Click);
            this.koma82.Click += new EventHandler(this.koma_Click);
            this.koma83.Click += new EventHandler(this.koma_Click);
            this.koma84.Click += new EventHandler(this.koma_Click);
            this.koma85.Click += new EventHandler(this.koma_Click);
            this.koma86.Click += new EventHandler(this.koma_Click);
            this.koma87.Click += new EventHandler(this.koma_Click);
            this.koma88.Click += new EventHandler(this.koma_Click);
        }

        private void InitKoma()
        {
            for(int i = 1; i < 9; i++)
            {
                for(int j=1;j<9;j++)
                {
                    this.mkomas[i, j].Visible = false;
                    this.mkomas[i, j]._color = 0;
                }
            }

            this.mkomas[4, 4].Visible = true;
            this.mkomas[4, 4]._color = 1;
            this.mkomas[4, 5].Visible = true;
            this.mkomas[4, 5]._color = -1;
            this.mkomas[5, 4].Visible = true;
            this.mkomas[5, 4]._color = -1;
            this.mkomas[5, 5].Visible = true;
            this.mkomas[5, 5]._color = 1;

            for (int i = 1; i < 9; i++)
            {
                for (int j = 1; j < 9; j++)
                {
                    this.mBord[i,j]= this.mkomas[i, j]._color;
                }
            }
        }

        private void koma_Click(object sender, EventArgs e)
        {
            if (!mblnOkClick)
            {
                return;
            }

            koma objKoma = (koma)(sender);
            if (!objKoma.Visible) { return;}
            if (objKoma._color!=0) { return;}
            this.mclsSelectedIdx.setIdx(int.Parse(objKoma.Name.Substring(4, 1)), int.Parse(objKoma.Name.Substring(5, 1)));
        }
        private void timExecKoma_Tick(object sender, EventArgs e)
        {
            this.timExecKoma.Enabled = false;
            if (mclsSelectedIdx.idxX == 0)
            {
                this.timExecKoma.Enabled = true;
                return;
            }
            this.DrawKoma();
        }
        private void DrawKoma()
        {
            if(mclsSelectedIdx.idxX == -1)
            {
                if (!this.mblnSilent) { MessageBox.Show("AIの故障のため、ターンをスキップします。"); }
                MessageBox.Show("AIの故障のため、ターンをスキップします。");
                this.TurnSkip();
            }
            else if(this.mkomas[mclsSelectedIdx.idxX, mclsSelectedIdx.idxY]._color!=0 || !this.mkomas[mclsSelectedIdx.idxX, mclsSelectedIdx.idxY].Visible)
            {
                if (!this.mblnSilent) { MessageBox.Show("AIの故障のため、ターンをスキップします。"); }
                MessageBox.Show("AIの故障のため、ターンをスキップします。");
                this.TurnSkip();
            }
            else
            {
                this.mstrKifu += this.KifuConvertIntToString[mclsSelectedIdx.idxX].ToString() + mclsSelectedIdx.idxY.ToString();

                this.mkomas[mclsSelectedIdx.idxX, mclsSelectedIdx.idxY]._color = _intTurn;
                this.ChkAround(mclsSelectedIdx.idxX, mclsSelectedIdx.idxY, true);
                this._intTurn *= -1;
            }
            this.GameCtrl();
        }

        private void startGame()
        {
            this.init();
            this.AILoading();
            this.GameCtrl();
        }
        #region メニューバー
        private void 開始SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("新規ゲームを開始します。", "",  MessageBoxButtons.OKCancel)==System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            mblnSilent = false;
            startGame();
        }

        FConfig mFrmConfig = new FConfig();
        private void 設定SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mFrmConfig._P1Mode = S_mp1Mode;
            mFrmConfig._P2Mode = S_mp2Mode;
            mFrmConfig._KifuPass = S_kifuPass;
            mFrmConfig.Display(this);

            if(mFrmConfig.mFormResult==System.Windows.Forms.DialogResult.OK)
            {
                S_mp1Mode = mFrmConfig._P1Mode;
                S_mp2Mode = mFrmConfig._P2Mode;
                S_kifuPass = mFrmConfig._KifuPass;
                S_P1AI = mFrmConfig._P1AI;
                S_P2AI = mFrmConfig._P2AI;
            }

            Properties.Settings.Default.Save();
        }
        private void 戦ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.StartSilentMode();
        }
        private void StartSilentMode()
        {
            this.mblnSilent = true;
            startGame();
        }
        #endregion


    }
}
