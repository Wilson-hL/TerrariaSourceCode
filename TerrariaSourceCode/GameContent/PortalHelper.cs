﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.PortalHelper
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.Localization;

namespace Terraria.GameContent
{
    public class PortalHelper
    {
        private static int[,] FoundPortals = new int[256, 2];
        private static int[] PortalCooldownForPlayers = new int[256];
        private static int[] PortalCooldownForNPCs = new int[200];

        private static readonly Vector2[] EDGES = new Vector2[4]
            {new Vector2(0.0f, 1f), new Vector2(0.0f, -1f), new Vector2(1f, 0.0f), new Vector2(-1f, 0.0f)};

        private static readonly Vector2[] SLOPE_EDGES = new Vector2[4]
            {new Vector2(1f, -1f), new Vector2(-1f, -1f), new Vector2(1f, 1f), new Vector2(-1f, 1f)};

        private static readonly Point[] SLOPE_OFFSETS = new Point[4]
            {new Point(1, -1), new Point(-1, -1), new Point(1, 1), new Point(-1, 1)};

        public const int PORTALS_PER_PERSON = 2;

        static PortalHelper()
        {
            for (var index = 0; index < PortalHelper.SLOPE_EDGES.Length; ++index)
                PortalHelper.SLOPE_EDGES[index].Normalize();
            for (var index = 0; index < PortalHelper.FoundPortals.GetLength(0); ++index)
            {
                PortalHelper.FoundPortals[index, 0] = -1;
                PortalHelper.FoundPortals[index, 1] = -1;
            }
        }

        public static void UpdatePortalPoints()
        {
            for (var index = 0; index < PortalHelper.FoundPortals.GetLength(0); ++index)
            {
                PortalHelper.FoundPortals[index, 0] = -1;
                PortalHelper.FoundPortals[index, 1] = -1;
            }

            for (var index = 0; index < PortalHelper.PortalCooldownForPlayers.Length; ++index)
            {
                if (PortalHelper.PortalCooldownForPlayers[index] > 0)
                    --PortalHelper.PortalCooldownForPlayers[index];
            }

            for (var index = 0; index < PortalHelper.PortalCooldownForNPCs.Length; ++index)
            {
                if (PortalHelper.PortalCooldownForNPCs[index] > 0)
                    --PortalHelper.PortalCooldownForNPCs[index];
            }

            for (var index = 0; index < 1000; ++index)
            {
                var projectile = Main.projectile[index];
                if (projectile.active && projectile.type == 602 &&
                    ((double) projectile.ai[1] >= 0.0 && (double) projectile.ai[1] <= 1.0) &&
                    (projectile.owner >= 0 && projectile.owner <= (int) byte.MaxValue))
                    PortalHelper.FoundPortals[projectile.owner, (int) projectile.ai[1]] = index;
            }
        }

