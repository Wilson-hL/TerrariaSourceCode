﻿// Decompiled with JetBrains decompiler
// Type: Terraria.Net.LegacyNetBufferPool
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System;
using System.Collections.Generic;

namespace Terraria.Net
{
    public class LegacyNetBufferPool
    {
        private static object bufferLock = new object();
        private static Queue<byte[]> _smallBufferQueue = new Queue<byte[]>();
        private static Queue<byte[]> _mediumBufferQueue = new Queue<byte[]>();
        private static Queue<byte[]> _largeBufferQueue = new Queue<byte[]>();
        private static int _smallBufferCount = 0;
        private static int _mediumBufferCount = 0;
        private static int _largeBufferCount = 0;
        private static int _customBufferCount = 0;
        private const int SMALL_BUFFER_SIZE = 256;
        private const int MEDIUM_BUFFER_SIZE = 1024;
        private const int LARGE_BUFFER_SIZE = 16384;

        public static byte[] RequestBuffer(int size)
        {
            lock (LegacyNetBufferPool.bufferLock)
            {
                if (size <= 256)
                {
                    if (LegacyNetBufferPool._smallBufferQueue.Count != 0)
                        return LegacyNetBufferPool._smallBufferQueue.Dequeue();
                    ++LegacyNetBufferPool._smallBufferCount;
                    return new byte[256];
                }

                if (size <= 1024)
                {
                    if (LegacyNetBufferPool._mediumBufferQueue.Count != 0)
                        return LegacyNetBufferPool._mediumBufferQueue.Dequeue();
                    ++LegacyNetBufferPool._mediumBufferCount;
                    return new byte[1024];
                }

                if (size <= 16384)
                {
                    if (LegacyNetBufferPool._largeBufferQueue.Count != 0)
                        return LegacyNetBufferPool._largeBufferQueue.Dequeue();
                    ++LegacyNetBufferPool._largeBufferCount;
                    return new byte[16384];
                }

                ++LegacyNetBufferPool._customBufferCount;
                return new byte[size];
            }
        }

        public static byte[] RequestBuffer(byte[] data, int offset, int size)
        {
            byte[] numArray = LegacyNetBufferPool.RequestBuffer(size);
            Buffer.BlockCopy((Array) data, offset, (Array) numArray, 0, size);
            return numArray;
        }

        public static void ReturnBuffer(byte[] buffer)
        {
            int length = buffer.Length;
            lock (LegacyNetBufferPool.bufferLock)
            {
                if (length <= 256)
                    LegacyNetBufferPool._smallBufferQueue.Enqueue(buffer);
                else if (length <= 1024)
                {
                    LegacyNetBufferPool._mediumBufferQueue.Enqueue(buffer);
                }
                else
                {
                    if (length > 16384)
                        return;
                    LegacyNetBufferPool._largeBufferQueue.Enqueue(buffer);
                }
            }
        }

        public static void DisplayBufferSizes()
        {
            lock (LegacyNetBufferPool.bufferLock)
            {
                Main.NewText(
                    "Small Buffers:  " + (object) LegacyNetBufferPool._smallBufferQueue.Count + " queued of " +
                    (object) LegacyNetBufferPool._smallBufferCount, byte.MaxValue, byte.MaxValue, byte.MaxValue, false);
                Main.NewText(
                    "Medium Buffers: " + (object) LegacyNetBufferPool._mediumBufferQueue.Count + " queued of " +
                    (object) LegacyNetBufferPool._mediumBufferCount, byte.MaxValue, byte.MaxValue, byte.MaxValue,
                    false);
                Main.NewText(
                    "Large Buffers:  " + (object) LegacyNetBufferPool._largeBufferQueue.Count + " queued of " +
                    (object) LegacyNetBufferPool._largeBufferCount, byte.MaxValue, byte.MaxValue, byte.MaxValue, false);
                Main.NewText("Custom Buffers: 0 queued of " + (object) LegacyNetBufferPool._customBufferCount,
                    byte.MaxValue, byte.MaxValue, byte.MaxValue, false);
            }
        }

        public static void PrintBufferSizes()
        {
            lock (LegacyNetBufferPool.bufferLock)
            {
                Console.WriteLine("Small Buffers:  " + (object) LegacyNetBufferPool._smallBufferQueue.Count +
                                  " queued of " + (object) LegacyNetBufferPool._smallBufferCount);
                Console.WriteLine("Medium Buffers: " + (object) LegacyNetBufferPool._mediumBufferQueue.Count +
                                  " queued of " + (object) LegacyNetBufferPool._mediumBufferCount);
                Console.WriteLine("Large Buffers:  " + (object) LegacyNetBufferPool._largeBufferQueue.Count +
                                  " queued of " + (object) LegacyNetBufferPool._largeBufferCount);
                Console.WriteLine("Custom Buffers: 0 queued of " + (object) LegacyNetBufferPool._customBufferCount);
                Console.WriteLine("");
            }
        }
    }
}