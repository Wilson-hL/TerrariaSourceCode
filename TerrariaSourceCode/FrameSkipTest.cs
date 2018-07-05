﻿// Decompiled with JetBrains decompiler
// Type: Terraria.FrameSkipTest
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using System.Collections.Generic;
using System.Threading;

namespace Terraria
{
    public class FrameSkipTest
    {
        private static float CallsThisSecond = 0.0f;
        private static float DeltasThisSecond = 0.0f;
        private static List<float> DeltaSamples = new List<float>();
        private static MultiTimer serverFramerateTest = new MultiTimer(60);
        private const int SamplesCount = 5;
        private static int LastRecordedSecondNumber;

        public static void Update(GameTime gameTime)
        {
            Thread.Sleep((int) MathHelper.Clamp((1f / 60f - (float) gameTime.ElapsedGameTime.TotalSeconds) * 1000f + 1f,
                0.0f, 1000f));
        }

        public static void CheckReset(GameTime gameTime)
        {
            if (FrameSkipTest.LastRecordedSecondNumber == gameTime.TotalGameTime.Seconds)
                return;
            FrameSkipTest.DeltaSamples.Add(FrameSkipTest.DeltasThisSecond / FrameSkipTest.CallsThisSecond);
            if (FrameSkipTest.DeltaSamples.Count > 5)
                FrameSkipTest.DeltaSamples.RemoveAt(0);
            FrameSkipTest.CallsThisSecond = 0.0f;
            FrameSkipTest.DeltasThisSecond = 0.0f;
            FrameSkipTest.LastRecordedSecondNumber = gameTime.TotalGameTime.Seconds;
        }

        public static void UpdateServerTest()
        {
            FrameSkipTest.serverFramerateTest.Record("frame time");
            FrameSkipTest.serverFramerateTest.StopAndPrint();
            FrameSkipTest.serverFramerateTest.Start();
        }
    }
}