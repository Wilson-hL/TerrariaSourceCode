// Decompiled with JetBrains decompiler
// Type: Terraria.Social.Steam.SteamP2PWriter
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Steamworks;
using System;
using System.Collections.Generic;

namespace Terraria.Social.Steam
{
    public class SteamP2PWriter
    {
        private Dictionary<CSteamID, Queue<SteamP2PWriter.WriteInformation>> _pendingSendData =
            new Dictionary<CSteamID, Queue<SteamP2PWriter.WriteInformation>>();

        private Dictionary<CSteamID, Queue<SteamP2PWriter.WriteInformation>> _pendingSendDataSwap =
            new Dictionary<CSteamID, Queue<SteamP2PWriter.WriteInformation>>();

        private Queue<byte[]> _bufferPool = new Queue<byte[]>();
        private object _lock = new object();
        private const int BUFFER_SIZE = 1024;
        private int _channel;

        public SteamP2PWriter(int channel)
        {
            this._channel = channel;
        }

        public void QueueSend(CSteamID user, byte[] data, int length)
        {
            lock (this._lock)
            {
                Queue<SteamP2PWriter.WriteInformation> writeInformationQueue;
                if (this._pendingSendData.ContainsKey(user))
                    writeInformationQueue = this._pendingSendData[user];
                else
                    this._pendingSendData[user] = writeInformationQueue = new Queue<SteamP2PWriter.WriteInformation>();
                var val1 = length;
                var sourceIndex = 0;
                while (val1 > 0)
                {
                    SteamP2PWriter.WriteInformation writeInformation;
                    if (writeInformationQueue.Count == 0 || 1024 - writeInformationQueue.Peek().Size == 0)
                    {
                        writeInformation = this._bufferPool.Count <= 0
                            ? new SteamP2PWriter.WriteInformation()
                            : new SteamP2PWriter.WriteInformation(this._bufferPool.Dequeue());
                        writeInformationQueue.Enqueue(writeInformation);
                    }
                    else
                        writeInformation = writeInformationQueue.Peek();

                    var length1 = Math.Min(val1, 1024 - writeInformation.Size);
                    Array.Copy((Array) data, sourceIndex, (Array) writeInformation.Data, writeInformation.Size,
                        length1);
                    writeInformation.Size += length1;
                    val1 -= length1;
                    sourceIndex += length1;
                }
            }
        }

        public void ClearUser(CSteamID user)
        {
            lock (this._lock)
            {
                if (this._pendingSendData.ContainsKey(user))
                {
                    var writeInformationQueue = this._pendingSendData[user];
                    while (writeInformationQueue.Count > 0)
                        this._bufferPool.Enqueue(writeInformationQueue.Dequeue().Data);
                }

                if (!this._pendingSendDataSwap.ContainsKey(user))
                    return;
                var writeInformationQueue1 = this._pendingSendDataSwap[user];
                while (writeInformationQueue1.Count > 0)
                    this._bufferPool.Enqueue(writeInformationQueue1.Dequeue().Data);
            }
        }

        public void SendAll()
        {
            lock (this._lock)
                Utils.Swap<Dictionary<CSteamID, Queue<SteamP2PWriter.WriteInformation>>>(ref this._pendingSendData,
                    ref this._pendingSendDataSwap);
            using (var enumerator =
                this._pendingSendDataSwap.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var current = enumerator.Current;
                    var writeInformationQueue = current.Value;
                    while (writeInformationQueue.Count > 0)
                    {
                        var writeInformation = writeInformationQueue.Dequeue();
                        SteamNetworking.SendP2PPacket(current.Key, writeInformation.Data, (uint) writeInformation.Size,
                            (EP2PSend) 2, this._channel);
                        this._bufferPool.Enqueue(writeInformation.Data);
                    }
                }
            }
        }

        public class WriteInformation
        {
            public byte[] Data;
            public int Size;

            public WriteInformation()
            {
                this.Data = new byte[1024];
                this.Size = 0;
            }

            public WriteInformation(byte[] data)
            {
                this.Data = data;
                this.Size = 0;
            }
        }
    }
}