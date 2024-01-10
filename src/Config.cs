using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using static YTMusicMod.VirtualKeys;

namespace YTMusicMod
{
    public class Settings
    {
        public Settings() { }

        public Settings(List<KeyCommand> keyCommands)
        {
            KeyCommands = keyCommands;
        }

        public List<KeyCommand> KeyCommands { get; /*private*/ set; }


        public static readonly Settings Default = new Settings(
            //@$"C:\Users\{Environment.UserName}\AppData\Roaming\YTMusic\YTMusic.exe",
            //10,
            new List<KeyCommand> {
                //Should probably have a Enum for ScanCodes in the future..., but this isn't too necessary as this is just used for the default and the mod handler will handle everything in the future!
            new KeyCommand(CommandType.OpenYTMusic, new List<KeyData> { new KeyData("[Default]", (int)RawKeyboardFlags.None, 82, (int)VKeys.NUMPAD0) }),
            new KeyCommand(CommandType.PreviousSong, new List<KeyData> { new KeyData("[Default]", (int)RawKeyboardFlags.None, 71, (int)VKeys.NUMPAD7) }),
            new KeyCommand(CommandType.NextSong, new List<KeyData> { new KeyData("[Default]", (int)RawKeyboardFlags.None, 73, (int)VKeys.NUMPAD9) }),
            new KeyCommand(CommandType.Rewind, new List<KeyData> { new KeyData("[Default]", (int)RawKeyboardFlags.None, 75, (int)VKeys.NUMPAD4) }),
            new KeyCommand(CommandType.FastForward, new List<KeyData> { new KeyData("[Default]", (int)RawKeyboardFlags.None, 77, (int)VKeys.NUMPAD6) }),
            new KeyCommand(CommandType.PlayPause, new List<KeyData> { new KeyData("[Default]", (int)RawKeyboardFlags.None, 76, (int)VKeys.NUMPAD5) }),
            new KeyCommand(CommandType.VolumeDown, new List<KeyData> { new KeyData("[Default]", (int)RawKeyboardFlags.None, 79, (int)VKeys.NUMPAD1) }),
            new KeyCommand(CommandType.VolumeUp, new List<KeyData> { new KeyData("[Default]", (int)RawKeyboardFlags.None, 81, (int)VKeys.NUMPAD3) })
        });

        public static Settings Current = Default;
    }

    class Config
    {
        public static readonly string ConfigDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configs");
        public static readonly string AppConfigDir = Path.Combine(ConfigDir, "Application");
        public static readonly string ModConfigDir = Path.Combine(ConfigDir, "Mods");
        public static readonly string YTMusicModConfigDir = Path.Combine(ModConfigDir, "YTMusicMod");
        public static readonly string YTMusicModDefaultConfig = Path.Combine(YTMusicModConfigDir, "YTMusicMod.cfg");

        public static bool Exists => File.Exists(YTMusicModDefaultConfig);

        public static void Load()
        {

            if (Exists)
            {
                Settings.Current = ReadConfig();
            }
            else
            {

                Directory.CreateDirectory(YTMusicModConfigDir);
                SaveConfig(Settings.Current);
            }

            UpdateKeyCombinations();
        }

        public static void UpdateKeyCombinations()
        {
            for (int i = 0; i < Settings.Current.KeyCommands.Count; i++)
            {

                ModKeyCombination.ModKeyCombinations.Add(new ModKeyCombination(Settings.Current.KeyCommands[i].Keys, ModMethods.MethodDict[Settings.Current.KeyCommands[i].CommandType]));
                for (int j = 0; j < Settings.Current.KeyCommands[i].Keys.Count; j++)
                {
                    KeyData d = Settings.Current.KeyCommands[i].Keys[j];
                    Mod.Tunnel.WriteDebugLine(d.Flags + " : " + d.KeyboardAlias + " : " + d.ScanCode + " : " + d.VirtualKey + " : " + Settings.Current.KeyCommands[i].CommandType);
                }
            }
        }

        public static Settings ReadConfig()
        {
            XmlSerializer xs = new XmlSerializer(typeof(Settings));

            try
            {

                using (TextReader tw = new StreamReader(YTMusicModDefaultConfig))
                {
                    return (Settings)xs.Deserialize(tw);
                }
            }
            catch (Exception ex)
            {
                new Thread(() =>
                {
                    Mod.Tunnel.ShowMessageBox($"Type: {ex.GetType()}\r\nMessage: {ex.Message}\r\n\r\nPlease check the config file for any invalid/misplaced characters and reload the mods before continuing!",
                      "YTMusic Mod - Execution Error @ ReadConfig (ConfigDeserialize)");
                }).Start();
            }
            return null;
        }

        public static void SaveConfig(Settings settings, Stream saveStream)
        {
            XmlSerializer xs = new XmlSerializer(typeof(Settings));

            xs.Serialize(saveStream, settings);
        }

        public static void SaveConfig(Settings settings)
        {
            using (var f = File.Create(YTMusicModDefaultConfig))
            {
                SaveConfig(settings, f);
            }
        }
    }

    public class KeyCommand
    {
        public KeyCommand() { }
        public KeyCommand(CommandType commandType, List<KeyData> keyData)
        {
            CommandType = commandType;
            Keys = keyData;
        }

        public CommandType CommandType;
        public List<KeyData> Keys;
    }

    [Flags]
    public enum RawKeyboardFlags : ushort
    {
        None = 0,
        Up = 1 << 0,
        KeyE0 = 1 << 1,
        KeyE1 = 1 << 2,
    }

    public enum CommandType
    {
        OpenYTMusic,
        PreviousSong,
        NextSong,
        Rewind,
        FastForward,
        PlayPause,
        VolumeDown,
        VolumeUp,
    }
}
