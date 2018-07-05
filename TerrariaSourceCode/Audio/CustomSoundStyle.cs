// Decompiled with JetBrains decompiler
// Type: Terraria.Audio.CustomSoundStyle
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework.Audio;
using Terraria.Utilities;

namespace Terraria.Audio
{
    public class CustomSoundStyle : SoundStyle
    {
        private static readonly UnifiedRandom _random = new UnifiedRandom();
        private readonly SoundEffect[] _soundEffects;

        public CustomSoundStyle(SoundEffect soundEffect, SoundType type = SoundType.Sound, float volume = 1f,
            float pitchVariance = 0.0f)
            : base(volume, pitchVariance, type)
        {
            _soundEffects = new SoundEffect[1] {soundEffect};
        }

        public CustomSoundStyle(SoundEffect[] soundEffects, SoundType type = SoundType.Sound, float volume = 1f,
            float pitchVariance = 0.0f)
            : base(volume, pitchVariance, type)
        {
            _soundEffects = soundEffects;
        }

        public override bool IsTrackable => true;

        public override SoundEffect GetRandomSound()
        {
            return _soundEffects[_random.Next(_soundEffects.Length)];
        }
    }
}