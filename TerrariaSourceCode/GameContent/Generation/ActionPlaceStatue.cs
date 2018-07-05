﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Generation.ActionPlaceStatue
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.World.Generation;

namespace Terraria.GameContent.Generation
{
    public class ActionPlaceStatue : GenAction
    {
        private int _statueIndex;

        public ActionPlaceStatue(int index = -1)
        {
            this._statueIndex = index;
        }

        public override bool Apply(Point origin, int x, int y, params object[] args)
        {
            var point16 = this._statueIndex != -1
                ? WorldGen.statueList[this._statueIndex]
                : WorldGen.statueList[GenBase._random.Next(2, WorldGen.statueList.Length)];
            WorldGen.PlaceTile(x, y, (int) point16.X, true, false, -1, (int) point16.Y);
            return this.UnitApply(origin, x, y, args);
        }
    }
}