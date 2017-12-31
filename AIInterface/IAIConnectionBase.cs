using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIInterface
{
    public interface IAIConnectionBase
    {
        void OnTrun();
        void OnSetting();
        void OnLoad();
    }
}
