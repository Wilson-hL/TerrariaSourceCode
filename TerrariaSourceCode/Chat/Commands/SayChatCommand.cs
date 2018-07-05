// Decompiled with JetBrains decompiler
// Type: Terraria.Chat.Commands.SayChatCommand
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System;
using Terraria.GameContent.NetModules;
using Terraria.Localization;
using Terraria.Net;

namespace Terraria.Chat.Commands
{
    [ChatCommand("Say")]
    public class SayChatCommand : IChatCommand
    {
        public void ProcessMessage(string text, byte clientId)
        {
            var packet = NetTextModule.SerializeServerMessage(NetworkText.FromLiteral(text),
                Main.player[clientId].ChatColor(), clientId);
            NetManager.Instance.Broadcast(packet, -1);
            Console.WriteLine("<{0}> {1}", Main.player[clientId].name, text);
        }
    }
}