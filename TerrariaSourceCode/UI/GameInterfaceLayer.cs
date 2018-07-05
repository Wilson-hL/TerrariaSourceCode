﻿// Decompiled with JetBrains decompiler
// Type: Terraria.UI.GameInterfaceLayer
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameInput;

namespace Terraria.UI
{
    public class GameInterfaceLayer
    {
        public readonly string Name;
        public InterfaceScaleType ScaleType;

        public GameInterfaceLayer(string name, InterfaceScaleType scaleType)
        {
            this.Name = name;
            this.ScaleType = scaleType;
        }

        public bool Draw()
        {
            Matrix transformMatrix;
            if (this.ScaleType == InterfaceScaleType.Game)
            {
                PlayerInput.SetZoom_World();
                transformMatrix = Main.GameViewMatrix.ZoomMatrix;
            }
            else if (this.ScaleType == InterfaceScaleType.UI)
            {
                PlayerInput.SetZoom_UI();
                transformMatrix = Main.UIScaleMatrix;
            }
            else
            {
                PlayerInput.SetZoom_Unscaled();
                transformMatrix = Matrix.Identity;
            }

            Main.spriteBatch.Begin(SpriteSortMode.Deferred, (BlendState) null, (SamplerState) null,
                (DepthStencilState) null, (RasterizerState) null, (Effect) null, transformMatrix);
            bool flag = this.DrawSelf();
            Main.spriteBatch.End();
            return flag;
        }

        protected virtual bool DrawSelf()
        {
            return true;
        }
    }
}