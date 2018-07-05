// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Biomes.MarbleBiome
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.World.Generation;

namespace Terraria.GameContent.Biomes
{
    public class MarbleBiome : MicroBiome
    {
        private const int SCALE = 3;
        private Slab[,] _slabs;

        private void SmoothSlope(int x, int y)
        {
            var slab = _slabs[x, y];
            if (!slab.IsSolid)
                return;
            switch (((_slabs[x, y - 1].IsSolid ? 1 : 0) << 3) | ((_slabs[x, y + 1].IsSolid ? 1 : 0) << 2) |
                    ((_slabs[x - 1, y].IsSolid ? 1 : 0) << 1) | (_slabs[x + 1, y].IsSolid ? 1 : 0))
            {
                case 4:
                    _slabs[x, y] = slab.WithState(SlabStates.HalfBrick);
                    break;
                case 5:
                    _slabs[x, y] =
                        slab.WithState(SlabStates.BottomRightFilled);
                    break;
                case 6:
                    _slabs[x, y] =
                        slab.WithState(SlabStates.BottomLeftFilled);
                    break;
                case 9:
                    _slabs[x, y] =
                        slab.WithState(SlabStates.TopRightFilled);
                    break;
                case 10:
                    _slabs[x, y] = slab.WithState(SlabStates.TopLeftFilled);
                    break;
                default:
                    _slabs[x, y] = slab.WithState(SlabStates.Solid);
                    break;
            }
        }

        private void PlaceSlab(Slab slab, int originX, int originY, int scale)
        {
            for (var x = 0; x < scale; ++x)
            for (var y = 0; y < scale; ++y)
            {
                var tile = _tiles[originX + x, originY + y];
                if (TileID.Sets.Ore[tile.type])
                    tile.ResetToType(tile.type);
                else
                    tile.ResetToType(367);
                var active = slab.State(x, y, scale);
                tile.active(active);
                if (slab.HasWall)
                    tile.wall = 178;
                WorldUtils.TileFrame(originX + x, originY + y, true);
                WorldGen.SquareWallFrame(originX + x, originY + y, true);
                Tile.SmoothSlope(originX + x, originY + y, true);
                if (WorldGen.SolidTile(originX + x, originY + y - 1) && _random.Next(4) == 0)
                    WorldGen.PlaceTight(originX + x, originY + y, 165, false);
                if (WorldGen.SolidTile(originX + x, originY + y) && _random.Next(4) == 0)
                    WorldGen.PlaceTight(originX + x, originY + y - 1, 165, false);
            }
        }

        private bool IsGroupSolid(int x, int y, int scale)
        {
            var num = 0;
            for (var index1 = 0; index1 < scale; ++index1)
            for (var index2 = 0; index2 < scale; ++index2)
                if (WorldGen.SolidOrSlopedTile(x + index1, y + index2))
                    ++num;

            return num > scale / 4 * 3;
        }

        public override bool Place(Point origin, StructureMap structures)
        {
            if (_slabs == null)
                _slabs = new Slab[56, 26];
            var num1 = _random.Next(80, 150) / 3;
            var num2 = _random.Next(40, 60) / 3;
            var num3 = (num2 * 3 - _random.Next(20, 30)) / 3;
            origin.X -= num1 * 3 / 2;
            origin.Y -= num2 * 3 / 2;
            for (var index1 = -1; index1 < num1 + 1; ++index1)
            {
                var num4 = (float) ((index1 - num1 / 2) / (double) num1 + 0.5);
                var num5 = (int) ((0.5 - Math.Abs(num4 - 0.5f)) * 5.0) - 2;
                for (var index2 = -1; index2 < num2 + 1; ++index2)
                {
                    var hasWall = true;
                    var flag1 = false;
                    var flag2 = IsGroupSolid(index1 * 3 + origin.X, index2 * 3 + origin.Y, 3);
                    var num6 = Math.Abs(index2 - num2 / 2) - num3 / 4 + num5;
                    if (num6 > 3)
                    {
                        flag1 = flag2;
                        hasWall = false;
                    }
                    else if (num6 > 0)
                    {
                        flag1 = index2 - num2 / 2 > 0 || flag2;
                        hasWall = index2 - num2 / 2 < 0 || num6 <= 2;
                    }
                    else if (num6 == 0)
                    {
                        flag1 = _random.Next(2) == 0 && (index2 - num2 / 2 > 0 || flag2);
                    }

                    if (Math.Abs(num4 - 0.5f) >
                        0.349999994039536 + _random.NextFloat() * 0.100000001490116 && !flag2)
                    {
                        hasWall = false;
                        flag1 = false;
                    }

                    _slabs[index1 + 1, index2 + 1] = Slab.Create(
                        flag1
                            ? SlabStates.Solid
                            : new SlabState(SlabStates.Empty), hasWall);
                }
            }

            for (var index1 = 0; index1 < num1; ++index1)
            for (var index2 = 0; index2 < num2; ++index2)
                SmoothSlope(index1 + 1, index2 + 1);

            var num7 = num1 / 2;
            var val1 = num2 / 2;
            var num8 = (val1 + 1) * (val1 + 1);
            var num9 = (float) (_random.NextFloat() * 2.0 - 1.0);
            var num10 = (float) (_random.NextFloat() * 2.0 - 1.0);
            var num11 = (float) (_random.NextFloat() * 2.0 - 1.0);
            var num12 = 0.0f;
            for (var index1 = 0; index1 <= num1; ++index1)
            {
                var num4 = val1 / (float) num7 * (index1 - num7);
                var num5 = Math.Min(val1, (int) Math.Sqrt(Math.Max(0.0f, num8 - num4 * num4)));
                if (index1 < num1 / 2)
                    num12 += MathHelper.Lerp(num9, num10, index1 / (float) (num1 / 2));
                else
                    num12 += MathHelper.Lerp(num10, num11, (float) (index1 / (double) (num1 / 2) - 1.0));
                for (var index2 = val1 - num5; index2 <= val1 + num5; ++index2)
                    PlaceSlab(_slabs[index1 + 1, index2 + 1], index1 * 3 + origin.X,
                        index2 * 3 + origin.Y + (int) num12, 3);
            }

            return true;
        }

        private delegate bool SlabState(int x, int y, int scale);

        private class SlabStates
        {
            public static bool Empty(int x, int y, int scale)
            {
                return false;
            }

            public static bool Solid(int x, int y, int scale)
            {
                return true;
            }

            public static bool HalfBrick(int x, int y, int scale)
            {
                return y >= scale / 2;
            }

            public static bool BottomRightFilled(int x, int y, int scale)
            {
                return x >= scale - y;
            }

            public static bool BottomLeftFilled(int x, int y, int scale)
            {
                return x < y;
            }

            public static bool TopRightFilled(int x, int y, int scale)
            {
                return x > y;
            }

            public static bool TopLeftFilled(int x, int y, int scale)
            {
                return x < scale - y;
            }
        }

        private struct Slab
        {
            public readonly SlabState State;
            public readonly bool HasWall;

            public bool IsSolid => State != SlabStates.Empty;

            private Slab(SlabState state, bool hasWall)
            {
                State = state;
                HasWall = hasWall;
            }

            public Slab WithState(SlabState state)
            {
                return new Slab(state, HasWall);
            }

            public static Slab Create(SlabState state, bool hasWall)
            {
                return new Slab(state, hasWall);
            }
        }
    }
}