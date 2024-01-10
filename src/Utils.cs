using System;
using System.Collections.Generic;
using System.Text;
using static YTMusicMod.Native.Messages;

namespace YTMusicMod
{
    public class Utils
    {
        public static int ShiftAppCommandCode(AppComandCode code)
        {
            return (int)code << 16;
        }
    }
}
