﻿// Decompiled with JetBrains decompiler
// Type: Terraria.World.Generation.WorldGenerator
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System.Collections.Generic;
using System.Diagnostics;
using Terraria.GameContent.UI.States;
using Terraria.UI;
using Terraria.Utilities;

namespace Terraria.World.Generation
{
    public class WorldGenerator
    {
        private List<GenPass> _passes = new List<GenPass>();
        private float _totalLoadWeight;
        private int _seed;

        public WorldGenerator(int seed)
        {
            this._seed = seed;
        }

        public void Append(GenPass pass)
        {
            this._passes.Add(pass);
            this._totalLoadWeight += pass.Weight;
        }

        public void GenerateWorld(GenerationProgress progress = null)
        {
            var stopwatch = new Stopwatch();
            var num = 0.0f;
            foreach (var pass in this._passes)
                num += pass.Weight;
            if (progress == null)
                progress = new GenerationProgress();
            progress.TotalWeight = num;
            Main.menuMode = 888;
            Main.MenuUI.SetState((UIState) new UIWorldLoad(progress));
            foreach (var pass in this._passes)
            {
                WorldGen._genRand = new UnifiedRandom(this._seed);
                Main.rand = new UnifiedRandom(this._seed);
                stopwatch.Start();
                progress.Start(pass.Weight);
                pass.Apply(progress);
                progress.End();
                stopwatch.Reset();
            }
        }
    }
}