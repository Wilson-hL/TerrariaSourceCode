// Decompiled with JetBrains decompiler
// Type: Terraria.Chat.Commands.RollCommand
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace Terraria.Chat.Commands
{
    [ChatCommand("Roll")]
    public class RollCommand : IChatCommand
    {
        private static readonly Color RESPONSE_COLOR = new Color(byte.MaxValue, 240, 20);

        public string InternalName => "roll";

        public void ProcessMessage(string text, byte clientId)
        {
            var num = Main.rand.Next(1, 101);
            NetMessage.BroadcastChatMessage(
                NetworkText.FromFormattable("*{0} {1} {2}", (object) Main.player[clientId].name,
                    (object) Lang.mp[9].ToNetworkText(), (object) num), RESPONSE_COLOR, -1);
        }
    }
}