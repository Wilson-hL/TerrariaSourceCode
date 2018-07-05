﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Generation.ActionGrass
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Terraria.World.Generation;

namespace Terraria.GameContent.Generation
{
    public class ActionGrass : GenAction
    {
        public override bool Apply(Point origin, int x, int y, params object[] args)
        {
            if (GenBase._tiles[x, y].active() || GenBase._tiles[x, y - 1].active())
                return false;
            WorldGen.PlaceTile(x, y, (int) Utils.SelectRandom<ushort>(GenBase._random, new ushort[2]
            {
                (ushort) 3,
                (ushort) 73
            }), true, false, -1, 0);
            return this.UnitApply(origin, x, y, args);
        }
    }
}