// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Generation.ShapeBranch
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria.World.Generation;

namespace Terraria.GameContent.Generation
{
    public class ShapeBranch : GenShape
    {
        private Point _offset;
        private List<Point> _endPoints;

        public ShapeBranch()
        {
            this._offset = new Point(10, -5);
        }

        public ShapeBranch(Point offset)
        {
            this._offset = offset;
        }

        public ShapeBranch(double angle, double distance)
        {
            this._offset = new Point((int) (Math.Cos(angle) * distance), (int) (Math.Sin(angle) * distance));
        }

        private bool PerformSegment(Point origin, GenAction action, Point start, Point end, int size)
        {
            size = Math.Max(1, size);
            for (var index1 = -(size >> 1); index1 < size - (size >> 1); ++index1)
            {
                for (var index2 = -(size >> 1); index2 < size - (size >> 1); ++index2)
                {
                    if (!Utils.PlotLine(new Point(start.X + index1, start.Y + index2), end,
                        (Utils.PerLinePoint) ((tileX, tileY) =>
                        {
                            if (!this.UnitApply(action, origin, tileX, tileY))
                                return !this._quitOnFail;
                            return true;
                        }), false))
                        return false;
                }
            }

            return true;
        }

        public override bool Perform(Point origin, GenAction action)
        {
            var num1 = new Vector2((float) this._offset.X, (float) this._offset.Y).Length();
            var size = (int) ((double) num1 / 6.0);
            if (this._endPoints != null)
                this._endPoints.Add(new Point(origin.X + this._offset.X, origin.Y + this._offset.Y));
            if (!this.PerformSegment(origin, action, origin,
                new Point(origin.X + this._offset.X, origin.Y + this._offset.Y), size))
                return false;
            var num2 = (int) ((double) num1 / 8.0);
            for (var index = 0; index < num2; ++index)
            {
                var num3 = (float) (((double) index + 1.0) / ((double) num2 + 1.0));
                var point1 = new Point((int) ((double) num3 * (double) this._offset.X),
                    (int) ((double) num3 * (double) this._offset.Y));
                var spinningpoint =
                    new Vector2((float) (this._offset.X - point1.X), (float) (this._offset.Y - point1.Y));
                spinningpoint =
                    spinningpoint.RotatedBy(
                        (GenBase._random.NextDouble() * 0.5 + 1.0) * (GenBase._random.Next(2) == 0 ? -1.0 : 1.0),
                        new Vector2()) * 0.75f;
                var point2 = new Point((int) spinningpoint.X + point1.X, (int) spinningpoint.Y + point1.Y);
                if (this._endPoints != null)
                    this._endPoints.Add(new Point(point2.X + origin.X, point2.Y + origin.Y));
                if (!this.PerformSegment(origin, action, new Point(point1.X + origin.X, point1.Y + origin.Y),
                    new Point(point2.X + origin.X, point2.Y + origin.Y), size - 1))
                    return false;
            }

            return true;
        }

        public ShapeBranch OutputEndpoints(List<Point> endpoints)
        {
            this._endPoints = endpoints;
            return this;
        }
    }
}