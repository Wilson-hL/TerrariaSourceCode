// Decompiled with JetBrains decompiler
// Type: Terraria.DataStructures.CachedBuffer
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System.IO;

namespace Terraria.DataStructures
{
    public class CachedBuffer
    {
        private readonly MemoryStream _memoryStream;
        public readonly byte[] Data;
        public readonly BinaryReader Reader;
        public readonly BinaryWriter Writer;

        public CachedBuffer(byte[] data)
        {
            Data = data;
            _memoryStream = new MemoryStream(data);
            Writer = new BinaryWriter(_memoryStream);
            Reader = new BinaryReader(_memoryStream);
        }

        public int Length => Data.Length;

        public bool IsActive { get; private set; } = true;

        internal CachedBuffer Activate()
        {
            IsActive = true;
            _memoryStream.Position = 0L;
            return this;
        }

        public void Recycle()
        {
            if (!IsActive)
                return;
            IsActive = false;
            BufferPool.Recycle(this);
        }
    }
}