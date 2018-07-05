﻿// Decompiled with JetBrains decompiler
// Type: Terraria.Cloud
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Utilities;

namespace Terraria
{
    public class Cloud
    {
        private static UnifiedRandom rand = new UnifiedRandom();
        public Vector2 position;
        public float scale;
        public float rotation;
        public float rSpeed;
        public float sSpeed;
        public bool active;
        public SpriteEffects spriteDir;
        public int type;
        public int width;
        public int height;
        public float Alpha;
        public bool kill;

        public static void resetClouds()
        {
            if (Main.dedServ || Main.cloudLimit < 10)
                return;
            Main.windSpeed = Main.windSpeedSet;
            for (var index = 0; index < 200; ++index)
                Main.cloud[index].active = false;
            for (var index = 0; index < Main.numClouds; ++index)
            {
                Cloud.addCloud();
                Main.cloud[index].Alpha = 1f;
            }

            for (var index = 0; index < 200; ++index)
                Main.cloud[index].Alpha = 1f;
        }

        public static void addCloud()
        {
            if (Main.netMode == 2)
                return;
            var index1 = -1;
            for (var index2 = 0; index2 < 200; ++index2)
            {
                if (!Main.cloud[index2].active)
                {
                    index1 = index2;
                    break;
                }
            }

            if (index1 < 0)
                return;
            Main.cloud[index1].kill = false;
            Main.cloud[index1].rSpeed = 0.0f;
            Main.cloud[index1].sSpeed = 0.0f;
            Main.cloud[index1].scale = (float) Cloud.rand.Next(70, 131) * 0.01f;
            Main.cloud[index1].rotation = (float) Cloud.rand.Next(-10, 11) * 0.01f;
            Main.cloud[index1].width = (int) ((double) Main.cloudTexture[Main.cloud[index1].type].Width *
                                              (double) Main.cloud[index1].scale);
            Main.cloud[index1].height = (int) ((double) Main.cloudTexture[Main.cloud[index1].type].Height *
                                               (double) Main.cloud[index1].scale);
            Main.cloud[index1].Alpha = 0.0f;
            Main.cloud[index1].spriteDir = SpriteEffects.None;
            if (Cloud.rand.Next(2) == 0)
                Main.cloud[index1].spriteDir = SpriteEffects.FlipHorizontally;
            var num1 = Main.windSpeed;
            if (!Main.gameMenu)
                num1 = Main.windSpeed - Main.player[Main.myPlayer].velocity.X * 0.1f;
            var num2 = 0;
            var num3 = 0;
            if ((double) num1 > 0.0)
                num2 -= 200;
            if ((double) num1 < 0.0)
                num3 += 200;
            var num4 = 300;
            var num5 = (float) WorldGen.genRand.Next(num2 - num4, Main.screenWidth + num3 + num4);
            Main.cloud[index1].Alpha = 0.0f;
            Main.cloud[index1].position.Y = (float) Cloud.rand.Next((int) ((double) -Main.screenHeight * 0.25),
                (int) ((double) Main.screenHeight * 0.25));
            Main.cloud[index1].position.Y -=
                (float) Cloud.rand.Next((int) ((double) Main.screenHeight * 0.150000005960464));
            Main.cloud[index1].position.Y -=
                (float) Cloud.rand.Next((int) ((double) Main.screenHeight * 0.150000005960464));
            Main.cloud[index1].type = Cloud.rand.Next(4);
            if ((double) Main.cloudAlpha > 0.0 && Cloud.rand.Next(4) != 0 ||
                (double) Main.cloudBGActive >= 1.0 && Cloud.rand.Next(2) == 0)
            {
                Main.cloud[index1].type = Cloud.rand.Next(18, 22);
                if ((double) Main.cloud[index1].scale >= 1.15)
                    Main.cloud[index1].position.Y -= 150f;
                if ((double) Main.cloud[index1].scale >= 1.0)
                    Main.cloud[index1].position.Y -= 150f;
            }
            else if (((double) Main.cloudBGActive <= 0.0 && (double) Main.cloudAlpha == 0.0 &&
                      ((double) Main.cloud[index1].scale < 1.0 && (double) Main.cloud[index1].position.Y <
                       (double) -Main.screenHeight * 0.200000002980232) ||
                      (double) Main.cloud[index1].position.Y < (double) -Main.screenHeight * 0.200000002980232) &&
                     (double) Main.numClouds < 50.0)
                Main.cloud[index1].type = Cloud.rand.Next(9, 14);
            else if (((double) Main.cloud[index1].scale < 1.15 && (double) Main.cloud[index1].position.Y <
                      (double) -Main.screenHeight * 0.300000011920929 ||
                      (double) Main.cloud[index1].scale < 0.85 && (double) Main.cloud[index1].position.Y <
                      (double) Main.screenHeight * 0.150000005960464) &&
                     ((double) Main.numClouds > 70.0 || (double) Main.cloudBGActive >= 1.0))
                Main.cloud[index1].type = Cloud.rand.Next(4, 9);
            else if ((double) Main.cloud[index1].position.Y > (double) -Main.screenHeight * 0.150000005960464 &&
                     Cloud.rand.Next(2) == 0 && (double) Main.numClouds > 20.0)
                Main.cloud[index1].type = Cloud.rand.Next(14, 18);

            if ((double) Main.cloud[index1].scale > 1.2)
                Main.cloud[index1].position.Y += 100f;
            if ((double) Main.cloud[index1].scale > 1.3)
                Main.cloud[index1].scale = 1.3f;
            if ((double) Main.cloud[index1].scale < 0.7)
                Main.cloud[index1].scale = 0.7f;
            Main.cloud[index1].active = true;
            Main.cloud[index1].position.X = num5;
            if ((double) Main.cloud[index1].position.X > (double) (Main.screenWidth + 100))
                Main.cloud[index1].Alpha = 1f;
            if ((double) Main.cloud[index1].position.X + (double) Main.cloudTexture[Main.cloud[index1].type].Width *
                (double) Main.cloud[index1].scale < -100.0)
                Main.cloud[index1].Alpha = 1f;
            var rectangle1 = new Rectangle((int) Main.cloud[index1].position.X,
                (int) Main.cloud[index1].position.Y, Main.cloud[index1].width, Main.cloud[index1].height);
            for (var index2 = 0; index2 < 200; ++index2)
            {
                if (index1 != index2 && Main.cloud[index2].active)
                {
                    var rectangle2 = new Rectangle((int) Main.cloud[index2].position.X,
                        (int) Main.cloud[index2].position.Y, Main.cloud[index2].width, Main.cloud[index2].height);
                    if (rectangle1.Intersects(rectangle2))
                        Main.cloud[index1].active = false;
                }
            }
        }

