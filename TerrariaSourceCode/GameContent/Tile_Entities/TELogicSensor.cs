﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Tile_Entities.TELogicSensor
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;

namespace Terraria.GameContent.Tile_Entities
{
    public class TELogicSensor : TileEntity
    {
        private static Dictionary<int, Rectangle> playerBox = new Dictionary<int, Rectangle>();
        private static List<Tuple<Point16, bool>> tripPoints = new List<Tuple<Point16, bool>>();
        private static List<int> markedIDsForRemoval = new List<int>();
        private static bool inUpdateLoop = false;
        private static bool playerBoxFilled = false;
        public TELogicSensor.LogicCheckType logicCheck;
        public bool On;
        public int CountedData;

        public static void Initialize()
        {
            TileEntity._UpdateStart += new Action(TELogicSensor.UpdateStartInternal);
            TileEntity._UpdateEnd += new Action(TELogicSensor.UpdateEndInternal);
            TileEntity._NetPlaceEntity += new Action<int, int, int>(TELogicSensor.NetPlaceEntity);
        }

        public static void NetPlaceEntity(int x, int y, int type)
        {
            if (type != 2 || !TELogicSensor.ValidTile(x, y))
                return;
            int number = TELogicSensor.Place(x, y);
            ((TELogicSensor) TileEntity.ByID[number]).FigureCheckState();
            NetMessage.SendData(86, -1, -1, (NetworkText) null, number, (float) x, (float) y, 0.0f, 0, 0, 0);
        }

        private static void UpdateStartInternal()
        {
            TELogicSensor.inUpdateLoop = true;
            TELogicSensor.markedIDsForRemoval.Clear();
            TELogicSensor.playerBox.Clear();
            TELogicSensor.playerBoxFilled = false;
            TELogicSensor.FillPlayerHitboxes();
        }

        private static void FillPlayerHitboxes()
        {
            if (TELogicSensor.playerBoxFilled)
                return;
            for (int index = 0; index < (int) byte.MaxValue; ++index)
            {
                if (Main.player[index].active)
                    TELogicSensor.playerBox[index] = Main.player[index].getRect();
            }

            TELogicSensor.playerBoxFilled = true;
        }

        private static void UpdateEndInternal()
        {
            TELogicSensor.inUpdateLoop = false;
            foreach (Tuple<Point16, bool> tripPoint in TELogicSensor.tripPoints)
            {
                Wiring.blockPlayerTeleportationForOneIteration = tripPoint.Item2;
                Wiring.HitSwitch((int) tripPoint.Item1.X, (int) tripPoint.Item1.Y);
            }

            Wiring.blockPlayerTeleportationForOneIteration = false;
            TELogicSensor.tripPoints.Clear();
            foreach (int key in TELogicSensor.markedIDsForRemoval)
            {
                TileEntity tileEntity;
                if (TileEntity.ByID.TryGetValue(key, out tileEntity) && tileEntity.type == (byte) 2)
                    TileEntity.ByID.Remove(key);
                TileEntity.ByPosition.Remove(tileEntity.Position);
            }

            TELogicSensor.markedIDsForRemoval.Clear();
        }

        public override void Update()
        {
            bool state = TELogicSensor.GetState((int) this.Position.X, (int) this.Position.Y, this.logicCheck, this);
            switch (this.logicCheck)
            {
                case TELogicSensor.LogicCheckType.Day:
                case TELogicSensor.LogicCheckType.Night:
                    if (!this.On && state)
                        this.ChangeState(true, true);
                    if (!this.On || state)
                        break;
                    this.ChangeState(false, false);
                    break;
                case TELogicSensor.LogicCheckType.PlayerAbove:
                case TELogicSensor.LogicCheckType.Water:
                case TELogicSensor.LogicCheckType.Lava:
                case TELogicSensor.LogicCheckType.Honey:
                case TELogicSensor.LogicCheckType.Liquid:
                    if (this.On == state)
                        break;
                    this.ChangeState(state, true);
                    break;
            }
        }

        public void ChangeState(bool onState, bool TripWire)
        {
            if (onState != this.On && !TELogicSensor.SanityCheck((int) this.Position.X, (int) this.Position.Y))
                return;
            Main.tile[(int) this.Position.X, (int) this.Position.Y].frameX = onState ? (short) 18 : (short) 0;
            this.On = onState;
            if (Main.netMode == 2)
                NetMessage.SendTileSquare(-1, (int) this.Position.X, (int) this.Position.Y, 1, TileChangeType.None);
            if (!TripWire || Main.netMode == 1)
                return;
            TELogicSensor.tripPoints.Add(Tuple.Create<Point16, bool>(this.Position,
                this.logicCheck == TELogicSensor.LogicCheckType.PlayerAbove));
        }

