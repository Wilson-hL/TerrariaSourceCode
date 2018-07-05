// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Biomes.DesertBiome
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.World.Generation;

namespace Terraria.GameContent.Biomes
{
    public class DesertBiome : MicroBiome
    {
        private void PlaceSand(ClusterGroup clusters, Point start, Vector2 scale)
        {
            var num = (int) (scale.X * clusters.Width);
            var num2 = (int) (scale.Y * clusters.Height);
            var num3 = 5;
            var num4 = start.Y + (num2 >> 1);
            var num5 = 0f;
            var array = new short[num + num3 * 2];
            for (var i = -num3; i < num + num3; i++)
            for (var j = 150; j < num4; j++)
                if (WorldGen.SolidOrSlopedTile(i + start.X, j))
                {
                    num5 += j - 1;
                    array[i + num3] = (short) (j - 1);
                    break;
                }

            var num6 = num5 / (num + num3 * 2);
            var num7 = 0;
            for (var k = -num3; k < num + num3; k++)
            {
                var value = Math.Abs((k + num3) / (float) (num + num3 * 2)) * 2f - 1f;
                value = MathHelper.Clamp(value, -1f, 1f);
                if (k % 3 == 0) num7 = Utils.Clamp(num7 + _random.Next(-1, 2), -10, 10);

                var num8 = (float) Math.Sqrt(1f - value * value * value * value);
                var num9 = num4 - (int) (num8 * (num4 - num6)) + num7;
                var val = num4 - (int) ((num4 - num6) *
                                        (num8 - 0.15f / (float) Math.Sqrt(Math.Max(0.01,
                                             Math.Abs(8f * value) - 0.1)) + 0.25f));
                val = Math.Min(num4, val);
                if (Math.Abs(value) < 0.8f)
                {
                    var num10 = Utils.SmoothStep(0.5f, 0.8f, Math.Abs(value));
                    num10 = num10 * num10 * num10;
                    var val2 = 10 + (int) (num6 - num10 * 20f) + num7;
                    val2 = Math.Min(val2, num9);
                    var num11 = 50;
                    for (var l = num11; (float) l < num6; l++)
                    {
                        var num12 = k + start.X;
                        if (_tiles[num12, l].active() &&
                            (_tiles[num12, l].type == 189 || _tiles[num12, l].type == 196))
                            num11 = l + 5;
                    }

                    for (var m = num11; m < val2; m++)
                    {
                        var num13 = k + start.X;
                        var num14 = m;
                        _tiles[num13, num14].active(false);
                        _tiles[num13, num14].wall = 0;
                    }

                    array[k + num3] = (short) val2;
                }

                for (var num15 = num4 - 1; num15 >= num9; num15--)
                {
                    var num16 = k + start.X;
                    var num17 = num15;
                    var tile = _tiles[num16, num17];
                    tile.liquid = 0;
                    var testTile = _tiles[num16, num17 + 1];
                    var testTile2 = _tiles[num16, num17 + 2];
                    tile.type = (ushort) (WorldGen.SolidTile(testTile) && WorldGen.SolidTile(testTile2) ? 53 : 397);
                    if (num15 > num9 + 5) tile.wall = 187;

                    tile.active(true);
                    if (tile.wall != 187) tile.wall = 0;

                    if (num15 < val)
                    {
                        if (num15 > num9 + 5) tile.wall = 187;

                        tile.active(false);
                    }

                    WorldGen.SquareWallFrame(num16, num17, true);
                }
            }
        }

        private void PlaceClusters(ClusterGroup clusters, Point start, Vector2 scale)
        {
            var num = (int) (scale.X * clusters.Width);
            var num2 = (int) (scale.Y * clusters.Height);
            var value = new Vector2(num, num2);
            var value2 = new Vector2(clusters.Width, clusters.Height);
            for (var i = -20; i < num + 20; i++)
            for (var j = -20; j < num2 + 20; j++)
            {
                var num3 = 0f;
                var num4 = -1;
                var num5 = 0f;
                var num6 = i + start.X;
                var num7 = j + start.Y;
                var value3 = new Vector2(i, j) / value * value2;
                var num8 = (new Vector2(i, j) / value * 2f - Vector2.One).Length();
                for (var k = 0; k < clusters.Count; k++)
                {
                    var cluster = clusters[k];
                    if (!(Math.Abs(cluster[0].Position.X - value3.X) > 10f) &&
                        !(Math.Abs(cluster[0].Position.Y - value3.Y) > 10f))
                    {
                        var num9 = 0f;
                        foreach (var item in cluster)
                        {
                            var current = item;
                            num9 += 1f / Vector2.DistanceSquared(current.Position, value3);
                        }

                        if (num9 > num3)
                        {
                            if (num3 > num5) num5 = num3;

                            num3 = num9;
                            num4 = k;
                        }
                        else if (num9 > num5)
                        {
                            num5 = num9;
                        }
                    }
                }

                var num10 = num3 + num5;
                var tile = _tiles[num6, num7];
                var flag = num8 >= 0.8f;
                if (num10 > 3.5f)
                {
                    tile.ClearEverything();
                    tile.wall = 187;
                    tile.liquid = 0;
                    if (num4 % 15 == 2)
                    {
                        tile.ResetToType(404);
                        tile.wall = 187;
                        tile.active(true);
                    }

                    Tile.SmoothSlope(num6, num7, true);
                }
                else if (num10 > 1.8f)
                {
                    tile.wall = 187;
                    if (!flag || tile.active())
                    {
                        tile.ResetToType(396);
                        tile.wall = 187;
                        tile.active(true);
                        Tile.SmoothSlope(num6, num7, true);
                    }

                    tile.liquid = 0;
                }
                else if (num10 > 0.7f || !flag)
                {
                    if (!flag || tile.active())
                    {
                        tile.ResetToType(397);
                        tile.active(true);
                        Tile.SmoothSlope(num6, num7, true);
                    }

                    tile.liquid = 0;
                    tile.wall = 216;
                }
                else if (num10 > 0.25f)
                {
                    var num11 = (num10 - 0.25f) / 0.45f;
                    if (_random.NextFloat() < num11)
                    {
                        if (tile.active())
                        {
                            tile.ResetToType(397);
                            tile.active(true);
                            Tile.SmoothSlope(num6, num7, true);
                            tile.wall = 216;
                        }

                        tile.liquid = 0;
                        tile.wall = 187;
                    }
                }
            }
        }

