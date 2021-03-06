﻿// Decompiled with JetBrains decompiler
// Type: Terraria.TileObject
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ObjectData;

namespace Terraria
{
    public struct TileObject
    {
        public static TileObject Empty = new TileObject();
        public static TileObjectPreviewData objectPreview = new TileObjectPreviewData();
        public int xCoord;
        public int yCoord;
        public int type;
        public int style;
        public int alternate;
        public int random;

        public static bool Place(TileObject toBePlaced)
        {
            var tileData =
                TileObjectData.GetTileData(toBePlaced.type, toBePlaced.style, toBePlaced.alternate);
            if (tileData == null)
                return false;
            if (tileData.HookPlaceOverride.hook != null)
            {
                int num1;
                int num2;
                if (tileData.HookPlaceOverride.processedCoordinates)
                {
                    num1 = toBePlaced.xCoord;
                    num2 = toBePlaced.yCoord;
                }
                else
                {
                    num1 = toBePlaced.xCoord + (int) tileData.Origin.X;
                    num2 = toBePlaced.yCoord + (int) tileData.Origin.Y;
                }

                if (tileData.HookPlaceOverride.hook(num1, num2, toBePlaced.type, toBePlaced.style, 1) ==
                    tileData.HookPlaceOverride.badReturn)
                    return false;
            }
            else
            {
                var type = (ushort) toBePlaced.type;
                var placementStyle =
                    tileData.CalculatePlacementStyle(toBePlaced.style, toBePlaced.alternate, toBePlaced.random);
                var num1 = 0;
                if (tileData.StyleWrapLimit > 0)
                {
                    num1 = placementStyle / tileData.StyleWrapLimit * tileData.StyleLineSkip;
                    placementStyle %= tileData.StyleWrapLimit;
                }

                int num2;
                int num3;
                if (tileData.StyleHorizontal)
                {
                    num2 = tileData.CoordinateFullWidth * placementStyle;
                    num3 = tileData.CoordinateFullHeight * num1;
                }
                else
                {
                    num2 = tileData.CoordinateFullWidth * num1;
                    num3 = tileData.CoordinateFullHeight * placementStyle;
                }

                var xCoord = toBePlaced.xCoord;
                var yCoord = toBePlaced.yCoord;
                for (var index1 = 0; index1 < tileData.Width; ++index1)
                {
                    for (var index2 = 0; index2 < tileData.Height; ++index2)
                    {
                        var tileSafely = Framing.GetTileSafely(xCoord + index1, yCoord + index2);
                        if (tileSafely.active() && Main.tileCut[(int) tileSafely.type])
                            WorldGen.KillTile(xCoord + index1, yCoord + index2, false, false, false);
                    }
                }

                for (var index1 = 0; index1 < tileData.Width; ++index1)
                {
                    var num4 = num2 + index1 * (tileData.CoordinateWidth + tileData.CoordinatePadding);
                    var num5 = num3;
                    for (var index2 = 0; index2 < tileData.Height; ++index2)
                    {
                        var tileSafely = Framing.GetTileSafely(xCoord + index1, yCoord + index2);
                        if (!tileSafely.active())
                        {
                            tileSafely.active(true);
                            tileSafely.frameX = (short) num4;
                            tileSafely.frameY = (short) num5;
                            tileSafely.type = type;
                        }

                        num5 += tileData.CoordinateHeights[index2] + tileData.CoordinatePadding;
                    }
                }
            }

            if (tileData.FlattenAnchors)
            {
                var anchorData = tileData.AnchorBottom;
                if (anchorData.tileCount != 0 && (anchorData.type & AnchorType.SolidTile) == AnchorType.SolidTile)
                {
                    var num = toBePlaced.xCoord + anchorData.checkStart;
                    var j = toBePlaced.yCoord + tileData.Height;
                    for (var index = 0; index < anchorData.tileCount; ++index)
                    {
                        var tileSafely = Framing.GetTileSafely(num + index, j);
                        if (Main.tileSolid[(int) tileSafely.type] && !Main.tileSolidTop[(int) tileSafely.type] &&
                            tileSafely.blockType() != 0)
                            WorldGen.SlopeTile(num + index, j, 0);
                    }
                }

                anchorData = tileData.AnchorTop;
                if (anchorData.tileCount != 0 && (anchorData.type & AnchorType.SolidTile) == AnchorType.SolidTile)
                {
                    var num = toBePlaced.xCoord + anchorData.checkStart;
                    var j = toBePlaced.yCoord - 1;
                    for (var index = 0; index < anchorData.tileCount; ++index)
                    {
                        var tileSafely = Framing.GetTileSafely(num + index, j);
                        if (Main.tileSolid[(int) tileSafely.type] && !Main.tileSolidTop[(int) tileSafely.type] &&
                            tileSafely.blockType() != 0)
                            WorldGen.SlopeTile(num + index, j, 0);
                    }
                }

                anchorData = tileData.AnchorRight;
                if (anchorData.tileCount != 0 && (anchorData.type & AnchorType.SolidTile) == AnchorType.SolidTile)
                {
                    var i = toBePlaced.xCoord + tileData.Width;
                    var num = toBePlaced.yCoord + anchorData.checkStart;
                    for (var index = 0; index < anchorData.tileCount; ++index)
                    {
                        var tileSafely = Framing.GetTileSafely(i, num + index);
                        if (Main.tileSolid[(int) tileSafely.type] && !Main.tileSolidTop[(int) tileSafely.type] &&
                            tileSafely.blockType() != 0)
                            WorldGen.SlopeTile(i, num + index, 0);
                    }
                }

                anchorData = tileData.AnchorLeft;
                if (anchorData.tileCount != 0 && (anchorData.type & AnchorType.SolidTile) == AnchorType.SolidTile)
                {
                    var i = toBePlaced.xCoord - 1;
                    var num = toBePlaced.yCoord + anchorData.checkStart;
                    for (var index = 0; index < anchorData.tileCount; ++index)
                    {
                        var tileSafely = Framing.GetTileSafely(i, num + index);
                        if (Main.tileSolid[(int) tileSafely.type] && !Main.tileSolidTop[(int) tileSafely.type] &&
                            tileSafely.blockType() != 0)
                            WorldGen.SlopeTile(i, num + index, 0);
                    }
                }
            }

            return true;
        }

