﻿// Decompiled with JetBrains decompiler
// Type: Terraria.World.Generation.WorldUtils
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;

namespace Terraria.World.Generation
{
    public static class WorldUtils
    {
        public static bool Gen(Point origin, GenShape shape, GenAction action)
        {
            return shape.Perform(origin, action);
        }

        public static bool Find(Point origin, GenSearch search, out Point result)
        {
            result = search.Find(origin);
            return !(result == GenSearch.NOT_FOUND);
        }

        public static void ClearTile(int x, int y, bool frameNeighbors = false)
        {
            Main.tile[x, y].ClearTile();
            if (!frameNeighbors)
                return;
            WorldGen.TileFrame(x + 1, y, false, false);
            WorldGen.TileFrame(x - 1, y, false, false);
            WorldGen.TileFrame(x, y + 1, false, false);
            WorldGen.TileFrame(x, y - 1, false, false);
        }

        public static void ClearWall(int x, int y, bool frameNeighbors = false)
        {
            Main.tile[x, y].wall = (byte) 0;
            if (!frameNeighbors)
                return;
            WorldGen.SquareWallFrame(x + 1, y, true);
            WorldGen.SquareWallFrame(x - 1, y, true);
            WorldGen.SquareWallFrame(x, y + 1, true);
            WorldGen.SquareWallFrame(x, y - 1, true);
        }

        public static void TileFrame(int x, int y, bool frameNeighbors = false)
        {
            WorldGen.TileFrame(x, y, true, false);
            if (!frameNeighbors)
                return;
            WorldGen.TileFrame(x + 1, y, true, false);
            WorldGen.TileFrame(x - 1, y, true, false);
            WorldGen.TileFrame(x, y + 1, true, false);
            WorldGen.TileFrame(x, y - 1, true, false);
        }

        public static void ClearChestLocation(int x, int y)
        {
            WorldUtils.ClearTile(x, y, true);
            WorldUtils.ClearTile(x - 1, y, true);
            WorldUtils.ClearTile(x, y - 1, true);
            WorldUtils.ClearTile(x - 1, y - 1, true);
        }

        public static void WireLine(Point start, Point end)
        {
            var point1 = start;
            var point2 = end;
            if (end.X < start.X)
                Utils.Swap<int>(ref end.X, ref start.X);
            if (end.Y < start.Y)
                Utils.Swap<int>(ref end.Y, ref start.Y);
            for (var x = start.X; x <= end.X; ++x)
                WorldGen.PlaceWire(x, point1.Y);
            for (var y = start.Y; y <= end.Y; ++y)
                WorldGen.PlaceWire(point2.X, y);
        }

        public static void DebugRegen()
        {
            WorldGen.clearWorld();
            WorldGen.generateWorld(Main.ActiveWorldFileData.Seed, (GenerationProgress) null);
            Main.NewText("World Regen Complete.", byte.MaxValue, byte.MaxValue, byte.MaxValue, false);
        }

        public static void DebugRotate()
        {
            var num1 = 0;
            var num2 = 0;
            var maxTilesY = Main.maxTilesY;
            for (var index1 = 0; index1 < Main.maxTilesX / Main.maxTilesY; ++index1)
            {
                for (var index2 = 0; index2 < maxTilesY / 2; ++index2)
                {
                    for (var index3 = index2; index3 < maxTilesY - index2; ++index3)
                    {
                        var tile = Main.tile[index3 + num1, index2 + num2];
                        Main.tile[index3 + num1, index2 + num2] = Main.tile[index2 + num1, maxTilesY - index3 + num2];
                        Main.tile[index2 + num1, maxTilesY - index3 + num2] =
                            Main.tile[maxTilesY - index3 + num1, maxTilesY - index2 + num2];
                        Main.tile[maxTilesY - index3 + num1, maxTilesY - index2 + num2] =
                            Main.tile[maxTilesY - index2 + num1, index3 + num2];
                        Main.tile[maxTilesY - index2 + num1, index3 + num2] = tile;
                    }
                }

                num1 += maxTilesY;
            }
        }
    }
}