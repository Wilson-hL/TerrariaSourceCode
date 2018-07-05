// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Biomes.EnchantedSwordBiome
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.World.Generation;

namespace Terraria.GameContent.Biomes
{
    public class EnchantedSwordBiome : MicroBiome
    {
        public override bool Place(Point origin, StructureMap structures)
        {
            var resultsOutput = new Dictionary<ushort, int>();
            WorldUtils.Gen(new Point(origin.X - 25, origin.Y - 25), new Shapes.Rectangle(50, 50),
                new Actions.TileScanner((ushort) 0, (ushort) 1).Output(resultsOutput));
            if (resultsOutput[0] + resultsOutput[1] < 1250)
                return false;
            Point result1;
            var flag = WorldUtils.Find(origin,
                Searches.Chain(new Searches.Up(1000), new Conditions.IsSolid().AreaOr(1, 50).Not()),
                out result1);
            Point result2;
            if (WorldUtils.Find(origin,
                    Searches.Chain(new Searches.Up(origin.Y - result1.Y),
                        (GenCondition) new Conditions.IsTile((ushort) 53)), out result2) || !flag)
                return false;
            result1.Y += 50;
            var data1 = new ShapeData();
            var shapeData = new ShapeData();
            var point1 = new Point(origin.X, origin.Y + 20);
            var point2 = new Point(origin.X, origin.Y + 30);
            var xScale = (float) (0.800000011920929 + _random.NextFloat() * 0.5);
            if (!structures.CanPlace(
                    new Rectangle(point1.X - (int) (20.0 * xScale), point1.Y - 20,
                        (int) (40.0 * xScale), 40), 0) || !structures.CanPlace(
                    new Rectangle(origin.X, result1.Y + 10, 1, origin.Y - result1.Y - 9), 2))
                return false;
            WorldUtils.Gen(point1, new Shapes.Slime(20, xScale, 1f),
                Actions.Chain((GenAction) new Modifiers.Blotches(2, 0.4), new Actions.ClearTile(true).Output(data1)));
            WorldUtils.Gen(point2, new Shapes.Mound(14, 14),
                Actions.Chain((GenAction) new Modifiers.Blotches(2, 1, 0.8),
                    (GenAction) new Actions.SetTile(0, false, true),
                    new Actions.SetFrames(true).Output(shapeData)));
            data1.Subtract(shapeData, point1, point2);
            WorldUtils.Gen(point1, new ModShapes.InnerOutline(data1, true),
                Actions.Chain((GenAction) new Actions.SetTile(2, false, true),
                    (GenAction) new Actions.SetFrames(true)));
            WorldUtils.Gen(point1, new ModShapes.All(data1),
                Actions.Chain((GenAction) new Modifiers.RectangleMask(-40, 40, 0, 40),
                    (GenAction) new Modifiers.IsEmpty(), (GenAction) new Actions.SetLiquid(0, byte.MaxValue)));
            WorldUtils.Gen(point1, new ModShapes.All(data1), Actions.Chain(
                (GenAction) new Actions.PlaceWall(68, true), (GenAction) new Modifiers.OnlyTiles((ushort) 2),
                (GenAction) new Modifiers.Offset(0, 1), (GenAction) new ActionVines(3, 5, 52)));
            var data2 = new ShapeData();
            WorldUtils.Gen(new Point(origin.X, result1.Y + 10),
                new Shapes.Rectangle(1, origin.Y - result1.Y - 9), Actions.Chain(
                    (GenAction) new Modifiers.Blotches(2, 0.2), new Actions.ClearTile(false).Output(data2),
                    (GenAction) new Modifiers.Expand(1), (GenAction) new Modifiers.OnlyTiles((ushort) 53),
                    new Actions.SetTile(397, false, true).Output(data2)));
            WorldUtils.Gen(new Point(origin.X, result1.Y + 10), new ModShapes.All(data2),
                new Actions.SetFrames(true));
            if (_random.Next(3) == 0)
                WorldGen.PlaceTile(point2.X, point2.Y - 15, 187, true, false, -1, 17);
            else
                WorldGen.PlaceTile(point2.X, point2.Y - 15, 186, true, false, -1, 15);
            WorldUtils.Gen(point2, new ModShapes.All(shapeData), Actions.Chain(
                (GenAction) new Modifiers.Offset(0, -1), (GenAction) new Modifiers.OnlyTiles((ushort) 2),
                (GenAction) new Modifiers.Offset(0, -1), (GenAction) new ActionGrass()));
            structures.AddStructure(
                new Rectangle(point1.X - (int) (20.0 * xScale), point1.Y - 20,
                    (int) (40.0 * xScale), 40), 4);
            return true;
        }
    }
}