// Decompiled with JetBrains decompiler
// Type: Terraria.Graphics.Capture.CaptureSettings
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using System;

namespace Terraria.Graphics.Capture
{
    public class CaptureSettings
    {
        public bool UseScaling = true;
        public bool CaptureEntities = true;
        public CaptureBiome Biome = CaptureBiome.Biomes[0];
        public Rectangle Area;
        public string OutputName;
        public bool CaptureMech;
        public bool CaptureBackground;

        public CaptureSettings()
        {
            var localTime = DateTime.Now.ToLocalTime();
            this.OutputName = "Capture " + localTime.Year.ToString("D4") + "-" + localTime.Month.ToString("D2") + "-" +
                              localTime.Day.ToString("D2") + " " + localTime.Hour.ToString("D2") + "_" +
                              localTime.Minute.ToString("D2") + "_" + localTime.Second.ToString("D2");
        }
    }
}