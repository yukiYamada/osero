using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Osero.Common;

namespace Osero.Interface
{
    public static class CInterfaceCreater
    {

        public static IAIInterface Create(string strIniFileName, string strStartingLoadPath, int intPlayTurn)
        {
            string strModulePath = strStartingLoadPath + "\\AI\\";
            string strModuleType = iniFileAccess.GetIniValue(strModulePath + strIniFileName, "AICommon", "moduleType");

            string strModuleName = iniFileAccess.GetIniValue(strModulePath + strIniFileName, "AICommon", "moduleName");
            string strModuleFullPath = strModulePath + strModuleName;

            IAIInterface returnValue = CreateInstance(strStartingLoadPath, intPlayTurn,strModuleType, strModuleFullPath);
            returnValue.OnLoad(intPlayTurn);
            return returnValue;

        }

        private static IAIInterface CreateInstance(string strStartingLoadPath, int intPlayTurn, string moduleType, string moduleFullPath)
        {
            IAIInterface returnValue = null;
            switch (moduleType)
            {
                case "C#":
                    returnValue = new CInterfaceForDotnet(strStartingLoadPath, moduleFullPath);
                    break;
                default:
                    break;
            }
            return returnValue;
        }        
    }

}