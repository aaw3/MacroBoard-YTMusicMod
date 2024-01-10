using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using YTMusicMod.Native;
using static YTMusicMod.Native.Messages;
using static YTMusicMod.Utils;

namespace YTMusicMod
{
    public class ModMethods
    {
        public static ModMethods Singleton = new ModMethods();

        public static Dictionary<CommandType, Action> MethodDict = new Dictionary<CommandType, Action>()
        {
            { CommandType.OpenYTMusic, Singleton.OpenYTMusic },
            { CommandType.PreviousSong, Singleton.PreviousSong },
            { CommandType.NextSong, Singleton.NextSong },
            { CommandType.Rewind, Singleton.Rewind },
            { CommandType.FastForward, Singleton.FastForward },
            { CommandType.PlayPause, Singleton.PlayPause },
            { CommandType.VolumeDown, Singleton.VolumeDown },
            { CommandType.VolumeUp, Singleton.VolumeUp }
        };
        public void OpenYTMusic()
        {
            try
            {
                Process.Start("music.youtube.com");
            }
            catch (Exception ex)
            {
                new Thread(() =>
                { Mod.Tunnel.ShowMessageBox($"Type: {ex.GetType()}\r\nMessage: {ex.Message}", "YTMusic Mod - Execution Error @ OpenYTMusic"); }).Start();
            }
        }

        public void PreviousSong()
        {
            if (YTMusic.IsNull()) return;

            PostMessage(YTMusic.MainWindowHandle, (int)WindowMessage.WM_APPCOMMAND, 0, ShiftAppCommandCode(AppComandCode.MEDIA_PREVIOUSTRACK));
        }

        public void NextSong()
        {
            if (YTMusic.IsNull()) return;

            PostMessage(YTMusic.MainWindowHandle, (int)WindowMessage.WM_APPCOMMAND, 0, ShiftAppCommandCode(AppComandCode.MEDIA_NEXTTRACK));
        }

        public void Rewind()
        {
            if (YTMusic.IsNull()) return;

            PostMessage(YTMusic.MainWindowHandle, (int)WindowMessage.WM_APPCOMMAND, 0, ShiftAppCommandCode(AppComandCode.MEDIA_REWIND));
        }

        public void FastForward()
        {
            if (YTMusic.IsNull()) return;

            PostMessage(YTMusic.MainWindowHandle, (int)WindowMessage.WM_APPCOMMAND, 0, ShiftAppCommandCode(AppComandCode.MEDIA_FASTFORWARD));
        }

        public void PlayPause()
        {
            if (YTMusic.IsNull()) return;

            Thread.Sleep(500);
            PostMessage(YTMusic.MainWindowHandle, (int)WindowMessage.WM_KEYDOWN, 0x00000020, 0x00390001);
        }

        public void VolumeDown()
        {
            if (YTMusic.IsNull()) return;



            ChangeVolume(YTMusic.Id, VolumeChange.Decrease, Settings.Current.VolumeDecreaseRate);
        }

        // Volume might not be able to change if the window isn't focused
        public void VolumeUp()
        {
            if (YTMusic.IsNull()) return;

            Thread.Sleep(500);


            // This might work and ChangeVolume might not, I haven't checked the code in years
            PostMessage((IntPtr)331986, (uint)WindowMessage.WM_KEYDOWN, (long)VirtualKeys.VKeys.SPACE, 0);


        
            ChangeVolume(YTMusic.Id, VolumeChange.Increase, Settings.Current.VolumeIncreaseRate);
        }
    }
}
