﻿// Decompiled with JetBrains decompiler
// Type: Terraria.World.Generation.Shapes
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using System;

namespace Terraria.World.Generation
{
    public static class Shapes
    {
        public class Circle : GenShape
        {
            private int _verticalRadius;
            private int _horizontalRadius;

            public Circle(int radius)
            {
                this._verticalRadius = radius;
                this._horizontalRadius = radius;
            }

            public Circle(int horizontalRadius, int verticalRadius)
            {
                this._horizontalRadius = horizontalRadius;
                this._verticalRadius = verticalRadius;
            }

            public override bool Perform(Point origin, GenAction action)
            {
                var num1 = (this._horizontalRadius + 1) * (this._horizontalRadius + 1);
                for (var y = origin.Y - this._verticalRadius; y <= origin.Y + this._verticalRadius; ++y)
                {
                    var num2 = (float) this._horizontalRadius / (float) this._verticalRadius * (float) (y - origin.Y);
                    var num3 = Math.Min(this._horizontalRadius,
                        (int) Math.Sqrt((double) num1 - (double) num2 * (double) num2));
                    for (var x = origin.X - num3; x <= origin.X + num3; ++x)
                    {
                        if (!this.UnitApply(action, origin, x, y) && this._quitOnFail)
                            return false;
                    }
                }

                return true;
            }
        }

        public class HalfCircle : GenShape
        {
            private int _radius;

            public HalfCircle(int radius)
            {
                this._radius = radius;
            }

            public override bool Perform(Point origin, GenAction action)
            {
                var num1 = (this._radius + 1) * (this._radius + 1);
                for (var y = origin.Y - this._radius; y <= origin.Y; ++y)
                {
                    var num2 = Math.Min(this._radius,
                        (int) Math.Sqrt((double) (num1 - (y - origin.Y) * (y - origin.Y))));
                    for (var x = origin.X - num2; x <= origin.X + num2; ++x)
                    {
                        if (!this.UnitApply(action, origin, x, y) && this._quitOnFail)
                            return false;
                    }
                }

                return true;
            }
        }

        public class Slime : GenShape
        {
            private int _radius;
            private float _xScale;
            private float _yScale;

            public Slime(int radius)
            {
                this._radius = radius;
                this._xScale = 1f;
                this._yScale = 1f;
            }

            public Slime(int radius, float xScale, float yScale)
            {
                this._radius = radius;
                this._xScale = xScale;
                this._yScale = yScale;
            }

            public override bool Perform(Point origin, GenAction action)
            {
                var radius = (float) this._radius;
                var num1 = (this._radius + 1) * (this._radius + 1);
                for (var y = origin.Y - (int) ((double) radius * (double) this._yScale); y <= origin.Y; ++y)
                {
                    var num2 = (float) (y - origin.Y) / this._yScale;
                    var num3 = (int) Math.Min((float) this._radius * this._xScale,
                        this._xScale * (float) Math.Sqrt((double) num1 - (double) num2 * (double) num2));
                    for (var x = origin.X - num3; x <= origin.X + num3; ++x)
                    {
                        if (!this.UnitApply(action, origin, x, y) && this._quitOnFail)
                            return false;
                    }
                }

                for (var y = origin.Y + 1;
                    y <= origin.Y + (int) ((double) radius * (double) this._yScale * 0.5) - 1;
                    ++y)
                {
                    var num2 = (float) (y - origin.Y) * (2f / this._yScale);
                    var num3 = (int) Math.Min((float) this._radius * this._xScale,
                        this._xScale * (float) Math.Sqrt((double) num1 - (double) num2 * (double) num2));
                    for (var x = origin.X - num3; x <= origin.X + num3; ++x)
                    {
                        if (!this.UnitApply(action, origin, x, y) && this._quitOnFail)
                            return false;
                    }
                }

                return true;
            }
        }

        public class Rectangle : GenShape
        {
            private int _width;
            private int _height;

            public Rectangle(int width, int height)
            {
                this._width = width;
                this._height = height;
            }

            public override bool Perform(Point origin, GenAction action)
            {
                for (var x = origin.X; x < origin.X + this._width; ++x)
                {
                    for (var y = origin.Y; y < origin.Y + this._height; ++y)
                    {
                        if (!this.UnitApply(action, origin, x, y) && this._quitOnFail)
                            return false;
                    }
                }

                return true;
            }
        }

        public class Tail : GenShape
        {
            private float _width;
            private Vector2 _endOffset;

            public Tail(float width, Vector2 endOffset)
            {
                this._width = width * 16f;
                this._endOffset = endOffset * 16f;
            }

            public override bool Perform(Point origin, GenAction action)
            {
                var start = new Vector2((float) (origin.X << 4), (float) (origin.Y << 4));
                return Utils.PlotTileTale(start, start + this._endOffset, this._width, (Utils.PerLinePoint) ((x, y) =>
                {
                    if (!this.UnitApply(action, origin, x, y))
                        return !this._quitOnFail;
                    return true;
                }));
            }
        }

        public class Mound : GenShape
        {
            private int _halfWidth;
            private int _height;

            public Mound(int halfWidth, int height)
            {
                this._halfWidth = halfWidth;
                this._height = height;
            }

            public override bool Perform(Point origin, GenAction action)
            {
                var halfWidth = (float) this._halfWidth;
                for (var index1 = -this._halfWidth; index1 <= this._halfWidth; ++index1)
                {
                    var num = Math.Min(this._height,
                        (int) (-((double) (this._height + 1) / ((double) halfWidth * (double) halfWidth)) *
                               ((double) index1 + (double) halfWidth) * ((double) index1 - (double) halfWidth)));
                    for (var index2 = 0; index2 < num; ++index2)
                    {
                        if (!this.UnitApply(action, origin, index1 + origin.X, origin.Y - index2) && this._quitOnFail)
                            return false;
                    }
                }

                return true;
            }
        }
    }
}