// Decompiled with JetBrains decompiler
// Type: Terraria.WaterfallManager
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace Terraria
{
    public class WaterfallManager
    {
        public Texture2D[] waterfallTexture = new Texture2D[23];
        private int waterfallDist = 100;
        private const int minWet = 160;
        private const int maxCount = 200;
        private const int maxLength = 100;
        private const int maxTypes = 23;
        private int qualityMax;
        private int currentMax;
        private WaterfallManager.WaterfallData[] waterfalls;
        private int wFallFrCounter;
        private int regularFrame;
        private int wFallFrCounter2;
        private int slowFrame;
        private int rainFrameCounter;
        private int rainFrameForeground;
        private int rainFrameBackground;
        private int snowFrameCounter;
        private int snowFrameForeground;
        private int findWaterfallCount;

        public WaterfallManager()
        {
            this.waterfalls = new WaterfallManager.WaterfallData[200];
        }

        public void LoadContent()
        {
            for (var index = 0; index < 23; ++index)
                this.waterfallTexture[index] =
                    Main.instance.OurLoad<Texture2D>("Images" + (object) Path.DirectorySeparatorChar + "Waterfall_" +
                                                     (object) index);
        }

        public bool CheckForWaterfall(int i, int j)
        {
            for (var index = 0; index < this.currentMax; ++index)
            {
                if (this.waterfalls[index].x == i && this.waterfalls[index].y == j)
                    return true;
            }

            return false;
        }

        public void FindWaterfalls(bool forced = false)
        {
            ++this.findWaterfallCount;
            if (this.findWaterfallCount < 30 && !forced)
                return;
            this.findWaterfallCount = 0;
            this.waterfallDist = (int) (75.0 * (double) Main.gfxQuality) + 25;
            this.qualityMax = (int) (175.0 * (double) Main.gfxQuality) + 25;
            this.currentMax = 0;
            var num1 = (int) ((double) Main.screenPosition.X / 16.0 - 1.0);
            var num2 = (int) (((double) Main.screenPosition.X + (double) Main.screenWidth) / 16.0) + 2;
            var num3 = (int) ((double) Main.screenPosition.Y / 16.0 - 1.0);
            var num4 = (int) (((double) Main.screenPosition.Y + (double) Main.screenHeight) / 16.0) + 2;
            var num5 = num1 - this.waterfallDist;
            var num6 = num2 + this.waterfallDist;
            var num7 = num3 - this.waterfallDist;
            var num8 = num4 + 20;
            if (num5 < 0)
                num5 = 0;
            if (num6 > Main.maxTilesX)
                num6 = Main.maxTilesX;
            if (num7 < 0)
                num7 = 0;
            if (num8 > Main.maxTilesY)
                num8 = Main.maxTilesY;
            for (var index1 = num5; index1 < num6; ++index1)
            {
                for (var index2 = num7; index2 < num8; ++index2)
                {
                    var tile = Main.tile[index1, index2];
                    if (tile == null)
                    {
                        tile = new Tile();
                        Main.tile[index1, index2] = tile;
                    }

                    if (tile.active())
                    {
                        if (tile.halfBrick())
                        {
                            var testTile1 = Main.tile[index1, index2 - 1];
                            if (testTile1 == null)
                            {
                                testTile1 = new Tile();
                                Main.tile[index1, index2 - 1] = testTile1;
                            }

                            if (testTile1.liquid < (byte) 16 || WorldGen.SolidTile(testTile1))
                            {
                                var testTile2 = Main.tile[index1 + 1, index2];
                                if (testTile2 == null)
                                {
                                    testTile2 = new Tile();
                                    Main.tile[index1 - 1, index2] = testTile2;
                                }

                                var testTile3 = Main.tile[index1 - 1, index2];
                                if (testTile3 == null)
                                {
                                    testTile3 = new Tile();
                                    Main.tile[index1 + 1, index2] = testTile3;
                                }

                                if ((testTile2.liquid > (byte) 160 || testTile3.liquid > (byte) 160) &&
                                    (testTile2.liquid == (byte) 0 && !WorldGen.SolidTile(testTile2) &&
                                     testTile2.slope() == (byte) 0 ||
                                     testTile3.liquid == (byte) 0 && !WorldGen.SolidTile(testTile3) &&
                                     testTile3.slope() == (byte) 0) && this.currentMax < this.qualityMax)
                                {
                                    this.waterfalls[this.currentMax].type = 0;
                                    this.waterfalls[this.currentMax].type =
                                        testTile1.lava() || testTile3.lava() || testTile2.lava()
                                            ? 1
                                            : (testTile1.honey() || testTile3.honey() || testTile2.honey() ? 14 : 0);
                                    this.waterfalls[this.currentMax].x = index1;
                                    this.waterfalls[this.currentMax].y = index2;
                                    ++this.currentMax;
                                }
                            }
                        }

                        if (tile.type == (ushort) 196)
                        {
                            var testTile = Main.tile[index1, index2 + 1];
                            if (testTile == null)
                            {
                                testTile = new Tile();
                                Main.tile[index1, index2 + 1] = testTile;
                            }

                            if (!WorldGen.SolidTile(testTile) && testTile.slope() == (byte) 0 &&
                                this.currentMax < this.qualityMax)
                            {
                                this.waterfalls[this.currentMax].type = 11;
                                this.waterfalls[this.currentMax].x = index1;
                                this.waterfalls[this.currentMax].y = index2 + 1;
                                ++this.currentMax;
                            }
                        }

                        if (tile.type == (ushort) 460)
                        {
                            var testTile = Main.tile[index1, index2 + 1];
                            if (testTile == null)
                            {
                                testTile = new Tile();
                                Main.tile[index1, index2 + 1] = testTile;
                            }

                            if (!WorldGen.SolidTile(testTile) && testTile.slope() == (byte) 0 &&
                                this.currentMax < this.qualityMax)
                            {
                                this.waterfalls[this.currentMax].type = 22;
                                this.waterfalls[this.currentMax].x = index1;
                                this.waterfalls[this.currentMax].y = index2 + 1;
                                ++this.currentMax;
                            }
                        }
                    }
                }
            }
        }

        public void UpdateFrame()
        {
            ++this.wFallFrCounter;
            if (this.wFallFrCounter > 2)
            {
                this.wFallFrCounter = 0;
                ++this.regularFrame;
                if (this.regularFrame > 15)
                    this.regularFrame = 0;
            }

            ++this.wFallFrCounter2;
            if (this.wFallFrCounter2 > 6)
            {
                this.wFallFrCounter2 = 0;
                ++this.slowFrame;
                if (this.slowFrame > 15)
                    this.slowFrame = 0;
            }

            ++this.rainFrameCounter;
            if (this.rainFrameCounter > 0)
            {
                ++this.rainFrameForeground;
                if (this.rainFrameForeground > 7)
                    this.rainFrameForeground -= 8;
                if (this.rainFrameCounter > 2)
                {
                    this.rainFrameCounter = 0;
                    --this.rainFrameBackground;
                    if (this.rainFrameBackground < 0)
                        this.rainFrameBackground = 7;
                }
            }

            if (++this.snowFrameCounter <= 3)
                return;
            this.snowFrameCounter = 0;
            if (++this.snowFrameForeground <= 7)
                return;
            this.snowFrameForeground = 0;
        }

        private void DrawWaterfall(SpriteBatch spriteBatch, int Style = 0, float Alpha = 1f)
        {
            var num1 = 0.0f;
            var num2 = 99999f;
            var num3 = 99999f;
            var num4 = -1;
            var num5 = -1;
            var num6 = 0.0f;
            var num7 = 99999f;
            var num8 = 99999f;
            var num9 = -1;
            var num10 = -1;
            for (var index1 = 0; index1 < this.currentMax; ++index1)
            {
                var num11 = 0;
                var index2 = this.waterfalls[index1].type;
                var x1 = this.waterfalls[index1].x;
                var y = this.waterfalls[index1].y;
                var num12 = 0;
                var num13 = 0;
                var num14 = 0;
                var num15 = 0;
                var num16 = 0;
                var index3 = 0;
                int x2;
                switch (index2)
                {
                    case 0:
                        index2 = Style;
                        goto default;
                    case 1:
                    case 14:
                        if (!Main.drewLava && this.waterfalls[index1].stopAtStep != 0)
                        {
                            x2 = 32 * this.slowFrame;
                            break;
                        }

                        continue;
                    case 2:
                        if (Main.drewLava)
                            continue;
                        goto default;
                    case 11:
                    case 22:
                        if (!Main.drewLava)
                        {
                            var num17 = this.waterfallDist / 4;
                            if (index2 == 22)
                                num17 = this.waterfallDist / 2;
                            if (this.waterfalls[index1].stopAtStep > num17)
                                this.waterfalls[index1].stopAtStep = num17;
                            if (this.waterfalls[index1].stopAtStep != 0 &&
                                (double) (y + num17) >= (double) Main.screenPosition.Y / 16.0 &&
                                ((double) x1 >= (double) Main.screenPosition.X / 16.0 - 1.0 && (double) x1 <=
                                 ((double) Main.screenPosition.X + (double) Main.screenWidth) / 16.0 + 1.0))
                            {
                                int num18;
                                int num19;
                                if (x1 % 2 == 0)
                                {
                                    num18 = this.rainFrameForeground + 3;
                                    if (num18 > 7)
                                        num18 -= 8;
                                    num19 = this.rainFrameBackground + 2;
                                    if (num19 > 7)
                                        num19 -= 8;
                                    if (index2 == 22)
                                    {
                                        num18 = this.snowFrameForeground + 3;
                                        if (num18 > 7)
                                            num18 -= 8;
                                    }
                                }
                                else
                                {
                                    num18 = this.rainFrameForeground;
                                    num19 = this.rainFrameBackground;
                                    if (index2 == 22)
                                        num18 = this.snowFrameForeground;
                                }

                                var rectangle1 = new Rectangle(num19 * 18, 0, 16, 16);
                                var rectangle2 = new Rectangle(num18 * 18, 0, 16, 16);
                                var origin = new Vector2(8f, 8f);
                                var position = y % 2 != 0
                                    ? new Vector2((float) (x1 * 16 + 8), (float) (y * 16 + 8)) - Main.screenPosition
                                    : new Vector2((float) (x1 * 16 + 9), (float) (y * 16 + 8)) - Main.screenPosition;
                                var flag = false;
                                for (var index4 = 0; index4 < num17; ++index4)
                                {
                                    var color1 = Lighting.GetColor(x1, y);
                                    var num20 = 0.6f;
                                    var num21 = 0.3f;
                                    if (index4 > num17 - 8)
                                    {
                                        var num22 = (float) (num17 - index4) / 8f;
                                        num20 *= num22;
                                        num21 *= num22;
                                    }

                                    var color2 = color1 * num20;
                                    var color3 = color1 * num21;
                                    if (index2 == 22)
                                    {
                                        spriteBatch.Draw(this.waterfallTexture[22], position,
                                            new Rectangle?(rectangle2), color2, 0.0f, origin, 1f, SpriteEffects.None,
                                            0.0f);
                                    }
                                    else
                                    {
                                        spriteBatch.Draw(this.waterfallTexture[12], position,
                                            new Rectangle?(rectangle1), color3, 0.0f, origin, 1f, SpriteEffects.None,
                                            0.0f);
                                        spriteBatch.Draw(this.waterfallTexture[11], position,
                                            new Rectangle?(rectangle2), color2, 0.0f, origin, 1f, SpriteEffects.None,
                                            0.0f);
                                    }

                                    if (!flag)
                                    {
                                        ++y;
                                        var testTile = Main.tile[x1, y];
                                        if (WorldGen.SolidTile(testTile))
                                            flag = true;
                                        if (testTile.liquid > (byte) 0)
                                        {
                                            var num22 =
                                                (int) (16.0 * ((double) testTile.liquid / (double) byte.MaxValue)) &
                                                254;
                                            if (num22 < 15)
                                            {
                                                rectangle2.Height -= num22;
                                                rectangle1.Height -= num22;
                                            }
                                            else
                                                break;
                                        }

                                        if (y % 2 == 0)
                                            ++position.X;
                                        else
                                            --position.X;
                                        position.Y += 16f;
                                    }
                                    else
                                        break;
                                }

                                this.waterfalls[index1].stopAtStep = 0;
                                continue;
                            }

                            continue;
                        }

                        continue;
                    default:
                        x2 = 32 * this.regularFrame;
                        break;
                }

                var num23 = 0;
                var num24 = this.waterfallDist;
                var color4 = Color.White;
                for (var index4 = 0; index4 < num24; ++index4)
                {
                    if (num23 < 2)
                    {
                        switch (index2)
                        {
                            case 1:
                                var num17 = (0.55f + (float) (270 - (int) Main.mouseTextColor) / 900f) * 0.4f;
                                var R1 = num17;
                                var G1 = num17 * 0.3f;
                                var B1 = num17 * 0.1f;
                                Lighting.AddLight(x1, y, R1, G1, B1);
                                break;
                            case 2:
                                var num18 = (float) Main.DiscoR / (float) byte.MaxValue;
                                var num19 = (float) Main.DiscoG / (float) byte.MaxValue;
                                var num20 = (float) Main.DiscoB / (float) byte.MaxValue;
                                var R2 = num18 * 0.2f;
                                var G2 = num19 * 0.2f;
                                var B2 = num20 * 0.2f;
                                Lighting.AddLight(x1, y, R2, G2, B2);
                                break;
                            case 15:
                                var R3 = 0.0f;
                                var G3 = 0.0f;
                                var B3 = 0.2f;
                                Lighting.AddLight(x1, y, R3, G3, B3);
                                break;
                            case 16:
                                var R4 = 0.0f;
                                var G4 = 0.2f;
                                var B4 = 0.0f;
                                Lighting.AddLight(x1, y, R4, G4, B4);
                                break;
                            case 17:
                                var R5 = 0.0f;
                                var G5 = 0.0f;
                                var B5 = 0.2f;
                                Lighting.AddLight(x1, y, R5, G5, B5);
                                break;
                            case 18:
                                var R6 = 0.0f;
                                var G6 = 0.2f;
                                var B6 = 0.0f;
                                Lighting.AddLight(x1, y, R6, G6, B6);
                                break;
                            case 19:
                                var R7 = 0.2f;
                                var G7 = 0.0f;
                                var B7 = 0.0f;
                                Lighting.AddLight(x1, y, R7, G7, B7);
                                break;
                            case 20:
                                Lighting.AddLight(x1, y, 0.2f, 0.2f, 0.2f);
                                break;
                            case 21:
                                var R8 = 0.2f;
                                var G8 = 0.0f;
                                var B8 = 0.0f;
                                Lighting.AddLight(x1, y, R8, G8, B8);
                                break;
                        }

                        var tile = Main.tile[x1, y];
                        if (tile == null)
                        {
                            tile = new Tile();
                            Main.tile[x1, y] = tile;
                        }

                        var testTile1 = Main.tile[x1 - 1, y];
                        if (testTile1 == null)
                        {
                            testTile1 = new Tile();
                            Main.tile[x1 - 1, y] = testTile1;
                        }

                        var testTile2 = Main.tile[x1, y + 1];
                        if (testTile2 == null)
                        {
                            testTile2 = new Tile();
                            Main.tile[x1, y + 1] = testTile2;
                        }

                        var testTile3 = Main.tile[x1 + 1, y];
                        if (testTile3 == null)
                        {
                            testTile3 = new Tile();
                            Main.tile[x1 + 1, y] = testTile3;
                        }

                        var num21 = (int) tile.liquid / 16;
                        var num22 = 0;
                        var num25 = num15;
                        int num26;
                        int num27;
                        if (testTile2.topSlope())
                        {
                            if (testTile2.slope() == (byte) 1)
                            {
                                num22 = 1;
                                num26 = 1;
                                num14 = 1;
                                num15 = num14;
                            }
                            else
                            {
                                num22 = -1;
                                num26 = -1;
                                num14 = -1;
                                num15 = num14;
                            }

                            num27 = 1;
                        }
                        else if ((!WorldGen.SolidTile(testTile2) && !testTile2.bottomSlope() ||
                                  testTile2.type == (ushort) 162) && !tile.halfBrick() ||
                                 !testTile2.active() && !tile.halfBrick())
                        {
                            num23 = 0;
                            num27 = 1;
                            num26 = 0;
                        }
                        else if ((WorldGen.SolidTile(testTile1) || testTile1.topSlope() ||
                                  testTile1.liquid > (byte) 0) &&
                                 (!WorldGen.SolidTile(testTile3) && testTile3.liquid == (byte) 0))
                        {
                            if (num14 == -1)
                                ++num23;
                            num26 = 1;
                            num27 = 0;
                            num14 = 1;
                        }
                        else if ((WorldGen.SolidTile(testTile3) || testTile3.topSlope() ||
                                  testTile3.liquid > (byte) 0) &&
                                 (!WorldGen.SolidTile(testTile1) && testTile1.liquid == (byte) 0))
                        {
                            if (num14 == 1)
                                ++num23;
                            num26 = -1;
                            num27 = 0;
                            num14 = -1;
                        }
                        else if ((!WorldGen.SolidTile(testTile3) && !tile.topSlope() || testTile3.liquid == (byte) 0) &&
                                 (!WorldGen.SolidTile(testTile1) && !tile.topSlope() && testTile1.liquid == (byte) 0))
                        {
                            num27 = 0;
                            num26 = num14;
                        }
                        else
                        {
                            ++num23;
                            num27 = 0;
                            num26 = 0;
                        }

                        if (num23 >= 2)
                        {
                            num14 *= -1;
                            num26 *= -1;
                        }

                        var num28 = -1;
                        if (index2 != 1 && index2 != 14)
                        {
                            if (testTile2.active())
                                num28 = (int) testTile2.type;
                            if (tile.active())
                                num28 = (int) tile.type;
                        }

                        switch (num28)
                        {
                            case -1:
                                if (WorldGen.SolidTile(testTile2) && !tile.halfBrick())
                                    num11 = 8;
                                else if (num13 != 0)
                                    num11 = 0;
                                var color1 = Lighting.GetColor(x1, y);
                                var color2 = color1;
                                float num29;
                                switch (index2)
                                {
                                    case 1:
                                        num29 = 1f;
                                        break;
                                    case 14:
                                        num29 = 0.8f;
                                        break;
                                    default:
                                        num29 = tile.wall != (byte) 0 || (double) y >= Main.worldSurface
                                            ? 0.6f * Alpha
                                            : Alpha;
                                        break;
                                }

                                if (index4 > num24 - 10)
                                    num29 *= (float) (num24 - index4) / 10f;
                                var num30 = (float) color1.R * num29;
                                var num31 = (float) color1.G * num29;
                                var num32 = (float) color1.B * num29;
                                var num33 = (float) color1.A * num29;
                                switch (index2)
                                {
                                    case 1:
                                        if ((double) num30 < 190.0 * (double) num29)
                                            num30 = 190f * num29;
                                        if ((double) num31 < 190.0 * (double) num29)
                                            num31 = 190f * num29;
                                        if ((double) num32 < 190.0 * (double) num29)
                                        {
                                            num32 = 190f * num29;
                                            break;
                                        }

                                        break;
                                    case 2:
                                        num30 = (float) Main.DiscoR * num29;
                                        num31 = (float) Main.DiscoG * num29;
                                        num32 = (float) Main.DiscoB * num29;
                                        break;
                                    default:
                                        if (index2 >= 15 && index2 <= 21)
                                        {
                                            num30 = (float) byte.MaxValue * num29;
                                            num31 = (float) byte.MaxValue * num29;
                                            num32 = (float) byte.MaxValue * num29;
                                            break;
                                        }

                                        break;
                                }

                                color1 = new Color((int) num30, (int) num31, (int) num32, (int) num33);
                                if (index2 == 1)
                                {
                                    var num34 =
                                        Math.Abs((float) (x1 * 16 + 8) -
                                                 (Main.screenPosition.X + (float) (Main.screenWidth / 2)));
                                    var num35 =
                                        Math.Abs((float) (y * 16 + 8) -
                                                 (Main.screenPosition.Y + (float) (Main.screenHeight / 2)));
                                    if ((double) num34 < (double) (Main.screenWidth * 2) &&
                                        (double) num35 < (double) (Main.screenHeight * 2))
                                    {
                                        var num36 =
                                            (float) (1.0 - Math.Sqrt(
                                                         (double) num34 * (double) num34 +
                                                         (double) num35 * (double) num35) /
                                                     ((double) Main.screenWidth * 0.75));
                                        if ((double) num36 > 0.0)
                                            num6 += num36;
                                    }

                                    if ((double) num34 < (double) num7)
                                    {
                                        num7 = num34;
                                        num9 = x1 * 16 + 8;
                                    }

                                    if ((double) num35 < (double) num8)
                                    {
                                        num8 = num34;
                                        num10 = y * 16 + 8;
                                    }
                                }
                                else if (index2 != 1 && index2 != 14 && (index2 != 11 && index2 != 12) && index2 != 22)
                                {
                                    var num34 =
                                        Math.Abs((float) (x1 * 16 + 8) -
                                                 (Main.screenPosition.X + (float) (Main.screenWidth / 2)));
                                    var num35 =
                                        Math.Abs((float) (y * 16 + 8) -
                                                 (Main.screenPosition.Y + (float) (Main.screenHeight / 2)));
                                    if ((double) num34 < (double) (Main.screenWidth * 2) &&
                                        (double) num35 < (double) (Main.screenHeight * 2))
                                    {
                                        var num36 =
                                            (float) (1.0 - Math.Sqrt(
                                                         (double) num34 * (double) num34 +
                                                         (double) num35 * (double) num35) /
                                                     ((double) Main.screenWidth * 0.75));
                                        if ((double) num36 > 0.0)
                                            num1 += num36;
                                    }

                                    if ((double) num34 < (double) num2)
                                    {
                                        num2 = num34;
                                        num4 = x1 * 16 + 8;
                                    }

                                    if ((double) num35 < (double) num3)
                                    {
                                        num3 = num34;
                                        num5 = y * 16 + 8;
                                    }
                                }

                                if (index4 > 50 &&
                                    (color2.R > (byte) 20 || color2.B > (byte) 20 || color2.G > (byte) 20))
                                {
                                    var num34 = (float) color2.R;
                                    if ((double) color2.G > (double) num34)
                                        num34 = (float) color2.G;
                                    if ((double) color2.B > (double) num34)
                                        num34 = (float) color2.B;
                                    if ((double) Main.rand.Next(20000) < (double) num34 / 30.0)
                                    {
                                        var index5 =
                                            Dust.NewDust(
                                                new Vector2((float) (x1 * 16 - num14 * 7), (float) (y * 16 + 6)), 10, 8,
                                                43, 0.0f, 0.0f, 254, Color.White, 0.5f);
                                        Main.dust[index5].velocity *= 0.0f;
                                    }
                                }

                                if (num12 == 0 && num22 != 0 && (num13 == 1 && num14 != num15))
                                {
                                    num22 = 0;
                                    num14 = num15;
                                    color1 = Color.White;
                                    if (num14 == 1)
                                        spriteBatch.Draw(this.waterfallTexture[index2],
                                            new Vector2((float) (x1 * 16 - 16), (float) (y * 16 + 16)) -
                                            Main.screenPosition, new Rectangle?(new Rectangle(x2, 24, 32, 16 - num21)),
                                            color1, 0.0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0.0f);
                                    else
                                        spriteBatch.Draw(this.waterfallTexture[index2],
                                            new Vector2((float) (x1 * 16 - 16), (float) (y * 16 + 16)) -
                                            Main.screenPosition, new Rectangle?(new Rectangle(x2, 24, 32, 8)), color1,
                                            0.0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0.0f);
                                }

                                if (num16 != 0 && num26 == 0 && num27 == 1)
                                {
                                    if (num14 == 1)
                                    {
                                        if (index3 != index2)
                                            spriteBatch.Draw(this.waterfallTexture[index3],
                                                new Vector2((float) (x1 * 16), (float) (y * 16 + num11 + 8)) -
                                                Main.screenPosition,
                                                new Rectangle?(new Rectangle(x2, 0, 16, 16 - num21 - 8)), color4, 0.0f,
                                                Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0.0f);
                                        else
                                            spriteBatch.Draw(this.waterfallTexture[index2],
                                                new Vector2((float) (x1 * 16), (float) (y * 16 + num11 + 8)) -
                                                Main.screenPosition,
                                                new Rectangle?(new Rectangle(x2, 0, 16, 16 - num21 - 8)), color1, 0.0f,
                                                Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0.0f);
                                    }
                                    else
                                        spriteBatch.Draw(this.waterfallTexture[index2],
                                            new Vector2((float) (x1 * 16), (float) (y * 16 + num11 + 8)) -
                                            Main.screenPosition,
                                            new Rectangle?(new Rectangle(x2, 0, 16, 16 - num21 - 8)), color1, 0.0f,
                                            Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
                                }

                                if (num11 == 8 && num13 == 1 && num16 == 0)
                                {
                                    if (num15 == -1)
                                    {
                                        if (index3 != index2)
                                            spriteBatch.Draw(this.waterfallTexture[index3],
                                                new Vector2((float) (x1 * 16), (float) (y * 16)) - Main.screenPosition,
                                                new Rectangle?(new Rectangle(x2, 24, 32, 8)), color4, 0.0f,
                                                new Vector2(), 1f, SpriteEffects.None, 0.0f);
                                        else
                                            spriteBatch.Draw(this.waterfallTexture[index2],
                                                new Vector2((float) (x1 * 16), (float) (y * 16)) - Main.screenPosition,
                                                new Rectangle?(new Rectangle(x2, 24, 32, 8)), color1, 0.0f,
                                                new Vector2(), 1f, SpriteEffects.None, 0.0f);
                                    }
                                    else if (index3 != index2)
                                        spriteBatch.Draw(this.waterfallTexture[index3],
                                            new Vector2((float) (x1 * 16 - 16), (float) (y * 16)) - Main.screenPosition,
                                            new Rectangle?(new Rectangle(x2, 24, 32, 8)), color4, 0.0f, new Vector2(),
                                            1f, SpriteEffects.FlipHorizontally, 0.0f);
                                    else
                                        spriteBatch.Draw(this.waterfallTexture[index2],
                                            new Vector2((float) (x1 * 16 - 16), (float) (y * 16)) - Main.screenPosition,
                                            new Rectangle?(new Rectangle(x2, 24, 32, 8)), color1, 0.0f, new Vector2(),
                                            1f, SpriteEffects.FlipHorizontally, 0.0f);
                                }

                                if (num22 != 0 && num12 == 0)
                                {
                                    if (num25 == 1)
                                    {
                                        if (index3 != index2)
                                            spriteBatch.Draw(this.waterfallTexture[index3],
                                                new Vector2((float) (x1 * 16 - 16), (float) (y * 16)) -
                                                Main.screenPosition,
                                                new Rectangle?(new Rectangle(x2, 24, 32, 16 - num21)), color4, 0.0f,
                                                new Vector2(), 1f, SpriteEffects.FlipHorizontally, 0.0f);
                                        else
                                            spriteBatch.Draw(this.waterfallTexture[index2],
                                                new Vector2((float) (x1 * 16 - 16), (float) (y * 16)) -
                                                Main.screenPosition,
                                                new Rectangle?(new Rectangle(x2, 24, 32, 16 - num21)), color1, 0.0f,
                                                new Vector2(), 1f, SpriteEffects.FlipHorizontally, 0.0f);
                                    }
                                    else if (index3 != index2)
                                        spriteBatch.Draw(this.waterfallTexture[index3],
                                            new Vector2((float) (x1 * 16), (float) (y * 16)) - Main.screenPosition,
                                            new Rectangle?(new Rectangle(x2, 24, 32, 16 - num21)), color4, 0.0f,
                                            new Vector2(), 1f, SpriteEffects.None, 0.0f);
                                    else
                                        spriteBatch.Draw(this.waterfallTexture[index2],
                                            new Vector2((float) (x1 * 16), (float) (y * 16)) - Main.screenPosition,
                                            new Rectangle?(new Rectangle(x2, 24, 32, 16 - num21)), color1, 0.0f,
                                            new Vector2(), 1f, SpriteEffects.None, 0.0f);
                                }

                                if (num27 == 1 && num22 == 0 && num16 == 0)
                                {
                                    if (num14 == -1)
                                    {
                                        if (num13 == 0)
                                            spriteBatch.Draw(this.waterfallTexture[index2],
                                                new Vector2((float) (x1 * 16), (float) (y * 16 + num11)) -
                                                Main.screenPosition,
                                                new Rectangle?(new Rectangle(x2, 0, 16, 16 - num21)), color1, 0.0f,
                                                new Vector2(), 1f, SpriteEffects.None, 0.0f);
                                        else if (index3 != index2)
                                            spriteBatch.Draw(this.waterfallTexture[index3],
                                                new Vector2((float) (x1 * 16), (float) (y * 16)) - Main.screenPosition,
                                                new Rectangle?(new Rectangle(x2, 24, 32, 16 - num21)), color4, 0.0f,
                                                new Vector2(), 1f, SpriteEffects.None, 0.0f);
                                        else
                                            spriteBatch.Draw(this.waterfallTexture[index2],
                                                new Vector2((float) (x1 * 16), (float) (y * 16)) - Main.screenPosition,
                                                new Rectangle?(new Rectangle(x2, 24, 32, 16 - num21)), color1, 0.0f,
                                                new Vector2(), 1f, SpriteEffects.None, 0.0f);
                                    }
                                    else if (num13 == 0)
                                        spriteBatch.Draw(this.waterfallTexture[index2],
                                            new Vector2((float) (x1 * 16), (float) (y * 16 + num11)) -
                                            Main.screenPosition, new Rectangle?(new Rectangle(x2, 0, 16, 16 - num21)),
                                            color1, 0.0f, new Vector2(), 1f, SpriteEffects.FlipHorizontally, 0.0f);
                                    else if (index3 != index2)
                                        spriteBatch.Draw(this.waterfallTexture[index3],
                                            new Vector2((float) (x1 * 16 - 16), (float) (y * 16)) - Main.screenPosition,
                                            new Rectangle?(new Rectangle(x2, 24, 32, 16 - num21)), color4, 0.0f,
                                            new Vector2(), 1f, SpriteEffects.FlipHorizontally, 0.0f);
                                    else
                                        spriteBatch.Draw(this.waterfallTexture[index2],
                                            new Vector2((float) (x1 * 16 - 16), (float) (y * 16)) - Main.screenPosition,
                                            new Rectangle?(new Rectangle(x2, 24, 32, 16 - num21)), color1, 0.0f,
                                            new Vector2(), 1f, SpriteEffects.FlipHorizontally, 0.0f);
                                }
                                else
                                {
                                    switch (num26)
                                    {
                                        case -1:
                                            if (Main.tile[x1, y].liquid <= (byte) 0 || Main.tile[x1, y].halfBrick())
                                            {
                                                if (num22 == -1)
                                                {
                                                    for (var index5 = 0; index5 < 8; ++index5)
                                                    {
                                                        var num34 = index5 * 2;
                                                        var num35 = index5 * 2;
                                                        var num36 = 14 - index5 * 2;
                                                        num11 = 8;
                                                        if (num12 == 0 && index5 > 5)
                                                            num36 = 4;
                                                        spriteBatch.Draw(this.waterfallTexture[index2],
                                                            new Vector2((float) (x1 * 16 + num34),
                                                                (float) (y * 16 + num11 + num36)) - Main.screenPosition,
                                                            new Rectangle?(new Rectangle(16 + x2 + num35, 0, 2,
                                                                16 - num11)), color1, 0.0f, new Vector2(), 1f,
                                                            SpriteEffects.FlipHorizontally, 0.0f);
                                                    }

                                                    break;
                                                }

                                                spriteBatch.Draw(this.waterfallTexture[index2],
                                                    new Vector2((float) (x1 * 16), (float) (y * 16 + num11)) -
                                                    Main.screenPosition,
                                                    new Rectangle?(new Rectangle(16 + x2, 0, 16, 16)), color1, 0.0f,
                                                    new Vector2(), 1f, SpriteEffects.None, 0.0f);
                                                break;
                                            }

                                            break;
                                        case 0:
                                            if (num27 == 0)
                                            {
                                                if (Main.tile[x1, y].liquid <= (byte) 0 || Main.tile[x1, y].halfBrick())
                                                    spriteBatch.Draw(this.waterfallTexture[index2],
                                                        new Vector2((float) (x1 * 16), (float) (y * 16 + num11)) -
                                                        Main.screenPosition,
                                                        new Rectangle?(new Rectangle(16 + x2, 0, 16, 16)), color1, 0.0f,
                                                        new Vector2(), 1f, SpriteEffects.None, 0.0f);
                                                index4 = 1000;
                                                break;
                                            }

                                            break;
                                        case 1:
                                            if (Main.tile[x1, y].liquid <= (byte) 0 || Main.tile[x1, y].halfBrick())
                                            {
                                                if (num22 == 1)
                                                {
                                                    for (var index5 = 0; index5 < 8; ++index5)
                                                    {
                                                        var num34 = index5 * 2;
                                                        var num35 = 14 - index5 * 2;
                                                        var num36 = num34;
                                                        num11 = 8;
                                                        if (num12 == 0 && index5 < 2)
                                                            num36 = 4;
                                                        spriteBatch.Draw(this.waterfallTexture[index2],
                                                            new Vector2((float) (x1 * 16 + num34),
                                                                (float) (y * 16 + num11 + num36)) - Main.screenPosition,
                                                            new Rectangle?(new Rectangle(16 + x2 + num35, 0, 2,
                                                                16 - num11)), color1, 0.0f, new Vector2(), 1f,
                                                            SpriteEffects.FlipHorizontally, 0.0f);
                                                    }

                                                    break;
                                                }

                                                spriteBatch.Draw(this.waterfallTexture[index2],
                                                    new Vector2((float) (x1 * 16), (float) (y * 16 + num11)) -
                                                    Main.screenPosition,
                                                    new Rectangle?(new Rectangle(16 + x2, 0, 16, 16)), color1, 0.0f,
                                                    new Vector2(), 1f, SpriteEffects.FlipHorizontally, 0.0f);
                                                break;
                                            }

                                            break;
                                    }
                                }

                                if (tile.liquid > (byte) 0 && !tile.halfBrick())
                                    index4 = 1000;
                                num13 = num27;
                                num15 = num14;
                                num12 = num26;
                                x1 += num26;
                                y += num27;
                                num16 = num22;
                                color4 = color1;
                                if (index3 != index2)
                                    index3 = index2;
                                if (testTile1.active() &&
                                    (testTile1.type == (ushort) 189 || testTile1.type == (ushort) 196) ||
                                    testTile3.active() &&
                                    (testTile3.type == (ushort) 189 || testTile3.type == (ushort) 196) ||
                                    testTile2.active() &&
                                    (testTile2.type == (ushort) 189 || testTile2.type == (ushort) 196))
                                {
                                    num24 = (int) ((double) (40 * (Main.maxTilesX / 4200)) * (double) Main.gfxQuality);
                                    continue;
                                }

                                continue;
                            case 160:
                                index2 = 2;
                                goto case -1;
                            default:
                                if (num28 >= 262 && num28 <= 268)
                                {
                                    index2 = 15 + num28 - 262;
                                    goto case -1;
                                }
                                else
                                    goto case -1;
                        }
                    }
                }
            }

            Main.ambientWaterfallX = (float) num4;
            Main.ambientWaterfallY = (float) num5;
            Main.ambientWaterfallStrength = num1;
            Main.ambientLavafallX = (float) num9;
            Main.ambientLavafallY = (float) num10;
            Main.ambientLavafallStrength = num6;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (var index = 0; index < this.currentMax; ++index)
                this.waterfalls[index].stopAtStep = this.waterfallDist;
            Main.drewLava = false;
            if ((double) Main.liquidAlpha[0] > 0.0)
                this.DrawWaterfall(spriteBatch, 0, Main.liquidAlpha[0]);
            if ((double) Main.liquidAlpha[2] > 0.0)
                this.DrawWaterfall(spriteBatch, 3, Main.liquidAlpha[2]);
            if ((double) Main.liquidAlpha[3] > 0.0)
                this.DrawWaterfall(spriteBatch, 4, Main.liquidAlpha[3]);
            if ((double) Main.liquidAlpha[4] > 0.0)
                this.DrawWaterfall(spriteBatch, 5, Main.liquidAlpha[4]);
            if ((double) Main.liquidAlpha[5] > 0.0)
                this.DrawWaterfall(spriteBatch, 6, Main.liquidAlpha[5]);
            if ((double) Main.liquidAlpha[6] > 0.0)
                this.DrawWaterfall(spriteBatch, 7, Main.liquidAlpha[6]);
            if ((double) Main.liquidAlpha[7] > 0.0)
                this.DrawWaterfall(spriteBatch, 8, Main.liquidAlpha[7]);
            if ((double) Main.liquidAlpha[8] > 0.0)
                this.DrawWaterfall(spriteBatch, 9, Main.liquidAlpha[8]);
            if ((double) Main.liquidAlpha[9] > 0.0)
                this.DrawWaterfall(spriteBatch, 10, Main.liquidAlpha[9]);
            if ((double) Main.liquidAlpha[10] <= 0.0)
                return;
            this.DrawWaterfall(spriteBatch, 13, Main.liquidAlpha[10]);
        }

        public struct WaterfallData
        {
            public int x;
            public int y;
            public int type;
            public int stopAtStep;
        }
    }
}