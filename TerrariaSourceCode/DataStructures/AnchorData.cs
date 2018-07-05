// Decompiled with JetBrains decompiler
// Type: Terraria.DataStructures.AnchorData
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Terraria.Enums;

namespace Terraria.DataStructures
{
    public struct AnchorData
    {
        public static AnchorData Empty = new AnchorData();
        public AnchorType type;
        public int tileCount;
        public int checkStart;

        public AnchorData(AnchorType type, int count, int start)
        {
            this.type = type;
            tileCount = count;
            checkStart = start;
        }

        public static bool operator ==(AnchorData data1, AnchorData data2)
        {
            if (data1.type == data2.type && data1.tileCount == data2.tileCount)
                return data1.checkStart == data2.checkStart;
            return false;
        }

        public static bool operator !=(AnchorData data1, AnchorData data2)
        {
            if (data1.type == data2.type && data1.tileCount == data2.tileCount)
                return data1.checkStart != data2.checkStart;
            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj is AnchorData && type == ((AnchorData) obj).type &&
                tileCount == ((AnchorData) obj).tileCount)
                return checkStart == ((AnchorData) obj).checkStart;
            return false;
        }

        public override int GetHashCode()
        {
            return ((ushort) type << 16) | ((byte) tileCount << 8) | (byte) checkStart;
        }
    }
}