using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Osero.Common;

namespace Osero.Interface
{
    public abstract class CAIInterfaceBase
    {
        protected string mstrNamespace;
        internal string mstrModulePath;
        internal string mstrIniFilePath;
        internal string mstrStartingLoadPath;
        internal const string mcstrResultPath = "AIResult.dat";

        public CAIInterfaceBase(string strStartingLoadPath, string strModulePath)
        {
            this.mstrStartingLoadPath = strStartingLoadPath;
            this.mstrModulePath = strModulePath;
            this.mstrIniFilePath = System.IO.Path.GetDirectoryName(strModulePath) + "\\" + System.IO.Path.GetFileNameWithoutExtension(strModulePath) + ".ini";

            GetAISettings();
        }

        internal virtual void GetAISettings()
        {
            this.mstrNamespace = iniFileAccess.GetIniValue(this.mstrIniFilePath, "loadop", "namespace");
        }
 
    }

}
