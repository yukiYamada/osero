using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using 共通君;

namespace kusozakonamekuji君
{
    internal class KifuConverter
    {
        private readonly System.Collections.ObjectModel.ReadOnlyCollection<string> KifuConvertNumberToWord = Array.AsReadOnly(new string[] { "", "a", "b", "c", "d", "e", "f", "g", "h" });
        Dictionary<string, string> KifuConvertWordToNumber = new Dictionary<string, string>();

        public KifuConverter()
        {
            this.KifuConvertWordToNumber.Add("a", "1");
            this.KifuConvertWordToNumber.Add("b", "2");
            this.KifuConvertWordToNumber.Add("c", "3");
            this.KifuConvertWordToNumber.Add("d", "4");
            this.KifuConvertWordToNumber.Add("e", "5");
            this.KifuConvertWordToNumber.Add("f", "6");
            this.KifuConvertWordToNumber.Add("g", "7");
            this.KifuConvertWordToNumber.Add("h", "8");
        }
        internal string ConvertNumberToWord(int index)
        {
            return this.KifuConvertNumberToWord[index];
        }
        internal string ConvertWordToNumber(string index)
        {
            return KifuConvertWordToNumber[index];
        }
        internal int GetXIndex(string strKifuMove)
        {
            return int.Parse(KifuConvertWordToNumber[strKifuMove.Substring(0,1)]);
        }
        internal int GetYIndex(string strKifuMove)
        {
            return int.Parse(strKifuMove.Substring(1, 1));
        }
    }
    public class kusozakonamekuji
    {
        private enum enmBordStatus
        {
            none = -2,
            black = -1,
            select = 0,
            white = 1
        }

        private string mstrLoadPath = "";
        private const string mcstrErrLogPath = "_ErrLog.log";
        private int mintMyTurn = 0;
        private const string mcstrKifuName = "_oseroKifu.log";
        private const string mcstrTurnSkip = "ts";
        internal const string mcstrResultPath = "AIResult.dat";
        private KifuConverter clsKifuCnv = new KifuConverter();

        private Dictionary<string, Dictionary<string, clsGameResult>> mBordMapping = new Dictionary<string, Dictionary<string, clsGameResult>>();
        public kusozakonamekuji()
        {
        }

        public void OnLoad(string strLoadPath, int intMyTurn)
        {
            mintMyTurn = intMyTurn;
            mstrLoadPath = strLoadPath;
            this.LoadKifu();    
        }

