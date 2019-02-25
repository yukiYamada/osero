using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Osero.Common;

namespace Osero.Interface
{
    internal class CInterfaceForDotnet : CAIInterfaceBase, IAIInterface
    {
        internal object mobjAI;
        internal Type mobjType;
        public object[] mobjParams;

        public CInterfaceForDotnet(string strStartingLoadPath, string strModulePath) : base(strStartingLoadPath, strModulePath)
        {
            // 処理なし
        }

        public void OnLoad(int intPlayTurn)
        {
            System.Reflection.Assembly oAssembly = System.Reflection.Assembly.LoadFrom(this.mstrModulePath);


            mobjAI = oAssembly.CreateInstance(mstrNamespace);
            //オブジェクトの型を取得
            this.mobjType = mobjAI.GetType();

            mobjParams = new object[2];
            mobjParams[0] = this.mstrStartingLoadPath;
            mobjParams[1] = intPlayTurn;
            this.InvokeMethod("OnLoad");
        }

        public common.pclsXY OnTurn(int intTurn, int[,] intBoad)
        {
            common.pclsXY clsAIResult = new common.pclsXY();

            mobjParams = new object[2];
            mobjParams[0] = intTurn;
            mobjParams[1] = intBoad;

            //デリゲート呼び出し
            this.InvokeMethod("OnTurn");

            System.IO.StreamReader sr = null;
            try
            {
                sr = new System.IO.StreamReader(mstrStartingLoadPath + "\\" + mcstrResultPath);
                string strResult = sr.ReadLine();

                common.pclsXY retValue = new common.pclsXY();
                clsAIResult.idxX = int.Parse(strResult.Substring(0, 1));
                clsAIResult.idxY = int.Parse(strResult.Substring(1, 1));
            }
            catch
            {
                clsAIResult.idxX = -1;
                clsAIResult.idxY = -1;
            }
            finally
            {
                sr.Close();
                sr.Dispose();
            }

            return clsAIResult;
        }

        public void OnSetting()
        {
        }

        internal void InvokeMethod(string strMethodName)
        {
            //メソッド情報を取得
            System.Reflection.MethodInfo oMethodInfo = mobjType.GetMethod(strMethodName);
            //デリゲート呼び出し
            oMethodInfo.Invoke(mobjAI, mobjParams);
        }
    }
}