        public static bool ValidTile(int x, int y)
        {
            return Main.tile[x, y].active() && Main.tile[x, y].type == (ushort) 423 &&
                   ((int) Main.tile[x, y].frameY % 18 == 0 && (int) Main.tile[x, y].frameX % 18 == 0);
        }

        public TELogicSensor()
        {
            this.logicCheck = TELogicSensor.LogicCheckType.None;
            this.On = false;
        }

        public static TELogicSensor.LogicCheckType FigureCheckType(int x, int y, out bool on)
        {
            on = false;
            if (!WorldGen.InWorld(x, y, 0))
                return TELogicSensor.LogicCheckType.None;
            Tile tile = Main.tile[x, y];
            if (tile == null)
                return TELogicSensor.LogicCheckType.None;
            TELogicSensor.LogicCheckType type = TELogicSensor.LogicCheckType.None;
            switch ((int) tile.frameY / 18)
            {
                case 0:
                    type = TELogicSensor.LogicCheckType.Day;
                    break;
                case 1:
                    type = TELogicSensor.LogicCheckType.Night;
                    break;
                case 2:
                    type = TELogicSensor.LogicCheckType.PlayerAbove;
                    break;
                case 3:
                    type = TELogicSensor.LogicCheckType.Water;
                    break;
                case 4:
                    type = TELogicSensor.LogicCheckType.Lava;
                    break;
                case 5:
                    type = TELogicSensor.LogicCheckType.Honey;
                    break;
                case 6:
                    type = TELogicSensor.LogicCheckType.Liquid;
                    break;
            }

            on = TELogicSensor.GetState(x, y, type, (TELogicSensor) null);
            return type;
        }

        public static bool GetState(int x, int y, TELogicSensor.LogicCheckType type, TELogicSensor instance = null)
        {
            switch (type)
            {
                case TELogicSensor.LogicCheckType.Day:
                    return Main.dayTime;
                case TELogicSensor.LogicCheckType.Night:
                    return !Main.dayTime;
                case TELogicSensor.LogicCheckType.PlayerAbove:
                    bool flag1 = false;
                    Rectangle rectangle = new Rectangle(x * 16 - 32 - 1, y * 16 - 160 - 1, 82, 162);
                    foreach (KeyValuePair<int, Rectangle> keyValuePair in TELogicSensor.playerBox)
                    {
                        if (keyValuePair.Value.Intersects(rectangle))
                        {
                            flag1 = true;
                            break;
                        }
                    }

                    return flag1;
                case TELogicSensor.LogicCheckType.Water:
                case TELogicSensor.LogicCheckType.Lava:
                case TELogicSensor.LogicCheckType.Honey:
                case TELogicSensor.LogicCheckType.Liquid:
                    if (instance == null)
                        return false;
                    Tile tile = Main.tile[x, y];
                    bool flag2 = true;
                    if (tile == null || tile.liquid == (byte) 0)
                        flag2 = false;
                    if (!tile.lava() && type == TELogicSensor.LogicCheckType.Lava)
                        flag2 = false;
                    if (!tile.honey() && type == TELogicSensor.LogicCheckType.Honey)
                        flag2 = false;
                    if ((tile.honey() || tile.lava()) && type == TELogicSensor.LogicCheckType.Water)
                        flag2 = false;
                    if (!flag2 && instance.On)
                    {
                        if (instance.CountedData == 0)
                            instance.CountedData = 15;
                        else if (instance.CountedData > 0)
                            --instance.CountedData;
                        flag2 = instance.CountedData > 0;
                    }

                    return flag2;
                default:
                    return false;
            }
        }

        public void FigureCheckState()
        {
            this.logicCheck = TELogicSensor.FigureCheckType((int) this.Position.X, (int) this.Position.Y, out this.On);
            TELogicSensor.GetFrame((int) this.Position.X, (int) this.Position.Y, this.logicCheck, this.On);
        }

        public static void GetFrame(int x, int y, TELogicSensor.LogicCheckType type, bool on)
        {
            Main.tile[x, y].frameX = on ? (short) 18 : (short) 0;
            switch (type)
            {
                case TELogicSensor.LogicCheckType.Day:
                    Main.tile[x, y].frameY = (short) 0;
                    break;
                case TELogicSensor.LogicCheckType.Night:
                    Main.tile[x, y].frameY = (short) 18;
                    break;
                case TELogicSensor.LogicCheckType.PlayerAbove:
                    Main.tile[x, y].frameY = (short) 36;
                    break;
                case TELogicSensor.LogicCheckType.Water:
                    Main.tile[x, y].frameY = (short) 54;
                    break;
                case TELogicSensor.LogicCheckType.Lava:
                    Main.tile[x, y].frameY = (short) 72;
                    break;
                case TELogicSensor.LogicCheckType.Honey:
                    Main.tile[x, y].frameY = (short) 90;
                    break;
                case TELogicSensor.LogicCheckType.Liquid:
                    Main.tile[x, y].frameY = (short) 108;
                    break;
                default:
                    Main.tile[x, y].frameY = (short) 0;
                    break;
            }
        }

