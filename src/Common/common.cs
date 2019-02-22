using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Osero.Common
{
    public class iniFileAccess
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(
        string lpApplicationName,
        string lpKeyName,
        string lpDefault,
        StringBuilder lpReturnedstring,
        int nSize,
        string lpFileName);

        public static string GetIniValue(string path, string section, string key)
        {
            StringBuilder sb = new StringBuilder(256);
            GetPrivateProfileString(section, key, string.Empty, sb, sb.Capacity, path);
            return sb.ToString();
        }
    }
    public class common
    {
        public class pclsXY
        {
            public int idxX, idxY;
            public pclsXY()
            {

            }
            public pclsXY(int x, int y)
            {
                this.idxX = x;
                this.idxY = y;
            }
            public void setIdx(int x, int y)
            {
                this.idxX = x;
                this.idxY = y;
            }
            public pclsXY clone()
            {
                pclsXY ret = new pclsXY();
                ret.setIdx(this.idxX, this.idxY);
                return ret;
            }
        }
    }
}
