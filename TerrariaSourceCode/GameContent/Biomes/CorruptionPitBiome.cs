// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Biomes.CorruptionPitBiome
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.World.Generation;

namespace Terraria.GameContent.Biomes
{
    public class CorruptionPitBiome : MicroBiome
    {
        public static bool[] ValidTiles = TileID.Sets.Factory.CreateBoolSet(true, 21, 31, 26);

        public override bool Place(Point origin, StructureMap structures)
        {
            if (WorldGen.SolidTile(origin.X, origin.Y) && _tiles[origin.X, origin.Y].wall == 3)
                return false;
            if (!WorldUtils.Find(origin,
                Searches.Chain(new Searches.Down(100), (GenCondition) new Conditions.IsSolid()),
                out origin))
                return false;
            Point result;
            if (!WorldUtils.Find(new Point(origin.X - 4, origin.Y),
                Searches.Chain(new Searches.Down(5),
                    new Conditions.IsTile((ushort) 25).AreaAnd(8, 1)), out result))
                return false;
            var data1 = new ShapeData();
            var shapeData1 = new ShapeData();
            var shapeData2 = new ShapeData();
            for (var index = 0; index < 6; ++index)
                WorldUtils.Gen(origin, new Shapes.Circle(_random.Next(10, 12) + index),
                    Actions.Chain((GenAction) new Modifiers.Offset(0, 5 * index + 5),
                        new Modifiers.Blotches(3, 0.3).Output(data1)));
            for (var index = 0; index < 6; ++index)
                WorldUtils.Gen(origin, new Shapes.Circle(_random.Next(5, 7) + index),
                    Actions.Chain((GenAction) new Modifiers.Offset(0, 2 * index + 18),
                        new Modifiers.Blotches(3, 0.3).Output(shapeData1)));
            for (var index = 0; index < 6; ++index)
                WorldUtils.Gen(origin, new Shapes.Circle(_random.Next(4, 6) + index / 2),
                    Actions.Chain((GenAction) new Modifiers.Offset(0, (int) (7.5 * index) - 10),
                        new Modifiers.Blotches(3, 0.3).Output(shapeData2)));
            var data2 = new ShapeData(shapeData1);
            shapeData1.Subtract(shapeData2, origin, origin);
            data2.Subtract(shapeData1, origin, origin);
            var bounds = ShapeData.GetBounds(origin, data1, shapeData2);
            if (!structures.CanPlace(bounds, ValidTiles, 2))
                return false;
            WorldUtils.Gen(origin, new ModShapes.All(data1),
                Actions.Chain((GenAction) new Actions.SetTile(25, true, true),
                    (GenAction) new Actions.PlaceWall(3, true)));
            WorldUtils.Gen(origin, new ModShapes.All(shapeData1),
                new Actions.SetTile(0, true, true));
            WorldUtils.Gen(origin, new ModShapes.All(shapeData2), new Actions.ClearTile(true));
            WorldUtils.Gen(origin, new ModShapes.All(shapeData1), Actions.Chain(
                (GenAction) new Modifiers.IsTouchingAir(true),
                (GenAction) new Modifiers.NotTouching(false, (ushort) 25),
                (GenAction) new Actions.SetTile(23, true, true)));
            WorldUtils.Gen(origin, new ModShapes.All(data2),
                new Actions.PlaceWall(69, true));
            structures.AddStructure(bounds, 2);
            return true;
        }
    }
}