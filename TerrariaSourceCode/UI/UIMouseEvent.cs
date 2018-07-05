﻿// Decompiled with JetBrains decompiler
// Type: Terraria.UI.UIMouseEvent
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;

namespace Terraria.UI
{
    public class UIMouseEvent : UIEvent
    {
        public readonly Vector2 MousePosition;

        public UIMouseEvent(UIElement target, Vector2 mousePosition)
            : base(target)
        {
            this.MousePosition = mousePosition;
        }
    }
}