        private void AddTileVariance(ClusterGroup clusters, Point start, Vector2 scale)
        {
            var num = (int) (scale.X * clusters.Width);
            var num2 = (int) (scale.Y * clusters.Height);
            for (var i = -20; i < num + 20; i++)
            for (var j = -20; j < num2 + 20; j++)
            {
                var num3 = i + start.X;
                var num4 = j + start.Y;
                var tile = _tiles[num3, num4];
                var testTile = _tiles[num3, num4 + 1];
                var testTile2 = _tiles[num3, num4 + 2];
                if (tile.type == 53 && (!WorldGen.SolidTile(testTile) || !WorldGen.SolidTile(testTile2)))
                    tile.type = 397;
            }

            for (var k = -20; k < num + 20; k++)
            for (var l = -20; l < num2 + 20; l++)
            {
                var num5 = k + start.X;
                var num6 = l + start.Y;
                var tile2 = _tiles[num5, num6];
                if (tile2.active() && tile2.type == 396)
                {
                    var flag = true;
                    for (var num7 = -1; num7 >= -3; num7--)
                        if (_tiles[num5, num6 + num7].active())
                        {
                            flag = false;
                            break;
                        }

                    var flag2 = true;
                    for (var m = 1; m <= 3; m++)
                        if (_tiles[num5, num6 + m].active())
                        {
                            flag2 = false;
                            break;
                        }

                    if (flag ^ flag2 && _random.Next(5) == 0)
                        WorldGen.PlaceTile(num5, num6 + (!flag ? 1 : -1), 165, true, true, -1, 0);
                    else if (flag && _random.Next(5) == 0)
                        WorldGen.PlaceTile(num5, num6 - 1, 187, true, true, -1, 29 + _random.Next(6));
                }
            }
        }

        private bool FindStart(Point origin, Vector2 scale, int xHubCount, int yHubCount, out Point start)
        {
            start = new Point(0, 0);
            var num = (int) (scale.X * xHubCount);
            var height = (int) (scale.Y * yHubCount);
            origin.X -= num >> 1;
            var num2 = 220;
            for (var i = -20; i < num + 20; i++)
            for (var j = 220; j < Main.maxTilesY; j++)
                if (WorldGen.SolidTile(i + origin.X, j))
                {
                    var type = _tiles[i + origin.X, j].type;
                    if (type != 59 && type != 60)
                    {
                        if (j > num2) num2 = j;

                        break;
                    }

                    return false;
                }

            WorldGen.UndergroundDesertLocation = new Rectangle(origin.X, num2, num, height);
            start = new Point(origin.X, num2);
            return true;
        }

        public override bool Place(Point origin, StructureMap structures)
        {
            var num = Main.maxTilesX / 4200f;
            var num2 = (int) (80f * num);
            var num3 = (int) ((_random.NextFloat() + 1f) * 80f * num);
            var scale = new Vector2(4f, 2f);
            if (!FindStart(origin, scale, num2, num3, out var start)) return false;

            var clusterGroup = new ClusterGroup();
            clusterGroup.Generate(num2, num3);
            PlaceSand(clusterGroup, start, scale);
            PlaceClusters(clusterGroup, start, scale);
            AddTileVariance(clusterGroup, start, scale);
            var num4 = (int) (scale.X * clusterGroup.Width);
            var num5 = (int) (scale.Y * clusterGroup.Height);
            for (var i = -20; i < num4 + 20; i++)
            for (var j = -20; j < num5 + 20; j++)
                if (i + start.X > 0 && i + start.X < Main.maxTilesX - 1 && j + start.Y > 0 &&
                    j + start.Y < Main.maxTilesY - 1)
                {
                    WorldGen.SquareWallFrame(i + start.X, j + start.Y, true);
                    WorldUtils.TileFrame(i + start.X, j + start.Y, true);
                }

            return true;
        }