        public void OnTurn(int intTurn, int[,] intBord)
        {
            enmBordStatus[,] intGameBord = convertEnmBord(intBord);
            this.ChkBord(ref intGameBord, intTurn);

            //現在のボード状態からキーを作成する
            string strBoadHash = GetBordStatusKey(intTurn,intGameBord);

            string resultX = "";
            string resultY = "";
            Dictionary<string, long> loseRoot = new Dictionary<string, long>();
            if (mBordMapping.ContainsKey(strBoadHash))
            {
                //マッピングの中から抽出する
                Dictionary<string, clsGameResult> mappingItem = mBordMapping[strBoadHash];

                long lngMaxBWinCnt = 0;
                long lngMaxWWinCnt = 0;
                if (mintMyTurn == -1)
                {
                    lngMaxBWinCnt = long.MinValue;
                    lngMaxWWinCnt = long.MaxValue;
                }
                else
                {
                    lngMaxBWinCnt = long.MaxValue;
                    lngMaxWWinCnt = long.MinValue;
                }
                foreach (string strKeys in mappingItem.Keys)
                {
                    clsGameResult result = mappingItem[strKeys];
                    if (mintMyTurn == -1)
                    {
                        //先攻の場合
                        if (result.BlackWin <= 0 && result.WhiteWin > 0)
                        {
                            //負けルート
                            loseRoot.Add(result.intX.ToString() + result.intY.ToString(), result.WhiteWin);
                            continue;
                        }
                        if (result.WhiteWin == 0)
                        {
                            //勝ちしかない
                            if (lngMaxWWinCnt == 0)
                            {
                                if (lngMaxBWinCnt < result.BlackWin)
                                {
                                    lngMaxBWinCnt = result.BlackWin;
                                    lngMaxWWinCnt = result.WhiteWin;
                                    resultX = result.intX.ToString();
                                    resultY = result.intY.ToString();
                                }
                            }
                            else
                            {
                                lngMaxBWinCnt = result.BlackWin;
                                lngMaxWWinCnt = result.WhiteWin;
                                resultX = result.intX.ToString();
                                resultY = result.intY.ToString();
                            }
                        }
                        else if (lngMaxWWinCnt > result.WhiteWin)
                        {
                            if (lngMaxBWinCnt < result.BlackWin)
                            {
                                lngMaxBWinCnt = result.BlackWin;
                                lngMaxWWinCnt = result.WhiteWin;
                                resultX = result.intX.ToString();
                                resultY = result.intY.ToString();
                            }
                        }
                    }
                    else
                    {
                        //後攻の場合
                        if (result.BlackWin > 0 && result.WhiteWin <= 0)
                        {
                            //負けルート
                            loseRoot.Add(result.intX.ToString() + result.intY.ToString(), result.BlackWin);
                            continue;
                        }
                        if (result.BlackWin == 0)
                        {
                            //勝ちしかない
                            if (lngMaxBWinCnt == 0)
                            {
                                if (lngMaxWWinCnt < result.WhiteWin)
                                {
                                    lngMaxBWinCnt = result.BlackWin;
                                    lngMaxWWinCnt = result.WhiteWin;
                                    resultX = result.intX.ToString();
                                    resultY = result.intY.ToString();
                                }
                            }
                            else
                            {
                                lngMaxBWinCnt = result.BlackWin;
                                lngMaxWWinCnt = result.WhiteWin;
                                resultX = result.intX.ToString();
                                resultY = result.intY.ToString();
                            }
                        }
                        else if (lngMaxBWinCnt > result.BlackWin)
                        {
                            if (lngMaxWWinCnt < result.WhiteWin)
                            {
                                lngMaxBWinCnt = result.BlackWin;
                                lngMaxWWinCnt = result.WhiteWin;
                                resultX = result.intX.ToString();
                                resultY = result.intY.ToString();
                            }
                        }
                    }
                }


            }
            bool blnIsAllLose = true;
            if (resultX == "")
            { 
                //マッピングできない場合は負けルート以外を選ぶ
                for (int i = 1; i < 9; i++)
                {
                    for (int j = 1; j < 9; j++)
                    {
                        if(intGameBord[i, j]==enmBordStatus.select)
                        {
                            if(loseRoot.ContainsKey(i.ToString()+ j.ToString()))
                            {
                                resultX = i.ToString();
                                resultY = j.ToString();
                                continue;
                            }
                            resultX = i.ToString();
                            resultY = j.ToString();
                            blnIsAllLose = false;
                            break;
                        }
                    }
                    if (!blnIsAllLose)
                    {
                        break;
                    }
                }
            }
            if(blnIsAllLose)
            {
                //負けしかない場合は負けの少ないルートを選択する
                long longCurrentLoseCount = long.MaxValue;
                foreach(string loseRootKey in loseRoot.Keys)
                {
                    if(longCurrentLoseCount > loseRoot[loseRootKey])
                    {
                        longCurrentLoseCount = loseRoot[loseRootKey];
                        resultX = loseRootKey.Substring(0, 1);
                        resultY = loseRootKey.Substring(1, 1);
                    }
                }

            }
            bool blnCmp = false;
            while(!blnCmp)
            {
                try
                {
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(mstrLoadPath + "\\" + mcstrResultPath, false);
                    sw.WriteLine(resultX + resultY);
                    sw.Close();
                    sw.Dispose();
                    blnCmp = true;
                }
                catch
                {
                    blnCmp = false;
                }
            }

        }
        private enmBordStatus[,] convertEnmBord(int[,] intBord)
        {
            enmBordStatus[,] ret = new enmBordStatus[9, 9];
            for (int i = 1; i < 9; i++)
            {
                for (int j = 1; j < 9; j++)
                {
                    ret[i, j] = (enmBordStatus)intBord[i, j];
                }
            }
            return ret;
        }
        private void LoadKifu()
        {

            Dictionary<string, object> ditReadedRoot = new Dictionary<string, object>();
            foreach (string strFilePath in System.IO.Directory.GetFiles(mstrLoadPath, "*" + mcstrKifuName))
            {              
                System.IO.StreamReader sr = new System.IO.StreamReader(strFilePath);
                while(!sr.EndOfStream)
                {
                    try
                    {
                        string strKifuLine = sr.ReadLine();
                        if( string.IsNullOrWhiteSpace(strKifuLine))
                        {
                            continue;
                        }

                        enmBordStatus[,] intGameBord = new enmBordStatus[9, 9];
                        for (int i =1;i<9;i++)
                        {
                            for(int j =1;j<9;j++)
                            {
                                intGameBord[i, j] = enmBordStatus.none;
                            }
                        }
                        intGameBord[4, 4] = enmBordStatus.white;
                        intGameBord[5, 4] = enmBordStatus.black;
                        intGameBord[4, 5] = enmBordStatus.black;
                        intGameBord[5, 5] = enmBordStatus.white;
                        int intTurn = -1;
                        string[] strKifuSplit = strKifuLine.Split(',');
                        bool bwin = true;
                        bool draw = false;
                        strKifuLine = strKifuSplit[0];
                        if (ditReadedRoot.ContainsKey(strKifuLine))
                        {
                            //読み込み済み    
                            continue;
                        }
                        else
                        {
                            ditReadedRoot.Add(strKifuLine, null);
                        }

                        switch (strKifuSplit[1].Substring(0, 1))
                        {
                            case "-":
                                bwin = false;
                                break;
                            case "0":
                                draw = true;
                                break;  
                        }

                        string strKifuMove = "";
                        while (true)
                        {

                            if (string.IsNullOrWhiteSpace(strKifuLine)) { break; }

                            //ボードの状態を更新する
                            ChkBord(ref intGameBord, intTurn);

                            strKifuMove = strKifuLine.Substring(0, 2);
                            strKifuLine = strKifuLine.Substring(2, strKifuLine.Length - 2);
                            if(mcstrTurnSkip==strKifuMove)
                            {
                                //ターン変更
                                intTurn *= -1;
                                continue;
                            }
                            //現在のボード状態からキーを作成する
                            string strBoadHash = GetBordStatusKey(intTurn,intGameBord);

                            Dictionary<string, clsGameResult> BordMappingItem = new Dictionary<string, clsGameResult>();
                            if (!mBordMapping.ContainsKey(strBoadHash))
                            {
                                BordMappingItem = new Dictionary<string, clsGameResult>();
                                mBordMapping.Add(strBoadHash, new Dictionary<string, clsGameResult>(BordMappingItem));
                            }
                            BordMappingItem = mBordMapping[strBoadHash];

                            clsGameResult BordMappingItemItem = new clsGameResult();
                            if (!BordMappingItem.ContainsKey(strKifuMove))
                            {
                                BordMappingItem.Add(strKifuMove, BordMappingItemItem.clone());
                            }
                            BordMappingItemItem = BordMappingItem[strKifuMove];

                            int intKifuIdxX = clsKifuCnv.GetXIndex(strKifuMove);
                            int intKifuIdxY = clsKifuCnv.GetYIndex(strKifuMove);
                            BordMappingItemItem.intX = intKifuIdxX;
                            BordMappingItemItem.intY = intKifuIdxY;
                            if (!draw)
                            {
                                if (bwin)
                                {
                                    BordMappingItemItem.BlackWin += 1;
                                }
                                else
                                {
                                    BordMappingItemItem.WhiteWin += 1;
                                }
                            } 

                            BordMappingItem[strKifuMove] = BordMappingItemItem.clone();
                            mBordMapping[strBoadHash] = new Dictionary<string, clsGameResult>(BordMappingItem);

                            //石を置く
                            intGameBord[intKifuIdxX, intKifuIdxY] = (enmBordStatus)intTurn;
                            ChkAround(intKifuIdxX, intKifuIdxY, true, ref intGameBord, intTurn);

                            //ターン制御
                            intTurn *= -1;
                        }
                    }
                    catch(Exception ex) 
                    {
                        System.IO.StreamWriter sw = new System.IO.StreamWriter(mstrLoadPath + "\\" + mcstrErrLogPath);
                        sw.WriteLine(ex.ToString());
                        sw.Close();
                        sw.Dispose();
                    }
                }
                sr.Close();
            }
        }

