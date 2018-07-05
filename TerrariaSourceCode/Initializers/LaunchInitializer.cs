﻿// Decompiled with JetBrains decompiler
// Type: Terraria.Initializers.LaunchInitializer
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System.Diagnostics;
using Terraria.Localization;
using Terraria.Social;

namespace Terraria.Initializers
{
    public static class LaunchInitializer
    {
        public static void LoadParameters(Main game)
        {
            LaunchInitializer.LoadSharedParameters(game);
            LaunchInitializer.LoadClientParameters(game);
        }

        private static void LoadSharedParameters(Main game)
        {
            var strArray1 = new string[1] {"-loadlib"};
            string path;
            if ((path = LaunchInitializer.TryParameter(strArray1)) != null)
                game.loadLib(path);
            var strArray2 = new string[2] {"-p", "-port"};
            string s;
            int result;
            if ((s = LaunchInitializer.TryParameter(strArray2)) == null || !int.TryParse(s, out result))
                return;
            Netplay.ListenPort = result;
        }

        private static void LoadClientParameters(Main game)
        {
            var strArray1 = new string[2] {"-j", "-join"};
            string IP;
            if ((IP = LaunchInitializer.TryParameter(strArray1)) != null)
                game.AutoJoin(IP);
            var strArray2 = new string[2] {"-pass", "-password"};
            string str;
            if ((str = LaunchInitializer.TryParameter(strArray2)) != null)
            {
                Netplay.ServerPassword = str;
                game.AutoPass();
            }

            if (!LaunchInitializer.HasParameter("-host"))
                return;
            game.AutoHost();
        }

        private static void LoadServerParameters(Main game)
        {
            try
            {
                var strArray = new string[1] {"-forcepriority"};
                string s;
                if ((s = LaunchInitializer.TryParameter(strArray)) != null)
                {
                    var currentProcess = Process.GetCurrentProcess();
                    int result;
                    if (int.TryParse(s, out result))
                    {
                        switch (result)
                        {
                            case 0:
                                currentProcess.PriorityClass = ProcessPriorityClass.RealTime;
                                break;
                            case 1:
                                currentProcess.PriorityClass = ProcessPriorityClass.High;
                                break;
                            case 2:
                                currentProcess.PriorityClass = ProcessPriorityClass.AboveNormal;
                                break;
                            case 3:
                                currentProcess.PriorityClass = ProcessPriorityClass.Normal;
                                break;
                            case 4:
                                currentProcess.PriorityClass = ProcessPriorityClass.BelowNormal;
                                break;
                            case 5:
                                currentProcess.PriorityClass = ProcessPriorityClass.Idle;
                                break;
                            default:
                                currentProcess.PriorityClass = ProcessPriorityClass.High;
                                break;
                        }
                    }
                    else
                        currentProcess.PriorityClass = ProcessPriorityClass.High;
                }
                else
                    Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            }
            catch
            {
            }

            var strArray1 = new string[2] {"-maxplayers", "-players"};
            string s1;
            int result1;
            if ((s1 = LaunchInitializer.TryParameter(strArray1)) != null && int.TryParse(s1, out result1))
                game.SetNetPlayers(result1);
            var strArray2 = new string[2] {"-pass", "-password"};
            string str1;
            if ((str1 = LaunchInitializer.TryParameter(strArray2)) != null)
                Netplay.ServerPassword = str1;
            var strArray3 = new string[1] {"-lang"};
            string s2;
            int result2;
            if ((s2 = LaunchInitializer.TryParameter(strArray3)) != null && int.TryParse(s2, out result2))
                LanguageManager.Instance.SetLanguage(result2);
            var strArray4 = new string[1] {"-language"};
            string cultureName;
            if ((cultureName = LaunchInitializer.TryParameter(strArray4)) != null)
                LanguageManager.Instance.SetLanguage(cultureName);
            var strArray5 = new string[1] {"-worldname"};
            string world1;
            if ((world1 = LaunchInitializer.TryParameter(strArray5)) != null)
                game.SetWorldName(world1);
            var strArray6 = new string[1] {"-motd"};
            string newMOTD;
            if ((newMOTD = LaunchInitializer.TryParameter(strArray6)) != null)
                game.NewMOTD(newMOTD);
            var strArray7 = new string[1] {"-banlist"};
            string str2;
            if ((str2 = LaunchInitializer.TryParameter(strArray7)) != null)
                Netplay.BanFilePath = str2;
            if (LaunchInitializer.HasParameter("-autoshutdown"))
                game.EnableAutoShutdown();
            if (LaunchInitializer.HasParameter("-secure"))
                Netplay.spamCheck = true;
            var strArray8 = new string[1] {"-autocreate"};
            string worldSize;
            if ((worldSize = LaunchInitializer.TryParameter(strArray8)) != null)
                game.autoCreate(worldSize);
            if (LaunchInitializer.HasParameter("-noupnp"))
                Netplay.UseUPNP = false;
            if (LaunchInitializer.HasParameter("-experimental"))
                Main.UseExperimentalFeatures = true;
            var strArray9 = new string[1] {"-world"};
            string world2;
            if ((world2 = LaunchInitializer.TryParameter(strArray9)) != null)
                game.SetWorld(world2, false);
            else if (SocialAPI.Mode == SocialMode.Steam)
            {
                var strArray10 = new string[1] {"-cloudworld"};
                string world3;
                if ((world3 = LaunchInitializer.TryParameter(strArray10)) != null)
                    game.SetWorld(world3, true);
            }

            var strArray11 = new string[1] {"-config"};
            string configPath;
            if ((configPath = LaunchInitializer.TryParameter(strArray11)) != null)
                game.LoadDedConfig(configPath);
            var strArray12 = new string[1] {"-seed"};
            string str3;
            if ((str3 = LaunchInitializer.TryParameter(strArray12)) == null)
                return;
            Main.AutogenSeedName = str3;
        }

        private static bool HasParameter(params string[] keys)
        {
            for (var index = 0; index < keys.Length; ++index)
            {
                if (Program.LaunchParameters.ContainsKey(keys[index]))
                    return true;
            }

            return false;
        }

        private static string TryParameter(params string[] keys)
        {
            for (var index = 0; index < keys.Length; ++index)
            {
                string str;
                if (Program.LaunchParameters.TryGetValue(keys[index], out str))
                {
                    if (str == null)
                        str = "";
                    return str;
                }
            }

            return (string) null;
        }
    }
}