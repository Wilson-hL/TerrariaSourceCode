﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Generation.ShapeRunner
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using System;
using Terraria.World.Generation;

namespace Terraria.GameContent.Generation
{
    public class ShapeRunner : GenShape
    {
        private float _startStrength;
        private int _steps;
        private Vector2 _startVelocity;

        public ShapeRunner(float strength, int steps, Vector2 velocity)
        {
            this._startStrength = strength;
            this._steps = steps;
            this._startVelocity = velocity;
        }

        public override bool Perform(Point origin, GenAction action)
        {
            float num1 = (float) this._steps;
            float steps = (float) this._steps;
            double num2 = (double) this._startStrength;
            Vector2 vector2_1 = new Vector2((float) origin.X, (float) origin.Y);
            Vector2 vector2_2 = this._startVelocity == Vector2.Zero
                ? Utils.RandomVector2(GenBase._random, -1f, 1f)
                : this._startVelocity;
            while ((double) num1 > 0.0 && num2 > 0.0)
            {
                num2 = (double) this._startStrength * ((double) num1 / (double) steps);
                float num3 = num1 - 1f;
                int num4 = Math.Max(1, (int) ((double) vector2_1.X - num2 * 0.5));
                int num5 = Math.Max(1, (int) ((double) vector2_1.Y - num2 * 0.5));
                int num6 = Math.Min(GenBase._worldWidth, (int) ((double) vector2_1.X + num2 * 0.5));
                int num7 = Math.Min(GenBase._worldHeight, (int) ((double) vector2_1.Y + num2 * 0.5));
                for (int x = num4; x < num6; ++x)
                {
                    for (int y = num5; y < num7; ++y)
                    {
                        if ((double) Math.Abs((float) x - vector2_1.X) + (double) Math.Abs((float) y - vector2_1.Y) <
                            num2 * 0.5 * (1.0 + (double) GenBase._random.Next(-10, 11) * 0.015))
                            this.UnitApply(action, origin, x, y);
                    }
                }

                int num8 = (int) (num2 / 50.0) + 1;
                num1 = num3 - (float) num8;
                vector2_1 += vector2_2;
                for (int index = 0; index < num8; ++index)
                {
                    vector2_1 += vector2_2;
                    vector2_2 += Utils.RandomVector2(GenBase._random, -0.5f, 0.5f);
                }

                vector2_2 = Vector2.Clamp(vector2_2 + Utils.RandomVector2(GenBase._random, -0.5f, 0.5f), -Vector2.One,
                    Vector2.One);
            }

            return true;
        }
    }
}