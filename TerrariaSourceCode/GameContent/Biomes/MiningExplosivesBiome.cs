// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Biomes.MiningExplosivesBiome
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.World.Generation;

namespace Terraria.GameContent.Biomes
{
    public class MiningExplosivesBiome : MicroBiome
    {
        public override bool Place(Point origin, StructureMap structures)
        {
            if (WorldGen.SolidTile(origin.X, origin.Y))
                return false;
            var type = Utils.SelectRandom(_random, WorldGen.goldBar == 19 ? (ushort) 8 : (ushort) 169,
                WorldGen.silverBar == 21 ? (ushort) 9 : (ushort) 168,
                WorldGen.ironBar == 22 ? (ushort) 6 : (ushort) 167,
                WorldGen.copperBar == 20 ? (ushort) 7 : (ushort) 166);
            var num1 = _random.NextDouble() * 2.0 - 1.0;
            if (!WorldUtils.Find(origin,
                Searches.Chain(num1 > 0.0 ? new Searches.Right(40) : (GenSearch) new Searches.Left(40),
                    (GenCondition) new Conditions.IsSolid()), out origin))
                return false;
            if (!WorldUtils.Find(origin,
                Searches.Chain(new Searches.Down(80), (GenCondition) new Conditions.IsSolid()), out origin))
                return false;
            var shapeData = new ShapeData();
            var count1 = new Ref<int>(0);
            var count2 = new Ref<int>(0);
            WorldUtils.Gen(origin, new ShapeRunner(10f, 20, new Vector2((float) num1, 1f)).Output(shapeData),
                Actions.Chain((GenAction) new Modifiers.Blotches(2, 0.3), (GenAction) new Actions.Scanner(count1),
                    (GenAction) new Modifiers.IsSolid(), (GenAction) new Actions.Scanner(count2)));
            if (count2.Value < count1.Value / 2)
                return false;
            var area =
                new Rectangle(origin.X - 15, origin.Y - 10, 30, 20);
            if (!structures.CanPlace(area, 0))
                return false;
            WorldUtils.Gen(origin, new ModShapes.All(shapeData),
                new Actions.SetTile(type, true, true));
            WorldUtils.Gen(new Point(origin.X - (int) (num1 * -5.0), origin.Y - 5), new Shapes.Circle(5),
                Actions.Chain((GenAction) new Modifiers.Blotches(2, 0.3), (GenAction) new Actions.ClearTile(true)));
            Point result1;
            var flag = ((true ? 1 : 0) & (WorldUtils.Find(new Point(origin.X - (num1 > 0.0 ? 3 : -3), origin.Y - 3),
                            Searches.Chain(new Searches.Down(10), (GenCondition) new Conditions.IsSolid()),
                            out result1)
                            ? 1
                            : 0)) != 0;
            var num2 = _random.Next(4) == 0 ? 3 : 7;
            Point result2;
            if (((flag ? 1 : 0) & (WorldUtils.Find(new Point(origin.X - (num1 > 0.0 ? -num2 : num2), origin.Y - 3),
                     Searches.Chain(new Searches.Down(10), (GenCondition) new Conditions.IsSolid()),
                     out result2)
                     ? 1
                     : 0)) == 0)
                return false;
            --result1.Y;
            --result2.Y;
            var tile1 = _tiles[result1.X, result1.Y + 1];
            tile1.slope(0);
            tile1.halfBrick(false);
            for (var index = -1; index <= 1; ++index)
            {
                WorldUtils.ClearTile(result2.X + index, result2.Y, false);
                var tile2 = _tiles[result2.X + index, result2.Y + 1];
                if (!WorldGen.SolidOrSlopedTile(tile2))
                {
                    tile2.ResetToType(1);
                    tile2.active(true);
                }

                tile2.slope(0);
                tile2.halfBrick(false);
                WorldUtils.TileFrame(result2.X + index, result2.Y + 1, true);
            }

            WorldGen.PlaceTile(result1.X, result1.Y, 141, false, false, -1, 0);
            WorldGen.PlaceTile(result2.X, result2.Y, 411, true, true, -1, 0);
            WorldUtils.WireLine(result1, result2);
            structures.AddStructure(area, 5);
            return true;
        }
    }
}