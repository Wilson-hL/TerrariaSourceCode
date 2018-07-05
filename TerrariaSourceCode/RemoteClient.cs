﻿// Decompiled with JetBrains decompiler
// Type: Terraria.RemoteClient
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria.Net.Sockets;

namespace Terraria
{
    public class RemoteClient
    {
        public string Name = "Anonymous";
        public string StatusText = "";
        public bool[,] TileSections = new bool[Main.maxTilesX / 200 + 1, Main.maxTilesY / 150 + 1];
        public float SpamProjectileMax = 100f;
        public float SpamAddBlockMax = 100f;
        public float SpamDeleteBlockMax = 500f;
        public float SpamWaterMax = 50f;
        public ISocket Socket;
        public int Id;
        public bool IsActive;
        public bool PendingTermination;
        public bool IsAnnouncementCompleted;
        public bool IsReading;
        public int State;
        public int TimeOutTimer;
        public string StatusText2;
        public int StatusCount;
        public int StatusMax;
        public byte[] ReadBuffer;
        public float SpamProjectile;
        public float SpamAddBlock;
        public float SpamDeleteBlock;
        public float SpamWater;

        public bool IsConnected()
        {
            if (this.Socket != null)
                return this.Socket.IsConnected();
            return false;
        }

        public void SpamUpdate()
        {
            if (!Netplay.spamCheck)
            {
                this.SpamProjectile = 0.0f;
                this.SpamDeleteBlock = 0.0f;
                this.SpamAddBlock = 0.0f;
                this.SpamWater = 0.0f;
            }
            else
            {
                if ((double) this.SpamProjectile > (double) this.SpamProjectileMax)
                    NetMessage.BootPlayer(this.Id, NetworkText.FromKey("Net.CheatingProjectileSpam"));
                if ((double) this.SpamAddBlock > (double) this.SpamAddBlockMax)
                    NetMessage.BootPlayer(this.Id, NetworkText.FromKey("Net.CheatingTileSpam"));
                if ((double) this.SpamDeleteBlock > (double) this.SpamDeleteBlockMax)
                    NetMessage.BootPlayer(this.Id, NetworkText.FromKey("Net.CheatingTileRemovalSpam"));
                if ((double) this.SpamWater > (double) this.SpamWaterMax)
                    NetMessage.BootPlayer(this.Id, NetworkText.FromKey("Net.CheatingLiquidSpam"));
                this.SpamProjectile -= 0.4f;
                if ((double) this.SpamProjectile < 0.0)
                    this.SpamProjectile = 0.0f;
                this.SpamAddBlock -= 0.3f;
                if ((double) this.SpamAddBlock < 0.0)
                    this.SpamAddBlock = 0.0f;
                this.SpamDeleteBlock -= 5f;
                if ((double) this.SpamDeleteBlock < 0.0)
                    this.SpamDeleteBlock = 0.0f;
                this.SpamWater -= 0.2f;
                if ((double) this.SpamWater >= 0.0)
                    return;
                this.SpamWater = 0.0f;
            }
        }

        public void SpamClear()
        {
            this.SpamProjectile = 0.0f;
            this.SpamAddBlock = 0.0f;
            this.SpamDeleteBlock = 0.0f;
            this.SpamWater = 0.0f;
        }

        public static void CheckSection(int playerIndex, Vector2 position, int fluff = 1)
        {
            var index1 = playerIndex;
            var sectionX = Netplay.GetSectionX((int) ((double) position.X / 16.0));
            var sectionY1 = Netplay.GetSectionY((int) ((double) position.Y / 16.0));
            var num = 0;
            for (var index2 = sectionX - fluff; index2 < sectionX + fluff + 1; ++index2)
            {
                for (var index3 = sectionY1 - fluff; index3 < sectionY1 + fluff + 1; ++index3)
                {
                    if (index2 >= 0 && index2 < Main.maxSectionsX && (index3 >= 0 && index3 < Main.maxSectionsY) &&
                        !Netplay.Clients[index1].TileSections[index2, index3])
                        ++num;
                }
            }

            if (num <= 0)
                return;
            var number = num;
            NetMessage.SendData(9, index1, -1, Lang.inter[44].ToNetworkText(), number, 0.0f, 0.0f, 0.0f, 0, 0, 0);
            Netplay.Clients[index1].StatusText2 = Language.GetTextValue("Net.IsReceivingTileData");
            Netplay.Clients[index1].StatusMax += number;
            for (var index2 = sectionX - fluff; index2 < sectionX + fluff + 1; ++index2)
            {
                for (var sectionY2 = sectionY1 - fluff; sectionY2 < sectionY1 + fluff + 1; ++sectionY2)
                {
                    if (index2 >= 0 && index2 < Main.maxSectionsX &&
                        (sectionY2 >= 0 && sectionY2 < Main.maxSectionsY) &&
                        !Netplay.Clients[index1].TileSections[index2, sectionY2])
                    {
                        NetMessage.SendSection(index1, index2, sectionY2, false);
                        NetMessage.SendData(11, index1, -1, (NetworkText) null, index2, (float) sectionY2,
                            (float) index2, (float) sectionY2, 0, 0, 0);
                    }
                }
            }
        }

        public bool SectionRange(int size, int firstX, int firstY)
        {
            for (var index = 0; index < 4; ++index)
            {
                var x = firstX;
                var y = firstY;
                if (index == 1)
                    x += size;
                if (index == 2)
                    y += size;
                if (index == 3)
                {
                    x += size;
                    y += size;
                }

                if (this.TileSections[Netplay.GetSectionX(x), Netplay.GetSectionY(y)])
                    return true;
            }

            return false;
        }

        public void ResetSections()
        {
            for (var index1 = 0; index1 < Main.maxSectionsX; ++index1)
            {
                for (var index2 = 0; index2 < Main.maxSectionsY; ++index2)
                    this.TileSections[index1, index2] = false;
            }
        }

        public void Reset()
        {
            this.ResetSections();
            if (this.Id < (int) byte.MaxValue)
                Main.player[this.Id] = new Player();
            this.TimeOutTimer = 0;
            this.StatusCount = 0;
            this.StatusMax = 0;
            this.StatusText2 = "";
            this.StatusText = "";
            this.State = 0;
            this.IsReading = false;
            this.PendingTermination = false;
            this.SpamClear();
            this.IsActive = false;
            NetMessage.buffer[this.Id].Reset();
            if (this.Socket == null)
                return;
            this.Socket.Close();
        }

        public void ServerWriteCallBack(object state)
        {
            --NetMessage.buffer[this.Id].spamCount;
            if (this.StatusMax <= 0)
                return;
            ++this.StatusCount;
        }

        public void ServerReadCallBack(object state, int length)
        {
            if (!Netplay.disconnect)
            {
                var streamLength = length;
                if (streamLength == 0)
                    this.PendingTermination = true;
                else if (Main.ignoreErrors)
                {
                    try
                    {
                        NetMessage.ReceiveBytes(this.ReadBuffer, streamLength, this.Id);
                    }
                    catch
                    {
                    }
                }
                else
                    NetMessage.ReceiveBytes(this.ReadBuffer, streamLength, this.Id);
            }

            this.IsReading = false;
        }
    }
}