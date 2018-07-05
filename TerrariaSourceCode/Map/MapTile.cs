﻿// Decompiled with JetBrains decompiler
// Type: Terraria.Map.MapTile
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

namespace Terraria.Map
{
    public struct MapTile
    {
        public ushort Type;
        public byte Light;
        private byte _extraData;

        public bool IsChanged
        {
            get { return ((int) this._extraData & 128) == 128; }
            set
            {
                if (value)
                    this._extraData |= (byte) 128;
                else
                    this._extraData &= (byte) 127;
            }
        }

        public byte Color
        {
            get { return (byte) ((uint) this._extraData & (uint) sbyte.MaxValue); }
            set { this._extraData = (byte) ((int) this._extraData & 128 | (int) value & (int) sbyte.MaxValue); }
        }

        private MapTile(ushort type, byte light, byte extraData)
        {
            this.Type = type;
            this.Light = light;
            this._extraData = extraData;
        }

        public bool Equals(ref MapTile other)
        {
            if ((int) this.Light == (int) other.Light && (int) this.Type == (int) other.Type)
                return (int) this.Color == (int) other.Color;
            return false;
        }

        public bool EqualsWithoutLight(ref MapTile other)
        {
            if ((int) this.Type == (int) other.Type)
                return (int) this.Color == (int) other.Color;
            return false;
        }

        public void Clear()
        {
            this.Type = (ushort) 0;
            this.Light = (byte) 0;
            this._extraData = (byte) 0;
        }

        public MapTile WithLight(byte light)
        {
            return new MapTile(this.Type, light, (byte) ((uint) this._extraData | 128U));
        }

        public static MapTile Create(ushort type, byte light, byte color)
        {
            return new MapTile(type, light, (byte) ((uint) color | 128U));
        }
    }
}