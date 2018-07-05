// Decompiled with JetBrains decompiler
// Type: Terraria.Cinematics.FrameEventData
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

namespace Terraria.Cinematics
{
    public struct FrameEventData
    {
        public int AbsoluteFrame { get; }

        public int Start { get; }

        public int Duration { get; }

        public int Frame => AbsoluteFrame - Start;

        public bool IsFirstFrame => Start == AbsoluteFrame;

        public bool IsLastFrame => Remaining == 0;

        public int Remaining => Start + Duration - AbsoluteFrame - 1;

        public FrameEventData(int absoluteFrame, int start, int duration)
        {
            AbsoluteFrame = absoluteFrame;
            Start = start;
            Duration = duration;
        }
    }
}