// Decompiled with JetBrains decompiler
// Type: Terraria.Audio.ActiveSound
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Terraria.Audio
{
    public class ActiveSound
    {
        public readonly bool IsGlobal;
        public Vector2 Position;
        public float Volume;

        public ActiveSound(SoundStyle style, Vector2 position)
        {
            Position = position;
            Volume = 1f;
            IsGlobal = false;
            Style = style;
            Play();
        }

        public ActiveSound(SoundStyle style)
        {
            Position = Vector2.Zero;
            Volume = 1f;
            IsGlobal = true;
            Style = style;
            Play();
        }

        public SoundEffectInstance Sound { get; private set; }

        public SoundStyle Style { get; }

        public bool IsPlaying => Sound.State == SoundState.Playing;

        private void Play()
        {
            var instance = Style.GetRandomSound().CreateInstance();
            instance.Pitch += Style.GetRandomPitch();
            Main.PlaySoundInstance(instance);
            Sound = instance;
            Update();
        }

        public void Stop()
        {
            if (Sound == null)
                return;
            Sound.Stop();
        }

        public void Pause()
        {
            if (Sound == null || Sound.State != SoundState.Playing)
                return;
            Sound.Pause();
        }

        public void Resume()
        {
            if (Sound == null || Sound.State != SoundState.Paused)
                return;
            Sound.Resume();
        }

        public void Update()
        {
            if (Sound == null)
                return;
            var vector2 = Main.screenPosition +
                          new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
            var num1 = 1f;
            if (!IsGlobal)
            {
                Sound.Pan =
                    MathHelper.Clamp(
                        (float) ((Position.X - (double) vector2.X) / (Main.screenWidth * 0.5)),
                        -1f, 1f);
                num1 = (float) (1.0 - Vector2.Distance(Position, vector2) /
                                (Main.screenWidth * 1.5));
            }

            var num2 = num1 * (Style.Volume * Volume);
            switch (Style.Type)
            {
                case SoundType.Sound:
                    num2 *= Main.soundVolume;
                    break;
                case SoundType.Ambient:
                    num2 *= Main.ambientVolume;
                    break;
                case SoundType.Music:
                    num2 *= Main.musicVolume;
                    break;
            }

            Sound.Volume = MathHelper.Clamp(num2, 0.0f, 1f);
        }
    }
}