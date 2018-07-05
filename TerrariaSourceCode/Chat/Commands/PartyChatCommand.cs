// Decompiled with JetBrains decompiler
// Type: Terraria.Chat.Commands.PartyChatCommand
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Terraria.GameContent.NetModules;
using Terraria.Localization;
using Terraria.Net;

namespace Terraria.Chat.Commands
{
    [ChatCommand("Party")]
    public class PartyChatCommand : IChatCommand
    {
        private static readonly Color ERROR_COLOR = new Color(byte.MaxValue, 240, 20);

        public void ProcessMessage(string text, byte clientId)
        {
            var team = Main.player[clientId].team;
            var color = Main.teamColor[team];
            if (team == 0)
            {
                SendNoTeamError(clientId);
            }
            else
            {
                if (text == "")
                    return;
                for (var playerId = 0; playerId < (int) byte.MaxValue; ++playerId)
                    if (Main.player[playerId].team == team)
                    {
                        var packet =
                            NetTextModule.SerializeServerMessage(NetworkText.FromLiteral(text), color, clientId);
                        NetManager.Instance.SendToClient(packet, playerId);
                    }
            }
        }

        private void SendNoTeamError(byte clientId)
        {
            var packet =
                NetTextModule.SerializeServerMessage(Lang.mp[10].ToNetworkText(), ERROR_COLOR);
            NetManager.Instance.SendToClient(packet, clientId);
        }
    }
}