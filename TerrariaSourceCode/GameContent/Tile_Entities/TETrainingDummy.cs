﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Tile_Entities.TETrainingDummy
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
    public class TETrainingDummy : TileEntity
    {
        private static Dictionary<int, Rectangle> playerBox = new Dictionary<int, Rectangle>();
        private static bool playerBoxFilled = false;
        public int npc;

        public static void Initialize()
        {
            TileEntity._UpdateStart += new Action(TETrainingDummy.ClearBoxes);
            TileEntity._NetPlaceEntity += new Action<int, int, int>(TETrainingDummy.NetPlaceEntity);
        }

        public static void NetPlaceEntity(int x, int y, int type)
        {
            if (type != 0 || !TETrainingDummy.ValidTile(x, y))
                return;
            TETrainingDummy.Place(x, y);
        }

        public static void ClearBoxes()
        {
            TETrainingDummy.playerBox.Clear();
            TETrainingDummy.playerBoxFilled = false;
        }

        public override void Update()
        {
            Rectangle rectangle = new Rectangle(0, 0, 32, 48);
            rectangle.Inflate(1600, 1600);
            int x = rectangle.X;
            int y = rectangle.Y;
            if (this.npc != -1)
            {
                if (Main.npc[this.npc].active && Main.npc[this.npc].type == 488 &&
                    ((double) Main.npc[this.npc].ai[0] == (double) this.Position.X &&
                     (double) Main.npc[this.npc].ai[1] == (double) this.Position.Y))
                    return;
                this.Deactivate();
            }
            else
            {
                TETrainingDummy.FillPlayerHitboxes();
                rectangle.X = (int) this.Position.X * 16 + x;
                rectangle.Y = (int) this.Position.Y * 16 + y;
                bool flag = false;
                foreach (KeyValuePair<int, Rectangle> keyValuePair in TETrainingDummy.playerBox)
                {
                    if (keyValuePair.Value.Intersects(rectangle))
                    {
                        flag = true;
                        break;
                    }
                }

                if (!flag)
                    return;
                this.Activate();
            }
        }

        private static void FillPlayerHitboxes()
        {
            if (TETrainingDummy.playerBoxFilled)
                return;
            for (int index = 0; index < (int) byte.MaxValue; ++index)
            {
                if (Main.player[index].active)
                    TETrainingDummy.playerBox[index] = Main.player[index].getRect();
            }

            TETrainingDummy.playerBoxFilled = true;
        }

        public static bool ValidTile(int x, int y)
        {
            return Main.tile[x, y].active() && Main.tile[x, y].type == (ushort) 378 &&
                   (Main.tile[x, y].frameY == (short) 0 && (int) Main.tile[x, y].frameX % 36 == 0);
        }

        public TETrainingDummy()
        {
            this.npc = -1;
        }

        public static int Place(int x, int y)
        {
            TETrainingDummy teTrainingDummy = new TETrainingDummy();
            teTrainingDummy.Position = new Point16(x, y);
            teTrainingDummy.ID = TileEntity.AssignNewID();
            teTrainingDummy.type = (byte) 0;
            TileEntity.ByID[teTrainingDummy.ID] = (TileEntity) teTrainingDummy;
            TileEntity.ByPosition[teTrainingDummy.Position] = (TileEntity) teTrainingDummy;
            return teTrainingDummy.ID;
        }

        public static int Hook_AfterPlacement(int x, int y, int type = 378, int style = 0, int direction = 1)
        {
            if (Main.netMode != 1)
                return TETrainingDummy.Place(x - 1, y - 2);
            NetMessage.SendTileSquare(Main.myPlayer, x - 1, y - 1, 3, TileChangeType.None);
            NetMessage.SendData(87, -1, -1, (NetworkText) null, x - 1, (float) (y - 2), 0.0f, 0.0f, 0, 0, 0);
            return -1;
        }

        public static void Kill(int x, int y)
        {
            TileEntity tileEntity;
            if (!TileEntity.ByPosition.TryGetValue(new Point16(x, y), out tileEntity) || tileEntity.type != (byte) 0)
                return;
            TileEntity.ByID.Remove(tileEntity.ID);
            TileEntity.ByPosition.Remove(new Point16(x, y));
        }

        public static int Find(int x, int y)
        {
            TileEntity tileEntity;
            if (TileEntity.ByPosition.TryGetValue(new Point16(x, y), out tileEntity) && tileEntity.type == (byte) 0)
                return tileEntity.ID;
            return -1;
        }

        public override void WriteExtraData(BinaryWriter writer, bool networkSend)
        {
            writer.Write((short) this.npc);
        }

        public override void ReadExtraData(BinaryReader reader, bool networkSend)
        {
            this.npc = (int) reader.ReadInt16();
        }

        public void Activate()
        {
            int index = NPC.NewNPC((int) this.Position.X * 16 + 16, (int) this.Position.Y * 16 + 48, 488, 100, 0.0f,
                0.0f, 0.0f, 0.0f, (int) byte.MaxValue);
            Main.npc[index].ai[0] = (float) this.Position.X;
            Main.npc[index].ai[1] = (float) this.Position.Y;
            Main.npc[index].netUpdate = true;
            this.npc = index;
            if (Main.netMode == 1)
                return;
            NetMessage.SendData(86, -1, -1, (NetworkText) null, this.ID, (float) this.Position.X,
                (float) this.Position.Y, 0.0f, 0, 0, 0);
        }

        public void Deactivate()
        {
            if (this.npc != -1)
                Main.npc[this.npc].active = false;
            this.npc = -1;
            if (Main.netMode == 1)
                return;
            NetMessage.SendData(86, -1, -1, (NetworkText) null, this.ID, (float) this.Position.X,
                (float) this.Position.Y, 0.0f, 0, 0, 0);
        }

        public override string ToString()
        {
            return ((int) this.Position.X).ToString() + "x  " + (object) this.Position.Y + "y npc: " +
                   (object) this.npc;
        }
    }
}