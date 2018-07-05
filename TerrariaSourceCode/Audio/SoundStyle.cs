// Decompiled with JetBrains decompiler
// Type: Terraria.Audio.SoundStyle
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework.Audio;
using Terraria.Utilities;

namespace Terraria.Audio
{
    public abstract class SoundStyle
    {
        private static readonly UnifiedRandom _random = new UnifiedRandom();

        public SoundStyle(float volume, float pitchVariance, SoundType type = SoundType.Sound)
        {
            Volume = volume;
            PitchVariance = pitchVariance;
            Type = type;
        }

        public SoundStyle(SoundType type = SoundType.Sound)
        {
            Volume = 1f;
            PitchVariance = 0.0f;
            Type = type;
        }

        public float Volume { get; }

        public float PitchVariance { get; }

        public SoundType Type { get; }

        public abstract bool IsTrackable { get; }

        public float GetRandomPitch()
        {
            return (float) (_random.NextFloat() * (double) PitchVariance -
                            PitchVariance * 0.5);
        }

        public abstract SoundEffect GetRandomSound();
    }
}