﻿// Decompiled with JetBrains decompiler
// Type: Terraria.Social.Steam.CloudSocialModule
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Steamworks;
using System;
using System.Collections.Generic;

namespace Terraria.Social.Steam
{
    public class CloudSocialModule : Terraria.Social.Base.CloudSocialModule
    {
        private object ioLock = new object();
        private byte[] writeBuffer = new byte[1024];
        private const uint WRITE_CHUNK_SIZE = 1024;

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Shutdown()
        {
        }

        public override IEnumerable<string> GetFiles()
        {
            lock (this.ioLock)
            {
                var fileCount = SteamRemoteStorage.GetFileCount();
                var stringList = new List<string>(fileCount);
                for (var index = 0; index < fileCount; ++index)
                {
                    int num;
                    stringList.Add(SteamRemoteStorage.GetFileNameAndSize(index, out num));
                }

                return (IEnumerable<string>) stringList;
            }
        }

        public override bool Write(string path, byte[] data, int length)
        {
            lock (this.ioLock)
            {
                var writeStreamHandleT = SteamRemoteStorage.FileWriteStreamOpen(path);
                uint num1 = 0;
                while ((long) num1 < (long) length)
                {
                    var num2 = (int) Math.Min(1024L, (long) length - (long) num1);
                    Array.Copy((Array) data, (long) num1, (Array) this.writeBuffer, 0L, (long) num2);
                    SteamRemoteStorage.FileWriteStreamWriteChunk(writeStreamHandleT, this.writeBuffer, num2);
                    num1 += 1024U;
                }

                return SteamRemoteStorage.FileWriteStreamClose(writeStreamHandleT);
            }
        }

        public override int GetFileSize(string path)
        {
            lock (this.ioLock)
                return SteamRemoteStorage.GetFileSize(path);
        }

        public override void Read(string path, byte[] buffer, int size)
        {
            lock (this.ioLock)
                SteamRemoteStorage.FileRead(path, buffer, size);
        }

        public override bool HasFile(string path)
        {
            lock (this.ioLock)
                return SteamRemoteStorage.FileExists(path);
        }

        public override bool Delete(string path)
        {
            lock (this.ioLock)
                return SteamRemoteStorage.FileDelete(path);
        }
    }
}