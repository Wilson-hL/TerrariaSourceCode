﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Generation.TrackGenerator
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using System;
using Terraria.ID;
using Terraria.World.Generation;

namespace Terraria.GameContent.Generation
{
    public class TrackGenerator
    {
        private static readonly byte[] INVALID_WALLS = new byte[13]
        {
            (byte) 7, (byte) 94, (byte) 95, (byte) 8, (byte) 98, (byte) 99, (byte) 9, (byte) 96, (byte) 97, (byte) 3,
            (byte) 83, (byte) 87, (byte) 86
        };

        private TrackGenerator.TrackHistory[] _historyCache = new TrackGenerator.TrackHistory[2048];
        private const int TOTAL_TILE_IGNORES = 150;
        private const int PLAYER_HEIGHT = 6;
        private const int MAX_RETRIES = 400;
        private const int MAX_SMOOTH_DISTANCE = 15;
        private const int MAX_ITERATIONS = 1000000;

        public void Generate(int trackCount, int minimumLength)
        {
            var num = trackCount;
            while (num > 0)
            {
                var x = WorldGen.genRand.Next(150, Main.maxTilesX - 150);
                var y = WorldGen.genRand.Next((int) Main.worldSurface + 25, Main.maxTilesY - 200);
                if (this.IsLocationEmpty(x, y))
                {
                    while (this.IsLocationEmpty(x, y + 1))
                        ++y;
                    if (this.FindPath(x, y, minimumLength, false))
                        --num;
                }
            }
        }

        private bool IsLocationEmpty(int x, int y)
        {
            if (y > Main.maxTilesY - 200 || x < 0 || (y < (int) Main.worldSurface || x > Main.maxTilesX - 5))
                return false;
            for (var index = 0; index < 6; ++index)
            {
                if (WorldGen.SolidTile(x, y - index))
                    return false;
            }

            return true;
        }

        private bool CanTrackBePlaced(int x, int y)
        {
            if (y > Main.maxTilesY - 200 || x < 0 || (y < (int) Main.worldSurface || x > Main.maxTilesX - 5))
                return false;
            var wall = Main.tile[x, y].wall;
            for (var index = 0; index < TrackGenerator.INVALID_WALLS.Length; ++index)
            {
                if ((int) wall == (int) TrackGenerator.INVALID_WALLS[index])
                    return false;
            }

            for (var index = -1; index <= 1; ++index)
            {
                if (Main.tile[x + index, y].active() && (Main.tile[x + index, y].type == (ushort) 314 ||
                                                         !TileID.Sets.GeneralPlacementTiles[
                                                             (int) Main.tile[x + index, y].type]))
                    return false;
            }

            return true;
        }

        private void SmoothTrack(TrackGenerator.TrackHistory[] history, int length)
        {
            var val2 = length - 1;
            var flag = false;
            for (var index1 = length - 1; index1 >= 0; --index1)
            {
                if (flag)
                {
                    val2 = Math.Min(index1 + 15, val2);
                    if ((int) history[index1].Y >= (int) history[val2].Y)
                    {
                        for (var index2 = index1 + 1; (int) history[index2].Y > (int) history[index1].Y; ++index2)
                            history[index2].Y = history[index1].Y;
                        if ((int) history[index1].Y == (int) history[val2].Y)
                            flag = false;
                    }
                }
                else if ((int) history[index1].Y > (int) history[val2].Y)
                    flag = true;
                else
                    val2 = index1;
            }
        }

