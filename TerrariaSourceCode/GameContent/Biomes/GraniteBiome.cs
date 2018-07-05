// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Biomes.GraniteBiome
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.World.Generation;

namespace Terraria.GameContent.Biomes
{
    public class GraniteBiome : MicroBiome
    {
        private const int MAX_MAGMA_ITERATIONS = 300;
        private static Magma[,] _sourceMagmaMap = new Magma[200, 200];
        private static Magma[,] _targetMagmaMap = new Magma[200, 200];

        public override bool Place(Point origin, StructureMap structures)
        {
            if (_tiles[origin.X, origin.Y].active())
                return false;
            var length1 = _sourceMagmaMap.GetLength(0);
            var length2 = _sourceMagmaMap.GetLength(1);
            var index1 = length1 / 2;
            var index2 = length2 / 2;
            origin.X -= index1;
            origin.Y -= index2;
            for (var index3 = 0; index3 < length1; ++index3)
            for (var index4 = 0; index4 < length2; ++index4)
            {
                var i = index3 + origin.X;
                var j = index4 + origin.Y;
                _sourceMagmaMap[index3, index4] =
                    Magma.CreateEmpty(WorldGen.SolidTile(i, j) ? 4f : 1f);
                _targetMagmaMap[index3, index4] = _sourceMagmaMap[index3, index4];
            }

            var max1 = index1;
            var min1 = index1;
            var max2 = index2;
            var min2 = index2;
            for (var index3 = 0; index3 < 300; ++index3)
            {
                for (var index4 = max1; index4 <= min1; ++index4)
                for (var index5 = max2; index5 <= min2; ++index5)
                {
                    var sourceMagma1 = _sourceMagmaMap[index4, index5];
                    if (sourceMagma1.IsActive)
                    {
                        var num1 = 0.0f;
                        var zero = Vector2.Zero;
                        for (var index6 = -1; index6 <= 1; ++index6)
                        for (var index7 = -1; index7 <= 1; ++index7)
                            if (index6 != 0 || index7 != 0)
                            {
                                var vector2 = new Vector2(index6, index7);
                                vector2.Normalize();
                                var sourceMagma2 =
                                    _sourceMagmaMap[index4 + index6, index5 + index7];
                                if (sourceMagma1.Pressure > 0.00999999977648258 &&
                                    !sourceMagma2.IsActive)
                                {
                                    if (index6 == -1)
                                        max1 = Utils.Clamp(index4 + index6, 1, max1);
                                    else
                                        min1 = Utils.Clamp(index4 + index6, min1, length1 - 2);
                                    if (index7 == -1)
                                        max2 = Utils.Clamp(index5 + index7, 1, max2);
                                    else
                                        min2 = Utils.Clamp(index5 + index7, min2, length2 - 2);
                                    _targetMagmaMap[index4 + index6, index5 + index7] =
                                        sourceMagma2.ToFlow();
                                }

                                var pressure = sourceMagma2.Pressure;
                                num1 += pressure;
                                zero += pressure * vector2;
                            }

                        var num2 = num1 / 8f;
                        if (num2 > (double) sourceMagma1.Resistance)
                        {
                            var num3 = zero.Length() / 8f;
                            var pressure = Math.Max(0.0f,
                                (float) (Math.Max(num2 - num3 - sourceMagma1.Pressure, 0.0f) +
                                         (double) num3 + sourceMagma1.Pressure * 0.875) -
                                sourceMagma1.Resistance);
                            _targetMagmaMap[index4, index5] = Magma.CreateFlow(pressure,
                                Math.Max(0.0f, sourceMagma1.Resistance - pressure * 0.02f));
                        }
                    }
                }

                if (index3 < 2)
                    _targetMagmaMap[index1, index2] = Magma.CreateFlow(25f, 0.0f);
                Utils.Swap(ref _sourceMagmaMap, ref _targetMagmaMap);
            }

            var flag1 = origin.Y + index2 > WorldGen.lavaLine - 30;
            var flag2 = false;
            for (var index3 = -50; index3 < 50 && !flag2; ++index3)
            for (var index4 = -50; index4 < 50 && !flag2; ++index4)
                if (_tiles[origin.X + index1 + index3, origin.Y + index2 + index4].active())
                    switch (_tiles[origin.X + index1 + index3, origin.Y + index2 + index4].type)
                    {
                        case 147:
                        case 161:
                        case 162:
                        case 163:
                        case 200:
                            flag1 = false;
                            flag2 = true;
                            continue;
                        default:
                            continue;
                    }

            for (var index3 = max1; index3 <= min1; ++index3)
            for (var index4 = max2; index4 <= min2; ++index4)
            {
                var sourceMagma = _sourceMagmaMap[index3, index4];
                if (sourceMagma.IsActive)
                {
                    var tile = _tiles[origin.X + index3, origin.Y + index4];
                    if (Math.Max(
                            1f - Math.Max(0.0f,
                                (float) (Math.Sin((origin.Y + index4) * 0.400000005960464) *
                                         0.699999988079071 + 1.20000004768372) *
                                (float) (0.200000002980232 + 0.5 / Math.Sqrt(Math.Max(0.0f,
                                             sourceMagma.Pressure - sourceMagma.Resistance)))),
                            sourceMagma.Pressure / 15f) > 0.349999994039536 +
                        (WorldGen.SolidTile(origin.X + index3, origin.Y + index4) ? 0.0 : 0.5))
                    {
                        if (TileID.Sets.Ore[tile.type])
                            tile.ResetToType(tile.type);
                        else
                            tile.ResetToType(368);
                        tile.wall = 180;
                    }
                    else if (sourceMagma.Resistance < 0.00999999977648258)
                    {
                        WorldUtils.ClearTile(origin.X + index3, origin.Y + index4, false);
                        tile.wall = 180;
                    }

                    if (tile.liquid > 0 && flag1)
                        tile.liquidType(1);
                }
            }

            var point16List = new List<Point16>();
            for (var index3 = max1; index3 <= min1; ++index3)
            for (var index4 = max2; index4 <= min2; ++index4)
                if (_sourceMagmaMap[index3, index4].IsActive)
                {
                    var num1 = 0;
                    var num2 = index3 + origin.X;
                    var num3 = index4 + origin.Y;
                    if (WorldGen.SolidTile(num2, num3))
                    {
                        for (var index5 = -1; index5 <= 1; ++index5)
                        for (var index6 = -1; index6 <= 1; ++index6)
                            if (WorldGen.SolidTile(num2 + index5, num3 + index6))
                                ++num1;

                        if (num1 < 3)
                            point16List.Add(new Point16(num2, num3));
                    }
                }

            foreach (var point16 in point16List)
            {
                var x = (int) point16.X;
                var y = (int) point16.Y;
                WorldUtils.ClearTile(x, y, true);
                _tiles[x, y].wall = 180;
            }

            point16List.Clear();
            for (var index3 = max1; index3 <= min1; ++index3)
            for (var index4 = max2; index4 <= min2; ++index4)
            {
                var sourceMagma = _sourceMagmaMap[index3, index4];
                var index5 = index3 + origin.X;
                var index6 = index4 + origin.Y;
                if (sourceMagma.IsActive)
                {
                    WorldUtils.TileFrame(index5, index6, false);
                    WorldGen.SquareWallFrame(index5, index6, true);
                    if (_random.Next(8) == 0 && _tiles[index5, index6].active())
                    {
                        if (!_tiles[index5, index6 + 1].active())
                            WorldGen.PlaceTight(index5, index6 + 1, 165, false);
                        if (!_tiles[index5, index6 - 1].active())
                            WorldGen.PlaceTight(index5, index6 - 1, 165, false);
                    }

                    if (_random.Next(2) == 0)
                        Tile.SmoothSlope(index5, index6, true);
                }
            }

            return true;
        }

        private struct Magma
        {
            public readonly float Pressure;
            public readonly float Resistance;
            public readonly bool IsActive;

            private Magma(float pressure, float resistance, bool active)
            {
                Pressure = pressure;
                Resistance = resistance;
                IsActive = active;
            }

            public Magma ToFlow()
            {
                return new Magma(Pressure, Resistance, true);
            }

            public static Magma CreateFlow(float pressure, float resistance = 0.0f)
            {
                return new Magma(pressure, resistance, true);
            }

            public static Magma CreateEmpty(float resistance = 0.0f)
            {
                return new Magma(0.0f, resistance, false);
            }
        }
    }
}