        private string GetBordStatusKey(int intTurn,enmBordStatus[,] intBord)
        {
            string retString = intTurn.ToString();
            for(int i=1;i<9;i++)
            {
                for(int j=1;j<9;j++)
                {
                    retString += clsKifuCnv.ConvertNumberToWord(i);
                    retString += j.ToString();

                    switch (intBord[i, j])
                    {
                        case enmBordStatus.none:
                        case enmBordStatus.select:
                            retString　+= "0";
                            break;
                        case enmBordStatus.black:
                            retString += "-1";
                            break;
                        case enmBordStatus.white:
                            retString += "1";
                            break;
                    }

                }
            }
            return retString;
        }
        
        //コマのステータス
        private void ChkBord(ref enmBordStatus[,] intBord,int intTurn)
        {
            for (int idxX = 1; idxX < 9; idxX++)
            {
                for (int idxY = 1; idxY < 9; idxY++)
                {
                    if (intBord[idxX, idxY] == enmBordStatus.select)
                    {
                        intBord[idxX, idxY] = enmBordStatus.none;
                    }
                    if (intBord[idxX, idxY] != enmBordStatus.none)
                    {
                        continue;
                    }
                    if (ChkAround(idxX, idxY, false, ref intBord, intTurn))
                    {
                        intBord[idxX, idxY] = enmBordStatus.select;
                    }
                }
            }
        }
        
