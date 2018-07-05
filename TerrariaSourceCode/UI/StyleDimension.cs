﻿// Decompiled with JetBrains decompiler
// Type: Terraria.UI.StyleDimension
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

namespace Terraria.UI
{
    public struct StyleDimension
    {
        public static StyleDimension Fill = new StyleDimension(0.0f, 1f);
        public static StyleDimension Empty = new StyleDimension(0.0f, 0.0f);
        public float Pixels;
        public float Precent;

        public StyleDimension(float pixels, float precent)
        {
            this.Pixels = pixels;
            this.Precent = precent;
        }

        public void Set(float pixels, float precent)
        {
            this.Pixels = pixels;
            this.Precent = precent;
        }

        public float GetValue(float containerSize)
        {
            return this.Pixels + this.Precent * containerSize;
        }
    }
}