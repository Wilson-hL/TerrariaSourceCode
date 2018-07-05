// Decompiled with JetBrains decompiler
// Type: Terraria.DataStructures.PlayerDeathReason
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System.IO;
using Terraria.Localization;

namespace Terraria.DataStructures
{
    public class PlayerDeathReason
    {
        private string SourceCustomReason;
        private int SourceItemPrefix;
        private int SourceItemType;
        private int SourceNPCIndex = -1;
        private int SourceOtherIndex = -1;
        private int SourcePlayerIndex = -1;
        private int SourceProjectileIndex = -1;
        private int SourceProjectileType;

        public static PlayerDeathReason LegacyEmpty()
        {
            return new PlayerDeathReason {SourceOtherIndex = 254};
        }

        public static PlayerDeathReason LegacyDefault()
        {
            return new PlayerDeathReason {SourceOtherIndex = byte.MaxValue};
        }

        public static PlayerDeathReason ByNPC(int index)
        {
            return new PlayerDeathReason {SourceNPCIndex = index};
        }

        public static PlayerDeathReason ByCustomReason(string reasonInEnglish)
        {
            return new PlayerDeathReason {SourceCustomReason = reasonInEnglish};
        }

        public static PlayerDeathReason ByPlayer(int index)
        {
            return new PlayerDeathReason
            {
                SourcePlayerIndex = index,
                SourceItemType = Main.player[index].inventory[Main.player[index].selectedItem].type,
                SourceItemPrefix = Main.player[index].inventory[Main.player[index].selectedItem].prefix
            };
        }

        public static PlayerDeathReason ByOther(int type)
        {
            return new PlayerDeathReason {SourceOtherIndex = type};
        }

        public static PlayerDeathReason ByProjectile(int playerIndex, int projectileIndex)
        {
            var playerDeathReason = new PlayerDeathReason
            {
                SourcePlayerIndex = playerIndex,
                SourceProjectileIndex = projectileIndex,
                SourceProjectileType = Main.projectile[projectileIndex].type
            };
            if (playerIndex >= 0 && playerIndex <= byte.MaxValue)
            {
                playerDeathReason.SourceItemType =
                    Main.player[playerIndex].inventory[Main.player[playerIndex].selectedItem].type;
                playerDeathReason.SourceItemPrefix =
                    Main.player[playerIndex].inventory[Main.player[playerIndex].selectedItem].prefix;
            }

            return playerDeathReason;
        }

        public NetworkText GetDeathText(string deadPlayerName)
        {
            if (SourceCustomReason != null)
                return NetworkText.FromLiteral(SourceCustomReason);
            return Lang.CreateDeathMessage(deadPlayerName, SourcePlayerIndex, SourceNPCIndex,
                SourceProjectileIndex, SourceOtherIndex, SourceProjectileType, SourceItemType);
        }

        public void WriteSelfTo(BinaryWriter writer)
        {
            var bitsByte = (BitsByte) 0;
            bitsByte[0] = SourcePlayerIndex != -1;
            bitsByte[1] = SourceNPCIndex != -1;
            bitsByte[2] = SourceProjectileIndex != -1;
            bitsByte[3] = SourceOtherIndex != -1;
            bitsByte[4] = SourceProjectileType != 0;
            bitsByte[5] = SourceItemType != 0;
            bitsByte[6] = SourceItemPrefix != 0;
            bitsByte[7] = SourceCustomReason != null;
            writer.Write(bitsByte);
            if (bitsByte[0])
                writer.Write((short) SourcePlayerIndex);
            if (bitsByte[1])
                writer.Write((short) SourceNPCIndex);
            if (bitsByte[2])
                writer.Write((short) SourceProjectileIndex);
            if (bitsByte[3])
                writer.Write((byte) SourceOtherIndex);
            if (bitsByte[4])
                writer.Write((short) SourceProjectileType);
            if (bitsByte[5])
                writer.Write((short) SourceItemType);
            if (bitsByte[6])
                writer.Write((byte) SourceItemPrefix);
            if (!bitsByte[7])
                return;
            writer.Write(SourceCustomReason);
        }

        public static PlayerDeathReason FromReader(BinaryReader reader)
        {
            var playerDeathReason = new PlayerDeathReason();
            var bitsByte = (BitsByte) reader.ReadByte();
            if (bitsByte[0])
                playerDeathReason.SourcePlayerIndex = reader.ReadInt16();
            if (bitsByte[1])
                playerDeathReason.SourceNPCIndex = reader.ReadInt16();
            if (bitsByte[2])
                playerDeathReason.SourceProjectileIndex = reader.ReadInt16();
            if (bitsByte[3])
                playerDeathReason.SourceOtherIndex = reader.ReadByte();
            if (bitsByte[4])
                playerDeathReason.SourceProjectileType = reader.ReadInt16();
            if (bitsByte[5])
                playerDeathReason.SourceItemType = reader.ReadInt16();
            if (bitsByte[6])
                playerDeathReason.SourceItemPrefix = reader.ReadByte();
            if (bitsByte[7])
                playerDeathReason.SourceCustomReason = reader.ReadString();
            return playerDeathReason;
        }
    }
}