        public static bool CanPlace(int x, int y, int type, int style, int dir, out TileObject objectData,
            bool onlyCheck = false)
        {
            var tileData1 = TileObjectData.GetTileData(type, style, 0);
            objectData = TileObject.Empty;
            if (tileData1 == null)
                return false;
            var num1 = x - (int) tileData1.Origin.X;
            var num2 = y - (int) tileData1.Origin.Y;
            if (num1 < 0 || num1 + tileData1.Width >= Main.maxTilesX ||
                (num2 < 0 || num2 + tileData1.Height >= Main.maxTilesY))
                return false;
            var flag1 = tileData1.RandomStyleRange > 0;
            if (TileObjectPreviewData.placementCache == null)
                TileObjectPreviewData.placementCache = new TileObjectPreviewData();
            TileObjectPreviewData.placementCache.Reset();
            var num3 = 0;
            var num4 = 0;
            if (tileData1.AlternatesCount != 0)
                num4 = tileData1.AlternatesCount;
            var num5 = -1f;
            var num6 = -1f;
            var num7 = 0;
            var tileObjectData = (TileObjectData) null;
            var alternate = num3 - 1;
            while (alternate < num4)
            {
                ++alternate;
                var tileData2 = TileObjectData.GetTileData(type, style, alternate);
                if (tileData2.Direction == TileObjectDirection.None ||
                    (tileData2.Direction != TileObjectDirection.PlaceLeft || dir != 1) &&
                    (tileData2.Direction != TileObjectDirection.PlaceRight || dir != -1))
                {
                    var num8 = x - (int) tileData2.Origin.X;
                    var num9 = y - (int) tileData2.Origin.Y;
                    if (num8 < 5 || num8 + tileData2.Width > Main.maxTilesX - 5 ||
                        (num9 < 5 || num9 + tileData2.Height > Main.maxTilesY - 5))
                        return false;
                    var rectangle = new Rectangle(0, 0, tileData2.Width, tileData2.Height);
                    var X = 0;
                    var Y = 0;
                    if (tileData2.AnchorTop.tileCount != 0)
                    {
                        if (rectangle.Y == 0)
                        {
                            rectangle.Y = -1;
                            ++rectangle.Height;
                            ++Y;
                        }

                        var checkStart = tileData2.AnchorTop.checkStart;
                        if (checkStart < rectangle.X)
                        {
                            rectangle.Width += rectangle.X - checkStart;
                            X += rectangle.X - checkStart;
                            rectangle.X = checkStart;
                        }

                        var num10 = checkStart + tileData2.AnchorTop.tileCount - 1;
                        var num11 = rectangle.X + rectangle.Width - 1;
                        if (num10 > num11)
                            rectangle.Width += num10 - num11;
                    }

                    if (tileData2.AnchorBottom.tileCount != 0)
                    {
                        if (rectangle.Y + rectangle.Height == tileData2.Height)
                            ++rectangle.Height;
                        var checkStart = tileData2.AnchorBottom.checkStart;
                        if (checkStart < rectangle.X)
                        {
                            rectangle.Width += rectangle.X - checkStart;
                            X += rectangle.X - checkStart;
                            rectangle.X = checkStart;
                        }

                        var num10 = checkStart + tileData2.AnchorBottom.tileCount - 1;
                        var num11 = rectangle.X + rectangle.Width - 1;
                        if (num10 > num11)
                            rectangle.Width += num10 - num11;
                    }

                    if (tileData2.AnchorLeft.tileCount != 0)
                    {
                        if (rectangle.X == 0)
                        {
                            rectangle.X = -1;
                            ++rectangle.Width;
                            ++X;
                        }

                        var checkStart = tileData2.AnchorLeft.checkStart;
                        if ((tileData2.AnchorLeft.type & AnchorType.Tree) == AnchorType.Tree)
                            --checkStart;
                        if (checkStart < rectangle.Y)
                        {
                            rectangle.Width += rectangle.Y - checkStart;
                            Y += rectangle.Y - checkStart;
                            rectangle.Y = checkStart;
                        }

                        var num10 = checkStart + tileData2.AnchorLeft.tileCount - 1;
                        if ((tileData2.AnchorLeft.type & AnchorType.Tree) == AnchorType.Tree)
                            num10 += 2;
                        var num11 = rectangle.Y + rectangle.Height - 1;
                        if (num10 > num11)
                            rectangle.Height += num10 - num11;
                    }

                    if (tileData2.AnchorRight.tileCount != 0)
                    {
                        if (rectangle.X + rectangle.Width == tileData2.Width)
                            ++rectangle.Width;
                        var checkStart = tileData2.AnchorLeft.checkStart;
                        if ((tileData2.AnchorRight.type & AnchorType.Tree) == AnchorType.Tree)
                            --checkStart;
                        if (checkStart < rectangle.Y)
                        {
                            rectangle.Width += rectangle.Y - checkStart;
                            Y += rectangle.Y - checkStart;
                            rectangle.Y = checkStart;
                        }

                        var num10 = checkStart + tileData2.AnchorRight.tileCount - 1;
                        if ((tileData2.AnchorRight.type & AnchorType.Tree) == AnchorType.Tree)
                            num10 += 2;
                        var num11 = rectangle.Y + rectangle.Height - 1;
                        if (num10 > num11)
                            rectangle.Height += num10 - num11;
                    }

                    if (onlyCheck)
                    {
                        TileObject.objectPreview.Reset();
                        TileObject.objectPreview.Active = true;
                        TileObject.objectPreview.Type = (ushort) type;
                        TileObject.objectPreview.Style = (short) style;
                        TileObject.objectPreview.Alternate = alternate;
                        TileObject.objectPreview.Size = new Point16(rectangle.Width, rectangle.Height);
                        TileObject.objectPreview.ObjectStart = new Point16(X, Y);
                        TileObject.objectPreview.Coordinates = new Point16(num8 - X, num9 - Y);
                    }

                    var num12 = 0.0f;
                    var num13 = (float) (tileData2.Width * tileData2.Height);
                    var num14 = 0.0f;
                    var num15 = 0.0f;
                    for (var index1 = 0; index1 < tileData2.Width; ++index1)
                    {
                        for (var index2 = 0; index2 < tileData2.Height; ++index2)
                        {
                            var tileSafely = Framing.GetTileSafely(num8 + index1, num9 + index2);
                            var flag2 = !tileData2.LiquidPlace(tileSafely);
                            var flag3 = false;
                            if (tileData2.AnchorWall)
                            {
                                ++num15;
                                if (!tileData2.isValidWallAnchor((int) tileSafely.wall))
                                    flag3 = true;
                                else
                                    ++num14;
                            }

                            var flag4 = false;
                            if (tileSafely.active() && !Main.tileCut[(int) tileSafely.type])
                                flag4 = true;
                            if (flag4 || flag2 || flag3)
                            {
                                if (onlyCheck)
                                    TileObject.objectPreview[index1 + X, index2 + Y] = 2;
                            }
                            else
                            {
                                if (onlyCheck)
                                    TileObject.objectPreview[index1 + X, index2 + Y] = 1;
                                ++num12;
                            }
                        }
                    }

                    var anchorBottom = tileData2.AnchorBottom;
                    if (anchorBottom.tileCount != 0)
                    {
                        num15 += (float) anchorBottom.tileCount;
                        var height = tileData2.Height;
                        for (var index = 0; index < anchorBottom.tileCount; ++index)
                        {
                            var num10 = anchorBottom.checkStart + index;
                            var tileSafely = Framing.GetTileSafely(num8 + num10, num9 + height);
                            var flag2 = false;
                            if (tileSafely.nactive())
                            {
                                if ((anchorBottom.type & AnchorType.SolidTile) == AnchorType.SolidTile &&
                                    Main.tileSolid[(int) tileSafely.type] &&
                                    (!Main.tileSolidTop[(int) tileSafely.type] &&
                                     !Main.tileNoAttach[(int) tileSafely.type]) &&
                                    (tileData2.FlattenAnchors || tileSafely.blockType() == 0))
                                    flag2 = tileData2.isValidTileAnchor((int) tileSafely.type);
                                if (!flag2 &&
                                    ((anchorBottom.type & AnchorType.SolidWithTop) == AnchorType.SolidWithTop ||
                                     (anchorBottom.type & AnchorType.Table) == AnchorType.Table))
                                {
                                    if (TileID.Sets.Platforms[(int) tileSafely.type])
                                    {
                                        var num11 = (int) tileSafely.frameX / TileObjectData.PlatformFrameWidth();
                                        if (!tileSafely.halfBrick() && num11 >= 0 && num11 <= 7 ||
                                            (num11 >= 12 && num11 <= 16 || num11 >= 25 && num11 <= 26))
                                            flag2 = true;
                                    }
                                    else if (Main.tileSolid[(int) tileSafely.type] &&
                                             Main.tileSolidTop[(int) tileSafely.type])
                                        flag2 = true;
                                }

                                if (!flag2 && (anchorBottom.type & AnchorType.Table) == AnchorType.Table &&
                                    (!TileID.Sets.Platforms[(int) tileSafely.type] &&
                                     Main.tileTable[(int) tileSafely.type]) && tileSafely.blockType() == 0)
                                    flag2 = true;
                                if (!flag2 && (anchorBottom.type & AnchorType.SolidSide) == AnchorType.SolidSide &&
                                    (Main.tileSolid[(int) tileSafely.type] &&
                                     !Main.tileSolidTop[(int) tileSafely.type]))
                                {
                                    switch (tileSafely.blockType())
                                    {
                                        case 4:
                                        case 5:
                                            flag2 = tileData2.isValidTileAnchor((int) tileSafely.type);
                                            break;
                                    }
                                }

                                if (!flag2 &&
                                    (anchorBottom.type & AnchorType.AlternateTile) == AnchorType.AlternateTile &&
                                    tileData2.isValidAlternateAnchor((int) tileSafely.type))
                                    flag2 = true;
                            }
                            else if (!flag2 && (anchorBottom.type & AnchorType.EmptyTile) == AnchorType.EmptyTile)
                                flag2 = true;

                            if (!flag2)
                            {
                                if (onlyCheck)
                                    TileObject.objectPreview[num10 + X, height + Y] = 2;
                            }
                            else
                            {
                                if (onlyCheck)
                                    TileObject.objectPreview[num10 + X, height + Y] = 1;
                                ++num14;
                            }
                        }
                    }

                    var anchorTop = tileData2.AnchorTop;
                    if (anchorTop.tileCount != 0)
                    {
                        num15 += (float) anchorTop.tileCount;
                        var num10 = -1;
                        for (var index = 0; index < anchorTop.tileCount; ++index)
                        {
                            var num11 = anchorTop.checkStart + index;
                            var tileSafely = Framing.GetTileSafely(num8 + num11, num9 + num10);
                            var flag2 = false;
                            if (tileSafely.nactive())
                            {
                                if (Main.tileSolid[(int) tileSafely.type] &&
                                    !Main.tileSolidTop[(int) tileSafely.type] &&
                                    !Main.tileNoAttach[(int) tileSafely.type] &&
                                    (tileData2.FlattenAnchors || tileSafely.blockType() == 0))
                                    flag2 = tileData2.isValidTileAnchor((int) tileSafely.type);
                                if (!flag2 && (anchorTop.type & AnchorType.SolidBottom) == AnchorType.SolidBottom &&
                                    (Main.tileSolid[(int) tileSafely.type] &&
                                     (!Main.tileSolidTop[(int) tileSafely.type] ||
                                      TileID.Sets.Platforms[(int) tileSafely.type] &&
                                      (tileSafely.halfBrick() || tileSafely.topSlope())) ||
                                     (tileSafely.halfBrick() || tileSafely.topSlope())) &&
                                    (!TileID.Sets.NotReallySolid[(int) tileSafely.type] && !tileSafely.bottomSlope()))
                                    flag2 = tileData2.isValidTileAnchor((int) tileSafely.type);
                                if (!flag2 && (anchorTop.type & AnchorType.SolidSide) == AnchorType.SolidSide &&
                                    (Main.tileSolid[(int) tileSafely.type] &&
                                     !Main.tileSolidTop[(int) tileSafely.type]))
                                {
                                    switch (tileSafely.blockType())
                                    {
                                        case 2:
                                        case 3:
                                            flag2 = tileData2.isValidTileAnchor((int) tileSafely.type);
                                            break;
                                    }
                                }

                                if (!flag2 && (anchorTop.type & AnchorType.AlternateTile) == AnchorType.AlternateTile &&
                                    tileData2.isValidAlternateAnchor((int) tileSafely.type))
                                    flag2 = true;
                            }
                            else if (!flag2 && (anchorTop.type & AnchorType.EmptyTile) == AnchorType.EmptyTile)
                                flag2 = true;

                            if (!flag2)
                            {
                                if (onlyCheck)
                                    TileObject.objectPreview[num11 + X, num10 + Y] = 2;
                            }
                            else
                            {
                                if (onlyCheck)
                                    TileObject.objectPreview[num11 + X, num10 + Y] = 1;
                                ++num14;
                            }
                        }
                    }

                    var anchorRight = tileData2.AnchorRight;
                    if (anchorRight.tileCount != 0)
                    {
                        num15 += (float) anchorRight.tileCount;
                        var width = tileData2.Width;
                        for (var index = 0; index < anchorRight.tileCount; ++index)
                        {
                            var num10 = anchorRight.checkStart + index;
                            var tileSafely1 = Framing.GetTileSafely(num8 + width, num9 + num10);
                            var flag2 = false;
                            if (tileSafely1.nactive())
                            {
                                if (Main.tileSolid[(int) tileSafely1.type] &&
                                    !Main.tileSolidTop[(int) tileSafely1.type] &&
                                    !Main.tileNoAttach[(int) tileSafely1.type] &&
                                    (tileData2.FlattenAnchors || tileSafely1.blockType() == 0))
                                    flag2 = tileData2.isValidTileAnchor((int) tileSafely1.type);
                                if (!flag2 && (anchorRight.type & AnchorType.SolidSide) == AnchorType.SolidSide &&
                                    (Main.tileSolid[(int) tileSafely1.type] &&
                                     !Main.tileSolidTop[(int) tileSafely1.type]))
                                {
                                    switch (tileSafely1.blockType())
                                    {
                                        case 2:
                                        case 4:
                                            flag2 = tileData2.isValidTileAnchor((int) tileSafely1.type);
                                            break;
                                    }
                                }

                                if (!flag2 && (anchorRight.type & AnchorType.Tree) == AnchorType.Tree &&
                                    tileSafely1.type == (ushort) 5)
                                {
                                    flag2 = true;
                                    if (index == 0)
                                    {
                                        ++num15;
                                        var tileSafely2 = Framing.GetTileSafely(num8 + width, num9 + num10 - 1);
                                        if (tileSafely2.nactive() && tileSafely2.type == (ushort) 5)
                                        {
                                            ++num14;
                                            if (onlyCheck)
                                                TileObject.objectPreview[width + X, num10 + Y - 1] = 1;
                                        }
                                        else if (onlyCheck)
                                            TileObject.objectPreview[width + X, num10 + Y - 1] = 2;
                                    }

                                    if (index == anchorRight.tileCount - 1)
                                    {
                                        ++num15;
                                        var tileSafely2 = Framing.GetTileSafely(num8 + width, num9 + num10 + 1);
                                        if (tileSafely2.nactive() && tileSafely2.type == (ushort) 5)
                                        {
                                            ++num14;
                                            if (onlyCheck)
                                                TileObject.objectPreview[width + X, num10 + Y + 1] = 1;
                                        }
                                        else if (onlyCheck)
                                            TileObject.objectPreview[width + X, num10 + Y + 1] = 2;
                                    }
                                }

                                if (!flag2 &&
                                    (anchorRight.type & AnchorType.AlternateTile) == AnchorType.AlternateTile &&
                                    tileData2.isValidAlternateAnchor((int) tileSafely1.type))
                                    flag2 = true;
                            }
                            else if (!flag2 && (anchorRight.type & AnchorType.EmptyTile) == AnchorType.EmptyTile)
                                flag2 = true;

                            if (!flag2)
                            {
                                if (onlyCheck)
                                    TileObject.objectPreview[width + X, num10 + Y] = 2;
                            }
                            else
                            {
                                if (onlyCheck)
                                    TileObject.objectPreview[width + X, num10 + Y] = 1;
                                ++num14;
                            }
                        }
                    }

                    var anchorLeft = tileData2.AnchorLeft;
                    if (anchorLeft.tileCount != 0)
                    {
                        num15 += (float) anchorLeft.tileCount;
                        var num10 = -1;
                        for (var index = 0; index < anchorLeft.tileCount; ++index)
                        {
                            var num11 = anchorLeft.checkStart + index;
                            var tileSafely1 = Framing.GetTileSafely(num8 + num10, num9 + num11);
                            var flag2 = false;
                            if (tileSafely1.nactive())
                            {
                                if (Main.tileSolid[(int) tileSafely1.type] &&
                                    !Main.tileSolidTop[(int) tileSafely1.type] &&
                                    !Main.tileNoAttach[(int) tileSafely1.type] &&
                                    (tileData2.FlattenAnchors || tileSafely1.blockType() == 0))
                                    flag2 = tileData2.isValidTileAnchor((int) tileSafely1.type);
                                if (!flag2 && (anchorLeft.type & AnchorType.SolidSide) == AnchorType.SolidSide &&
                                    (Main.tileSolid[(int) tileSafely1.type] &&
                                     !Main.tileSolidTop[(int) tileSafely1.type]))
                                {
                                    switch (tileSafely1.blockType())
                                    {
                                        case 3:
                                        case 5:
                                            flag2 = tileData2.isValidTileAnchor((int) tileSafely1.type);
                                            break;
                                    }
                                }

                                if (!flag2 && (anchorLeft.type & AnchorType.Tree) == AnchorType.Tree &&
                                    tileSafely1.type == (ushort) 5)
                                {
                                    flag2 = true;
                                    if (index == 0)
                                    {
                                        ++num15;
                                        var tileSafely2 = Framing.GetTileSafely(num8 + num10, num9 + num11 - 1);
                                        if (tileSafely2.nactive() && tileSafely2.type == (ushort) 5)
                                        {
                                            ++num14;
                                            if (onlyCheck)
                                                TileObject.objectPreview[num10 + X, num11 + Y - 1] = 1;
                                        }
                                        else if (onlyCheck)
                                            TileObject.objectPreview[num10 + X, num11 + Y - 1] = 2;
                                    }

                                    if (index == anchorLeft.tileCount - 1)
                                    {
                                        ++num15;
                                        var tileSafely2 = Framing.GetTileSafely(num8 + num10, num9 + num11 + 1);
                                        if (tileSafely2.nactive() && tileSafely2.type == (ushort) 5)
                                        {
                                            ++num14;
                                            if (onlyCheck)
                                                TileObject.objectPreview[num10 + X, num11 + Y + 1] = 1;
                                        }
                                        else if (onlyCheck)
                                            TileObject.objectPreview[num10 + X, num11 + Y + 1] = 2;
                                    }
                                }

                                if (!flag2 &&
                                    (anchorLeft.type & AnchorType.AlternateTile) == AnchorType.AlternateTile &&
                                    tileData2.isValidAlternateAnchor((int) tileSafely1.type))
                                    flag2 = true;
                            }
                            else if (!flag2 && (anchorLeft.type & AnchorType.EmptyTile) == AnchorType.EmptyTile)
                                flag2 = true;

                            if (!flag2)
                            {
                                if (onlyCheck)
                                    TileObject.objectPreview[num10 + X, num11 + Y] = 2;
                            }
                            else
                            {
                                if (onlyCheck)
                                    TileObject.objectPreview[num10 + X, num11 + Y] = 1;
                                ++num14;
                            }
                        }
                    }

                    if (tileData2.HookCheck.hook != null)
                    {
                        if (tileData2.HookCheck.processedCoordinates)
                        {
                            var x1 = (int) tileData2.Origin.X;
                            var y1 = (int) tileData2.Origin.Y;
                        }

                        if (tileData2.HookCheck.hook(x, y, type, style, dir) == tileData2.HookCheck.badReturn &&
                            tileData2.HookCheck.badResponse == 0)
                        {
                            num14 = 0.0f;
                            num12 = 0.0f;
                            TileObject.objectPreview.AllInvalid();
                        }
                    }

                    var num16 = num14 / num15;
                    var num17 = num12 / num13;
                    if ((double) num17 == 1.0 && (double) num15 == 0.0)
                    {
                        num16 = 1f;
                        num17 = 1f;
                    }

                    if ((double) num16 == 1.0 && (double) num17 == 1.0)
                    {
                        num5 = 1f;
                        num6 = 1f;
                        num7 = alternate;
                        tileObjectData = tileData2;
                        break;
                    }

                    if ((double) num16 > (double) num5 ||
                        (double) num16 == (double) num5 && (double) num17 > (double) num6)
                    {
                        TileObjectPreviewData.placementCache.CopyFrom(TileObject.objectPreview);
                        num5 = num16;
                        num6 = num17;
                        tileObjectData = tileData2;
                        num7 = alternate;
                    }
                }
            }

            var num18 = -1;
            if (flag1)
            {
                if (TileObjectPreviewData.randomCache == null)
                    TileObjectPreviewData.randomCache = new TileObjectPreviewData();
                var flag2 = false;
                if ((int) TileObjectPreviewData.randomCache.Type == type)
                {
                    var coordinates = TileObjectPreviewData.randomCache.Coordinates;
                    var objectStart = TileObjectPreviewData.randomCache.ObjectStart;
                    var num8 = (int) coordinates.X + (int) objectStart.X;
                    var num9 = (int) coordinates.Y + (int) objectStart.Y;
                    var num10 = x - (int) tileData1.Origin.X;
                    var num11 = y - (int) tileData1.Origin.Y;
                    if (num8 != num10 || num9 != num11)
                        flag2 = true;
                }
                else
                    flag2 = true;

                num18 = !flag2 ? TileObjectPreviewData.randomCache.Random : Main.rand.Next(tileData1.RandomStyleRange);
            }

            if (onlyCheck)
            {
                if ((double) num5 != 1.0 || (double) num6 != 1.0)
                {
                    TileObject.objectPreview.CopyFrom(TileObjectPreviewData.placementCache);
                    alternate = num7;
                }

                TileObject.objectPreview.Random = num18;
                if (tileData1.RandomStyleRange > 0)
                    TileObjectPreviewData.randomCache.CopyFrom(TileObject.objectPreview);
            }

            if (!onlyCheck)
            {
                objectData.xCoord = x - (int) tileObjectData.Origin.X;
                objectData.yCoord = y - (int) tileObjectData.Origin.Y;
                objectData.type = type;
                objectData.style = style;
                objectData.alternate = alternate;
                objectData.random = num18;
            }

            if ((double) num5 == 1.0)
                return (double) num6 == 1.0;
            return false;
        }

