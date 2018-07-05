﻿// Decompiled with JetBrains decompiler
// Type: Terraria.Localization.GameCulture
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Terraria.Localization
{
    public class GameCulture
    {
        public static readonly GameCulture English = new GameCulture("en-US", 1);
        public static readonly GameCulture German = new GameCulture("de-DE", 2);
        public static readonly GameCulture Italian = new GameCulture("it-IT", 3);
        public static readonly GameCulture French = new GameCulture("fr-FR", 4);
        public static readonly GameCulture Spanish = new GameCulture("es-ES", 5);
        public static readonly GameCulture Russian = new GameCulture("ru-RU", 6);
        public static readonly GameCulture Chinese = new GameCulture("zh-Hans", 7);
        public static readonly GameCulture Portuguese = new GameCulture("pt-BR", 8);
        public static readonly GameCulture Polish = new GameCulture("pl-PL", 9);
        private static Dictionary<int, GameCulture> _legacyCultures;
        public readonly CultureInfo CultureInfo;
        public readonly int LegacyId;

        public bool IsActive
        {
            get { return Language.ActiveCulture == this; }
        }

        public string Name
        {
            get { return this.CultureInfo.Name; }
        }

        public static GameCulture FromLegacyId(int id)
        {
            if (id < 1)
                id = 1;
            return GameCulture._legacyCultures[id];
        }

        public static GameCulture FromName(string name)
        {
            return GameCulture._legacyCultures.Values.SingleOrDefault<GameCulture>(
                       (Func<GameCulture, bool>) (culture => culture.Name == name)) ?? GameCulture.English;
        }

        public GameCulture(string name, int legacyId)
        {
            this.CultureInfo = new CultureInfo(name);
            this.LegacyId = legacyId;
            GameCulture.RegisterLegacyCulture(this, legacyId);
        }

        private static void RegisterLegacyCulture(GameCulture culture, int legacyId)
        {
            if (GameCulture._legacyCultures == null)
                GameCulture._legacyCultures = new Dictionary<int, GameCulture>();
            GameCulture._legacyCultures.Add(legacyId, culture);
        }
    }
}