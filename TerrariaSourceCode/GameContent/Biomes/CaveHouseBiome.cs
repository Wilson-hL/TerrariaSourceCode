﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Biomes.CaveHouseBiome
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.World.Generation;

namespace Terraria.GameContent.Biomes
{
    public class CaveHouseBiome : MicroBiome
    {
        private const int VERTICAL_EXIT_WIDTH = 3;

        private static readonly bool[] _blacklistedTiles =
            TileID.Sets.Factory.CreateBoolSet(true, 225, 41, 43, 44, 226, 203, 112, 25, 151);

        private int _extractinatorCount;
        private int _sharpenerCount;

        private Rectangle GetRoom(Point origin)
        {
            Point result1;
            var flag1 = WorldUtils.Find(origin,
                Searches.Chain(new Searches.Left(25), (GenCondition) new Conditions.IsSolid()),
                out result1);
            Point result2;
            var flag2 = WorldUtils.Find(origin,
                Searches.Chain(new Searches.Right(25), (GenCondition) new Conditions.IsSolid()),
                out result2);
            if (!flag1)
                result1 = new Point(origin.X - 25, origin.Y);
            if (!flag2)
                result2 = new Point(origin.X + 25, origin.Y);
            var rectangle =
                new Rectangle(origin.X, origin.Y, 0, 0);
            if (origin.X - result1.X > result2.X - origin.X)
            {
                rectangle.X = result1.X;
                rectangle.Width = Utils.Clamp(result2.X - result1.X, 15, 30);
            }
            else
            {
                rectangle.Width = Utils.Clamp(result2.X - result1.X, 15, 30);
                rectangle.X = result2.X - rectangle.Width;
            }

            Point result3;
            var flag3 = WorldUtils.Find(result1,
                Searches.Chain(new Searches.Up(10), (GenCondition) new Conditions.IsSolid()), out result3);
            Point result4;
            var flag4 = WorldUtils.Find(result2,
                Searches.Chain(new Searches.Up(10), (GenCondition) new Conditions.IsSolid()), out result4);
            if (!flag3)
                result3 = new Point(origin.X, origin.Y - 10);
            if (!flag4)
                result4 = new Point(origin.X, origin.Y - 10);
            rectangle.Height = Utils.Clamp(Math.Max(origin.Y - result3.Y, origin.Y - result4.Y), 8, 12);
            rectangle.Y -= rectangle.Height;
            return rectangle;
        }

