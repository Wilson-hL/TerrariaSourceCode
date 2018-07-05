// Decompiled with JetBrains decompiler
// Type: Terraria.DataStructures.DrawAnimationVertical
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.DataStructures
{
    public class DrawAnimationVertical : DrawAnimation
    {
        public DrawAnimationVertical(int ticksperframe, int frameCount)
        {
            Frame = 0;
            FrameCounter = 0;
            FrameCount = frameCount;
            TicksPerFrame = ticksperframe;
        }

        public override void Update()
        {
            if (++FrameCounter < TicksPerFrame)
                return;
            FrameCounter = 0;
            if (++Frame < FrameCount)
                return;
            Frame = 0;
        }

        public override Rectangle GetFrame(Texture2D texture)
        {
            return texture.Frame(1, FrameCount, 0, Frame);
        }
    }
}