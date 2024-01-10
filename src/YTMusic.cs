using System;
using System.Diagnostics;

namespace YTMusicMod
{
    class YTMusic
    {
        static Process Process;

        public static IntPtr MainWindowHandle => Process.MainWindowHandle;
        public static int Id => Process.Id;


        public static bool IsNull()
        {
            Debug.WriteLine("Checking if null...");

            if (Process == null)
            {
                Process = GetFirstProcessByProcessName("YouTube Music Desktop App");

                if (Process == null)
                    return true;

                //Process = processes[0];
                Process.EnableRaisingEvents = true;
                Process.Exited += (s, eargs) =>
                {
                    Process.Dispose(); //Note builds before 12/11/2021 won't have this line, needs a recompile
                    YTMusicClosed();
                };
            }
            return false;
        }
        static void YTMusicClosed() //If this event isnt called or something, may need to put "YTMusic.WaitForExit();"
        {
            Process = null;
        }

        /// <summary>
        /// Gets First Process By Window Name
        /// </summary>
        /// <param name="c1">See if it contains the first string</param>
        /// <param name="c2">See if it contains the second string (optional)</param>
        /// <returns></returns>
        static Process GetFirstProcessByWindowName(string c1, string c2 = "")
        {
            Process[] p = Process.GetProcesses();

            for (int i = 0; i < p.Length; i++)
            {
                if (p[i].MainWindowTitle.Contains("YouTube Music"))
                    Debug.WriteLine("Contains \"Youtube Music\": " + p[i].MainWindowTitle.Contains("YouTube Music") + " : " + p[i].MainWindowTitle + " : " + p[i].Id);
            }

            if (c2 != "")
            {
                for (int i = 0; i < p.Length; i++)
                {
                    if (p[i].MainWindowTitle.Contains(c1) && p[i].MainWindowTitle.Contains(c2))
                    {
                        return p[i];
                    }
                }
            }
            else
            {
                for (int i = 0; i < p.Length; i++)
                {
                    if (p[i].MainWindowTitle.Contains(c1))
                    {
                        return p[i];
                    }
                }
            }



            return null;
        }

        static Process GetFirstProcessByProcessName(string name)
        {
            Process[] p = Process.GetProcesses();
            for (int i = 0; i < p.Length; i++)
            {
                if (!String.IsNullOrWhiteSpace(p[i].MainWindowTitle) && p[i].ProcessName == name)
                    return p[i];

                Debug.WriteLine(p[i].ProcessName);
            }

            return null;
        }
    }
}
