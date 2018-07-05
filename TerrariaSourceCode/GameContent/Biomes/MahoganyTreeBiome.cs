// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Biomes.MahoganyTreeBiome
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.World.Generation;

namespace Terraria.GameContent.Biomes
{
    public class MahoganyTreeBiome : MicroBiome
    {
        public override bool Place(Point origin, StructureMap structures)
        {
            Point result1;
            if (!WorldUtils.Find(new Point(origin.X - 3, origin.Y),
                Searches.Chain(new Searches.Down(200), new Conditions.IsSolid().AreaAnd(6, 1)),
                out result1))
                return false;
            Point result2;
            if (!WorldUtils.Find(new Point(result1.X, result1.Y - 5),
                    Searches.Chain(new Searches.Up(120), new Conditions.IsSolid().AreaOr(6, 1)),
                    out result2) || result1.Y - 5 - result2.Y > 60 || result1.Y - result2.Y < 30 ||
                !structures.CanPlace(new Rectangle(result1.X - 30, result1.Y - 60, 60, 90),
                    0))
                return false;
            var resultsOutput = new Dictionary<ushort, int>();
            WorldUtils.Gen(new Point(result1.X - 25, result1.Y - 25), new Shapes.Rectangle(50, 50),
                new Actions.TileScanner((ushort) 0, (ushort) 59, (ushort) 147, (ushort) 1).Output(resultsOutput));
            var num1 = resultsOutput[0] + resultsOutput[1];
            var num2 = resultsOutput[59];
            if (resultsOutput[147] > num2 || num1 > num2 || num2 < 50)
                return false;
            var num3 = (result1.Y - result2.Y - 9) / 5;
            var num4 = num3 * 5;
            var num5 = 0;
            var num6 = _random.NextDouble() + 1.0;
            var num7 = _random.NextDouble() + 2.0;
            if (_random.Next(2) == 0)
                num7 = -num7;
            for (var index = 0; index < num3; ++index)
            {
                var num8 = (int) (Math.Sin((index + 1) / 12.0 * num6 * 3.14159274101257) * num7);
                var num9 = num8 < num5 ? num8 - num5 : 0;
                WorldUtils.Gen(new Point(result1.X + num5 + num9, result1.Y - (index + 1) * 5),
                    new Shapes.Rectangle(6 + Math.Abs(num8 - num5), 7),
                    Actions.Chain((GenAction) new Actions.RemoveWall(),
                        (GenAction) new Actions.SetTile(383, false, true),
                        (GenAction) new Actions.SetFrames(false)));
                WorldUtils.Gen(new Point(result1.X + num5 + num9 + 2, result1.Y - (index + 1) * 5),
                    new Shapes.Rectangle(2 + Math.Abs(num8 - num5), 5),
                    Actions.Chain((GenAction) new Actions.ClearTile(true),
                        (GenAction) new Actions.PlaceWall(78, true)));
                WorldUtils.Gen(new Point(result1.X + num5 + 2, result1.Y - index * 5),
                    new Shapes.Rectangle(2, 2),
                    Actions.Chain((GenAction) new Actions.ClearTile(true),
                        (GenAction) new Actions.PlaceWall(78, true)));
                num5 = num8;
            }

            var num10 = 6;
            if (num7 < 0.0)
                num10 = 0;
            var endpoints = new List<Point>();
            for (var index = 0; index < 2; ++index)
            {
                var num8 = (index + 1.0) / 3.0;
                var num9 = num10 + (int) (Math.Sin(num3 * num8 / 12.0 * num6 * 3.14159274101257) * num7);
                var angle = _random.NextDouble() * 0.785398185253143 - 0.785398185253143 - 0.200000002980232;
                if (num10 == 0)
                    angle -= 1.57079637050629;
                WorldUtils.Gen(new Point(result1.X + num9, result1.Y - (int) (num3 * 5 * num8)),
                    new ShapeBranch(angle, _random.Next(12, 16)).OutputEndpoints(endpoints),
                    Actions.Chain((GenAction) new Actions.SetTile(383, false, true),
                        (GenAction) new Actions.SetFrames(true)));
                num10 = 6 - num10;
            }

            var num11 = (int) (Math.Sin(num3 / 12.0 * num6 * 3.14159274101257) * num7);
            WorldUtils.Gen(new Point(result1.X + 6 + num11, result1.Y - num4),
                new ShapeBranch(-0.685398185253143, _random.Next(16, 22))
                    .OutputEndpoints(endpoints),
                Actions.Chain((GenAction) new Actions.SetTile(383, false, true),
                    (GenAction) new Actions.SetFrames(true)));
            WorldUtils.Gen(new Point(result1.X + num11, result1.Y - num4),
                new ShapeBranch(-2.45619455575943, _random.Next(16, 22))
                    .OutputEndpoints(endpoints),
                Actions.Chain((GenAction) new Actions.SetTile(383, false, true),
                    (GenAction) new Actions.SetFrames(true)));
            foreach (var origin1 in endpoints)
            {
                var circle = new Shapes.Circle(4);
                var action = Actions.Chain((GenAction) new Modifiers.Blotches(4, 2, 0.3),
                    (GenAction) new Modifiers.SkipTiles((ushort) 383),
                    (GenAction) new Modifiers.SkipWalls((byte) 78),
                    (GenAction) new Actions.SetTile(384, false, true),
                    (GenAction) new Actions.SetFrames(true));
                WorldUtils.Gen(origin1, circle, action);
            }

            for (var index = 0; index < 4; ++index)
            {
                var angle = (float) (index / 3.0 * 2.0 + 0.570749998092651);
                WorldUtils.Gen(result1, new ShapeRoot(angle, _random.Next(40, 60), 4f, 1f),
                    new Actions.SetTile(383, true, true));
            }

            WorldGen.AddBuriedChest(result1.X + 3, result1.Y - 1,
                _random.Next(4) == 0 ? 0 : WorldGen.GetNextJungleChestItem(), false, 10);
            structures.AddStructure(new Rectangle(result1.X - 30, result1.Y - 30, 60, 60), 0);
            return true;
        }
    }
}