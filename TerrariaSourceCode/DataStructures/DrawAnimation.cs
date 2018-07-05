// Decompiled with JetBrains decompiler
// Type: Terraria.DataStructures.DrawAnimation
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.DataStructures
{
    public class DrawAnimation
    {
        public int Frame;
        public int FrameCount;
        public int FrameCounter;
        public int TicksPerFrame;

        public virtual void Update()
        {
        }

        public virtual Rectangle GetFrame(Texture2D texture)
        {
            return texture.Frame(1, 1, 0, 0);
        }
    }
}