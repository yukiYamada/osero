using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using 共通君;

namespace SearchRoot
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
            return int.Parse(KifuConvertWordToNumber[strKifuMove.Substring(0, 1)]);
        }
        internal int GetYIndex(string strKifuMove)
        {
            return int.Parse(strKifuMove.Substring(1, 1));
        }
    }
    public class SearchRoot
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
        public SearchRoot()
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
            string strBoadHash = GetBordStatusKey(intTurn, intGameBord);

            string resultX = "";
            string resultY = "";
            List<string> loseRoot = new List<string>();
            if (mBordMapping.ContainsKey(strBoadHash))
            {
                //マッピングの中から抽出する
                Dictionary<string, clsGameResult> mappingItem = mBordMapping[strBoadHash];

                foreach (string strKeys in mappingItem.Keys)
                {
                    clsGameResult result = mappingItem[strKeys];
                    //既存ルート（ほかにルートのないもののみ既存とする。）
                    if (result.blnIsReaed)
                    {
                        loseRoot.Add(result.intX.ToString() + result.intY.ToString());
                    }
                    continue;
                }

            }
            bool blnIsAllLose = true;
            //既存ルート以外を選ぶ
            for (int i = 1; i < 9; i++)
            {
                for (int j = 1; j < 9; j++)
                {
                    if (intGameBord[i, j] == enmBordStatus.select)
                    {
                        if (loseRoot.Contains(i.ToString() + j.ToString()))
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
                if (!blnIsAllLose){break;}
            }
            if (blnIsAllLose)
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(mstrLoadPath + "\\" + mcstrErrLogPath);
                sw.WriteLine("ルート走破");
                sw.Close();
                sw.Dispose();
            }
            bool blnCmp = false;
            while (!blnCmp)
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
                while (!sr.EndOfStream)
                {
                    try
                    {
                        string strKifuLine = sr.ReadLine();
                        if (string.IsNullOrWhiteSpace(strKifuLine))
                        {
                            continue;
                        }

                        enmBordStatus[,] intGameBord = new enmBordStatus[9, 9];
                        for (int i = 1; i < 9; i++)
                        {
                            for (int j = 1; j < 9; j++)
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

                        string strKifuMove = "";
                        ChkBord(ref intGameBord, intTurn);
                        while (true)
                        {
                            if (string.IsNullOrWhiteSpace(strKifuLine)) { break; }

                            strKifuMove = strKifuLine.Substring(0, 2);
                            strKifuLine = strKifuLine.Substring(2, strKifuLine.Length - 2);
                            if (mcstrTurnSkip == strKifuMove)
                            {
                                //ターン変更
                                intTurn *= -1;
                                continue;
                            }
                            //現在のボード状態からキーを作成する
                            string strBoadHash = GetBordStatusKey(intTurn, intGameBord);

                            Dictionary<string, clsGameResult> BordMappingItem = new Dictionary<string, clsGameResult>();
                            if (!mBordMapping.ContainsKey(strBoadHash))
                            {
                                BordMappingItem = new Dictionary<string, clsGameResult>();
                                mBordMapping.Add(strBoadHash, new Dictionary<string, clsGameResult>(BordMappingItem));
                            }
                            BordMappingItem = mBordMapping[strBoadHash];


                            //すべての選択を突っ込んでおく
                            for (int i = 1; i < 9; i++)
                            {
                                for (int j = 1; j < 9; j++)
                                {
                                    if (intGameBord[i, j] == enmBordStatus.select)
                                    {
                                        string strMove = clsKifuCnv.ConvertNumberToWord(i) + j.ToString();

                                        if (!BordMappingItem.ContainsKey(strMove))
                                        {
                                            clsGameResult ResultTemp = new clsGameResult();
                                            BordMappingItem.Add(strMove, ResultTemp.clone());
                                        }
                                    }
                                }
                            }
                            clsGameResult BordMappingItemItem = BordMappingItem[strKifuMove];
                            BordMappingItemItem.blnIsReaed = true;
                            int intKifuIdxX = clsKifuCnv.GetXIndex(strKifuMove);
                            int intKifuIdxY = clsKifuCnv.GetYIndex(strKifuMove);
                            BordMappingItemItem.intX = intKifuIdxX;
                            BordMappingItemItem.intY = intKifuIdxY;

                            BordMappingItem[strKifuMove] = BordMappingItemItem.clone();
                            mBordMapping[strBoadHash] = new Dictionary<string, clsGameResult>(BordMappingItem);

                            //石を置く
                            intGameBord[intKifuIdxX, intKifuIdxY] = (enmBordStatus)intTurn;
                            ChkAround(intKifuIdxX, intKifuIdxY, true, ref intGameBord, intTurn);

                            //ターン制御
                            intTurn *= -1;

                            //ボードの更新
                            ChkBord(ref intGameBord, intTurn);

                        }
                    }
                    catch (Exception ex)
                    {
                        System.IO.StreamWriter sw = new System.IO.StreamWriter(mstrLoadPath + "\\" + mcstrErrLogPath);
                        sw.WriteLine(ex.ToString());
                        sw.Close();
                        sw.Dispose();
                    }
                }
                sr.Close();
            }
            foreach(string strBordKey in mBordMapping.Keys)
            {
                bool blnIsOtherRoot = false;
                foreach(string strItemKey in mBordMapping[strBordKey].Keys)
                {
                    clsGameResult result = mBordMapping[strBordKey][strItemKey];
                    if (result.blnIsReaed)
                    {
                        blnIsOtherRoot = true;
                        break;
                    }
                }
                //全部埋まってる場合
                if(!blnIsOtherRoot)
                {

                }
            }
        }

        private string GetBordStatusKey(int intTurn, enmBordStatus[,] intBord)
        {
            string retString = intTurn.ToString();
            for (int i = 1; i < 9; i++)
            {
                for (int j = 1; j < 9; j++)
                {
                    retString += clsKifuCnv.ConvertNumberToWord(i);
                    retString += j.ToString();

                    switch (intBord[i, j])
                    {
                        case enmBordStatus.none:
                        case enmBordStatus.select:
                            retString += "0";
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
        private void ChkBord(ref enmBordStatus[,] intBord, int intTurn)
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

        private bool ChkAround(int idxX, int idxY, bool blnOnDraw, ref enmBordStatus[,] intBord, int intTurn)
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
        private bool ChkKoma(int vctX, int vctY, int idxX, int idxY, bool blnOnDraw, ref enmBordStatus[,] intBord, int intTurn)
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
        public bool blnIsReaed;
        public bool blnIsOtherRoot;
        public int intX;
        public int intY;
        public clsGameResult()
        {
            blnIsReaed = false;
            blnIsOtherRoot = true;
            intX = -1;
            intY = -1;
        }

        public clsGameResult clone()
        {
            clsGameResult ret = new clsGameResult();
            ret.blnIsReaed = this.blnIsReaed;
            ret.blnIsOtherRoot = this.blnIsOtherRoot;
            ret.intX = this.intX;
            ret.intY = this.intY;

            return ret;
        }
    }
}