        private float RoomSolidPrecentage(Rectangle room)
        {
            var num = room.Width * room.Height;
            var count = new Ref<int>(0);
            WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height),
                Actions.Chain((GenAction) new Modifiers.IsSolid(), (GenAction) new Actions.Count(count)));
            return (float) count.Value / num;
        }

        private bool FindVerticalExit(Rectangle wall, bool isUp, out int exitX)
        {
            Point result;
            var flag = WorldUtils.Find(new Point(wall.X + wall.Width - 3, wall.Y + (isUp ? -5 : 0)),
                Searches.Chain(new Searches.Left(wall.Width - 3),
                    new Conditions.IsSolid().Not().AreaOr(3, 5)), out result);
            exitX = result.X;
            return flag;
        }

        private bool FindSideExit(Rectangle wall, bool isLeft, out int exitY)
        {
            Point result;
            var flag = WorldUtils.Find(new Point(wall.X + (isLeft ? -4 : 0), wall.Y + wall.Height - 3),
                Searches.Chain(new Searches.Up(wall.Height - 3),
                    new Conditions.IsSolid().Not().AreaOr(4, 3)), out result);
            exitY = result.Y;
            return flag;
        }

        private int SortBiomeResults(Tuple<BuildData, int> item1,
            Tuple<BuildData, int> item2)
        {
            return item2.Item2.CompareTo(item1.Item2);
        }

        public override bool Place(Point origin, StructureMap structures)
        {
            Point result1;
            if (!WorldUtils.Find(origin,
                    Searches.Chain(new Searches.Down(200), (GenCondition) new Conditions.IsSolid()),
                    out result1) || result1 == origin)
                return false;
            var room1 = GetRoom(result1);
            var room2 = GetRoom(new Point(room1.Center.X, room1.Y + 1));
            var room3 =
                GetRoom(new Point(room1.Center.X, room1.Y + room1.Height + 10));
            room3.Y = room1.Y + room1.Height - 1;
            var num1 = RoomSolidPrecentage(room2);
            var num2 = RoomSolidPrecentage(room3);
            room1.Y += 3;
            room2.Y += 3;
            room3.Y += 3;
            var rectangleList1 = new List<Rectangle>();
            if (_random.NextFloat() > num1 + 0.200000002980232)
                rectangleList1.Add(room2);
            else
                room2 = room1;
            rectangleList1.Add(room1);
            if (_random.NextFloat() > num2 + 0.200000002980232)
                rectangleList1.Add(room3);
            else
                room3 = room1;
            foreach (var rectangle in rectangleList1)
                if (rectangle.Y + rectangle.Height > Main.maxTilesY - 220)
                    return false;

            var resultsOutput = new Dictionary<ushort, int>();
            foreach (var rectangle in rectangleList1)
                WorldUtils.Gen(new Point(rectangle.X - 10, rectangle.Y - 10),
                    new Shapes.Rectangle(rectangle.Width + 20, rectangle.Height + 20),
                    new Actions.TileScanner((ushort) 0, (ushort) 59, (ushort) 147, (ushort) 1, (ushort) 161,
                            (ushort) 53, (ushort) 396, (ushort) 397, (ushort) 368, (ushort) 367, (ushort) 60,
                            (ushort) 70)
                        .Output(resultsOutput));
            var tupleList1 = new List<Tuple<BuildData, int>>();
            tupleList1.Add(Tuple.Create(BuildData.Default,
                resultsOutput[0] + resultsOutput[1]));
            tupleList1.Add(Tuple.Create(BuildData.Jungle,
                resultsOutput[59] + resultsOutput[60] * 10));
            tupleList1.Add(Tuple.Create(BuildData.Mushroom,
                resultsOutput[59] + resultsOutput[70] * 10));
            tupleList1.Add(Tuple.Create(BuildData.Snow,
                resultsOutput[147] + resultsOutput[161]));
            tupleList1.Add(Tuple.Create(BuildData.Desert,
                resultsOutput[397] + resultsOutput[396] + resultsOutput[53]));
            tupleList1.Add(Tuple.Create(BuildData.Granite,
                resultsOutput[368]));
            tupleList1.Add(Tuple.Create(BuildData.Marble,
                resultsOutput[367]));
            tupleList1.Sort(SortBiomeResults);
            var buildData = tupleList1[0].Item1;
            foreach (var area in rectangleList1)
            {
                if (buildData != BuildData.Granite)
                {
                    Point result2;
                    if (WorldUtils.Find(new Point(area.X - 2, area.Y - 2),
                        Searches.Chain(new Searches.Rectangle(area.Width + 4, area.Height + 4).RequireAll(false),
                            (GenCondition) new Conditions.HasLava()), out result2))
                        return false;
                }

                if (!structures.CanPlace(area, _blacklistedTiles, 5))
                    return false;
            }

            var val1_1 = room1.X;
            var val1_2 = room1.X + room1.Width - 1;
            var rectangleList2 = new List<Rectangle>();
            foreach (var rectangle in rectangleList1)
            {
                val1_1 = Math.Min(val1_1, rectangle.X);
                val1_2 = Math.Max(val1_2, rectangle.X + rectangle.Width - 1);
            }

            var num3 = 6;
            while (num3 > 4 && (val1_2 - val1_1) % num3 != 0)
                --num3;
            var x1 = val1_1;
            while (x1 <= val1_2)
            {
                for (var index1 = 0; index1 < rectangleList1.Count; ++index1)
                {
                    var rectangle = rectangleList1[index1];
                    if (x1 >= rectangle.X && x1 < rectangle.X + rectangle.Width)
                    {
                        var y = rectangle.Y + rectangle.Height;
                        var num4 = 50;
                        for (var index2 = index1 + 1; index2 < rectangleList1.Count; ++index2)
                            if (x1 >= rectangleList1[index2].X &&
                                x1 < rectangleList1[index2].X + rectangleList1[index2].Width)
                                num4 = Math.Min(num4, rectangleList1[index2].Y - y);

                        if (num4 > 0)
                        {
                            Point result2;
                            var flag = WorldUtils.Find(new Point(x1, y),
                                Searches.Chain(new Searches.Down(num4),
                                    (GenCondition) new Conditions.IsSolid()), out result2);
                            if (num4 < 50)
                            {
                                flag = true;
                                result2 = new Point(x1, y + num4);
                            }

                            if (flag)
                                rectangleList2.Add(new Rectangle(x1, y, 1, result2.Y - y));
                        }
                    }
                }

                x1 += num3;
            }

            var pointList1 = new List<Point>();
            foreach (var rectangle in rectangleList1)
            {
                int exitY;
                if (FindSideExit(
                    new Rectangle(rectangle.X + rectangle.Width, rectangle.Y + 1, 1,
                        rectangle.Height - 2), false, out exitY))
                    pointList1.Add(new Point(rectangle.X + rectangle.Width - 1, exitY));
                if (FindSideExit(
                    new Rectangle(rectangle.X, rectangle.Y + 1, 1, rectangle.Height - 2), true,
                    out exitY))
                    pointList1.Add(new Point(rectangle.X, exitY));
            }

            var tupleList2 = new List<Tuple<Point, Point>>();
            for (var index = 1; index < rectangleList1.Count; ++index)
            {
                var rectangle1 = rectangleList1[index];
                var rectangle2 = rectangleList1[index - 1];
                if (rectangle2.X - rectangle1.X > rectangle1.X + rectangle1.Width - (rectangle2.X + rectangle2.Width))
                    tupleList2.Add(new Tuple<Point, Point>(
                        new Point(rectangle1.X + rectangle1.Width - 1, rectangle1.Y + 1),
                        new Point(rectangle1.X + rectangle1.Width - rectangle1.Height + 1,
                            rectangle1.Y + rectangle1.Height - 1)));
                else
                    tupleList2.Add(new Tuple<Point, Point>(new Point(rectangle1.X, rectangle1.Y + 1),
                        new Point(rectangle1.X + rectangle1.Height - 1, rectangle1.Y + rectangle1.Height - 1)));
            }

            var pointList2 = new List<Point>();
            int exitX;
            if (FindVerticalExit(new Rectangle(room2.X + 2, room2.Y, room2.Width - 4, 1),
                true, out exitX))
                pointList2.Add(new Point(exitX, room2.Y));
            if (FindVerticalExit(
                new Rectangle(room3.X + 2, room3.Y + room3.Height - 1, room3.Width - 4, 1),
                false, out exitX))
                pointList2.Add(new Point(exitX, room3.Y + room3.Height - 1));
            foreach (var area in rectangleList1)
            {
                WorldUtils.Gen(new Point(area.X, area.Y), new Shapes.Rectangle(area.Width, area.Height),
                    Actions.Chain((GenAction) new Actions.SetTile(buildData.Tile, false, true),
                        (GenAction) new Actions.SetFrames(true)));
                WorldUtils.Gen(new Point(area.X + 1, area.Y + 1),
                    new Shapes.Rectangle(area.Width - 2, area.Height - 2),
                    Actions.Chain((GenAction) new Actions.ClearTile(true),
                        (GenAction) new Actions.PlaceWall(buildData.Wall, true)));
                structures.AddStructure(area, 8);
            }

            foreach (var tuple in tupleList2)
            {
                var origin1 = tuple.Item1;
                var point = tuple.Item2;
                var num4 = point.X > origin1.X ? 1 : -1;
                var data = new ShapeData();
                for (var y = 0; y < point.Y - origin1.Y; ++y)
                    data.Add(num4 * (y + 1), y);
                WorldUtils.Gen(origin1, new ModShapes.All(data),
                    Actions.Chain((GenAction) new Actions.PlaceTile(19, buildData.PlatformStyle),
                        (GenAction) new Actions.SetSlope(num4 == 1 ? 1 : 2), (GenAction) new Actions.SetFrames(true)));
                WorldUtils.Gen(new Point(origin1.X + (num4 == 1 ? 1 : -4), origin1.Y - 1),
                    new Shapes.Rectangle(4, 1),
                    Actions.Chain((GenAction) new Actions.Clear(),
                        (GenAction) new Actions.PlaceWall(buildData.Wall, true),
                        (GenAction) new Actions.PlaceTile(19, buildData.PlatformStyle),
                        (GenAction) new Actions.SetFrames(true)));
            }

            foreach (var origin1 in pointList1)
            {
                WorldUtils.Gen(origin1, new Shapes.Rectangle(1, 3), new Actions.ClearTile(true));
                WorldGen.PlaceTile(origin1.X, origin1.Y, 10, true, true, -1, buildData.DoorStyle);
            }

            foreach (var origin1 in pointList2)
            {
                var rectangle = new Shapes.Rectangle(3, 1);
                var action = Actions.Chain((GenAction) new Actions.ClearMetadata(),
                    (GenAction) new Actions.PlaceTile(19, buildData.PlatformStyle),
                    (GenAction) new Actions.SetFrames(true));
                WorldUtils.Gen(origin1, rectangle, action);
            }

            foreach (var rectangle in rectangleList2)
                if (rectangle.Height > 1 && _tiles[rectangle.X, rectangle.Y - 1].type != 19)
                {
                    WorldUtils.Gen(new Point(rectangle.X, rectangle.Y),
                        new Shapes.Rectangle(rectangle.Width, rectangle.Height),
                        Actions.Chain((GenAction) new Actions.SetTile(124, false, true),
                            (GenAction) new Actions.SetFrames(true)));
                    var tile = _tiles[rectangle.X, rectangle.Y + rectangle.Height];
                    tile.slope(0);
                    tile.halfBrick(false);
                }

            var pointArray = new Point[7]
            {
                new Point(14, buildData.TableStyle), new Point(16, 0), new Point(18, buildData.WorkbenchStyle),
                new Point(86, 0), new Point(87, buildData.PianoStyle), new Point(94, 0),
                new Point(101, buildData.BookcaseStyle)
            };
            foreach (var rectangle in rectangleList1)
            {
                var num4 = rectangle.Width / 8;
                var num5 = rectangle.Width / (num4 + 1);
                var num6 = _random.Next(2);
                for (var index1 = 0; index1 < num4; ++index1)
                {
                    var num7 = (index1 + 1) * num5 + rectangle.X;
                    switch (index1 + num6 % 2)
                    {
                        case 0:
                            var num8 = rectangle.Y + Math.Min(rectangle.Height / 2, rectangle.Height - 5);
                            var vector2 = WorldGen.randHousePicture();
                            var x2 = (int) vector2.X;
                            var y = (int) vector2.Y;
                            if (!WorldGen.nearPicture(num7, num8))
                                WorldGen.PlaceTile(num7, num8, x2, true, false, -1, y);

                            break;
                        case 1:
                            var j = rectangle.Y + 1;
                            WorldGen.PlaceTile(num7, j, 34, true, false, -1, _random.Next(6));
                            for (var index2 = -1; index2 < 2; ++index2)
                            for (var index3 = 0; index3 < 3; ++index3)
                                _tiles[index2 + num7, index3 + j].frameX += 54;

                            break;
                    }
                }

                var num9 = rectangle.Width / 8 + 3;
                WorldGen.SetupStatueList();
                for (; num9 > 0; --num9)
                {
                    var num7 = _random.Next(rectangle.Width - 3) + 1 + rectangle.X;
                    var num8 = rectangle.Y + rectangle.Height - 2;
                    switch (_random.Next(4))
                    {
                        case 0:
                            WorldGen.PlaceSmallPile(num7, num8, _random.Next(31, 34), 1, 185);
                            break;
                        case 1:
                            WorldGen.PlaceTile(num7, num8, 186, true, false, -1, _random.Next(22, 26));
                            break;
                        case 2:
                            var index = _random.Next(2, WorldGen.statueList.Length);
                            WorldGen.PlaceTile(num7, num8, WorldGen.statueList[index].X, true, false, -1,
                                WorldGen.statueList[index].Y);
                            if (WorldGen.StatuesWithTraps.Contains(index)) WorldGen.PlaceStatueTrap(num7, num8);

                            break;
                        case 3:
                            var point = Utils.SelectRandom(_random, pointArray);
                            WorldGen.PlaceTile(num7, num8, point.X, true, false, -1, point.Y);
                            break;
                    }
                }
            }

            foreach (var room4 in rectangleList1)
                buildData.ProcessRoom(room4);
            var flag1 = false;
            foreach (var rectangle in rectangleList1)
            {
                var j = rectangle.Height - 1 + rectangle.Y;
                var Style = j > (int) Main.worldSurface ? buildData.ChestStyle : 0;
                var num4 = 0;
                while (num4 < 10 && !(flag1 =
                           WorldGen.AddBuriedChest(_random.Next(2, rectangle.Width - 2) + rectangle.X, j, 0,
                               false, Style)))
                    ++num4;
                if (!flag1)
                {
                    var i = rectangle.X + 2;
                    while (i <= rectangle.X + rectangle.Width - 2 &&
                           !(flag1 = WorldGen.AddBuriedChest(i, j, 0, false, Style)))
                        ++i;
                    if (flag1)
                        break;
                }
                else
                {
                    break;
                }
            }

            if (!flag1)
                foreach (var rectangle in rectangleList1)
                {
                    var j = rectangle.Y - 1;
                    var Style = j > (int) Main.worldSurface ? buildData.ChestStyle : 0;
                    var num4 = 0;
                    while (num4 < 10 && !(flag1 =
                               WorldGen.AddBuriedChest(_random.Next(2, rectangle.Width - 2) + rectangle.X, j, 0,
                                   false, Style)))
                        ++num4;
                    if (!flag1)
                    {
                        var i = rectangle.X + 2;
                        while (i <= rectangle.X + rectangle.Width - 2 &&
                               !(flag1 = WorldGen.AddBuriedChest(i, j, 0, false, Style)))
                            ++i;
                        if (flag1)
                            break;
                    }
                    else
                    {
                        break;
                    }
                }

            if (!flag1)
                for (var index = 0; index < 1000; ++index)
                {
                    var i = _random.Next(rectangleList1[0].X - 30, rectangleList1[0].X + 30);
                    var j = _random.Next(rectangleList1[0].Y - 30, rectangleList1[0].Y + 30);
                    var Style = j > (int) Main.worldSurface ? buildData.ChestStyle : 0;
                    if (WorldGen.AddBuriedChest(i, j, 0, false, Style))
                        break;
                }

            if (buildData == BuildData.Jungle && _sharpenerCount < _random.Next(2, 5))
            {
                var flag2 = false;
                foreach (var rectangle in rectangleList1)
                {
                    var j = rectangle.Height - 2 + rectangle.Y;
                    for (var index = 0; index < 10; ++index)
                    {
                        var i = _random.Next(2, rectangle.Width - 2) + rectangle.X;
                        WorldGen.PlaceTile(i, j, 377, true, true, -1, 0);
                        if (flag2 = _tiles[i, j].active() && _tiles[i, j].type == 377)
                            break;
                    }

                    if (!flag2)
                    {
                        var i = rectangle.X + 2;
                        while (i <= rectangle.X + rectangle.Width - 2 &&
                               !(flag2 = WorldGen.PlaceTile(i, j, 377, true, true, -1, 0)))
                            ++i;
                        if (flag2)
                            break;
                    }
                    else
                    {
                        break;
                    }
                }

                if (flag2)
                    ++_sharpenerCount;
            }

            if (buildData == BuildData.Desert && _extractinatorCount < _random.Next(2, 5))
            {
                var flag2 = false;
                foreach (var rectangle in rectangleList1)
                {
                    var j = rectangle.Height - 2 + rectangle.Y;
                    for (var index = 0; index < 10; ++index)
                    {
                        var i = _random.Next(2, rectangle.Width - 2) + rectangle.X;
                        WorldGen.PlaceTile(i, j, 219, true, true, -1, 0);
                        if (flag2 = _tiles[i, j].active() && _tiles[i, j].type == 219)
                            break;
                    }

                    if (!flag2)
                    {
                        var i = rectangle.X + 2;
                        while (i <= rectangle.X + rectangle.Width - 2 &&
                               !(flag2 = WorldGen.PlaceTile(i, j, 219, true, true, -1, 0)))
                            ++i;
                        if (flag2)
                            break;
                    }
                    else
                    {
                        break;
                    }
                }

                if (flag2)
                    ++_extractinatorCount;
            }

            return true;
        }

        public override void Reset()
        {
            _sharpenerCount = 0;
            _extractinatorCount = 0;
        }

        internal static void AgeDefaultRoom(Rectangle room)
        {
            for (var index = 0; index < room.Width * room.Height / 16; ++index)
                WorldUtils.Gen(
                    new Point(_random.Next(1, room.Width - 1) + room.X,
                        _random.Next(1, room.Height - 1) + room.Y), new Shapes.Rectangle(2, 2),
                    Actions.Chain((GenAction) new Modifiers.Dither(0.5), (GenAction) new Modifiers.Blotches(2, 2.0),
                        (GenAction) new Modifiers.IsEmpty(), (GenAction) new Actions.SetTile(51, true, true)));
            WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height),
                Actions.Chain((GenAction) new Modifiers.Dither(0.850000023841858),
                    (GenAction) new Modifiers.Blotches(2, 0.3),
                    (GenAction) new Modifiers.OnlyWalls(BuildData.Default.Wall),
                    (double) room.Y > Main.worldSurface
                        ? (GenAction) new Actions.ClearWall(true)
                        : (GenAction) new Actions.PlaceWall(2, true)));
            WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height),
                Actions.Chain((GenAction) new Modifiers.Dither(0.949999988079071),
                    (GenAction) new Modifiers.OnlyTiles((ushort) 30, (ushort) 321, (ushort) 158),
                    (GenAction) new Actions.ClearTile(true)));
        }

        internal static void AgeSnowRoom(Rectangle room)
        {
            WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height),
                Actions.Chain((GenAction) new Modifiers.Dither(0.600000023841858),
                    (GenAction) new Modifiers.Blotches(2, 0.600000023841858),
                    (GenAction) new Modifiers.OnlyTiles(BuildData.Snow.Tile),
                    (GenAction) new Actions.SetTile(161, true, true),
                    (GenAction) new Modifiers.Dither(0.8), (GenAction) new Actions.SetTile(147, true, true)));
            WorldUtils.Gen(new Point(room.X + 1, room.Y), new Shapes.Rectangle(room.Width - 2, 1),
                Actions.Chain((GenAction) new Modifiers.Dither(0.5), (GenAction) new Modifiers.OnlyTiles((ushort) 161),
                    (GenAction) new Modifiers.Offset(0, 1), (GenAction) new ActionStalagtite()));
            WorldUtils.Gen(new Point(room.X + 1, room.Y + room.Height - 1),
                new Shapes.Rectangle(room.Width - 2, 1), Actions.Chain((GenAction) new Modifiers.Dither(0.5),
                    (GenAction) new Modifiers.OnlyTiles((ushort) 161), (GenAction) new Modifiers.Offset(0, 1),
                    (GenAction) new ActionStalagtite()));
            WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height),
                Actions.Chain((GenAction) new Modifiers.Dither(0.850000023841858),
                    (GenAction) new Modifiers.Blotches(2, 0.8),
                    (double) room.Y > Main.worldSurface
                        ? (GenAction) new Actions.ClearWall(true)
                        : (GenAction) new Actions.PlaceWall(40, true)));
        }

        internal static void AgeDesertRoom(Rectangle room)
        {
            WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height),
                Actions.Chain((GenAction) new Modifiers.Dither(0.800000011920929),
                    (GenAction) new Modifiers.Blotches(2, 0.200000002980232),
                    (GenAction) new Modifiers.OnlyTiles(BuildData.Desert.Tile),
                    (GenAction) new Actions.SetTile(396, true, true),
                    (GenAction) new Modifiers.Dither(0.5), (GenAction) new Actions.SetTile(397, true, true)));
            WorldUtils.Gen(new Point(room.X + 1, room.Y), new Shapes.Rectangle(room.Width - 2, 1),
                Actions.Chain((GenAction) new Modifiers.Dither(0.5),
                    (GenAction) new Modifiers.OnlyTiles((ushort) 397, (ushort) 396),
                    (GenAction) new Modifiers.Offset(0, 1), (GenAction) new ActionStalagtite()));
            WorldUtils.Gen(new Point(room.X + 1, room.Y + room.Height - 1),
                new Shapes.Rectangle(room.Width - 2, 1), Actions.Chain((GenAction) new Modifiers.Dither(0.5),
                    (GenAction) new Modifiers.OnlyTiles((ushort) 397, (ushort) 396),
                    (GenAction) new Modifiers.Offset(0, 1), (GenAction) new ActionStalagtite()));
            WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height),
                Actions.Chain((GenAction) new Modifiers.Dither(0.800000011920929),
                    (GenAction) new Modifiers.Blotches(2, 0.3),
                    (GenAction) new Modifiers.OnlyWalls(BuildData.Desert.Wall),
                    (GenAction) new Actions.PlaceWall(216, true)));
        }

        internal static void AgeGraniteRoom(Rectangle room)
        {
            WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height),
                Actions.Chain((GenAction) new Modifiers.Dither(0.600000023841858),
                    (GenAction) new Modifiers.Blotches(2, 0.600000023841858),
                    (GenAction) new Modifiers.OnlyTiles(BuildData.Granite.Tile),
                    (GenAction) new Actions.SetTile(368, true, true)));
            WorldUtils.Gen(new Point(room.X + 1, room.Y), new Shapes.Rectangle(room.Width - 2, 1),
                Actions.Chain((GenAction) new Modifiers.Dither(0.800000011920929),
                    (GenAction) new Modifiers.OnlyTiles((ushort) 368), (GenAction) new Modifiers.Offset(0, 1),
                    (GenAction) new ActionStalagtite()));
            WorldUtils.Gen(new Point(room.X + 1, room.Y + room.Height - 1),
                new Shapes.Rectangle(room.Width - 2, 1), Actions.Chain(
                    (GenAction) new Modifiers.Dither(0.800000011920929),
                    (GenAction) new Modifiers.OnlyTiles((ushort) 368), (GenAction) new Modifiers.Offset(0, 1),
                    (GenAction) new ActionStalagtite()));
            WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height),
                Actions.Chain((GenAction) new Modifiers.Dither(0.850000023841858),
                    (GenAction) new Modifiers.Blotches(2, 0.3), (GenAction) new Actions.PlaceWall(180, true)));
        }

        internal static void AgeMarbleRoom(Rectangle room)
        {
            WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height),
                Actions.Chain((GenAction) new Modifiers.Dither(0.600000023841858),
                    (GenAction) new Modifiers.Blotches(2, 0.600000023841858),
                    (GenAction) new Modifiers.OnlyTiles(BuildData.Marble.Tile),
                    (GenAction) new Actions.SetTile(367, true, true)));
            WorldUtils.Gen(new Point(room.X + 1, room.Y), new Shapes.Rectangle(room.Width - 2, 1),
                Actions.Chain((GenAction) new Modifiers.Dither(0.800000011920929),
                    (GenAction) new Modifiers.OnlyTiles((ushort) 367), (GenAction) new Modifiers.Offset(0, 1),
                    (GenAction) new ActionStalagtite()));
            WorldUtils.Gen(new Point(room.X + 1, room.Y + room.Height - 1),
                new Shapes.Rectangle(room.Width - 2, 1), Actions.Chain(
                    (GenAction) new Modifiers.Dither(0.800000011920929),
                    (GenAction) new Modifiers.OnlyTiles((ushort) 367), (GenAction) new Modifiers.Offset(0, 1),
                    (GenAction) new ActionStalagtite()));
            WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height),
                Actions.Chain((GenAction) new Modifiers.Dither(0.850000023841858),
                    (GenAction) new Modifiers.Blotches(2, 0.3), (GenAction) new Actions.PlaceWall(178, true)));
        }

        internal static void AgeMushroomRoom(Rectangle room)
        {
            WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height),
                Actions.Chain((GenAction) new Modifiers.Dither(0.699999988079071),
                    (GenAction) new Modifiers.Blotches(2, 0.5),
                    (GenAction) new Modifiers.OnlyTiles(BuildData.Mushroom.Tile),
                    (GenAction) new Actions.SetTile(70, true, true)));
            WorldUtils.Gen(new Point(room.X + 1, room.Y), new Shapes.Rectangle(room.Width - 2, 1),
                Actions.Chain((GenAction) new Modifiers.Dither(0.600000023841858),
                    (GenAction) new Modifiers.OnlyTiles((ushort) 70), (GenAction) new Modifiers.Offset(0, -1),
                    (GenAction) new Actions.SetTile(71, false, true)));
            WorldUtils.Gen(new Point(room.X + 1, room.Y + room.Height - 1),
                new Shapes.Rectangle(room.Width - 2, 1), Actions.Chain(
                    (GenAction) new Modifiers.Dither(0.600000023841858),
                    (GenAction) new Modifiers.OnlyTiles((ushort) 70), (GenAction) new Modifiers.Offset(0, -1),
                    (GenAction) new Actions.SetTile(71, false, true)));
            WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height),
                Actions.Chain((GenAction) new Modifiers.Dither(0.850000023841858),
                    (GenAction) new Modifiers.Blotches(2, 0.3), (GenAction) new Actions.ClearWall(false)));
        }

        internal static void AgeJungleRoom(Rectangle room)
        {
            WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height),
                Actions.Chain((GenAction) new Modifiers.Dither(0.600000023841858),
                    (GenAction) new Modifiers.Blotches(2, 0.600000023841858),
                    (GenAction) new Modifiers.OnlyTiles(BuildData.Jungle.Tile),
                    (GenAction) new Actions.SetTile(60, true, true),
                    (GenAction) new Modifiers.Dither(0.800000011920929),
                    (GenAction) new Actions.SetTile(59, true, true)));
            WorldUtils.Gen(new Point(room.X + 1, room.Y), new Shapes.Rectangle(room.Width - 2, 1),
                Actions.Chain((GenAction) new Modifiers.Dither(0.5), (GenAction) new Modifiers.OnlyTiles((ushort) 60),
                    (GenAction) new Modifiers.Offset(0, 1), (GenAction) new ActionVines(3, room.Height, 62)));
            WorldUtils.Gen(new Point(room.X + 1, room.Y + room.Height - 1),
                new Shapes.Rectangle(room.Width - 2, 1), Actions.Chain((GenAction) new Modifiers.Dither(0.5),
                    (GenAction) new Modifiers.OnlyTiles((ushort) 60), (GenAction) new Modifiers.Offset(0, 1),
                    (GenAction) new ActionVines(3, room.Height, 62)));
            WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height),
                Actions.Chain((GenAction) new Modifiers.Dither(0.850000023841858),
                    (GenAction) new Modifiers.Blotches(2, 0.3), (GenAction) new Actions.PlaceWall(64, true)));
        }

        private class BuildData
        {
            public delegate void ProcessRoomMethod(Rectangle room);

            public static readonly BuildData Snow = CreateSnowData();
            public static readonly BuildData Jungle = CreateJungleData();
            public static readonly BuildData Default = CreateDefaultData();
            public static readonly BuildData Granite = CreateGraniteData();
            public static readonly BuildData Marble = CreateMarbleData();
            public static readonly BuildData Mushroom = CreateMushroomData();
            public static readonly BuildData Desert = CreateDesertData();
            public int BookcaseStyle;
            public int ChairStyle;
            public int ChestStyle;
            public int DoorStyle;
            public int PianoStyle;
            public int PlatformStyle;
            public ProcessRoomMethod ProcessRoom;
            public int TableStyle;
            public ushort Tile;
            public byte Wall;
            public int WorkbenchStyle;

            public static BuildData CreateSnowData()
            {
                return new BuildData
                {
                    Tile = 321,
                    Wall = 149,
                    DoorStyle = 30,
                    PlatformStyle = 19,
                    TableStyle = 28,
                    WorkbenchStyle = 23,
                    PianoStyle = 23,
                    BookcaseStyle = 25,
                    ChairStyle = 30,
                    ChestStyle = 11,
                    ProcessRoom = AgeSnowRoom
                };
            }

            public static BuildData CreateDesertData()
            {
                return new BuildData
                {
                    Tile = 396,
                    Wall = 187,
                    PlatformStyle = 0,
                    DoorStyle = 0,
                    TableStyle = 0,
                    WorkbenchStyle = 0,
                    PianoStyle = 0,
                    BookcaseStyle = 0,
                    ChairStyle = 0,
                    ChestStyle = 1,
                    ProcessRoom = AgeDesertRoom
                };
            }

            public static BuildData CreateJungleData()
            {
                return new BuildData
                {
                    Tile = 158,
                    Wall = 42,
                    PlatformStyle = 2,
                    DoorStyle = 2,
                    TableStyle = 2,
                    WorkbenchStyle = 2,
                    PianoStyle = 2,
                    BookcaseStyle = 12,
                    ChairStyle = 3,
                    ChestStyle = 8,
                    ProcessRoom = AgeJungleRoom
                };
            }

            public static BuildData CreateGraniteData()
            {
                return new BuildData
                {
                    Tile = 369,
                    Wall = 181,
                    PlatformStyle = 28,
                    DoorStyle = 34,
                    TableStyle = 33,
                    WorkbenchStyle = 29,
                    PianoStyle = 28,
                    BookcaseStyle = 30,
                    ChairStyle = 34,
                    ChestStyle = 50,
                    ProcessRoom = AgeGraniteRoom
                };
            }

            public static BuildData CreateMarbleData()
            {
                return new BuildData
                {
                    Tile = 357,
                    Wall = 179,
                    PlatformStyle = 29,
                    DoorStyle = 35,
                    TableStyle = 34,
                    WorkbenchStyle = 30,
                    PianoStyle = 29,
                    BookcaseStyle = 31,
                    ChairStyle = 35,
                    ChestStyle = 51,
                    ProcessRoom = AgeMarbleRoom
                };
            }

            public static BuildData CreateMushroomData()
            {
                return new BuildData
                {
                    Tile = 190,
                    Wall = 74,
                    PlatformStyle = 18,
                    DoorStyle = 6,
                    TableStyle = 27,
                    WorkbenchStyle = 7,
                    PianoStyle = 22,
                    BookcaseStyle = 24,
                    ChairStyle = 9,
                    ChestStyle = 32,
                    ProcessRoom = AgeMushroomRoom
                };
            }

            public static BuildData CreateDefaultData()
            {
                return new BuildData
                {
                    Tile = 30,
                    Wall = 27,
                    PlatformStyle = 0,
                    DoorStyle = 0,
                    TableStyle = 0,
                    WorkbenchStyle = 0,
                    PianoStyle = 0,
                    BookcaseStyle = 0,
                    ChairStyle = 0,
                    ChestStyle = 1,
                    ProcessRoom = AgeDefaultRoom
                };
            }
        }
    }
}