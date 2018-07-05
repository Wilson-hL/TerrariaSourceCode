﻿// Decompiled with JetBrains decompiler
// Type: Terraria.Graphics.TextureManager
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Terraria.Graphics
{
    public static class TextureManager
    {
        private static ConcurrentDictionary<string, Texture2D>
            _textures = new ConcurrentDictionary<string, Texture2D>();

        private static ConcurrentQueue<TextureManager.LoadPair> _loadQueue =
            new ConcurrentQueue<TextureManager.LoadPair>();

        private static readonly object _loadThreadLock = new object();
        private static Thread _loadThread;
        public static Texture2D BlankTexture;

        public static void Initialize()
        {
            TextureManager.BlankTexture = new Texture2D(Main.graphics.GraphicsDevice, 4, 4);
        }

        public static Texture2D Load(string name)
        {
            if (TextureManager._textures.ContainsKey(name))
                return TextureManager._textures[name];
            var texture2D = TextureManager.BlankTexture;
            if (name != "")
            {
                if (name != null)
                {
                    try
                    {
                        texture2D = Main.instance.OurLoad<Texture2D>(name);
                    }
                    catch (Exception ex)
                    {
                        texture2D = TextureManager.BlankTexture;
                    }
                }
            }

            TextureManager._textures[name] = texture2D;
            return texture2D;
        }

        public static Ref<Texture2D> AsyncLoad(string name)
        {
            return new Ref<Texture2D>(TextureManager.Load(name));
        }

        private static void Run(object context)
        {
            var looping = true;
            Main.instance.Exiting += (EventHandler<EventArgs>) ((obj, args) =>
            {
                looping = false;
                if (!Monitor.TryEnter(TextureManager._loadThreadLock))
                    return;
                Monitor.Pulse(TextureManager._loadThreadLock);
                Monitor.Exit(TextureManager._loadThreadLock);
            });
            Monitor.Enter(TextureManager._loadThreadLock);
            while (looping)
            {
                if (TextureManager._loadQueue.Count != 0)
                {
                    TextureManager.LoadPair result;
                    if (TextureManager._loadQueue.TryDequeue(out result))
                        result.TextureRef.Value = TextureManager.Load(result.Path);
                }
                else
                    Monitor.Wait(TextureManager._loadThreadLock);
            }

            Monitor.Exit(TextureManager._loadThreadLock);
        }

        private struct LoadPair
        {
            public string Path;
            public Ref<Texture2D> TextureRef;

            public LoadPair(string path, Ref<Texture2D> textureRef)
            {
                this.Path = path;
                this.TextureRef = textureRef;
            }
        }
    }
}