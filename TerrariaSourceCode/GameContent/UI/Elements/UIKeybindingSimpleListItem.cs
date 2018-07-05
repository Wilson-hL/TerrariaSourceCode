﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.UI.Elements.UIKeybindingSimpleListItem
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Elements
{
    public class UIKeybindingSimpleListItem : UIElement
    {
        private Color _color;
        private Func<string> _GetTextFunction;

        public UIKeybindingSimpleListItem(Func<string> getText, Color color)
        {
            this._color = color;
            this._GetTextFunction = getText != null ? getText : (Func<string>) (() => "???");
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            float num1 = 6f;
            base.DrawSelf(spriteBatch);
            CalculatedStyle dimensions = this.GetDimensions();
            float num2 = dimensions.Width + 1f;
            Vector2 vector2 = new Vector2(dimensions.X, dimensions.Y);
            Vector2 baseScale = new Vector2(0.8f);
            Color baseColor = Color.Lerp(this.IsMouseHovering ? Color.White : Color.Silver, Color.White,
                this.IsMouseHovering ? 0.5f : 0.0f);
            Color color = this.IsMouseHovering ? this._color : this._color.MultiplyRGBA(new Color(180, 180, 180));
            Vector2 position = vector2;
            Utils.DrawSettings2Panel(spriteBatch, position, num2, color);
            position.X += 8f;
            position.Y += 2f + num1;
            string text = this._GetTextFunction();
            Vector2 stringSize = ChatManager.GetStringSize(Main.fontItemStack, text, baseScale, -1f);
            position.X =
                (float) ((double) dimensions.X + (double) dimensions.Width / 2.0 - (double) stringSize.X / 2.0);
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontItemStack, text, position, baseColor, 0.0f,
                Vector2.Zero, baseScale, num2, 2f);
        }
    }
}