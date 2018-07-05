﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.UI.Elements.UIImage
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
    public class UIImage : UIElement
    {
        public float ImageScale = 1f;
        private Texture2D _texture;

        public UIImage(Texture2D texture)
        {
            this._texture = texture;
            this.Width.Set((float) this._texture.Width, 0.0f);
            this.Height.Set((float) this._texture.Height, 0.0f);
        }

        public void SetImage(Texture2D texture)
        {
            this._texture = texture;
            this.Width.Set((float) this._texture.Width, 0.0f);
            this.Height.Set((float) this._texture.Height, 0.0f);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = this.GetDimensions();
            spriteBatch.Draw(this._texture, dimensions.Position() + this._texture.Size() * (1f - this.ImageScale) / 2f,
                new Rectangle?(), Color.White, 0.0f, Vector2.Zero, this.ImageScale, SpriteEffects.None, 0.0f);
        }
    }
}