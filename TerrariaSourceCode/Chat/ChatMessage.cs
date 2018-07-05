// Decompiled with JetBrains decompiler
// Type: Terraria.Chat.ChatMessage
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System.IO;
using System.Text;
using Terraria.Chat.Commands;

namespace Terraria.Chat
{
    public class ChatMessage
    {
        public ChatMessage(string message)
        {
            CommandId = ChatCommandId.FromType<SayChatCommand>();
            Text = message;
        }

        private ChatMessage(string message, ChatCommandId commandId)
        {
            CommandId = commandId;
            Text = message;
        }

        public ChatCommandId CommandId { get; private set; }

        public string Text { get; set; }

        public void Serialize(BinaryWriter writer)
        {
            CommandId.Serialize(writer);
            writer.Write(Text);
        }

        public int GetMaxSerializedSize()
        {
            return 0 + CommandId.GetMaxSerializedSize() + 4 + Encoding.UTF8.GetByteCount(Text);
        }

        public static ChatMessage Deserialize(BinaryReader reader)
        {
            var commandId = ChatCommandId.Deserialize(reader);
            return new ChatMessage(reader.ReadString(), commandId);
        }

        public void SetCommand(ChatCommandId commandId)
        {
            CommandId = commandId;
        }

        public void SetCommand<T>() where T : IChatCommand
        {
            CommandId = ChatCommandId.FromType<T>();
        }
    }
}