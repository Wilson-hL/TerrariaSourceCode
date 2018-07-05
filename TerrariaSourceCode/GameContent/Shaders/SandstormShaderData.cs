﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Shaders.SandstormShaderData
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent.Shaders
{
    public class SandstormShaderData : ScreenShaderData
    {
        private Vector2 _texturePosition = Vector2.Zero;

        public SandstormShaderData(string passName)
            : base(passName)
        {
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 vector2 = new Vector2(-Main.windSpeed, -1f) * new Vector2(20f, 0.1f);
            vector2.Normalize();
            Vector2 direction = vector2 * new Vector2(2f, 0.2f);
            if (!Main.gamePaused && Main.hasFocus)
                this._texturePosition += direction * (float) gameTime.ElapsedGameTime.TotalSeconds;
            this._texturePosition.X %= 10f;
            this._texturePosition.Y %= 10f;
            this.UseDirection(direction);
            base.Update(gameTime);
        }

        public override void Apply()
        {
            this.UseTargetPosition(this._texturePosition);
            base.Apply();
        }
    }
}