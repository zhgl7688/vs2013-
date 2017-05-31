using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoWoSoft.Cache.Factory
{
    public class Cache
    {
        public static Interface.ICache CreateInstance()
        {
            return new FoWoSoft.Cache.InProc.Cache();
        }
    }
}
