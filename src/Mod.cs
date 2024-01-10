using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using static YTMusicMod.HandlerCommunications;

namespace YTMusicMod
{
    class Mod
    {
        public void Call(long wParam, long lParam, params dynamic[] args)
        {
            //HandleKey(wParam, lParam, args);
        }

        private dynamic HostWindow;
        public static dynamic Tunnel;
        public int Init(dynamic tunnelInstance)
        {
            Tunnel = tunnelInstance;
            HostWindow = Tunnel.HostWindow;
            Config.Load();

            return (int)HandleType.HandlerOnly;
        }

        public dynamic ReturnKeyCombinations()
        {
            return ModKeyCombination.ModKeyCombinations;
        }

        public void SetKeyCombinations()
        {
            Tunnel.UpdateKeyCombinations(this, ModKeyCombination.ModKeyCombinations);
        }

        public void Reload()
        {
            Config.Load();
        }

        public void Closing()
        {
            Config.SaveConfig(Settings.Current);
        }

        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            //using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("YTMusicMod.EmbeddedAssemblies.Newtonsoft.Json.dll"))
            //{
            //    var assemblyData = new byte[stream.Length];
            //    stream.Read(assemblyData, 0, assemblyData.Length);
            //    return Assembly.Load(assemblyData);
            //}

            return null;
        }

    }
}
