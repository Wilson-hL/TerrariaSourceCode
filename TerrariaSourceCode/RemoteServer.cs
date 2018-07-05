﻿// Decompiled with JetBrains decompiler
// Type: Terraria.RemoteServer
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System;
using System.IO;
using Terraria.Localization;
using Terraria.Net.Sockets;

namespace Terraria
{
    public class RemoteServer
    {
        public ISocket Socket = (ISocket) new TcpSocket();
        public bool IsActive;
        public int State;
        public int TimeOutTimer;
        public bool IsReading;
        public byte[] ReadBuffer;
        public string StatusText;
        public int StatusCount;
        public int StatusMax;

        public void ClientWriteCallBack(object state)
        {
            --NetMessage.buffer[256].spamCount;
        }

        public void ClientReadCallBack(object state, int length)
        {
            try
            {
                if (!Netplay.disconnect)
                {
                    int streamLength = length;
                    if (streamLength == 0)
                    {
                        Netplay.disconnect = true;
                        Main.statusText = Language.GetTextValue("Net.LostConnection");
                    }
                    else if (Main.ignoreErrors)
                    {
                        try
                        {
                            NetMessage.ReceiveBytes(this.ReadBuffer, streamLength, 256);
                        }
                        catch
                        {
                        }
                    }
                    else
                        NetMessage.ReceiveBytes(this.ReadBuffer, streamLength, 256);
                }

                this.IsReading = false;
            }
            catch (Exception ex)
            {
                try
                {
                    using (StreamWriter streamWriter = new StreamWriter("client-crashlog.txt", true))
                    {
                        streamWriter.WriteLine((object) DateTime.Now);
                        streamWriter.WriteLine((object) ex);
                        streamWriter.WriteLine("");
                    }
                }
                catch
                {
                }

                Netplay.disconnect = true;
            }
        }
    }
}