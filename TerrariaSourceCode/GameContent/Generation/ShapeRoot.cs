﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Generation.ShapeRoot
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using System;
using Terraria.World.Generation;

namespace Terraria.GameContent.Generation
{
    public class ShapeRoot : GenShape
    {
        private float _angle;
        private float _startingSize;
        private float _endingSize;
        private float _distance;

        public ShapeRoot(float angle, float distance = 10f, float startingSize = 4f, float endingSize = 1f)
        {
            this._angle = angle;
            this._distance = distance;
            this._startingSize = startingSize;
            this._endingSize = endingSize;
        }

        private bool DoRoot(Point origin, GenAction action, float angle, float distance, float startingSize)
        {
            var x = (float) origin.X;
            var y = (float) origin.Y;
            for (var num1 = 0.0f; (double) num1 < (double) distance * 0.850000023841858; ++num1)
            {
                var amount = num1 / distance;
                var num2 = MathHelper.Lerp(startingSize, this._endingSize, amount);
                x += (float) Math.Cos((double) angle);
                y += (float) Math.Sin((double) angle);
                angle += (float) ((double) GenBase._random.NextFloat() - 0.5 + (double) GenBase._random.NextFloat() *
                                  ((double) this._angle - 1.57079637050629) * 0.100000001490116 *
                                  (1.0 - (double) amount));
                angle = (float) ((double) angle * 0.400000005960464 +
                                 0.449999988079071 * (double) MathHelper.Clamp(angle,
                                     this._angle - (float) (2.0 * (1.0 - 0.5 * (double) amount)),
                                     this._angle + (float) (2.0 * (1.0 - 0.5 * (double) amount))) +
                                 (double) MathHelper.Lerp(this._angle, 1.570796f, amount) * 0.150000005960464);
                for (var index1 = 0; index1 < (int) num2; ++index1)
                {
                    for (var index2 = 0; index2 < (int) num2; ++index2)
                    {
                        if (!this.UnitApply(action, origin, (int) x + index1, (int) y + index2) && this._quitOnFail)
                            return false;
                    }
                }
            }

            return true;
        }

        public override bool Perform(Point origin, GenAction action)
        {
            return this.DoRoot(origin, action, this._angle, this._distance, this._startingSize);
        }
    }
}