        public static void TryGoingThroughPortals(Entity ent)
        {
            var collisionPoint = 0.0f;
            var velocity = ent.velocity;
            var width = ent.width;
            var height = ent.height;
            var gravDir = 1;
            if (ent is Player)
                gravDir = (int) ((Player) ent).gravDir;
            for (var index1 = 0; index1 < PortalHelper.FoundPortals.GetLength(0); ++index1)
            {
                if (PortalHelper.FoundPortals[index1, 0] != -1 && PortalHelper.FoundPortals[index1, 1] != -1 &&
                    (!(ent is Player) || index1 < PortalHelper.PortalCooldownForPlayers.Length &&
                     PortalHelper.PortalCooldownForPlayers[index1] <= 0) &&
                    (!(ent is NPC) || index1 < PortalHelper.PortalCooldownForNPCs.Length &&
                     PortalHelper.PortalCooldownForNPCs[index1] <= 0))
                {
                    for (var index2 = 0; index2 < 2; ++index2)
                    {
                        var projectile1 = Main.projectile[PortalHelper.FoundPortals[index1, index2]];
                        Vector2 start;
                        Vector2 end;
                        PortalHelper.GetPortalEdges(projectile1.Center, projectile1.ai[0], out start, out end);
                        if (Collision.CheckAABBvLineCollision(ent.position + ent.velocity, ent.Size, start, end, 2f,
                            ref collisionPoint))
                        {
                            var projectile2 = Main.projectile[PortalHelper.FoundPortals[index1, 1 - index2]];
                            var num1 = ent.Hitbox.Distance(projectile1.Center);
                            int bonusX;
                            int bonusY;
                            var newPos =
                                PortalHelper.GetPortalOutingPoint(ent.Size, projectile2.Center, projectile2.ai[0],
                                    out bonusX, out bonusY) +
                                Vector2.Normalize(new Vector2((float) bonusX, (float) bonusY)) * num1;
                            var Velocity1 = Vector2.UnitX * 16f;
                            if (!(Collision.TileCollision(newPos - Velocity1, Velocity1, width, height, true, true,
                                      gravDir) != Velocity1))
                            {
                                var Velocity2 = -Vector2.UnitX * 16f;
                                if (!(Collision.TileCollision(newPos - Velocity2, Velocity2, width, height, true, true,
                                          gravDir) != Velocity2))
                                {
                                    var Velocity3 = Vector2.UnitY * 16f;
                                    if (!(Collision.TileCollision(newPos - Velocity3, Velocity3, width, height, true,
                                              true, gravDir) != Velocity3))
                                    {
                                        var Velocity4 = -Vector2.UnitY * 16f;
                                        if (!(Collision.TileCollision(newPos - Velocity4, Velocity4, width, height,
                                                  true, true, gravDir) != Velocity4))
                                        {
                                            var num2 = 0.1f;
                                            if (bonusY == -gravDir)
                                                num2 = 0.1f;
                                            if (ent.velocity == Vector2.Zero)
                                                ent.velocity =
                                                    (projectile1.ai[0] - 1.570796f).ToRotationVector2() * num2;
                                            if ((double) ent.velocity.Length() < (double) num2)
                                            {
                                                ent.velocity.Normalize();
                                                ent.velocity *= num2;
                                            }

                                            var vec =
                                                Vector2.Normalize(new Vector2((float) bonusX, (float) bonusY));
                                            if (vec.HasNaNs() || vec == Vector2.Zero)
                                                vec = Vector2.UnitX * (float) ent.direction;
                                            ent.velocity = vec * ent.velocity.Length();
                                            if (bonusY == -gravDir && Math.Sign(ent.velocity.Y) != -gravDir ||
                                                (double) Math.Abs(ent.velocity.Y) < 0.100000001490116)
                                                ent.velocity.Y = (float) -gravDir * 0.1f;
                                            var extraInfo =
                                                (int) ((double) (projectile2.owner * 2) + (double) projectile2.ai[1]);
                                            var num3 = extraInfo + (extraInfo % 2 == 0 ? 1 : -1);
                                            if (ent is Player)
                                            {
                                                var player = (Player) ent;
                                                player.lastPortalColorIndex = num3;
                                                player.Teleport(newPos, 4, extraInfo);
                                                if (Main.netMode == 1)
                                                {
                                                    NetMessage.SendData(96, -1, -1, (NetworkText) null, player.whoAmI,
                                                        newPos.X, newPos.Y, (float) extraInfo, 0, 0, 0);
                                                    NetMessage.SendData(13, -1, -1, (NetworkText) null, player.whoAmI,
                                                        0.0f, 0.0f, 0.0f, 0, 0, 0);
                                                }

                                                PortalHelper.PortalCooldownForPlayers[index1] = 10;
                                                return;
                                            }

                                            if (!(ent is NPC))
                                                return;
                                            var npc = (NPC) ent;
                                            npc.lastPortalColorIndex = num3;
                                            npc.Teleport(newPos, 4, extraInfo);
                                            if (Main.netMode == 1)
                                            {
                                                NetMessage.SendData(100, -1, -1, (NetworkText) null, npc.whoAmI,
                                                    newPos.X, newPos.Y, (float) extraInfo, 0, 0, 0);
                                                NetMessage.SendData(23, -1, -1, (NetworkText) null, npc.whoAmI, 0.0f,
                                                    0.0f, 0.0f, 0, 0, 0);
                                            }

                                            PortalHelper.PortalCooldownForPlayers[index1] = 10;
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static int TryPlacingPortal(Projectile theBolt, Vector2 velocity, Vector2 theCrashVelocity)
        {
            var vector2_1 = velocity / velocity.Length();
            var tileCoordinates = PortalHelper
                .FindCollision(theBolt.position, theBolt.position + velocity + vector2_1 * 32f).ToTileCoordinates();
            var tile = Main.tile[tileCoordinates.X, tileCoordinates.Y];
            var position = new Vector2((float) (tileCoordinates.X * 16 + 8), (float) (tileCoordinates.Y * 16 + 8));
            if (!WorldGen.SolidOrSlopedTile(tile))
                return -1;
            var num = (int) tile.slope();
            var flag = tile.halfBrick();
            for (var index = 0; index < (flag ? 2 : PortalHelper.EDGES.Length); ++index)
            {
                Point bestPosition;
                if ((double) Vector2.Dot(PortalHelper.EDGES[index], vector2_1) > 0.0 && PortalHelper.FindValidLine(
                        tileCoordinates, (int) PortalHelper.EDGES[index].Y, (int) -(double) PortalHelper.EDGES[index].X,
                        out bestPosition))
                {
                    position = new Vector2((float) (bestPosition.X * 16 + 8), (float) (bestPosition.Y * 16 + 8));
                    return PortalHelper.AddPortal(position - PortalHelper.EDGES[index] * (flag ? 0.0f : 8f),
                        (float) Math.Atan2((double) PortalHelper.EDGES[index].Y, (double) PortalHelper.EDGES[index].X) +
                        1.570796f, (int) theBolt.ai[0], theBolt.direction);
                }
            }

            if (num != 0)
            {
                var vector2_2 = PortalHelper.SLOPE_EDGES[num - 1];
                Point bestPosition;
                if ((double) Vector2.Dot(vector2_2, -vector2_1) > 0.0 && PortalHelper.FindValidLine(tileCoordinates,
                        -PortalHelper.SLOPE_OFFSETS[num - 1].Y, PortalHelper.SLOPE_OFFSETS[num - 1].X,
                        out bestPosition))
                {
                    position = new Vector2((float) (bestPosition.X * 16 + 8), (float) (bestPosition.Y * 16 + 8));
                    return PortalHelper.AddPortal(position,
                        (float) Math.Atan2((double) vector2_2.Y, (double) vector2_2.X) - 1.570796f, (int) theBolt.ai[0],
                        theBolt.direction);
                }
            }

            return -1;
        }

        private static bool FindValidLine(Point position, int xOffset, int yOffset, out Point bestPosition)
        {
            bestPosition = position;
            if (PortalHelper.IsValidLine(position, xOffset, yOffset))
                return true;
            var position1 = new Point(position.X - xOffset, position.Y - yOffset);
            if (PortalHelper.IsValidLine(position1, xOffset, yOffset))
            {
                bestPosition = position1;
                return true;
            }

            var position2 = new Point(position.X + xOffset, position.Y + yOffset);
            if (!PortalHelper.IsValidLine(position2, xOffset, yOffset))
                return false;
            bestPosition = position2;
            return true;
        }

        private static bool IsValidLine(Point position, int xOffset, int yOffset)
        {
            var tile1 = Main.tile[position.X, position.Y];
            var tile2 = Main.tile[position.X - xOffset, position.Y - yOffset];
            var tile3 = Main.tile[position.X + xOffset, position.Y + yOffset];
            return !PortalHelper.BlockPortals(Main.tile[position.X + yOffset, position.Y - xOffset]) &&
                   !PortalHelper.BlockPortals(Main.tile[position.X + yOffset - xOffset,
                       position.Y - xOffset - yOffset]) &&
                   (!PortalHelper.BlockPortals(
                        Main.tile[position.X + yOffset + xOffset, position.Y - xOffset + yOffset]) &&
                    WorldGen.SolidOrSlopedTile(tile1)) &&
                   (WorldGen.SolidOrSlopedTile(tile2) && WorldGen.SolidOrSlopedTile(tile3) &&
                    (tile2.HasSameSlope(tile1) && tile3.HasSameSlope(tile1)));
        }

        private static bool BlockPortals(Tile t)
        {
            return t.active() && !Main.tileCut[(int) t.type] &&
                   (!TileID.Sets.BreakableWhenPlacing[(int) t.type] && Main.tileSolid[(int) t.type]);
        }

        private static Vector2 FindCollision(Vector2 startPosition, Vector2 stopPosition)
        {
            var lastX = 0;
            var lastY = 0;
            Utils.PlotLine(startPosition.ToTileCoordinates(), stopPosition.ToTileCoordinates(),
                (Utils.PerLinePoint) ((x, y) =>
                {
                    lastX = x;
                    lastY = y;
                    return !WorldGen.SolidOrSlopedTile(x, y);
                }), false);
            return new Vector2((float) lastX * 16f, (float) lastY * 16f);
        }

        private static int AddPortal(Vector2 position, float angle, int form, int direction)
        {
            if (!PortalHelper.SupportedTilesAreFine(position, angle))
                return -1;
            PortalHelper.RemoveMyOldPortal(form);
            PortalHelper.RemoveIntersectingPortals(position, angle);
            var index = Projectile.NewProjectile(position.X, position.Y, 0.0f, 0.0f, 602, 0, 0.0f, Main.myPlayer, angle,
                (float) form);
            Main.projectile[index].direction = direction;
            Main.projectile[index].netUpdate = true;
            return index;
        }

        private static void RemoveMyOldPortal(int form)
        {
            for (var index = 0; index < 1000; ++index)
            {
                var projectile = Main.projectile[index];
                if (projectile.active && projectile.type == 602 &&
                    (projectile.owner == Main.myPlayer && (double) projectile.ai[1] == (double) form))
                {
                    projectile.Kill();
                    break;
                }
            }
        }

        private static void RemoveIntersectingPortals(Vector2 position, float angle)
        {
            Vector2 start1;
            Vector2 end1;
            PortalHelper.GetPortalEdges(position, angle, out start1, out end1);
            for (var number = 0; number < 1000; ++number)
            {
                var projectile = Main.projectile[number];
                if (projectile.active && projectile.type == 602)
                {
                    Vector2 start2;
                    Vector2 end2;
                    PortalHelper.GetPortalEdges(projectile.Center, projectile.ai[0], out start2, out end2);
                    if (Collision.CheckLinevLine(start1, end1, start2, end2).Length > 0)
                    {
                        if (projectile.owner != Main.myPlayer && Main.netMode != 2)
                            NetMessage.SendData(95, -1, -1, (NetworkText) null, number, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        projectile.Kill();
                        if (Main.netMode == 2)
                            NetMessage.SendData(29, -1, -1, (NetworkText) null, projectile.whoAmI,
                                (float) projectile.owner, 0.0f, 0.0f, 0, 0, 0);
                    }
                }
            }
        }

        public static Color GetPortalColor(int colorIndex)
        {
            return PortalHelper.GetPortalColor(colorIndex / 2, colorIndex % 2);
        }

        public static Color GetPortalColor(int player, int portal)
        {
            var white = Color.White;
            Color color;
            if (Main.netMode == 0)
            {
                color = portal != 0 ? Main.hslToRgb(0.52f, 1f, 0.6f) : Main.hslToRgb(0.12f, 1f, 0.5f);
            }
            else
            {
                var num = 0.08f;
                color = Main.hslToRgb(
                    (float) ((0.5 + (double) player * ((double) num * 2.0) + (double) portal * (double) num) % 1.0), 1f,
                    0.5f);
            }

            color.A = (byte) 66;
            return color;
        }

        private static void GetPortalEdges(Vector2 position, float angle, out Vector2 start, out Vector2 end)
        {
            var rotationVector2 = angle.ToRotationVector2();
            start = position + rotationVector2 * -22f;
            end = position + rotationVector2 * 22f;
        }

        private static Vector2 GetPortalOutingPoint(Vector2 objectSize, Vector2 portalPosition, float portalAngle,
            out int bonusX, out int bonusY)
        {
            var num = (int) Math.Round((double) MathHelper.WrapAngle(portalAngle) / 0.785398185253143);
            switch (num)
            {
                case -3:
                case 3:
                    bonusX = num == -3 ? 1 : -1;
                    bonusY = -1;
                    return portalPosition + new Vector2(num == -3 ? 0.0f : -objectSize.X, -objectSize.Y);
                case -2:
                case 2:
                    bonusX = num == 2 ? -1 : 1;
                    bonusY = 0;
                    return portalPosition + new Vector2(num == 2 ? -objectSize.X : 0.0f,
                               (float) (-(double) objectSize.Y / 2.0));
                case -1:
                case 1:
                    bonusX = num == -1 ? 1 : -1;
                    bonusY = 1;
                    return portalPosition + new Vector2(num == -1 ? 0.0f : -objectSize.X, 0.0f);
                case 0:
                case 4:
                    bonusX = 0;
                    bonusY = num == 0 ? 1 : -1;
                    return portalPosition + new Vector2((float) (-(double) objectSize.X / 2.0),
                               num == 0 ? 0.0f : -objectSize.Y);
                default:
                    Main.NewText("Broken portal! (over4s = " + (object) num + ")", byte.MaxValue, byte.MaxValue,
                        byte.MaxValue, false);
                    bonusX = 0;
                    bonusY = 0;
                    return portalPosition;
            }
        }

        public static void SyncPortalsOnPlayerJoin(int plr, int fluff, List<Point> dontInclude, out List<Point> portals,
            out List<Point> portalCenters)
        {
            portals = new List<Point>();
            portalCenters = new List<Point>();
            for (var index = 0; index < 1000; ++index)
            {
                var projectile = Main.projectile[index];
                if (projectile.active && (projectile.type == 602 || projectile.type == 601))
                {
                    var center = projectile.Center;
                    var sectionX = Netplay.GetSectionX((int) ((double) center.X / 16.0));
                    var sectionY = Netplay.GetSectionY((int) ((double) center.Y / 16.0));
                    for (var x = sectionX - fluff; x < sectionX + fluff + 1; ++x)
                    {
                        for (var y = sectionY - fluff; y < sectionY + fluff + 1; ++y)
                        {
                            if (x >= 0 && x < Main.maxSectionsX && (y >= 0 && y < Main.maxSectionsY) &&
                                (!Netplay.Clients[plr].TileSections[x, y] && !dontInclude.Contains(new Point(x, y))))
                            {
                                portals.Add(new Point(x, y));
                                if (!portalCenters.Contains(new Point(sectionX, sectionY)))
                                    portalCenters.Add(new Point(sectionX, sectionY));
                            }
                        }
                    }
                }
            }
        }

        public static void SyncPortalSections(Vector2 portalPosition, int fluff)
        {
            for (var playerIndex = 0; playerIndex < (int) byte.MaxValue; ++playerIndex)
            {
                if (Main.player[playerIndex].active)
                    RemoteClient.CheckSection(playerIndex, portalPosition, fluff);
            }
        }

        public static bool SupportedTilesAreFine(Vector2 portalCenter, float portalAngle)
        {
            var tileCoordinates = portalCenter.ToTileCoordinates();
            var num1 = (int) Math.Round((double) MathHelper.WrapAngle(portalAngle) / 0.785398185253143);
            int num2;
            int num3;
            switch (num1)
            {
                case -3:
                case 3:
                    num2 = num1 == -3 ? 1 : -1;
                    num3 = -1;
                    break;
                case -2:
                case 2:
                    num2 = num1 == 2 ? -1 : 1;
                    num3 = 0;
                    break;
                case -1:
                case 1:
                    num2 = num1 == -1 ? 1 : -1;
                    num3 = 1;
                    break;
                case 0:
                case 4:
                    num2 = 0;
                    num3 = num1 == 0 ? 1 : -1;
                    break;
                default:
                    Main.NewText("Broken portal! (over4s = " + (object) num1 + " , " + (object) portalAngle + ")",
                        byte.MaxValue, byte.MaxValue, byte.MaxValue, false);
                    return false;
            }

            if (num2 != 0 && num3 != 0)
            {
                var num4 = 3;
                if (num2 == -1 && num3 == 1)
                    num4 = 5;
                if (num2 == 1 && num3 == -1)
                    num4 = 2;
                if (num2 == 1 && num3 == 1)
                    num4 = 4;
                var slope = num4 - 1;
                if (PortalHelper.SupportedSlope(tileCoordinates.X, tileCoordinates.Y, slope) &&
                    PortalHelper.SupportedSlope(tileCoordinates.X + num2, tileCoordinates.Y - num3, slope))
                    return PortalHelper.SupportedSlope(tileCoordinates.X - num2, tileCoordinates.Y + num3, slope);
                return false;
            }

            switch (num2)
            {
                case 0:
                    switch (num3)
                    {
                        case 0:
                            return true;
                        case 1:
                            --tileCoordinates.Y;
                            break;
                    }

                    if (PortalHelper.SupportedNormal(tileCoordinates.X, tileCoordinates.Y) &&
                        PortalHelper.SupportedNormal(tileCoordinates.X + 1, tileCoordinates.Y) &&
                        PortalHelper.SupportedNormal(tileCoordinates.X - 1, tileCoordinates.Y))
                        return true;
                    if (PortalHelper.SupportedHalfbrick(tileCoordinates.X, tileCoordinates.Y) &&
                        PortalHelper.SupportedHalfbrick(tileCoordinates.X + 1, tileCoordinates.Y))
                        return PortalHelper.SupportedHalfbrick(tileCoordinates.X - 1, tileCoordinates.Y);
                    return false;
                case 1:
                    --tileCoordinates.X;
                    break;
            }

            if (PortalHelper.SupportedNormal(tileCoordinates.X, tileCoordinates.Y) &&
                PortalHelper.SupportedNormal(tileCoordinates.X, tileCoordinates.Y - 1))
                return PortalHelper.SupportedNormal(tileCoordinates.X, tileCoordinates.Y + 1);
            return false;
        }

        private static bool SupportedSlope(int x, int y, int slope)
        {
            var tile = Main.tile[x, y];
            if (tile != null && tile.nactive() &&
                (!Main.tileCut[(int) tile.type] && !TileID.Sets.BreakableWhenPlacing[(int) tile.type]) &&
                Main.tileSolid[(int) tile.type])
                return (int) tile.slope() == slope;
            return false;
        }

        private static bool SupportedHalfbrick(int x, int y)
        {
            var tile = Main.tile[x, y];
            if (tile != null && tile.nactive() &&
                (!Main.tileCut[(int) tile.type] && !TileID.Sets.BreakableWhenPlacing[(int) tile.type]) &&
                Main.tileSolid[(int) tile.type])
                return tile.halfBrick();
            return false;
        }

        private static bool SupportedNormal(int x, int y)
        {
            var tile = Main.tile[x, y];
            if (tile != null && tile.nactive() &&
                (!Main.tileCut[(int) tile.type] && !TileID.Sets.BreakableWhenPlacing[(int) tile.type]) &&
                (Main.tileSolid[(int) tile.type] && !TileID.Sets.NotReallySolid[(int) tile.type] && !tile.halfBrick()))
                return tile.slope() == (byte) 0;
            return false;
        }
    }
}