// Decompiled with JetBrains decompiler
// Type: Terraria.Enums.AnchorType
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System;

namespace Terraria.Enums
{
    [Flags]
    public enum AnchorType
    {
        None = 0,
        SolidTile = 1,
        SolidWithTop = 2,
        Table = 4,
        SolidSide = 8,
        Tree = 16, // 0x00000010
        AlternateTile = 32, // 0x00000020
        EmptyTile = 64, // 0x00000040
        SolidBottom = 128 // 0x00000080
    }
}