        public static bool SanityCheck(int x, int y)
        {
            if (Main.tile[x, y].active() && Main.tile[x, y].type == (ushort) 423)
                return true;
            TELogicSensor.Kill(x, y);
            return false;
        }

        public static int Place(int x, int y)
        {
            TELogicSensor teLogicSensor = new TELogicSensor();
            teLogicSensor.Position = new Point16(x, y);
            teLogicSensor.ID = TileEntity.AssignNewID();
            teLogicSensor.type = (byte) 2;
            TileEntity.ByID[teLogicSensor.ID] = (TileEntity) teLogicSensor;
            TileEntity.ByPosition[teLogicSensor.Position] = (TileEntity) teLogicSensor;
            return teLogicSensor.ID;
        }

        public static int Hook_AfterPlacement(int x, int y, int type = 423, int style = 0, int direction = 1)
        {
            bool on;
            TELogicSensor.LogicCheckType type1 = TELogicSensor.FigureCheckType(x, y, out on);
            TELogicSensor.GetFrame(x, y, type1, on);
            if (Main.netMode == 1)
            {
                NetMessage.SendTileSquare(Main.myPlayer, x, y, 1, TileChangeType.None);
                NetMessage.SendData(87, -1, -1, (NetworkText) null, x, (float) y, 2f, 0.0f, 0, 0, 0);
                return -1;
            }

            int index = TELogicSensor.Place(x, y);
            ((TELogicSensor) TileEntity.ByID[index]).FigureCheckState();
            return index;
        }

        public static void Kill(int x, int y)
        {
            TileEntity tileEntity;
            if (!TileEntity.ByPosition.TryGetValue(new Point16(x, y), out tileEntity) || tileEntity.type != (byte) 2)
                return;
            Wiring.blockPlayerTeleportationForOneIteration =
                ((TELogicSensor) tileEntity).logicCheck == TELogicSensor.LogicCheckType.PlayerAbove;
            if (((TELogicSensor) tileEntity).logicCheck == TELogicSensor.LogicCheckType.PlayerAbove &&
                ((TELogicSensor) tileEntity).On)
                Wiring.HitSwitch((int) tileEntity.Position.X, (int) tileEntity.Position.Y);
            if (((TELogicSensor) tileEntity).logicCheck == TELogicSensor.LogicCheckType.Water &&
                ((TELogicSensor) tileEntity).On)
                Wiring.HitSwitch((int) tileEntity.Position.X, (int) tileEntity.Position.Y);
            if (((TELogicSensor) tileEntity).logicCheck == TELogicSensor.LogicCheckType.Lava &&
                ((TELogicSensor) tileEntity).On)
                Wiring.HitSwitch((int) tileEntity.Position.X, (int) tileEntity.Position.Y);
            if (((TELogicSensor) tileEntity).logicCheck == TELogicSensor.LogicCheckType.Honey &&
                ((TELogicSensor) tileEntity).On)
                Wiring.HitSwitch((int) tileEntity.Position.X, (int) tileEntity.Position.Y);
            if (((TELogicSensor) tileEntity).logicCheck == TELogicSensor.LogicCheckType.Liquid &&
                ((TELogicSensor) tileEntity).On)
                Wiring.HitSwitch((int) tileEntity.Position.X, (int) tileEntity.Position.Y);
            Wiring.blockPlayerTeleportationForOneIteration = false;
            if (TELogicSensor.inUpdateLoop)
            {
                TELogicSensor.markedIDsForRemoval.Add(tileEntity.ID);
            }
            else
            {
                TileEntity.ByPosition.Remove(new Point16(x, y));
                TileEntity.ByID.Remove(tileEntity.ID);
            }
        }

        public static int Find(int x, int y)
        {
            TileEntity tileEntity;
            if (TileEntity.ByPosition.TryGetValue(new Point16(x, y), out tileEntity) && tileEntity.type == (byte) 2)
                return tileEntity.ID;
            return -1;
        }

        public override void WriteExtraData(BinaryWriter writer, bool networkSend)
        {
            if (networkSend)
                return;
            writer.Write((byte) this.logicCheck);
            writer.Write(this.On);
        }

        public override void ReadExtraData(BinaryReader reader, bool networkSend)
        {
            if (networkSend)
                return;
            this.logicCheck = (TELogicSensor.LogicCheckType) reader.ReadByte();
            this.On = reader.ReadBoolean();
        }

        public override string ToString()
        {
            return ((int) this.Position.X).ToString() + "x  " + (object) this.Position.Y + "y " +
                   (object) this.logicCheck;
        }

        public enum LogicCheckType
        {
            None,
            Day,
            Night,
            PlayerAbove,
            Water,
            Lava,
            Honey,
            Liquid,
        }
    }
}