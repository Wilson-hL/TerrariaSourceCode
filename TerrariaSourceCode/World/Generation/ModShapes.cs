﻿// Decompiled with JetBrains decompiler
// Type: Terraria.World.Generation.ModShapes
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Terraria.World.Generation
{
    public static class ModShapes
    {
        public class All : GenModShape
        {
            public All(ShapeData data)
                : base(data)
            {
            }

            public override bool Perform(Point origin, GenAction action)
            {
                foreach (Point16 point16 in this._data.GetData())
                {
                    if (!this.UnitApply(action, origin, (int) point16.X + origin.X, (int) point16.Y + origin.Y) &&
                        this._quitOnFail)
                        return false;
                }

                return true;
            }
        }

        public class OuterOutline : GenModShape
        {
            private static readonly int[] POINT_OFFSETS = new int[16]
            {
                1,
                0,
                -1,
                0,
                0,
                1,
                0,
                -1,
                1,
                1,
                1,
                -1,
                -1,
                1,
                -1,
                -1
            };

            private bool _useDiagonals;
            private bool _useInterior;

            public OuterOutline(ShapeData data, bool useDiagonals = true, bool useInterior = false)
                : base(data)
            {
                this._useDiagonals = useDiagonals;
                this._useInterior = useInterior;
            }

            public override bool Perform(Point origin, GenAction action)
            {
                int num = this._useDiagonals ? 16 : 8;
                foreach (Point16 point16 in this._data.GetData())
                {
                    if (this._useInterior &&
                        !this.UnitApply(action, origin, (int) point16.X + origin.X, (int) point16.Y + origin.Y) &&
                        this._quitOnFail)
                        return false;
                    int index = 0;
                    while (index < num)
                    {
                        if (!this._data.Contains((int) point16.X + ModShapes.OuterOutline.POINT_OFFSETS[index],
                                (int) point16.Y + ModShapes.OuterOutline.POINT_OFFSETS[index + 1]) && !this.UnitApply(
                                action, origin,
                                origin.X + (int) point16.X + ModShapes.OuterOutline.POINT_OFFSETS[index],
                                origin.Y + (int) point16.Y + ModShapes.OuterOutline.POINT_OFFSETS[index + 1]) &&
                            this._quitOnFail)
                            return false;
                        index += 2;
                    }
                }

                return true;
            }
        }

        public class InnerOutline : GenModShape
        {
            private static readonly int[] POINT_OFFSETS = new int[16]
            {
                1,
                0,
                -1,
                0,
                0,
                1,
                0,
                -1,
                1,
                1,
                1,
                -1,
                -1,
                1,
                -1,
                -1
            };

            private bool _useDiagonals;

            public InnerOutline(ShapeData data, bool useDiagonals = true)
                : base(data)
            {
                this._useDiagonals = useDiagonals;
            }

            public override bool Perform(Point origin, GenAction action)
            {
                int num = this._useDiagonals ? 16 : 8;
                foreach (Point16 point16 in this._data.GetData())
                {
                    bool flag = false;
                    int index = 0;
                    while (index < num)
                    {
                        if (!this._data.Contains((int) point16.X + ModShapes.InnerOutline.POINT_OFFSETS[index],
                            (int) point16.Y + ModShapes.InnerOutline.POINT_OFFSETS[index + 1]))
                        {
                            flag = true;
                            break;
                        }

                        index += 2;
                    }

                    if (flag &&
                        !this.UnitApply(action, origin, (int) point16.X + origin.X, (int) point16.Y + origin.Y) &&
                        this._quitOnFail)
                        return false;
                }

                return true;
            }
        }
    }
}