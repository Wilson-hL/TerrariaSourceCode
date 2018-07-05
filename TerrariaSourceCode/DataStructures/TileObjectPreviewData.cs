// Decompiled with JetBrains decompiler
// Type: Terraria.DataStructures.TileObjectPreviewData
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System;

namespace Terraria.DataStructures
{
    public class TileObjectPreviewData
    {
        public const int None = 0;
        public const int ValidSpot = 1;
        public const int InvalidSpot = 2;
        public static TileObjectPreviewData placementCache;
        public static TileObjectPreviewData randomCache;
        private int[,] _data;
        private Point16 _dataSize;
        private float _percentValid;
        private Point16 _size;

        public bool Active { get; set; }

        public ushort Type { get; set; }

        public short Style { get; set; }

        public int Alternate { get; set; }

        public int Random { get; set; }

        public Point16 Size
        {
            get => _size;
            set
            {
                if (value.X <= 0 || value.Y <= 0)
                    throw new FormatException("PlacementData.Size was set to a negative value.");
                if (value.X > _dataSize.X || value.Y > _dataSize.Y)
                {
                    var X = (int) value.X > (int) _dataSize.X ? value.X : (int) _dataSize.X;
                    var Y = (int) value.Y > (int) _dataSize.Y ? value.Y : (int) _dataSize.Y;
                    var numArray = new int[X, Y];
                    if (_data != null)
                        for (var index1 = 0; index1 < (int) _dataSize.X; ++index1)
                        for (var index2 = 0; index2 < (int) _dataSize.Y; ++index2)
                            numArray[index1, index2] = _data[index1, index2];

                    _data = numArray;
                    _dataSize = new Point16(X, Y);
                }

                _size = value;
            }
        }

        public Point16 Coordinates { get; set; }

        public Point16 ObjectStart { get; set; }

        public int this[int x, int y]
        {
            get
            {
                if (x < 0 || y < 0 || x >= _size.X || y >= _size.Y)
                    throw new IndexOutOfRangeException();
                return _data[x, y];
            }
            set
            {
                if (x < 0 || y < 0 || x >= _size.X || y >= _size.Y)
                    throw new IndexOutOfRangeException();
                _data[x, y] = value;
            }
        }

        public void Reset()
        {
            Active = false;
            _size = Point16.Zero;
            Coordinates = Point16.Zero;
            ObjectStart = Point16.Zero;
            _percentValid = 0.0f;
            Type = 0;
            Style = 0;
            Alternate = -1;
            Random = -1;
            if (_data == null)
                return;
            Array.Clear(_data, 0, _dataSize.X * _dataSize.Y);
        }

        public void CopyFrom(TileObjectPreviewData copy)
        {
            Type = copy.Type;
            Style = copy.Style;
            Alternate = copy.Alternate;
            Random = copy.Random;
            Active = copy.Active;
            _size = copy._size;
            Coordinates = copy.Coordinates;
            ObjectStart = copy.ObjectStart;
            _percentValid = copy._percentValid;
            if (_data == null)
            {
                _data = new int[copy._dataSize.X, copy._dataSize.Y];
                _dataSize = copy._dataSize;
            }
            else
            {
                Array.Clear(_data, 0, _data.Length);
            }

            if (_dataSize.X < copy._dataSize.X || _dataSize.Y < copy._dataSize.Y)
            {
                var X = (int) copy._dataSize.X > (int) _dataSize.X
                    ? copy._dataSize.X
                    : (int) _dataSize.X;
                var Y = (int) copy._dataSize.Y > (int) _dataSize.Y
                    ? copy._dataSize.Y
                    : (int) _dataSize.Y;
                _data = new int[X, Y];
                _dataSize = new Point16(X, Y);
            }

            for (var index1 = 0; index1 < (int) copy._dataSize.X; ++index1)
            for (var index2 = 0; index2 < (int) copy._dataSize.Y; ++index2)
                _data[index1, index2] = copy._data[index1, index2];
        }

        public void AllInvalid()
        {
            for (var index1 = 0; index1 < (int) _size.X; ++index1)
            for (var index2 = 0; index2 < (int) _size.Y; ++index2)
                if (_data[index1, index2] != 0)
                    _data[index1, index2] = 2;
        }
    }
}