// Decompiled with JetBrains decompiler
// Type: Terraria.Audio.LegacySoundStyle
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework.Audio;
using Terraria.Utilities;

namespace Terraria.Audio
{
    public class LegacySoundStyle : SoundStyle
    {
        private static readonly UnifiedRandom _random = new UnifiedRandom();
        private readonly int _style;

        public LegacySoundStyle(int soundId, int style, SoundType type = SoundType.Sound)
            : base(type)
        {
            _style = style;
            Variations = 1;
            SoundId = soundId;
        }

        public LegacySoundStyle(int soundId, int style, int variations, SoundType type = SoundType.Sound)
            : base(type)
        {
            _style = style;
            Variations = variations;
            SoundId = soundId;
        }

        private LegacySoundStyle(int soundId, int style, int variations, SoundType type, float volume,
            float pitchVariance)
            : base(volume, pitchVariance, type)
        {
            _style = style;
            Variations = variations;
            SoundId = soundId;
        }

        public int Style
        {
            get
            {
                if (Variations != 1)
                    return _random.Next(_style, _style + Variations);
                return _style;
            }
        }

        public int Variations { get; }

        public int SoundId { get; }

        public override bool IsTrackable => SoundId == 42;

        public LegacySoundStyle WithVolume(float volume)
        {
            return new LegacySoundStyle(SoundId, _style, Variations, Type, volume,
                PitchVariance);
        }

        public LegacySoundStyle WithPitchVariance(float pitchVariance)
        {
            return new LegacySoundStyle(SoundId, _style, Variations, Type, Volume,
                pitchVariance);
        }

        public LegacySoundStyle AsMusic()
        {
            return new LegacySoundStyle(SoundId, _style, Variations, SoundType.Music, Volume,
                PitchVariance);
        }

        public LegacySoundStyle AsAmbient()
        {
            return new LegacySoundStyle(SoundId, _style, Variations, SoundType.Ambient,
                Volume, PitchVariance);
        }

        public LegacySoundStyle AsSound()
        {
            return new LegacySoundStyle(SoundId, _style, Variations, SoundType.Sound, Volume,
                PitchVariance);
        }

        public bool Includes(int soundId, int style)
        {
            if (SoundId == soundId && style >= _style)
                return style < _style + Variations;
            return false;
        }

        public override SoundEffect GetRandomSound()
        {
            if (IsTrackable)
                return Main.trackableSounds[Style];
            return null;
        }
    }
}