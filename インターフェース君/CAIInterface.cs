using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using 共通君;

namespace オセロ君
{
  
    
    public abstract class CInterfaceBase
    {
        internal object mobjAI;
        internal Type mobjType;
        public object[] mobjParams;
        internal string mstrModulePath;
        internal string mstrIniFilePath;
        internal string mstrStartingLoadPath;
        internal const string mcstrResultPath = "AIResult.dat";
        internal void OnLoadBase(string strStartingLoadPath, int intPlayTurn, string strModulePath)
        {
            this.mstrStartingLoadPath = strStartingLoadPath;
            this.mstrModulePath = strModulePath;
            this.mstrIniFilePath = System.IO.Path.GetDirectoryName(strModulePath) + "\\" + System.IO.Path.GetFileNameWithoutExtension(strModulePath) + ".ini";

            GetAISettings();
            OnLoad(intPlayTurn);
        }
        internal virtual void GetAISettings() { }
        internal virtual void OnLoad(int intPlayTurn) { }
        abstract public common.pclsXY OnTurn(int intTurn, int[,] intBoad);
        abstract public void OnSetting();
        internal void InvokeMethod(string strMethodName)
        {
            //メソッド情報を取得
            System.Reflection.MethodInfo oMethodInfo = mobjType.GetMethod(strMethodName);
            //デリゲート呼び出し
            oMethodInfo.Invoke(mobjAI, mobjParams);
        }
    }
    public class CInterface : CInterfaceBase
    {
        private CInterfaceForDotnet mIFDotnet;
        private string mstrModuleType;
        private string mstrModuleName;
        private string mstrModuleFullPath;
        private int mintPlayTurn;
        public CInterface(string strIniFileName, string strStartingLoadPath, int intPlayTurn)
        {
            string strModulePath = strStartingLoadPath + "\\AI\\";
            this.mstrModuleType = iniFileAccess.GetIniValue(strModulePath + strIniFileName, "AICommon", "moduleType");
            this.mstrModuleName = iniFileAccess.GetIniValue(strModulePath + strIniFileName, "AICommon", "moduleName");
            this.mstrStartingLoadPath = strStartingLoadPath;
            this.mstrModuleFullPath = strModulePath + mstrModuleName;
            this.mintPlayTurn = intPlayTurn;

            this.CreateInstance();
        }

        private void CreateInstance()
        {
            switch (this.mstrModuleType)
            {
                case "C#":
                    this.mIFDotnet = new CInterfaceForDotnet();
                    this.mIFDotnet.OnLoadBase(this.mstrStartingLoadPath, this.mintPlayTurn, mstrModuleFullPath);
                    break;
                default:
                    break;
            }
        }

        public override common.pclsXY OnTurn(int intTurn, int[,] intBoad)
        {
            common.pclsXY ret;
            switch (this.mstrModuleType)
            {
                case "C#":
                    ret = this.mIFDotnet.OnTurn(intTurn, intBoad);
                    break;
                default:
                    ret = null;
                    break;
            }
            return ret;
        }

        public override void OnSetting()
        {
            switch (this.mstrModuleType)
            {
                case "C#":
                    this.mIFDotnet.OnSetting();
                    break;
                default:
                    break;
            }
        }
    }
    internal class CInterfaceForDotnet : CInterfaceBase
    {
        private string mstrNamespace;

        public CInterfaceForDotnet()
        {
        }
        internal override void GetAISettings()
        {
            this.mstrNamespace = iniFileAccess.GetIniValue(this.mstrIniFilePath, "loadop", "namespace");
        }
        internal override void OnLoad( int intPlayTurn)
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

        public override common.pclsXY OnTurn(int intTurn, int[,] intBoad)
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

        public override void OnSetting()
        {
        }
    }

}