        private struct Hub
        {
            public readonly Vector2 Position;

            public Hub(Vector2 position)
            {
                Position = position;
            }

            public Hub(float x, float y)
            {
                Position = new Vector2(x, y);
            }
        }

        private class Cluster : List<Hub>
        {
        }

        private class ClusterGroup : List<Cluster>
        {
            public int Height;
            public int Width;

            private void SearchForCluster(bool[,] hubMap, List<Point> pointCluster, int x, int y, int level = 2)
            {
                pointCluster.Add(new Point(x, y));
                hubMap[x, y] = false;
                level--;
                if (level != -1)
                {
                    if (x > 0 && hubMap[x - 1, y]) SearchForCluster(hubMap, pointCluster, x - 1, y, level);

                    if (x < hubMap.GetLength(0) - 1 && hubMap[x + 1, y])
                        SearchForCluster(hubMap, pointCluster, x + 1, y, level);

                    if (y > 0 && hubMap[x, y - 1]) SearchForCluster(hubMap, pointCluster, x, y - 1, level);

                    if (y < hubMap.GetLength(1) - 1 && hubMap[x, y + 1])
                        SearchForCluster(hubMap, pointCluster, x, y + 1, level);
                }
            }

            private void AttemptClaim(int x, int y, int[,] clusterIndexMap, List<List<Point>> pointClusters, int index)
            {
                var num = clusterIndexMap[x, y];
                if (num != -1 && num != index)
                {
                    var num2 = WorldGen.genRand.Next(2) == 0 ? -1 : index;
                    var list = pointClusters[num];
                    foreach (var item in list)
                    {
                        var current = item;
                        clusterIndexMap[current.X, current.Y] = num2;
                    }
                }
            }

            public void Generate(int width, int height)
            {
                Width = width;
                Height = height;
                Clear();
                var array = new bool[width, height];
                var num = (width >> 1) - 1;
                var num2 = (height >> 1) - 1;
                var num3 = (num + 1) * (num + 1);
                var point = new Point(num, num2);
                for (var i = point.Y - num2; i <= point.Y + num2; i++)
                {
                    var num4 = num / (float) num2 * (i - point.Y);
                    var num5 = Math.Min(num, (int) Math.Sqrt(num3 - num4 * num4));
                    for (var j = point.X - num5; j <= point.X + num5; j++)
                    {
                        var array2 = array;
                        var num6 = j;
                        var num7 = i;
                        var num8 = WorldGen.genRand.Next(2) == 0;
                        array2[num6, num7] = num8;
                    }
                }

                var list = new List<List<Point>>();
                for (var k = 0; k < array.GetLength(0); k++)
                for (var l = 0; l < array.GetLength(1); l++)
                    if (array[k, l] && WorldGen.genRand.Next(2) == 0)
                    {
                        var list2 = new List<Point>();
                        SearchForCluster(array, list2, k, l, 2);
                        if (list2.Count > 2) list.Add(list2);
                    }

                var array3 = new int[array.GetLength(0), array.GetLength(1)];
                for (var m = 0; m < array3.GetLength(0); m++)
                for (var n = 0; n < array3.GetLength(1); n++)
                    array3[m, n] = -1;

                for (var num9 = 0; num9 < list.Count; num9++)
                    foreach (var item in list[num9])
                    {
                        var current = item;
                        array3[current.X, current.Y] = num9;
                    }

                for (var num10 = 0; num10 < list.Count; num10++)
                {
                    var list3 = list[num10];
                    foreach (var item2 in list3)
                    {
                        var current2 = item2;
                        var x = current2.X;
                        var y = current2.Y;
                        if (array3[x, y] == -1) break;

                        var index = array3[x, y];
                        if (x > 0) AttemptClaim(x - 1, y, array3, list, index);

                        if (x < array3.GetLength(0) - 1) AttemptClaim(x + 1, y, array3, list, index);

                        if (y > 0) AttemptClaim(x, y - 1, array3, list, index);

                        if (y < array3.GetLength(1) - 1) AttemptClaim(x, y + 1, array3, list, index);
                    }
                }

                foreach (var item3 in list) item3.Clear();

                for (var num11 = 0; num11 < array3.GetLength(0); num11++)
                for (var num12 = 0; num12 < array3.GetLength(1); num12++)
                    if (array3[num11, num12] != -1)
                        list[array3[num11, num12]].Add(new Point(num11, num12));

                foreach (var item4 in list)
                    if (item4.Count < 4)
                        item4.Clear();

                foreach (var item5 in list)
                {
                    var cluster = new Cluster();
                    if (item5.Count > 0)
                    {
                        foreach (var item6 in item5)
                        {
                            var current6 = item6;
                            cluster.Add(new Hub(current6.X + (WorldGen.genRand.NextFloat() - 0.5f) * 0.5f,
                                current6.Y + (WorldGen.genRand.NextFloat() - 0.5f) * 0.5f));
                        }

                        Add(cluster);
                    }
                }
            }
        }
    }
}