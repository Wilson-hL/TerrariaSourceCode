﻿// Decompiled with JetBrains decompiler
// Type: Terraria.Social.SocialAPI
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System.Collections.Generic;
using Terraria.Social.Steam;

namespace Terraria.Social
{
    public static class SocialAPI
    {
        private static SocialMode _mode = SocialMode.None;
        public static Terraria.Social.Base.FriendsSocialModule Friends;
        public static Terraria.Social.Base.AchievementsSocialModule Achievements;
        public static Terraria.Social.Base.CloudSocialModule Cloud;
        public static Terraria.Social.Base.NetSocialModule Network;
        public static Terraria.Social.Base.OverlaySocialModule Overlay;
        private static List<ISocialModule> _modules;

        public static SocialMode Mode
        {
            get { return SocialAPI._mode; }
        }

        public static void Initialize(SocialMode? mode = null)
        {
            if (!mode.HasValue)
            {
                mode = new SocialMode?(SocialMode.None);
                mode = new SocialMode?(SocialMode.Steam);
            }

            SocialAPI._mode = mode.Value;
            SocialAPI._modules = new List<ISocialModule>();
            if (SocialAPI.Mode == SocialMode.Steam)
                SocialAPI.LoadSteam();
            foreach (ISocialModule module in SocialAPI._modules)
                module.Initialize();
        }

        public static void Shutdown()
        {
            if (SocialAPI._modules == null)
                return;

            SocialAPI._modules.Reverse();
            foreach (ISocialModule module in SocialAPI._modules)
                module.Shutdown();
        }

        private static T LoadModule<T>() where T : ISocialModule, new()
        {
            T obj = new T();
            SocialAPI._modules.Add((ISocialModule) obj);
            return obj;
        }

        private static T LoadModule<T>(T module) where T : ISocialModule
        {
            SocialAPI._modules.Add((ISocialModule) module);
            return module;
        }

        private static void LoadSteam()
        {
            SocialAPI.LoadModule<CoreSocialModule>();
            SocialAPI.Friends =
                (Terraria.Social.Base.FriendsSocialModule) SocialAPI
                    .LoadModule<Terraria.Social.Steam.FriendsSocialModule>();
            SocialAPI.Achievements =
                (Terraria.Social.Base.AchievementsSocialModule) SocialAPI
                    .LoadModule<Terraria.Social.Steam.AchievementsSocialModule>();
            SocialAPI.Cloud =
                (Terraria.Social.Base.CloudSocialModule) SocialAPI
                    .LoadModule<Terraria.Social.Steam.CloudSocialModule>();
            SocialAPI.Overlay =
                (Terraria.Social.Base.OverlaySocialModule) SocialAPI
                    .LoadModule<Terraria.Social.Steam.OverlaySocialModule>();
            SocialAPI.Network = (Terraria.Social.Base.NetSocialModule) SocialAPI.LoadModule<NetClientSocialModule>();
        }
    }
}