        private bool ChkAround(int idxX, int idxY, bool blnOnDraw,ref enmBordStatus[,] intBord,int intTurn)
        {
            bool blnRet = false;

            while (true)
            {
                //左上
                if (ChkKoma(-1, -1, idxX, idxY, blnOnDraw, ref intBord, intTurn))
                {
                    blnRet = true;
                    if (!blnOnDraw) { break; }
                }
                //上
                if (ChkKoma(0, -1, idxX, idxY, blnOnDraw, ref intBord, intTurn))
                {
                    blnRet = true;
                    if (!blnOnDraw) { break; }
                }
                //右上
                if (ChkKoma(1, -1, idxX, idxY, blnOnDraw, ref intBord, intTurn))
                {
                    blnRet = true;
                    if (!blnOnDraw) { break; }
                }
                //左
                if (ChkKoma(-1, 0, idxX, idxY, blnOnDraw, ref intBord, intTurn))
                {
                    blnRet = true;
                    if (!blnOnDraw) { break; }
                }
                //右
                if (ChkKoma(1, 0, idxX, idxY, blnOnDraw, ref intBord, intTurn))
                {
                    blnRet = true;
                    if (!blnOnDraw) { break; }
                }
                //左下
                if (ChkKoma(-1, 1, idxX, idxY, blnOnDraw, ref intBord, intTurn))
                {
                    blnRet = true;
                    if (!blnOnDraw) { break; }
                }
                //下
                if (ChkKoma(0, 1, idxX, idxY, blnOnDraw, ref intBord, intTurn))
                {
                    blnRet = true;
                    if (!blnOnDraw) { break; }
                }
                //右下
                if (ChkKoma(1, 1, idxX, idxY, blnOnDraw, ref intBord, intTurn))
                {
                    blnRet = true;
                    if (!blnOnDraw) { break; }
                }
                break;
            }
            return blnRet;
        }
        private bool ChkKoma(int vctX, int vctY, int idxX, int idxY, bool blnOnDraw,ref enmBordStatus[,] intBord, int intTurn)
        {
            bool blnRet = false;


            int intChkIdxX = idxX + vctX;
            int intChkIdxY = idxY + vctY;

            if (intChkIdxX > 8 || intChkIdxX < 1 || intChkIdxY > 8 || intChkIdxY < 1)
            {
                return false;
            }
            if (intBord[intChkIdxX, intChkIdxY] == (enmBordStatus)intTurn)
            {
                return false;
            }
            common.pclsXY drawkomasTmp = new common.pclsXY(0, 0);
            List<common.pclsXY> drawKomas = new List<common.pclsXY>();
            while (intChkIdxX < 9 && intChkIdxX > 0 && intChkIdxY < 9 && intChkIdxY > 0)
            {
                if (intBord[intChkIdxX, intChkIdxY] == enmBordStatus.select || intBord[intChkIdxX, intChkIdxY] == enmBordStatus.none)
                {
                    break;
                }

                drawkomasTmp.setIdx(intChkIdxX, intChkIdxY);
                drawKomas.Add(drawkomasTmp.clone());

                if (intBord[intChkIdxX, intChkIdxY] == (enmBordStatus)intTurn)
                {
                    blnRet = true;
                    break;
                }
                intChkIdxX += vctX;
                intChkIdxY += vctY;
            }
            if (blnOnDraw && blnRet)
            {
                for (int i = 0; i < drawKomas.Count; i++)
                {
                    intBord[drawKomas[i].idxX, drawKomas[i].idxY] = (enmBordStatus)intTurn;
                }
            }
            return blnRet;
        }
    }

    internal class clsGameResult
    {
        public long BlackWin;
        public long WhiteWin;
        public int intX;
        public int intY;
        public clsGameResult()
        {
            this.BlackWin = 0;
            this.WhiteWin = 0;
            intX = -1;
            intY = -1;
        }

        public clsGameResult clone()
        {
            clsGameResult ret = new clsGameResult();
            ret.BlackWin = this.BlackWin;
            ret.WhiteWin = this.WhiteWin;
            ret.intX = this.intX;
            ret.intY = this.intY;

            return ret;
        }
    }
}
