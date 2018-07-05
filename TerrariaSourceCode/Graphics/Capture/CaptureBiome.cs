﻿// Decompiled with JetBrains decompiler
// Type: Terraria.Graphics.Capture.CaptureBiome
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

namespace Terraria.Graphics.Capture
{
    public class CaptureBiome
    {
        public static CaptureBiome[] Biomes = new CaptureBiome[12]
        {
            new CaptureBiome(0, 0, 0, CaptureBiome.TileColorStyle.Normal),
            null,
            new CaptureBiome(1, 2, 2, CaptureBiome.TileColorStyle.Corrupt),
            new CaptureBiome(3, 0, 3, CaptureBiome.TileColorStyle.Jungle),
            new CaptureBiome(6, 2, 4, CaptureBiome.TileColorStyle.Normal),
            new CaptureBiome(7, 4, 5, CaptureBiome.TileColorStyle.Normal),
            new CaptureBiome(2, 1, 6, CaptureBiome.TileColorStyle.Normal),
            new CaptureBiome(9, 6, 7, CaptureBiome.TileColorStyle.Mushroom),
            new CaptureBiome(0, 0, 8, CaptureBiome.TileColorStyle.Normal),
            null,
            new CaptureBiome(8, 5, 10, CaptureBiome.TileColorStyle.Crimson),
            null
        };

        public readonly int WaterStyle;
        public readonly int BackgroundIndex;
        public readonly int BackgroundIndex2;
        public readonly CaptureBiome.TileColorStyle TileColor;

        public CaptureBiome(int backgroundIndex, int backgroundIndex2, int waterStyle,
            CaptureBiome.TileColorStyle tileColorStyle = CaptureBiome.TileColorStyle.Normal)
        {
            this.BackgroundIndex = backgroundIndex;
            this.BackgroundIndex2 = backgroundIndex2;
            this.WaterStyle = waterStyle;
            this.TileColor = tileColorStyle;
        }

        public enum TileColorStyle
        {
            Normal,
            Jungle,
            Crimson,
            Corrupt,
            Mushroom,
        }
    }
}