﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.UI.Elements.UIKeybindingToggleListItem
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.Graphics;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Elements
{
    public class UIKeybindingToggleListItem : UIElement
    {
        private Color _color;
        private Func<string> _TextDisplayFunction;
        private Func<bool> _IsOnFunction;
        private Texture2D _toggleTexture;

        public UIKeybindingToggleListItem(Func<string> getText, Func<bool> getStatus, Color color)
        {
            this._color = color;
            this._toggleTexture = TextureManager.Load("Images/UI/Settings_Toggle");
            this._TextDisplayFunction = getText != null ? getText : (Func<string>) (() => "???");
            this._IsOnFunction = getStatus != null ? getStatus : (Func<bool>) (() => false);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            var num1 = 6f;
            base.DrawSelf(spriteBatch);
            var dimensions = this.GetDimensions();
            var num2 = dimensions.Width + 1f;
            var vector2_1 = new Vector2(dimensions.X, dimensions.Y);
            var flag = false;
            var baseScale = new Vector2(0.8f);
            var baseColor = Color.Lerp(flag ? Color.Gold : (this.IsMouseHovering ? Color.White : Color.Silver),
                Color.White, this.IsMouseHovering ? 0.5f : 0.0f);
            var color = this.IsMouseHovering ? this._color : this._color.MultiplyRGBA(new Color(180, 180, 180));
            var position = vector2_1;
            Utils.DrawSettingsPanel(spriteBatch, position, num2, color);
            position.X += 8f;
            position.Y += 2f + num1;
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontItemStack, this._TextDisplayFunction(),
                position, baseColor, 0.0f, Vector2.Zero, baseScale, num2, 2f);
            position.X -= 17f;
            var rectangle = new Rectangle(this._IsOnFunction() ? (this._toggleTexture.Width - 2) / 2 + 2 : 0, 0,
                (this._toggleTexture.Width - 2) / 2, this._toggleTexture.Height);
            var vector2_2 = new Vector2((float) rectangle.Width, 0.0f);
            position = new Vector2(
                (float) ((double) dimensions.X + (double) dimensions.Width - (double) vector2_2.X - 10.0),
                dimensions.Y + 2f + num1);
            spriteBatch.Draw(this._toggleTexture, position, new Rectangle?(rectangle), Color.White, 0.0f, Vector2.Zero,
                Vector2.One, SpriteEffects.None, 0.0f);
        }
    }
}