// Decompiled with JetBrains decompiler
// Type: Terraria.DataStructures.ColorSlidersSet
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;

namespace Terraria.DataStructures
{
    public class ColorSlidersSet
    {
        public float Alpha = 1f;
        public float Hue;
        public float Luminance;
        public float Saturation;

        public void SetHSL(Color color)
        {
            var hsl = Main.rgbToHsl(color);
            Hue = hsl.X;
            Saturation = hsl.Y;
            Luminance = hsl.Z;
        }

        public void SetHSL(Vector3 vector)
        {
            Hue = vector.X;
            Saturation = vector.Y;
            Luminance = vector.Z;
        }

        public Color GetColor()
        {
            var rgb = Main.hslToRgb(Hue, Saturation, Luminance);
            rgb.A = (byte) (Alpha * (double) byte.MaxValue);
            return rgb;
        }

        public Vector3 GetHSLVector()
        {
            return new Vector3(Hue, Saturation, Luminance);
        }

        public void ApplyToMainLegacyBars()
        {
            Main.hBar = Hue;
            Main.sBar = Saturation;
            Main.lBar = Luminance;
            Main.aBar = Alpha;
        }
    }
}