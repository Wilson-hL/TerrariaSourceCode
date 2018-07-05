// Decompiled with JetBrains decompiler
// Type: Terraria.Chat.Commands.ListPlayersCommand
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace Terraria.Chat.Commands
{
    [ChatCommand("Playing")]
    public class ListPlayersCommand : IChatCommand
    {
        private static readonly Color RESPONSE_COLOR = new Color(byte.MaxValue, 240, 20);

        public void ProcessMessage(string text, byte clientId)
        {
            NetMessage.SendChatMessageToClient(
                NetworkText.FromLiteral(string.Join(", ",
                    Main.player.Where(player => player.active)
                        .Select(player => player.name))),
                RESPONSE_COLOR, clientId);
        }
    }
}