        public Color cloudColor(Color bgColor)
        {
            var num = this.scale * this.Alpha;
            if ((double) num > 1.0)
                num = 1f;
            return new Color((int) (byte) (float) (int) ((double) bgColor.R * (double) num),
                (int) (byte) (float) (int) ((double) bgColor.G * (double) num),
                (int) (byte) (float) (int) ((double) bgColor.B * (double) num),
                (int) (byte) (float) (int) ((double) bgColor.A * (double) num));
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public static void UpdateClouds()
        {
            if (Main.netMode == 2)
                return;
            var maxValue = 0;
            for (var index = 0; index < 200; ++index)
            {
                if (Main.cloud[index].active)
                {
                    Main.cloud[index].Update();
                    if (!Main.cloud[index].kill)
                        ++maxValue;
                }
            }

            for (var index = 0; index < 200; ++index)
            {
                if (Main.cloud[index].active)
                {
                    if (index > 1 && (!Main.cloud[index - 1].active ||
                                      (double) Main.cloud[index - 1].scale > (double) Main.cloud[index].scale + 0.02))
                    {
                        var cloud = (Cloud) Main.cloud[index - 1].Clone();
                        Main.cloud[index - 1] = (Cloud) Main.cloud[index].Clone();
                        Main.cloud[index] = cloud;
                    }

                    if (index < 199 && (!Main.cloud[index].active ||
                                        (double) Main.cloud[index + 1].scale < (double) Main.cloud[index].scale - 0.02))
                    {
                        var cloud = (Cloud) Main.cloud[index + 1].Clone();
                        Main.cloud[index + 1] = (Cloud) Main.cloud[index].Clone();
                        Main.cloud[index] = cloud;
                    }
                }
            }

            if (maxValue < Main.numClouds)
            {
                Cloud.addCloud();
            }
            else
            {
                if (maxValue <= Main.numClouds)
                    return;
                var index1 = Cloud.rand.Next(maxValue);
                for (var index2 = 0; Main.cloud[index1].kill && index2 < 100; index1 = Cloud.rand.Next(maxValue))
                    ++index2;
                Main.cloud[index1].kill = true;
            }
        }

        public void Update()
        {
            if (Main.gameMenu)
            {
                this.position.X += (float) ((double) Main.windSpeed * (double) this.scale * 3.0);
            }
            else
            {
                if ((double) this.scale == 1.0)
                    this.scale -= 0.0001f;
                if ((double) this.scale == 1.15)
                    this.scale -= 0.0001f;
                float num1;
                if ((double) this.scale < 1.0)
                {
                    var num2 = 0.07f;
                    var num3 = (float) (((double) (this.scale + 0.15f) + 1.0) / 2.0);
                    var num4 = num3 * num3;
                    num1 = num2 * num4;
                }
                else if ((double) this.scale <= 1.15)
                {
                    var num2 = 0.19f;
                    var num3 = this.scale - 0.075f;
                    var num4 = num3 * num3;
                    num1 = num2 * num4;
                }
                else
                {
                    var num2 = 0.23f;
                    var num3 = (float) ((double) this.scale - 0.150000005960464 - 0.0750000029802322);
                    var num4 = num3 * num3;
                    num1 = num2 * num4;
                }

                this.position.X += (float) ((double) Main.windSpeed * (double) num1 * 5.0) * (float) Main.dayRate;
                this.position.X -= (Main.screenPosition.X - Main.screenLastPosition.X) * num1;
            }

            var num = 600f;
            if (!this.kill)
            {
                if ((double) this.Alpha < 1.0)
                {
                    this.Alpha += 1f / 1000f * (float) Main.dayRate;
                    if ((double) this.Alpha > 1.0)
                        this.Alpha = 1f;
                }
            }
            else
            {
                this.Alpha -= 1f / 1000f * (float) Main.dayRate;
                if ((double) this.Alpha <= 0.0)
                    this.active = false;
            }

            if ((double) this.position.X + (double) Main.cloudTexture[this.type].Width * (double) this.scale <
                -(double) num || (double) this.position.X > (double) Main.screenWidth + (double) num)
                this.active = false;
            this.rSpeed += (float) Cloud.rand.Next(-10, 11) * 2E-05f;
            if ((double) this.rSpeed > 0.0002)
                this.rSpeed = 0.0002f;
            if ((double) this.rSpeed < -0.0002)
                this.rSpeed = -0.0002f;
            if ((double) this.rotation > 0.02)
                this.rotation = 0.02f;
            if ((double) this.rotation < -0.02)
                this.rotation = -0.02f;
            this.rotation += this.rSpeed;
            this.width = (int) ((double) Main.cloudTexture[this.type].Width * (double) this.scale);
            this.height = (int) ((double) Main.cloudTexture[this.type].Height * (double) this.scale);
            if (this.type < 9 || this.type > 13 || (double) Main.cloudAlpha <= 0.0 && (double) Main.cloudBGActive < 1.0)
                return;
            this.kill = true;
        }
    }
}