using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Osero.Common;

namespace Osero.Interface
{
    public interface IAIInterface
    {
        common.pclsXY OnTurn(int intTurn, int[,] intBoad);
        void OnLoad(int intPlayTurn);
        void OnSetting();

    }
}
