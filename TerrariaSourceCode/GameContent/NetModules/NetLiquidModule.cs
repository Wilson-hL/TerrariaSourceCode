﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.NetModules.NetLiquidModule
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System.Collections.Generic;
using System.IO;
using Terraria.Net;

namespace Terraria.GameContent.NetModules
{
    public class NetLiquidModule : NetModule
    {
        public static NetPacket Serialize(HashSet<int> changes)
        {
            var packet = NetModule.CreatePacket<NetLiquidModule>(changes.Count * 6 + 2);
            packet.Writer.Write((ushort) changes.Count);
            foreach (var change in changes)
            {
                var index1 = change >> 16 & (int) ushort.MaxValue;
                var index2 = change & (int) ushort.MaxValue;
                packet.Writer.Write(change);
                packet.Writer.Write(Main.tile[index1, index2].liquid);
                packet.Writer.Write(Main.tile[index1, index2].liquidType());
            }

            return packet;
        }

        public override bool Deserialize(BinaryReader reader, int userId)
        {
            var num1 = (int) reader.ReadUInt16();
            for (var index1 = 0; index1 < num1; ++index1)
            {
                var num2 = reader.ReadInt32();
                var num3 = reader.ReadByte();
                var num4 = reader.ReadByte();
                var index2 = num2 >> 16 & (int) ushort.MaxValue;
                var index3 = num2 & (int) ushort.MaxValue;
                var tile = Main.tile[index2, index3];
                if (tile != null)
                {
                    tile.liquid = num3;
                    tile.liquidType((int) num4);
                }
            }

            return true;
        }
    }
}