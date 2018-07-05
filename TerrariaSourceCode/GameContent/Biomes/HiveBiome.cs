﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Biomes.HiveBiome
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System;
using Microsoft.Xna.Framework;
using Terraria.World.Generation;

namespace Terraria.GameContent.Biomes
{
    public class HiveBiome : MicroBiome
    {
        public override bool Place(Point origin, StructureMap structures)
        {
            var count1 = new Ref<int>(0);
            var count2 = new Ref<int>(0);
            var count3 = new Ref<int>(0);
            var count4 = new Ref<int>(0);
            WorldUtils.Gen(origin, new Shapes.Circle(15), Actions.Chain(
                (GenAction) new Actions.Scanner(count3), (GenAction) new Modifiers.IsSolid(),
                (GenAction) new Actions.Scanner(count1), (GenAction) new Modifiers.OnlyTiles((ushort) 60, (ushort) 59),
                (GenAction) new Actions.Scanner(count2), (GenAction) new Modifiers.OnlyTiles((ushort) 60),
                (GenAction) new Actions.Scanner(count4)));
            if (count2.Value / (double) count1.Value < 0.75 || count4.Value < 2 ||
                !structures.CanPlace(new Rectangle(origin.X - 50, origin.Y - 50, 100, 100), 0))
                return false;
            var x1 = origin.X;
            var y1 = origin.Y;
            var num1 = 150;
            var index1 = x1 - num1;
            while (index1 < x1 + num1)
            {
                if (index1 > 0 && index1 <= Main.maxTilesX - 1)
                {
                    var index2 = y1 - num1;
                    while (index2 < y1 + num1)
                    {
                        if (index2 > 0 && index2 <= Main.maxTilesY - 1 &&
                            (Main.tile[index1, index2].active() && Main.tile[index1, index2].type == 226 ||
                             Main.tile[index1, index2].wall == 87 || Main.tile[index1, index2].wall == 3 ||
                             Main.tile[index1, index2].wall == 83))
                            return false;
                        index2 += 10;
                    }
                }

                index1 += 10;
            }

            var x2 = origin.X;
            var y2 = origin.Y;
            var index3 = 0;
            var numArray1 = new int[10];
            var numArray2 = new int[10];
            var vector2_1 = new Vector2(x2, y2);
            var vector2_2 = vector2_1;
            var num2 = WorldGen.genRand.Next(2, 5);
            for (var index2 = 0; index2 < num2; ++index2)
            {
                var num3 = WorldGen.genRand.Next(2, 5);
                for (var index4 = 0; index4 < num3; ++index4)
                    vector2_2 = WorldGen.Hive((int) vector2_1.X, (int) vector2_1.Y);
                vector2_1 = vector2_2;
                numArray1[index3] = (int) vector2_1.X;
                numArray2[index3] = (int) vector2_1.Y;
                ++index3;
            }

            for (var index2 = 0; index2 < index3; ++index2)
            {
                var index4 = numArray1[index2];
                var index5 = numArray2[index2];
                var flag = false;
                var num3 = 1;
                if (WorldGen.genRand.Next(2) == 0)
                    num3 = -1;
                while (index4 > 10 && index4 < Main.maxTilesX - 10 && index5 > 10 && index5 < Main.maxTilesY - 10 &&
                       (!Main.tile[index4, index5].active() || !Main.tile[index4, index5 + 1].active() ||
                        !Main.tile[index4 + 1, index5].active() || !Main.tile[index4 + 1, index5 + 1].active()))
                {
                    index4 += num3;
                    if (Math.Abs(index4 - numArray1[index2]) > 50)
                    {
                        flag = true;
                        break;
                    }
                }

                if (!flag)
                {
                    var i = index4 + num3;
                    for (var index6 = i - 1; index6 <= i + 2; ++index6)
                    for (var index7 = index5 - 1; index7 <= index5 + 2; ++index7)
                        if (index6 < 10 || index6 > Main.maxTilesX - 10)
                        {
                            flag = true;
                        }
                        else if (Main.tile[index6, index7].active() &&
                                 Main.tile[index6, index7].type != 225)
                        {
                            flag = true;
                            break;
                        }

                    if (!flag)
                    {
                        for (var index6 = i - 1; index6 <= i + 2; ++index6)
                        for (var index7 = index5 - 1; index7 <= index5 + 2; ++index7)
                            if (index6 >= i && index6 <= i + 1 && index7 >= index5 && index7 <= index5 + 1)
                            {
                                Main.tile[index6, index7].active(false);
                                Main.tile[index6, index7].liquid = byte.MaxValue;
                                Main.tile[index6, index7].honey(true);
                            }
                            else
                            {
                                Main.tile[index6, index7].active(true);
                                Main.tile[index6, index7].type = 225;
                            }

                        var num4 = num3 * -1;
                        var j = index5 + 1;
                        var num5 = 0;
                        while ((num5 < 4 || WorldGen.SolidTile(i, j)) && i > 10 && i < Main.maxTilesX - 10)
                        {
                            ++num5;
                            i += num4;
                            if (WorldGen.SolidTile(i, j))
                            {
                                WorldGen.PoundTile(i, j);
                                if (!Main.tile[i, j + 1].active())
                                {
                                    Main.tile[i, j + 1].active(true);
                                    Main.tile[i, j + 1].type = 225;
                                }
                            }
                        }
                    }
                }
            }

            WorldGen.larvaX[WorldGen.numLarva] = Utils.Clamp((int) vector2_1.X, 5, Main.maxTilesX - 5);
            WorldGen.larvaY[WorldGen.numLarva] = Utils.Clamp((int) vector2_1.Y, 5, Main.maxTilesY - 5);
            ++WorldGen.numLarva;
            var x3 = (int) vector2_1.X;
            var y3 = (int) vector2_1.Y;
            for (var index2 = x3 - 1; index2 <= x3 + 1 && index2 > 0 && index2 < Main.maxTilesX; ++index2)
            for (var index4 = y3 - 2; index4 <= y3 + 1 && index4 > 0 && index4 < Main.maxTilesY; ++index4)
                if (index4 != y3 + 1)
                {
                    Main.tile[index2, index4].active(false);
                }
                else
                {
                    Main.tile[index2, index4].active(true);
                    Main.tile[index2, index4].type = 225;
                    Main.tile[index2, index4].slope(0);
                    Main.tile[index2, index4].halfBrick(false);
                }

            structures.AddStructure(new Rectangle(origin.X - 50, origin.Y - 50, 100, 100), 5);
            return true;
        }
    }
}