        public static void DrawPreview(SpriteBatch sb, TileObjectPreviewData op, Vector2 position)
        {
            var coordinates = op.Coordinates;
            var texture = Main.tileTexture[(int) op.Type];
            var tileData = TileObjectData.GetTileData((int) op.Type, (int) op.Style, op.Alternate);
            var placementStyle = tileData.CalculatePlacementStyle((int) op.Style, op.Alternate, op.Random);
            var num1 = 0;
            var drawYoffset = tileData.DrawYOffset;
            if (tileData.StyleWrapLimit > 0)
            {
                num1 = placementStyle / tileData.StyleWrapLimit * tileData.StyleLineSkip;
                placementStyle %= tileData.StyleWrapLimit;
            }

            int num2;
            int num3;
            if (tileData.StyleHorizontal)
            {
                num2 = tileData.CoordinateFullWidth * placementStyle;
                num3 = tileData.CoordinateFullHeight * num1;
            }
            else
            {
                num2 = tileData.CoordinateFullWidth * num1;
                num3 = tileData.CoordinateFullHeight * placementStyle;
            }

            for (var index1 = 0; index1 < (int) op.Size.X; ++index1)
            {
                var x = num2 + (index1 - (int) op.ObjectStart.X) *
                        (tileData.CoordinateWidth + tileData.CoordinatePadding);
                var y = num3;
                for (var index2 = 0; index2 < (int) op.Size.Y; ++index2)
                {
                    var i = (int) coordinates.X + index1;
                    var num4 = (int) coordinates.Y + index2;
                    if (index2 == 0 && tileData.DrawStepDown != 0 &&
                        WorldGen.SolidTile(Framing.GetTileSafely(i, num4 - 1)))
                        drawYoffset += tileData.DrawStepDown;
                    Color color1;
                    switch (op[index1, index2])
                    {
                        case 1:
                            color1 = Color.White;
                            break;
                        case 2:
                            color1 = Color.Red * 0.7f;
                            break;
                        default:
                            continue;
                    }

                    var color2 = color1 * 0.5f;
                    if (index1 >= (int) op.ObjectStart.X && index1 < (int) op.ObjectStart.X + tileData.Width &&
                        (index2 >= (int) op.ObjectStart.Y && index2 < (int) op.ObjectStart.Y + tileData.Height))
                    {
                        var effects = SpriteEffects.None;
                        if (tileData.DrawFlipHorizontal && index1 % 2 == 1)
                            effects |= SpriteEffects.FlipHorizontally;
                        if (tileData.DrawFlipVertical && index2 % 2 == 1)
                            effects |= SpriteEffects.FlipVertically;
                        var rectangle = new Rectangle(x, y, tileData.CoordinateWidth,
                            tileData.CoordinateHeights[index2 - (int) op.ObjectStart.Y]);
                        sb.Draw(texture,
                            new Vector2(
                                (float) (i * 16 - (int) ((double) position.X +
                                                         (double) (tileData.CoordinateWidth - 16) / 2.0)),
                                (float) (num4 * 16 - (int) position.Y + drawYoffset)), new Rectangle?(rectangle),
                            color2, 0.0f, Vector2.Zero, 1f, effects, 0.0f);
                        y += tileData.CoordinateHeights[index2 - (int) op.ObjectStart.Y] + tileData.CoordinatePadding;
                    }
                }
            }
        }
    }
}