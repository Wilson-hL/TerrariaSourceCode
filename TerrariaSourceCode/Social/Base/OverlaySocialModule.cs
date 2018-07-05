﻿// Decompiled with JetBrains decompiler
// Type: Terraria.Social.Base.OverlaySocialModule
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

namespace Terraria.Social.Base
{
    public abstract class OverlaySocialModule : ISocialModule
    {
        public abstract void Initialize();

        public abstract void Shutdown();

        public abstract bool IsGamepadTextInputActive();

        public abstract bool ShowGamepadTextInput(string description, uint maxLength, bool multiLine = false,
            string existingText = "", bool password = false);

        public abstract string GetGamepadText();
    }
}