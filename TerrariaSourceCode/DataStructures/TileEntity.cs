// Decompiled with JetBrains decompiler
// Type: Terraria.DataStructures.TileEntity
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System;
using System.Collections.Generic;
using System.IO;
using Terraria.GameContent.Tile_Entities;

namespace Terraria.DataStructures
{
    public abstract class TileEntity
    {
        public const int MaxEntitiesPerChunk = 1000;
        public static Dictionary<int, TileEntity> ByID = new Dictionary<int, TileEntity>();
        public static Dictionary<Point16, TileEntity> ByPosition = new Dictionary<Point16, TileEntity>();
        public static int TileEntitiesNextID;
        public int ID;
        public Point16 Position;
        public byte type;

        public static int AssignNewID()
        {
            return TileEntitiesNextID++;
        }

        public static event Action _UpdateStart;

        public static event Action _UpdateEnd;

        public static event Action<int, int, int> _NetPlaceEntity;

        public static void Clear()
        {
            ByID.Clear();
            ByPosition.Clear();
            TileEntitiesNextID = 0;
        }

        public static void UpdateStart()
        {
            if (_UpdateStart == null)
                return;
            _UpdateStart();
        }

        public static void UpdateEnd()
        {
            if (_UpdateEnd == null)
                return;
            _UpdateEnd();
        }

        public static void InitializeAll()
        {
            TETrainingDummy.Initialize();
            TEItemFrame.Initialize();
            TELogicSensor.Initialize();
        }

        public static void PlaceEntityNet(int x, int y, int type)
        {
            if (!WorldGen.InWorld(x, y, 0) || ByPosition.ContainsKey(new Point16(x, y)) ||
                _NetPlaceEntity == null)
                return;
            _NetPlaceEntity(x, y, type);
        }

        public virtual void Update()
        {
        }

        public static void Write(BinaryWriter writer, TileEntity ent, bool networkSend = false)
        {
            writer.Write(ent.type);
            ent.WriteInner(writer, networkSend);
        }

        public static TileEntity Read(BinaryReader reader, bool networkSend = false)
        {
            var tileEntity = (TileEntity) null;
            var num = reader.ReadByte();
            switch (num)
            {
                case 0:
                    tileEntity = (TileEntity) new TETrainingDummy();
                    break;
                case 1:
                    tileEntity = (TileEntity) new TEItemFrame();
                    break;
                case 2:
                    tileEntity = (TileEntity) new TELogicSensor();
                    break;
            }

            tileEntity.type = num;
            tileEntity.ReadInner(reader, networkSend);
            return tileEntity;
        }

        private void WriteInner(BinaryWriter writer, bool networkSend)
        {
            if (!networkSend)
                writer.Write(ID);
            writer.Write(Position.X);
            writer.Write(Position.Y);
            WriteExtraData(writer, networkSend);
        }

        private void ReadInner(BinaryReader reader, bool networkSend)
        {
            if (!networkSend)
                ID = reader.ReadInt32();
            Position = new Point16(reader.ReadInt16(), reader.ReadInt16());
            ReadExtraData(reader, networkSend);
        }

        public virtual void WriteExtraData(BinaryWriter writer, bool networkSend)
        {
        }

        public virtual void ReadExtraData(BinaryReader reader, bool networkSend)
        {
        }
    }
}