        public bool FindPath(int x, int y, int minimumLength, bool debugMode = false)
        {
            var historyCache = this._historyCache;
            var index1 = 0;
            var tile = Main.tile;
            var flag1 = true;
            var num1 = WorldGen.genRand.Next(2) == 0 ? 1 : -1;
            if (debugMode)
                num1 = Main.player[Main.myPlayer].direction;
            var yDirection = 1;
            var length = 0;
            var num2 = 400;
            var flag2 = false;
            var num3 = 150;
            var num4 = 0;
            for (var index2 = 1000000; index2 > 0 && flag1 && index1 < historyCache.Length - 1; ++index1)
            {
                --index2;
                historyCache[index1] = new TrackGenerator.TrackHistory(x, y, yDirection);
                var flag3 = false;
                var num5 = 1;
                if (index1 > minimumLength >> 1)
                    num5 = -1;
                else if (index1 > (minimumLength >> 1) - 5)
                    num5 = 0;
                if (flag2)
                {
                    var num6 = 0;
                    var num7 = num3;
                    var flag4 = false;
                    for (var index3 = Math.Min(1, yDirection + 1); index3 >= Math.Max(-1, yDirection - 1); --index3)
                    {
                        int num8;
                        for (num8 = 0; num8 <= num3; ++num8)
                        {
                            if (this.IsLocationEmpty(x + (num8 + 1) * num1, y + (num8 + 1) * index3 * num5))
                            {
                                flag4 = true;
                                break;
                            }
                        }

                        if (num8 < num7)
                        {
                            num7 = num8;
                            num6 = index3;
                        }
                    }

                    if (flag4)
                    {
                        yDirection = num6;
                        for (var index3 = 0; index3 < num7 - 1; ++index3)
                        {
                            ++index1;
                            x += num1;
                            y += yDirection * num5;
                            historyCache[index1] = new TrackGenerator.TrackHistory(x, y, yDirection);
                            num4 = index1;
                        }

                        x += num1;
                        y += yDirection * num5;
                        length = index1 + 1;
                        flag2 = false;
                    }

                    num3 -= num7;
                    if (num3 < 0)
                        flag1 = false;
                }
                else
                {
                    for (var index3 = Math.Min(1, yDirection + 1); index3 >= Math.Max(-1, yDirection - 1); --index3)
                    {
                        if (this.IsLocationEmpty(x + num1, y + index3 * num5))
                        {
                            yDirection = index3;
                            flag3 = true;
                            x += num1;
                            y += yDirection * num5;
                            length = index1 + 1;
                            break;
                        }
                    }

                    if (!flag3)
                    {
                        while (index1 > num4 && y == (int) historyCache[index1].Y)
                            --index1;
                        x = (int) historyCache[index1].X;
                        y = (int) historyCache[index1].Y;
                        yDirection = (int) historyCache[index1].YDirection - 1;
                        --num2;
                        if (num2 <= 0)
                        {
                            index1 = length;
                            x = (int) historyCache[index1].X;
                            y = (int) historyCache[index1].Y;
                            yDirection = (int) historyCache[index1].YDirection;
                            flag2 = true;
                            num2 = 200;
                        }

                        --index1;
                    }
                }
            }

            if (length <= minimumLength && !debugMode)
                return false;
            this.SmoothTrack(historyCache, length);
            if (!debugMode)
            {
                for (var index2 = 0; index2 < length; ++index2)
                {
                    for (var index3 = -1; index3 < 7; ++index3)
                    {
                        if (!this.CanTrackBePlaced((int) historyCache[index2].X, (int) historyCache[index2].Y - index3))
                            return false;
                    }
                }
            }

            for (var index2 = 0; index2 < length; ++index2)
            {
                var trackHistory = historyCache[index2];
                for (var index3 = 0; index3 < 6; ++index3)
                    Main.tile[(int) trackHistory.X, (int) trackHistory.Y - index3].active(false);
            }

            for (var index2 = 0; index2 < length; ++index2)
            {
                var trackHistory = historyCache[index2];
                Tile.SmoothSlope((int) trackHistory.X, (int) trackHistory.Y + 1, true);
                Tile.SmoothSlope((int) trackHistory.X, (int) trackHistory.Y - 6, true);
                var wire = Main.tile[(int) trackHistory.X, (int) trackHistory.Y].wire();
                Main.tile[(int) trackHistory.X, (int) trackHistory.Y].ResetToType((ushort) 314);
                Main.tile[(int) trackHistory.X, (int) trackHistory.Y].wire(wire);
                if (index2 != 0)
                {
                    for (var index3 = 0; index3 < 6; ++index3)
                        WorldUtils.TileFrame((int) historyCache[index2 - 1].X,
                            (int) historyCache[index2 - 1].Y - index3, true);
                    if (index2 == length - 1)
                    {
                        for (var index3 = 0; index3 < 6; ++index3)
                            WorldUtils.TileFrame((int) trackHistory.X, (int) trackHistory.Y - index3, true);
                    }
                }
            }

            return true;
        }

        public static void Run(int trackCount = 30, int minimumLength = 250)
        {
            new TrackGenerator().Generate(trackCount, minimumLength);
        }

        public static void Run(Point start)
        {
            new TrackGenerator().FindPath(start.X, start.Y, 250, true);
        }

        private struct TrackHistory
        {
            public short X;
            public short Y;
            public byte YDirection;

            public TrackHistory(int x, int y, int yDirection)
            {
                this.X = (short) x;
                this.Y = (short) y;
                this.YDirection = (byte) yDirection;
            }

            public TrackHistory(short x, short y, byte yDirection)
            {
                this.X = x;
                this.Y = y;
                this.YDirection = yDirection;
            }
        }
    }
}