﻿// Decompiled with JetBrains decompiler
// Type: Terraria.UI.LegacyGameInterfaceLayer
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

namespace Terraria.UI
{
    public class LegacyGameInterfaceLayer : GameInterfaceLayer
    {
        private GameInterfaceDrawMethod _drawMethod;

        public LegacyGameInterfaceLayer(string name, GameInterfaceDrawMethod drawMethod,
            InterfaceScaleType scaleType = InterfaceScaleType.Game)
            : base(name, scaleType)
        {
            this._drawMethod = drawMethod;
        }

        protected override bool DrawSelf()
        {
            return this._drawMethod();
        }
    }
}